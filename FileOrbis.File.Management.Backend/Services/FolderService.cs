using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;

namespace FileOrbis.File.Management.Backend.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository folderRepository;
        private readonly IFileService fileService;
        private readonly IConfiguration configuration;
        private string mainFolderPath = "";

        public FolderService(IFolderRepository folderRepository, IFileService fileService , IConfiguration configuration) 
        {
            this.folderRepository = folderRepository;
            this.fileService = fileService;
            this.configuration = configuration;
            mainFolderPath = configuration.GetSection("MainFolderPath").Value;
        }

        public FolderResponse GetById(int id)
        {
            Folder foundFolder = folderRepository.GetById(id);
            FolderResponse folderResponse = new FolderResponse(foundFolder, configuration);
            string folderPath = Path.Combine(mainFolderPath, foundFolder.Path);
            folderResponse.LastModifiedDate = Directory.GetLastWriteTime(folderPath);

            foreach (var file in folderResponse.SubFiles)
            {
                FileInfo fileInfo = new FileInfo(file.Path);
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
            FolderResponse folderResponse = new FolderResponse(foundFolder, configuration);
            string folderPath = Path.Combine(configuration.GetSection("MainFolderPath").Value, foundFolder.Path);
            folderResponse.LastModifiedDate = Directory.GetLastWriteTime(folderPath);

            return folderResponse;
        }

        public FolderResponse Create(CreateFolderRequest createFolderRequest)
        {
            string folderPath = "";
            if(createFolderRequest.ParentFolderId != null)
                folderPath = folderRepository.GetById((int)createFolderRequest.ParentFolderId).Path + "/" + createFolderRequest.Name;
            else
                folderPath = createFolderRequest.Name;

            string path = Path.Combine(mainFolderPath, folderPath);
            Directory.CreateDirectory(path);

            Folder newFolder = new Folder()
            {
                Name = createFolderRequest.Name,  
                CreatedDate = DateTime.Now,
                ParentFolderId = createFolderRequest.ParentFolderId,
                Path = folderPath,
                Trashed = 0
            };

            return new FolderResponse(folderRepository.Create(newFolder), configuration);
        }

        public FolderResponse Rename(int id, string name)
        {
            Folder foundFolder = folderRepository.GetById(id);
            if (foundFolder != null)
            {
                string newPath = "";
                string[] paths = foundFolder.Path.Split("/");
                for(int  i=0; i<paths.Length; i++)
                {
                    if(i != paths.Length - 1)
                        newPath = newPath + paths[i] + "/";
                    else
                        newPath = newPath + name;
                }

                Directory.Move(Path.Combine(mainFolderPath, foundFolder.Path), Path.Combine(mainFolderPath, newPath));
                
                foundFolder.Name = name;
                foundFolder.Path = newPath;
                return new FolderResponse(folderRepository.Update(foundFolder), configuration);
            }
            else
                throw new DirectoryNotFoundException();
        }

        public FolderResponse Trash(int id)
        {
            Folder foundFolder = folderRepository.GetById(id);
            if (foundFolder != null)
            {
                foundFolder.Trashed = 1;
                return new FolderResponse(folderRepository.Update(foundFolder), configuration);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        public FolderResponse Restore(int id)
        {
            Folder foundFolder = folderRepository.CheckById(id);
            if (foundFolder != null)
            {
                foundFolder.Trashed = 0;
                return new FolderResponse(folderRepository.Update(foundFolder), configuration);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        public bool DeleteById(int id)
        {
            Folder foundFolder = folderRepository.GetById(id);
            if (foundFolder != null)
            {
                int[] fileIds = new int[foundFolder.SubFiles.Count];
                int counter = 0;
                foreach (var file in foundFolder.SubFiles)
                {
                    fileIds[counter] = file.Id;
                    counter++;
                }

                foreach (var fileId in fileIds)
                {
                    fileService.DeleteById(fileId);
                }

                counter = 0;
                int[] folderIds = new int[foundFolder.SubFolders.Count];
                foreach (var folder in foundFolder.SubFolders)
                {
                    folderIds[counter] = folder.Id;
                    counter++;
                }

                foreach(var folderId in folderIds)
                {
                    DeleteById(folderId);
                }

                folderRepository.Delete(foundFolder);
                Directory.Delete(Path.Combine(mainFolderPath, foundFolder.Path), true);
                return true;
            }

            return false;
        }

    }

}
