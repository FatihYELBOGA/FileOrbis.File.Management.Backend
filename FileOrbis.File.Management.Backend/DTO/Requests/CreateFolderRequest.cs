namespace FileOrbis.File.Management.Backend.DTO.Requests
{
    public class CreateFolderRequest
    {
        public string Path { get; set; }
        public int? ParentFolderId { get; set; }

    }

}
