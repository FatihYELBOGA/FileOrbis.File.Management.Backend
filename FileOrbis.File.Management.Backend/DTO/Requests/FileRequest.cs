namespace FileOrbis.File.Management.Backend.DTO.Requests
{
    public class FileRequest
    {
        public string Path { get; set; }
        public IFormFile Content { get; set; }
        public string Email { get; set; }
        public int? FolderId { get; set; }

    }

}
