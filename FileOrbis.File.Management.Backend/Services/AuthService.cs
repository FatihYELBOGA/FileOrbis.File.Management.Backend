using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FileOrbis.File.Management.Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly IFolderService folderService;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthService> logger;

        public AuthService(IAuthRepository authRepository, IFolderService folderService, IConfiguration configuration, ILogger<AuthService> logger)
        {
            this.authRepository = authRepository;
            this.folderService = folderService;
            this.configuration = configuration;
            this.logger = logger;
        }

        private JwtSecurityToken CreateToken(User user)
        {
            string signingKey = configuration.GetSection("JWTConfig:key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
             };
            return new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToInt64(configuration.GetSection("JWTConfig:token-expiration-hour").Value)),
                notBefore: DateTime.Now,
                signingCredentials: credentials
            );
        }

        public bool CheckTokenIsValid(string token)
        {
            if (ConvertStringToToken(token) != null)
                return true;

            return false;
        }

        private JwtSecurityToken ConvertStringToToken(string token)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTConfig:key").Value));
            try
            {
                JwtSecurityTokenHandler handler = new();
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken) validatedToken;
                return jwtToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CheckRefreshTokenIsValid(RefreshTokenRequest refreshTokenRequest)
        {
            RefreshToken updatedRefreshToken = authRepository.CheckRefreshToken(refreshTokenRequest);
            if (updatedRefreshToken != null && updatedRefreshToken.Expiration.CompareTo(DateTime.Now) > 0)
                return true;

            return false;
        }

        public LoginResponse RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            LoginResponse loginResponse = new LoginResponse()
            {
                Success= false,
                JWTToken = null,
                RefreshToken = null,
                Id = null,
                Username = null,
                Role = null,
                Message = "user id or refresh token is mistake!"
            };

            RefreshToken updatedRefreshToken = authRepository.CheckRefreshToken(refreshTokenRequest);
            if (updatedRefreshToken != null)
            {
                if (updatedRefreshToken.Expiration.CompareTo(DateTime.Now) > 0)
                {
                    updatedRefreshToken.Token = CreateRefreshToken();
                    updatedRefreshToken.Expiration = DateTime.Now.AddHours(Convert.ToInt64(configuration.GetSection("JWTConfig:refresh-token-expiration-hour").Value)); ;
                    authRepository.UpdateRefreshToken(updatedRefreshToken);
                    loginResponse.Success = true;
                    var jwtSecurityToken = CreateToken(updatedRefreshToken.User);
                    loginResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    loginResponse.RefreshToken = updatedRefreshToken.Token;
                    loginResponse.Id = updatedRefreshToken.UserId;
                    loginResponse.Username = updatedRefreshToken.User.Email;
                    loginResponse.Role = updatedRefreshToken.User.Role;
                    loginResponse.Message = "successful refresh token";
                }
            }

            return loginResponse;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            LoginResponse loginResponse = new LoginResponse()
            {
                Success = false,
                JWTToken = null,
                RefreshToken = null,
                Id = null,
                Username = null,
                Role = null,
                Message = "username or password is mistake!"
            };

            User foundUser = authRepository.Login(loginRequest);
            if (foundUser != null)
            {
                loginResponse.Success = true;
                var jwtSecurityToken = CreateToken(foundUser);
                loginResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                loginResponse.RefreshToken = CreateRefreshToken();
                loginResponse.Id = foundUser.Id;
                loginResponse.Username = foundUser.Email;
                loginResponse.Role = foundUser.Role;
                loginResponse.Message = "successful login!";
                loginResponse.RootFolderId = foundUser.RootFolderId;

                RefreshToken foundRefreshToken = authRepository.ExistRefreshToken(foundUser.Id);
                if (foundRefreshToken != null)
                {
                    foundRefreshToken.Token = loginResponse.RefreshToken;
                    foundRefreshToken.Expiration = DateTime.Now.AddHours(Convert.ToInt64(configuration.GetSection("JWTConfig:refresh-token-expiration-hour").Value));
                    authRepository.UpdateRefreshToken(foundRefreshToken);
                }
                else
                {
                    RefreshToken addedRefreshToken = new RefreshToken()
                    {
                        UserId = foundUser.Id,
                        Token = loginResponse.RefreshToken,
                        Expiration = DateTime.Now.AddHours(Convert.ToInt64(configuration.GetSection("JWTConfig:token-expiration-hour").Value))
                    };
                    authRepository.CreateRefreshToken(addedRefreshToken);
                }
                logger.LogInformation("user credentials is authenticated");
            } else
            {
                logger.LogWarning("username or password is mistake!");
            }

            return loginResponse;
        }

        private string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }

        public UserResponse Register(RegisterRequest registerRequest)
        {
            bool foundUser = authRepository.ExistEmail(registerRequest.Email);
            if (foundUser)
                return new UserResponse();

            FolderResponse rootFolder = folderService.Create(new CreateFolderRequest()
            {
                Name = registerRequest.Email,
                ParentFolderId = null
            });

            User addedUser = new User()
            {
                Email = registerRequest.Email,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerRequest.Password)),
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Role = Enumerations.Role.USER,
                RootFolderId = rootFolder.Id
            };

            User returnedUser = authRepository.Register(addedUser);
            return new UserResponse(returnedUser, configuration);
        }

    }

}
