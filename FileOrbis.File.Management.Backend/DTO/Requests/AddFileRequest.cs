namespace FileOrbis.File.Management.Backend.DTO.Requests
{
    public class AddFileRequest
    {
        public string Path { get; set; }
        public IFormFile Content { get; set; }
        public int? FolderId { get; set; }

    }

}
