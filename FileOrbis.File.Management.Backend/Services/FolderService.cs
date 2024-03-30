using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;
using System.IO;
using System.IO.Compression;

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
        public void AddFolderToZip(ZipArchive zipArchive, string folderPath, string parentFolder)
        {
            bool hasContent = Directory.GetFiles(folderPath).Length > 0 || Directory.GetDirectories(folderPath).Length > 0;

            if (!hasContent)
            {
                zipArchive.CreateEntry(parentFolder);
            }
            else
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    if (fileService.GetIdByPath(file) != null)
                    {
                        zipArchive.CreateEntryFromFile(file, Path.Combine(parentFolder, Path.GetFileName(file)));
                    }
                }

                foreach (string subFolder in Directory.GetDirectories(folderPath))
                {
                    string path_1 = NormalizePath(subFolder);
                    string usernameFilter = path_1.Split(mainFolderPath)[1].Split("/")[1] + "/";
                    if (GetIdByPath(path_1, usernameFilter) != null)
                    {
                        string subFolderEntryPath = Path.Combine(parentFolder, Path.GetFileName(subFolder)) + "/";
                        AddFolderToZip(zipArchive, subFolder, subFolderEntryPath);
                    }
                }
            }
        }

        static string NormalizePath(string path)
        {
            return path.Replace("\\", "/");
        }

        public int? GetIdByPath(string path, string filterPath)
        {
            foreach (var folder in folderRepository.GetAll(filterPath))
            {
                string path_2 = NormalizePath(Path.Combine(mainFolderPath, folder.Path));

                if (path.Equals(path_2))
                {
                    return folder.Id;
                }
            }
            return null;
        }

        public FolderResponse GetById(int id)
        {
            Folder foundFolder = folderRepository.GetById(id);
            FolderResponse folderResponse = new FolderResponse(foundFolder, configuration);
            string folderPath = Path.Combine(mainFolderPath, foundFolder.Path);
            folderResponse.LastModifiedDate = Directory.GetLastWriteTime(folderPath);

            foreach (var file in folderResponse.SubFiles)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(mainFolderPath, file.Path));
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

        public string GetNameById(int id)
        {
            return folderRepository.GetById(id).Name;
        }

        public string GetFolderPath(int id)
        {
            return Path.Combine(mainFolderPath, folderRepository.GetById(id).Path);
        }

        public List<FolderResponse> GetAllTrashes()
        {
            List<FolderResponse> folders = new List<FolderResponse>();
            foreach (Folder folder in folderRepository.GetAllTrashes())
            {
                folders.Add(new FolderResponse(folder, configuration));
            }

            return folders;
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

            Directory.CreateDirectory(mainFolderPath + "/" + folderPath);

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
                        newPath += name;
                }

                Directory.Move(Path.Combine(mainFolderPath, foundFolder.Path), Path.Combine(mainFolderPath, newPath));
                
                foundFolder.Name = name;
                string oldPath = foundFolder.Path;
                foundFolder.Path = newPath;

                foreach (var folder in folderRepository.GetAllStartsWith(oldPath + "/"))
                {
                    string remainingPartOfoldPath = folder.Path.Split(oldPath)[1];
                    string currentPath = newPath + remainingPartOfoldPath;
                    folder.Path = currentPath;
                    folderRepository.Update(folder);
                }

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
                foundFolder.DeletedDate = DateTime.Now;
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
            Folder foundFolder = folderRepository.CheckById(id);
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
