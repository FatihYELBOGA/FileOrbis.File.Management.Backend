using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Database database;
        public AuthRepository(Database database)
        {
            this.database = database;
        }
        public RefreshToken CheckRefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            return database.RefreshTokens
                .Where(rt => rt.UserId == refreshTokenRequest.UserId && rt.Token == refreshTokenRequest.Token)
                .Include(rt => rt.User)
                .FirstOrDefault();
        }

        public RefreshToken ExistRefreshToken(int userId)
        {
            return database.RefreshTokens.Where(rt => rt.UserId == userId).FirstOrDefault();
        }

        public void CreateRefreshToken(RefreshToken refreshToken)
        {
            database.RefreshTokens.Add(refreshToken);
            database.SaveChanges();
        }

        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
            database.RefreshTokens.Update(refreshToken);
            database.SaveChanges();
        }

        public bool ExistEmail(string email)
        {
            User foundUser = database.Users.Where(u => u.Email == email).FirstOrDefault();
            if (foundUser != null)
                return true;

            return false;
        }

        public User Login(LoginRequest loginRequest)
        {
            return database.Users
                 .Where(u => u.Email == loginRequest.Email && u.Password == Convert.ToBase64String(Encoding.UTF8.GetBytes(loginRequest.Password)))
                 .FirstOrDefault();
        }

        public User Register(User addedUser)
        {
            User returnedUser = database.Users.Add(addedUser).Entity;
            database.SaveChanges();
            return returnedUser;
        }

    }
}
