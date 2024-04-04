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
        public int Trashed {  get; set;  }
        public DateTime? DeletedDate { get; set; }

        public FileResponse(Models.File file, IConfiguration configuration) 
        {
            Id = file.Id;
            Name = file.Name;
            Type = file.Type;
            CreatedDate = file.CreatedDate;
            Path = file.Folder.Path;

            FileInfo fileInfo = new FileInfo(configuration.GetSection("MainFolderPath").Value + "/" + Path + "/" + file.Name);
            LastModifiedDate = fileInfo.LastWriteTime;

            double kb = (double)fileInfo.Length / 1024;
            Size = kb.ToString("0") + " KB";
            double mb;
            if (kb >= 1024)
            {
                mb = (double)kb / 1024;
                Size = mb.ToString("0.00") + " MB";
            }

            Trashed = file.Trashed;
            DeletedDate = file.DeletedDate;
        }

    }

}
