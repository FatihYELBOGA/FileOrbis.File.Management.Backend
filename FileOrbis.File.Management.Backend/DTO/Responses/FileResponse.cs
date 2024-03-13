namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class FileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public float Size { get; set; }
        public string Path { get; set; }

        public FileResponse(Models.File file) 
        {
            Id = file.Id;
            Name = file.Name;
            Type = file.Type;
            CreatedDate = file.CreatedDate;
            Size = file.Size;
            Path = file.Path;
        }

    }

}
