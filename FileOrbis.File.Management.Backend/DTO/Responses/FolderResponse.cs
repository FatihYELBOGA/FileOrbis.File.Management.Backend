using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class FolderResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<FolderResponse> SubFolders { get; set; }
        public List<FileResponse> SubFiles { get; set; }

        public FolderResponse(Folder folder) 
        {
            Id = folder.Id;
            Name = folder.Name;
            CreatedDate = folder.CreatedDate;
            SubFolders = new List<FolderResponse>();
            SubFiles = new List<FileResponse>();

            if(folder.SubFolders != null)
            {
                foreach (var subfolder in folder.SubFolders)
                {
                    subfolder.SubFolders = null;
                    subfolder.ParentFolder = null;
                    SubFolders.Add(new FolderResponse(subfolder));
                }
            }

            if(folder.SubFiles != null)
            {
                foreach (var subfile in folder.SubFiles)
                {
                    subfile.Folder = null;
                    SubFiles.Add(new FileResponse(subfile));
                }
            } 

        }

    }

}
