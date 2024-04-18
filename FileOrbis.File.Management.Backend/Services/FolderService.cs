using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;
using System.IO.Compression;

namespace FileOrbis.File.Management.Backend.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository folderRepository;
        private readonly IFileService fileService;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly string mainFolderPath = "";

        public FolderService(IFolderRepository folderRepository, IFileService fileService, IUserRepository userRepository, IConfiguration configuration) 
        {
            this.folderRepository = folderRepository;
            this.fileService = fileService;
            this.configuration = configuration;
            this.userRepository = userRepository;
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

        public FolderResponse GetById(int folderId, int userId)
        {
            Folder foundFolder = folderRepository.GetById(folderId);
            foreach (var folder in foundFolder.SubFolders)
            {
                foreach (var subFavorites in folder.InFavorites)
                {
                    if(subFavorites.UserId == userId)
                    {
                        folder.Starred = true;
                        break;
                    }
                }
            }
            foreach (var file in foundFolder.SubFiles)
            {
                foreach (var subFavorites in file.InFavorites)
                {
                    if(subFavorites.UserId  == userId)
                    {
                        file.Starred = true;
                        break;
                    }
                }
            }

            FolderResponse folderResponse = new FolderResponse(foundFolder, configuration);
            return folderResponse;
        }

        public bool CheckNameExists(string name, int parentFolderId)
        {
            return folderRepository.CheckNameExists(name, parentFolderId);
        }

        public string GetNameById(int id)
        {
            return folderRepository.GetById(id).Name;
        }

        public string GetFolderPath(int id)
        {
            return Path.Combine(mainFolderPath, folderRepository.GetById(id).Path);
        }

        public List<FolderResponse> GetAllTrashes(string username)
        {
            User user = userRepository.GetFavoritesByUsername(username);
            List<FolderResponse> folders = new List<FolderResponse>();
            foreach (Folder folder in folderRepository.GetAllTrashes(username))
            {
                foreach (var favFolder in user.FavoriteFolders)
                {
                    if (folder.Id == favFolder.Folder.Id)
                    {
                        folder.Starred = true;
                    }
                }
                folder.SubFolders = null;
                folder.SubFiles = null;
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
            string folderPath;
            if (createFolderRequest.ParentFolderId != null)
                folderPath = folderRepository.GetById((int)createFolderRequest.ParentFolderId).Path + "/" + createFolderRequest.Name;
            else
                folderPath = createFolderRequest.Name;

            Directory.CreateDirectory(mainFolderPath + "/" + folderPath.Trim());

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

                if (foundFolder.InFavorites != null)
                {
                    for (int i = 0; i < foundFolder.InFavorites.Count; i++)
                    {
                        userRepository.DeleteFavoriteFolderById(foundFolder.InFavorites[i].Id);
                    }
                }

                counter = 0;
                int[] folderIds = new int[foundFolder.SubFolders.Count];
                foreach (var folder in foundFolder.SubFolders)
                {
                    folderIds[counter] = folder.Id;
                    counter++;
                    if (folder.InFavorites != null)
                    {
                        for (int i = 0; i < folder.InFavorites.Count; i++)
                        {
                            userRepository.DeleteFavoriteFolderById(folder.InFavorites[i].Id);
                        }
                    }
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
