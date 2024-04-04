namespace FileOrbis.File.Management.Backend.Models
{
    public class FavoriteFiles
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public Models.File File { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
