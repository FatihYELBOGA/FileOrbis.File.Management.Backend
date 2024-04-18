using System.ComponentModel.DataAnnotations.Schema;

namespace FileOrbis.File.Management.Backend.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Path { get; set; }
        public int Trashed {  get; set; }
        public DateTime? DeletedDate { get; set; }

        [NotMapped]
        public bool Starred { get; set; }
        public int? ParentFolderId {  get; set; } 
        public Folder? ParentFolder { get; set; }
        public User RootFolderUser { get; set; }
        public List<Folder> SubFolders { get; set; }
        public List<File> SubFiles { get; set; }
        public List<FavoriteFolders> InFavorites { get; set; }

    }

}
