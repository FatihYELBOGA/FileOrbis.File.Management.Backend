namespace FileOrbis.File.Management.Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? RootFolderId { get; set; }
        public Folder? RootFolder { get; set; }
        public List<Folder> Folders { get; set; }
        public List<Models.File> Files { get; set; }

    }

}
