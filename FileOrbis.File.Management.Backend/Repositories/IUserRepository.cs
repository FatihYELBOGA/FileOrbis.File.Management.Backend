using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public User GetById(int id);
        public User GetFavoritesById(int userId);
        public User GetFavoritesByUsername(string username);
        public User AddFavoriteFile(FavoriteFiles favorite);
        public User AddFavoriteFolder(FavoriteFolders favorite);
        public bool DeleteFavoriteFileById(int id);
        public bool DeleteFavoriteFolderById(int id);

    }

}
