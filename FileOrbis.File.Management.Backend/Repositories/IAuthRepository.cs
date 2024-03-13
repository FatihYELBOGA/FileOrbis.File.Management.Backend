using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IAuthRepository
    {
        public RefreshToken CheckRefreshToken(RefreshTokenRequest refreshTokenRequest);
        public RefreshToken ExistRefreshToken(int userId);
        public void UpdateRefreshToken(RefreshToken refreshToken);
        public void CreateRefreshToken(RefreshToken refreshToken);
        public bool ExistEmail(string email);
        public User Login(LoginRequest loginRequest);
        public User Register(User addedUser);


    }

}
