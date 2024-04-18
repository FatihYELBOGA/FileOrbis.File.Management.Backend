using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Database database;

        public UserRepository(Database database)
        {
            this.database = database;
        }

        public List<User> GetAll()
        {
            return database.Users.ToList();
        }

        public User GetById(int id)
        {
            return database.Users
                .Where(u => u.Id == id)
                .Include(u =>u.RootFolder)
                    .ThenInclude(f => f.SubFolders)
                .Include(u => u.RootFolder)
                    .ThenInclude(f => f.SubFiles)
                .FirstOrDefault();
        }

        public User GetFavoritesById(int userId)
        {
            return database.Users
                .Where(u => u.Id == userId)
                .Include(u => u.FavoriteFolders)
                    .ThenInclude(f => f.Folder)
                .Include(u => u.FavoriteFiles)
                    .ThenInclude(f => f.File)
                    .ThenInclude(f => f.Folder)
                .FirstOrDefault();
        }

        public User GetFavoritesByUsername(string username)
        {
            return database.Users
                .Where(u => u.Email.StartsWith(username))
                .Include(u => u.FavoriteFolders)
                    .ThenInclude(f => f.Folder)
                .Include(u => u.FavoriteFiles)
                    .ThenInclude(f => f.File)
                    .ThenInclude(f => f.Folder)
                .FirstOrDefault();
        }

        public User AddFavoriteFile(FavoriteFiles favorite)
        {
            database.FavoriteFiles.Add(favorite);
            database.SaveChanges();
            return GetFavoritesById(favorite.UserId);
        }

        public User AddFavoriteFolder(FavoriteFolders favorite)
        {
            database.FavoriteFolders.Add(favorite);
            database.SaveChanges ();
            return GetFavoritesById(favorite.UserId);
        }

        public bool DeleteFavoriteFileById(int id)
        {
            try
            {
                FavoriteFiles? file = database.FavoriteFiles.Find(id);
                if (file != null)
                {
                    database.FavoriteFiles.Remove(file);
                    database.SaveChanges();
                    return true;
                }  else
                {
                    return false;

                }
            } 
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteFavoriteFolderById(int id)
        {
            try
            {
                FavoriteFolders? folder = database.FavoriteFolders.Find(id);
                if (folder != null)
                {
                    database.FavoriteFolders.Remove(folder);
                    database.SaveChanges();
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }

}
