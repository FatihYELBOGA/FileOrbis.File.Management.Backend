﻿using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository fileRepository;
        private IFolderRepository folderRepository; 
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly string mainFolderPath = "";

        public FileService(IFileRepository fileRepository, IFolderRepository folderRepository,  IConfiguration configuration, IUserRepository userRepository)
        {
            this.fileRepository = fileRepository;
            this.folderRepository = folderRepository;
            this.configuration = configuration;
            mainFolderPath = configuration.GetSection("MainFolderPath").Value;
            this.userRepository = userRepository;
        }

        public IActionResult GetById(int id)
        {
            Models.File foundFile = fileRepository.GetById(id);
            if (foundFile != null)
            {
                FileStream fileStream = new FileStream(mainFolderPath + '/' + foundFile.Folder.Path + '/' + foundFile.Name, FileMode.Open, FileAccess.Read);
                return new FileStreamResult(fileStream, "application/octet-stream")
                {
                    FileDownloadName = Path.GetFileName(mainFolderPath + '/' + foundFile.Folder.Path + '/' + foundFile.Name)
                };
            }

            return null;
        }

        public string GetNameById(int id)
        {
            return fileRepository.GetById(id).Name;
        }

        public int? GetIdByPath(string path)
        {
            foreach (var file in fileRepository.GetAll())
            {
                string path_1 = NormalizePath(path);
                string path_2 = NormalizePath(Path.Combine(mainFolderPath, file.Folder.Path, file.Name));

                if (path_1.Equals(path_2))
                {
                    return file.Id;
                }
            }
            return null;
        }

        static string NormalizePath(string path)
        {
            return path.Replace("\\", "/");
        }

        public List<FileResponse> GetAllTrashes(string username)
        {
            List<FileResponse> files = new List<FileResponse>();
            foreach (Models.File file in fileRepository.GetAllTrashes(username))
            {
                files.Add(new FileResponse(file, configuration));
            }

            return files;
        }

        public FileResponse Add(AddFileRequest addFileRequest)
        {
            string path = mainFolderPath + "/" + folderRepository.GetById(addFileRequest.FolderId).Path + "/" + addFileRequest.Content.FileName;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                addFileRequest.Content.CopyTo(stream);
            }

            Models.File newFile = new Models.File()
            {
                Name = addFileRequest.Content.FileName,
                Type = addFileRequest.Content.ContentType,
                CreatedDate = DateTime.Now,
                FolderId = addFileRequest.FolderId,
                Trashed = 0
            };

            return new FileResponse(fileRepository.Create(newFile), configuration);
        }

        public FileResponse Rename(int id, string name)
        {
            Models.File foundFile = fileRepository.GetById(id);
            if (foundFile != null)
            {
                string extension = "";
                string[] names = foundFile.Name.Split(".");
                if(names.Length > 1)
                {
                    extension = "." + names[names.Length - 1];
                }

                string newPath = "";
                string[] paths = (mainFolderPath + '/' + foundFile.Folder.Path + '/' + foundFile.Name).Split("/");
                for (int i = 0; i < paths.Length; i++)
                {
                    if (i != paths.Length - 1)
                        newPath = newPath + paths[i] + "/";
                    else
                        newPath = newPath + name + extension;
                }

                System.IO.File.Move(mainFolderPath + '/' + foundFile.Folder.Path + '/' + foundFile.Name, newPath);

                foundFile.Name = name + extension;
                return new FileResponse(fileRepository.Update(foundFile), configuration);
            }
            else
                throw new FileNotFoundException();
        }

        public FileResponse Trash(int id)
        {
            Models.File foundFile = fileRepository.GetById(id);
            if (foundFile != null)
            {
                foundFile.Trashed = 1;
                foundFile.DeletedDate = DateTime.Now;
                return new FileResponse(fileRepository.Update(foundFile), configuration);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public FileResponse Restore(int id)
        {
            Models.File foundFile = fileRepository.CheckById(id);
            if (foundFile != null)
            {
                foundFile.Trashed = 0;
                return new FileResponse(fileRepository.Update(foundFile), configuration);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public bool DeleteById(int id)
        {
            Models.File foundFile = fileRepository.CheckById(id);
            if (foundFile != null)
            {
                if (foundFile.InFavorites != null)
                {
                    for (int i = 0; i < foundFile.InFavorites.Count; i++)
                    {
                        userRepository.DeleteFavoriteFileById(foundFile.InFavorites[i].Id);
                    }
                }

                fileRepository.Delete(foundFile);

                using (var fileStream = new FileStream(mainFolderPath + '/' + foundFile.Folder.Path + '/' + foundFile.Name, FileMode.Open))
                {
                    fileStream.Close();
                    System.IO.File.Delete(mainFolderPath + '/' + foundFile.Folder.Path + '/' + foundFile.Name);
                }
                return true;
            }

            return false;
        }

    }

}
