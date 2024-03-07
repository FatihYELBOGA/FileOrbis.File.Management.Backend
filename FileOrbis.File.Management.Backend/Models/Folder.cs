namespace FileOrbis.File.Management.Backend.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ParentFolderId {  get; set; } 
        public Folder? ParentFolder { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public User? RootFolderUser { get; set; }
        public List<Folder> SubFolders { get; set; }
        public List<File> SubFiles { get; set; }

    }

}
