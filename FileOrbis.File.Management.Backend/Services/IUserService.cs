using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IUserService
    {
        public List<UserResponse> GetAll();
        public UserResponse GetById(int id);
        public UserResponse GetFavoritesById(int userId);
        public string CheckFileInTrash(int fileId, string username);
        public string CheckFolderInTrash(int folderId, string username);
        public UserResponse AddFavoriteFile(AddFavoriteFileRequest addFavoriteFile);
        public UserResponse AddFavoriteFolder(AddFavoriteFolderRequest addFavoriteFolder);
        public bool DeleteFavoriteFileById(int id);
        public bool DeleteFavoriteFolderById(int id);

    }

}
