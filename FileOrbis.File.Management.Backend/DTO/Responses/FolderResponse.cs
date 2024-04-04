using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class FolderResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Path { get; set; }
        public int Trashed { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<FolderResponse> SubFolders { get; set; }
        public List<FileResponse> SubFiles { get; set; }

        public FolderResponse(Folder folder, IConfiguration configuration) 
        {
            Id = folder.Id;
            Name = folder.Name;
            CreatedDate = folder.CreatedDate;
            Path = folder.Path;
            Trashed = folder.Trashed;
            DeletedDate = folder.DeletedDate;

            string folderPath = configuration.GetSection("MainFolderPath").Value + "/" + folder.Path;
            LastModifiedDate = Directory.GetLastWriteTime(folderPath);

            SubFolders = new List<FolderResponse>();
            SubFiles = new List<FileResponse>();

            if(folder.SubFolders != null)
            {
                foreach (var subfolder in folder.SubFolders)
                {
                    if(subfolder.Trashed == 0)
                    {
                        subfolder.SubFolders = null;
                        subfolder.ParentFolder = null;
                        SubFolders.Add(new FolderResponse(subfolder, configuration));
                    }
                }
            }

            if(folder.SubFiles != null)
            {
                foreach (var subfile in folder.SubFiles)
                {
                    if(subfile.Trashed == 0)
                    {
                        SubFiles.Add(new FileResponse(subfile, configuration));
                    }
                }
            } 

        }

    }

}
