using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;

namespace FileOrbis.File.Management.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public List<UserResponse> GetAll()
        {
            List<UserResponse> userResponses = new List<UserResponse>();
            foreach (var user in userRepository.GetAll())
            {
                userResponses.Add(new UserResponse(user, configuration));
            }

            return userResponses;
        }

        public UserResponse GetById(int id)
        {
            return new UserResponse(userRepository.GetById(id), configuration);
        }

        public UserResponse GetFavoritesById(int userId)
        {
            return new UserResponse(userRepository.GetFavoritesById(userId), configuration);
        }

        public UserResponse AddFavoriteFile(AddFavoriteFileRequest addFavoriteFile)
        {
            FavoriteFiles favorite = new FavoriteFiles()
            {
                UserId = addFavoriteFile.UserId,
                FileId = addFavoriteFile.FileId
            };

            return new UserResponse(userRepository.AddFavoriteFile(favorite), configuration);
        }

        public UserResponse AddFavoriteFolder(AddFavoriteFolderRequest addFavoriteFolder)
        {
            FavoriteFolders favorite = new FavoriteFolders()
            {
                UserId = addFavoriteFolder.UserId,
                FolderId = addFavoriteFolder.FolderId
            };

            return new UserResponse(userRepository.AddFavoriteFolder(favorite), configuration);
        }

        public bool DeleteFavoriteFileById(int id)
        {
            return userRepository.DeleteFavoriteFileById(id);
        }

        public bool DeleteFavoriteFolderById(int id)
        {
            return userRepository.DeleteFavoriteFolderById(id);
        }

    }

}
