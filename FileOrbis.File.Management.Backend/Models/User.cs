using FileOrbis.File.Management.Backend.Enumerations;

namespace FileOrbis.File.Management.Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public int RootFolderId { get; set; }
        public Folder RootFolder { get; set; }
        public List<FavoriteFolders> FavoriteFolders { get; set; }
        public List<FavoriteFiles> FavoriteFiles { get; set; }

    }

}
