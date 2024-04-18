using System.ComponentModel.DataAnnotations.Schema;

namespace FileOrbis.File.Management.Backend.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? RecentDate { get; set; }
        public int Trashed { get; set; }
        public DateTime? DeletedDate { get; set; }

        [NotMapped]
        public bool Starred { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
        public List<FavoriteFiles> InFavorites { get; set; }

    }

}
