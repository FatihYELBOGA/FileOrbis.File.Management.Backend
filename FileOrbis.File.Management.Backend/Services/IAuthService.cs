using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IAuthService
    {
        public bool CheckTokenIsValid(string token);
        public bool CheckRefreshTokenIsValid(RefreshTokenRequest refreshTokenRequest);
        public LoginResponse RefreshToken(RefreshTokenRequest refreshTokenRequest);
        public LoginResponse Login(LoginRequest loginRequest);
        public UserResponse Register(RegisterRequest registerRequest);

    }

}
