using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public FolderResponse MainFolders { get; set; }
        public List<FavoriteFileResponse> FavoriteFiles { get; set; }
        public List<FavoriteFolderResponse> FavoriteFolders { get; set; }

        public UserResponse() { }

        public UserResponse(User user, IConfiguration configuration) 
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            FavoriteFiles = new List<FavoriteFileResponse>();
            FavoriteFolders = new List<FavoriteFolderResponse>();

            if(user.RootFolder != null)
                MainFolders = new FolderResponse(user.RootFolder, configuration);

            if(user.FavoriteFolders != null || user.FavoriteFolders.Count() != 0)
            {
                foreach (var folder in user.FavoriteFolders)
                {
                    FavoriteFolders.Add(new FavoriteFolderResponse(folder.Id, new FolderResponse(folder.Folder, configuration)));
                }
            }

            if(user.FavoriteFiles != null ||user.FavoriteFiles.Count() != 0)
            {
                foreach (var file in user.FavoriteFiles)
                {
                    FavoriteFiles.Add(new FavoriteFileResponse(file.Id, new FileResponse(file.File, configuration)));
                }
            }

        }

    }

}
