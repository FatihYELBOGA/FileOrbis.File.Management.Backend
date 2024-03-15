using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;

namespace FileOrbis.File.Management.Backend.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository folderRepository;
        private readonly IConfiguration configuration;

        public FolderService(IFolderRepository folderRepository, IConfiguration configuration) 
        {
            this.folderRepository = folderRepository;
            this.configuration = configuration;
        }

        public FolderResponse GetById(int id)
        {
            Folder foundFolder = folderRepository.GetById(id);
            FolderResponse folderResponse = new FolderResponse(foundFolder);
            string folderPath = Path.Combine(configuration.GetSection("MainFolderPath").Value, foundFolder.Path);
            folderResponse.LastModifiedDate = Directory.GetLastWriteTime(folderPath);

            foreach (var file in folderResponse.SubFiles)
            {
                string filePath = file.Path;
                FileInfo fileInfo = new FileInfo(filePath);
                file.LastModifiedDate = fileInfo.LastWriteTime; 
                double kb = (double)fileInfo.Length / 1024;
                file.Size = kb.ToString("0") + " KB";
                double mb;
                if (kb >= 1024)
                {
                    mb = (double)kb / 1024;
                    file.Size = mb.ToString("0.00") + " MB";
                }

            }

            return folderResponse;
        }

        public FolderResponse GetByPath(string path)
        {
            Folder foundFolder = folderRepository.GetByPath(path);
            FolderResponse folderResponse = new FolderResponse(foundFolder);
            string folderPath = Path.Combine(configuration.GetSection("MainFolderPath").Value, foundFolder.Path);
            folderResponse.LastModifiedDate = Directory.GetLastWriteTime(folderPath);

            return folderResponse;
        }

        public FolderResponse Create(CreateFolderRequest createFolderRequest)
        {
            string path = Path.Combine(configuration.GetSection("MainFolderPath").Value, createFolderRequest.Path);
            Directory.CreateDirectory(path);

            string[] folderName = createFolderRequest.Path.Split('/');
            Folder newFolder = new Folder()
            {
                Name = folderName[folderName.Length-1],  
                CreatedDate = DateTime.Now,
                ParentFolderId = createFolderRequest.ParentFolderId,
                Path = createFolderRequest.Path
            };

            return new FolderResponse(folderRepository.Create(newFolder));
        }

        public bool HasFile(int id)
        {
            Folder foundFolder = folderRepository.GetById(id);
            return foundFolder.SubFiles.Count != 0 ? true : false;
        }

        public bool DeleteById(int id)
        {
            Folder foundFolder = folderRepository.CheckById(id);
            if (foundFolder != null)
            {
                if (HasFile(id)) {
                    return false;
                }
                folderRepository.Delete(foundFolder);
                return true;
            }

            return false;
        }

    }

}
