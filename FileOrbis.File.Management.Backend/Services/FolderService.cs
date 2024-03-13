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
            return new FolderResponse(folderRepository.GetById(id));
        }

        public FolderResponse Create(CreateFolderRequest createFolderRequest)
        {
            string path = Path.Combine(configuration.GetSection("MainFolderPath").Value, createFolderRequest.Path);
            Directory.CreateDirectory(path);

            Folder newFolder = new Folder()
            {
                CreatedDate = DateTime.Now,
                ParentFolderId = createFolderRequest.ParentFolderId,
                Path = path
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
