namespace FileOrbis.File.Management.Backend.Models
{
    public class FavoriteFolders
    {

        public int Id { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }

}
