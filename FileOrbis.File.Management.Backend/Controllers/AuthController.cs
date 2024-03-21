using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("/auth/token-validation")]
        public bool CheckTokenIsValid([FromForm] string token)
        {
            return authService.CheckTokenIsValid(token);
        }

        [HttpPost("/auth/refresh-token-validation")]
        public bool CheckRefreshTokenIsValid([FromForm] RefreshTokenRequest refreshTokenRequest)
        {
            return authService.CheckRefreshTokenIsValid(refreshTokenRequest);
        }

        [HttpPut("/auth/refresh-token")]
        public LoginResponse RefreshToken([FromForm] RefreshTokenRequest refreshTokenRequest)
        {
            return authService.RefreshToken(refreshTokenRequest);
        }

        [HttpPost("/auth/login")]
        public LoginResponse Login([FromForm] LoginRequest loginRequest)
        {
            return authService.Login(loginRequest);
        }

        [HttpPost("/auth/register")]
        public UserResponse Regist([FromForm] RegisterRequest registerRequest)
        {
            return authService.Register(registerRequest);
        }

    }

}
