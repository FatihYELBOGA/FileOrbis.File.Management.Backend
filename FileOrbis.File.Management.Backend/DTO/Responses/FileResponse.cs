namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class FileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Path { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Size { get; set; }

        public FileResponse(Models.File file) 
        {
            Id = file.Id;
            Name = file.Name;
            Type = file.Type;
            CreatedDate = file.CreatedDate;
            Path = file.Path;
        }

    }

}
