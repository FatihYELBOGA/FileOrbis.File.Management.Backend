namespace FileOrbis.File.Management.Backend.DTO.Requests
{
    public class CreateFolderRequest
    {
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }

    }

}
