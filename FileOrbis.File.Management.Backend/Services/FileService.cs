using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
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
            this.userRepository = userRepository;
            mainFolderPath = configuration.GetSection("MainFolderPath").Value;
        }

        public List<StorageResponse> GetStorageDetails(string username)
        {
            List<StorageResponse> storageDetails = new List<StorageResponse>();
            int jpgNumbers = 0, pngNumbers = 0, pdfNumbers = 0, docxNumbers = 0, xlsxNumbers = 0, pptxNumbers = 0, mp3Numbers = 0, mp4Numbers = 0, txtNumbers = 0, zipNumbers = 0, otherNumbers = 0;
            double jpgSize = 0.0, pngSize = 0.0, pdfSize = 0.0, docxSize = 0.0, xlsxSize = 0.0, pptxSize = 0.0, mp3Size = 0.0, mp4Size = 0.0, txtSize = 0.0, zipSize = 0.0, otherSize = 0.0;
            foreach (var file in fileRepository.GetAllByUsername(username))
            {
                string[] splitNamesByDot = file.Name.Split(".");
                if (splitNamesByDot.Length > 0)
                {
                    string extension = splitNamesByDot[splitNamesByDot.Length - 1];
                    FileInfo fileInfo = new FileInfo(mainFolderPath + "/" + file.Folder.Path + "/" + file.Name);
                    switch (extension)
                    {
                        case "jpg":
                            jpgNumbers++;
                            jpgSize += (double) fileInfo.Length;
                            break;
                        case "jpeg":
                            jpgNumbers++;
                            jpgSize += (double)fileInfo.Length;
                            break;
                        case "png":
                            pngNumbers++;
                            pngSize += (double)fileInfo.Length;
                            break;
                        case "pdf":
                            pdfNumbers++;
                            pdfSize += (double)fileInfo.Length;
                            break;
                        case "docx":
                            docxNumbers++;
                            docxSize += (double)fileInfo.Length;
                            break;
                        case "xlsx":
                            xlsxNumbers++;
                            xlsxSize += (double)fileInfo.Length;
                            break;
                        case "pptx":
                            pptxNumbers++;
                            pptxSize += (double)fileInfo.Length;
                            break;
                        case "mp3":
                            mp3Numbers++;
                            mp3Size += (double)fileInfo.Length;
                            break;
                        case "mp4":
                            mp4Numbers++;
                            mp4Size += (double)fileInfo.Length;
                            break;
                        case "txt":
                            txtNumbers++;
                            txtSize += (double)fileInfo.Length;
                            break;
                        case "zip":
                            zipNumbers++;
                            zipSize += (double)fileInfo.Length;
                            break;
                        default:
                            otherNumbers++;
                            otherSize += (double)fileInfo.Length;
                            break;
                    }
                }
            }

            AddItemToStorageList(storageDetails, ".jpeg / .jpg", jpgNumbers, jpgSize, "JPEG image files");
            AddItemToStorageList(storageDetails, ".png", pngNumbers, pngSize, "Portable Network Graphics format, uncompressed image files");
            AddItemToStorageList(storageDetails, ".pdf", pdfNumbers, pdfSize, "Portable Document Format, a file format for capturing and sending electronic documents in exactly the intended format");
            AddItemToStorageList(storageDetails, ".docx", docxNumbers, docxSize, "File extension used for Microsoft Word documents");
            AddItemToStorageList(storageDetails, ".xlsx", xlsxNumbers, xlsxSize, "File extension used for Microsoft Excel spreadsheet files");
            AddItemToStorageList(storageDetails, ".pptx", pptxNumbers, pptxSize, " File extension used for Microsoft PowerPoint presentation files");
            AddItemToStorageList(storageDetails, ".mp3", mp3Numbers, mp3Size, "MPEG-3 Audio Layer, compressed audio files");
            AddItemToStorageList(storageDetails, ".mp4", mp4Numbers, mp4Size, "MPEG-4 Video File, compressed video and audio files");
            AddItemToStorageList(storageDetails, ".txt", txtNumbers, txtSize, "Text files, files where the content is stored in a plain text format");
            AddItemToStorageList(storageDetails, ".zip", zipNumbers, zipSize, "Compressed file archive");
            AddItemToStorageList(storageDetails, "others", otherNumbers, otherSize, "Other file extensions");
            
            int totalNumbers = jpgNumbers + pngNumbers + pdfNumbers + docxNumbers + xlsxNumbers + pptxNumbers + mp3Numbers + mp4Numbers + txtNumbers + zipNumbers + otherNumbers;
            double totalSize = jpgSize + pngSize + pdfSize + docxSize + xlsxSize + pptxSize + mp3Size + mp4Size + txtSize + zipSize + otherSize;

            double kb = (double)totalSize / 1024;
            string sizeStr = kb.ToString("0") + " KB";
            double mb;
            if (kb >= 1024)
            {
                mb = (double)kb / 1024;
                sizeStr = mb.ToString("0.00") + " MB";
            }

            storageDetails.Add(new StorageResponse()
            {
                Type = "",
                TotalNumber = 0,
                Size = "",
                Description = "",
                TotalNumbers = totalNumbers,
                TotalSize = sizeStr
            });

            return storageDetails;
        }

        private void AddItemToStorageList(List<StorageResponse> storage, string fileType, int totalNumber, double size, string description)
        {
            if (totalNumber > 0)
            {
                double kb =(double) size / 1024;
                string Size = kb.ToString("0") + " KB";
                double mb;
                if (kb >= 1024)
                {
                    mb = (double) kb / 1024;
                    Size = mb.ToString("0.00") + " MB";
                }
                storage.Add(new StorageResponse()
                {
                    Type = fileType,
                    TotalNumber = totalNumber,
                    Size = Size,
                    Description = description
                });
            }
        }

        public Models.File GetById(int id)
        {
            return fileRepository.GetById(id);
        }

        public string GetPathById(int id)
        {
            Models.File file = fileRepository.GetById(id);
            string folderPath = file.Folder.Path;
            string combinedPath = mainFolderPath + "/" + folderPath + "/" + file.Name;
            return combinedPath;
        }

        public IActionResult GetDownloadFileById(int id)
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

        public List<FileResponse> GetAllRecents(string username)
        {
            List<Models.File> files = fileRepository.GetAllRecents(username)
                                                          .OrderByDescending(f => f.RecentDate)
                                                          .ToList();

            List<FileResponse> fileResponses = new List<FileResponse>();
            foreach (var file in files)
            {
                fileResponses.Add(new FileResponse(file, configuration));
            }

            return fileResponses;
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
            User user = userRepository.GetFavoritesByUsername(username);
            List<FileResponse> files = new List<FileResponse>();
            foreach (Models.File file in fileRepository.GetAllTrashes(username))
            {
                foreach (var favFile in user.FavoriteFiles)
                {
                    if(file.Id == favFile.File.Id)
                    {
                        file.Starred = true;
                    }
                }
                files.Add(new FileResponse(file, configuration));
            }

            return files;
        }

        public FileResponse Add(AddFileRequest addFileRequest)
        {
            string path = mainFolderPath + "/" + folderRepository.GetById(addFileRequest.FolderId).Path + "/" + addFileRequest.Content.FileName.Trim();

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

        public FileResponse UpdateRecentDate(int id)
        {
            Models.File foundFile = fileRepository.GetById(id);
            if (foundFile != null)
            {
                foundFile.RecentDate = DateTime.Now;
                return new FileResponse(fileRepository.Update(foundFile), configuration);
            }

            return null;
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
