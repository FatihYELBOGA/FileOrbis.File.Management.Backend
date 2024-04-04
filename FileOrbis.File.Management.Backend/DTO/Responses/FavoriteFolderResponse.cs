namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class FavoriteFolderResponse
    {
        public int Id { get; set; }
        public FolderResponse Folder { get; set; }

        public FavoriteFolderResponse(int id, FolderResponse folderResponse) 
        {
            Id = id;    
            Folder = folderResponse;
        }

    }

}
