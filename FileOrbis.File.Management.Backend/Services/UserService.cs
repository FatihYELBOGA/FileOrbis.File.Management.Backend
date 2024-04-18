using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;

namespace FileOrbis.File.Management.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IFolderRepository folderRepository;
        private readonly IFileRepository fileRepository;
        private readonly IConfiguration configuration;

        public UserService(IUserRepository userRepository, IFolderRepository folderRepository, IFileRepository fileRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.folderRepository = folderRepository;
            this.fileRepository = fileRepository;
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

        public string CheckFileInTrash(int fileId, string username)
        {
            return CheckFolderInTrash(fileRepository.GetFileWithParentFolder(fileId).Folder.Id, username);
        }

        public string CheckFolderInTrash(int folderId, string username)
        {
            List<Folder> folders = folderRepository.GetAllTrashes(username);
            for (int i = 0; i < folders.Count; i++)
            {
                Folder folderWithParent = folderRepository.GetFolderWithParentFolder(folderId);
                Folder trashFolder = folders[i];
                if (trashFolder.Id == folderId)
                {
                    return "true";
                }

                folderWithParent = folderWithParent.ParentFolder;
                do
                {
                    if (folderWithParent == null)
                    {
                        return "false";
                    }
                    else
                    {
                        if(folderWithParent.Trashed == 1)
                        {
                            return "true";
                        }
                        else
                        {
                            folderWithParent = folderRepository.GetFolderWithParentFolder(folderWithParent.Id).ParentFolder;
                        }
                    }
                } while (folderWithParent != null);
            }
            return "false";
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
