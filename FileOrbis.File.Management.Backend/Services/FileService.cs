using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository fileRepository;
        private readonly IConfiguration configuration;

        public FileService(IFileRepository fileRepository, IConfiguration configuration)
        {
            this.fileRepository = fileRepository;
            this.configuration = configuration;
        }

        public IActionResult GetById(int id)
        {
            Models.File foundFile = fileRepository.GetById(id);
            if (foundFile != null)
            {
                FileStream fileStream = new FileStream(foundFile.Path, FileMode.Open, FileAccess.Read);
                return new FileStreamResult(fileStream, "application/octet-stream")
                {
                    FileDownloadName = Path.GetFileName(foundFile.Path)
                };
            }

            return null;
        }

        public FileResponse Create(CreateFileRequest createFileRequest)
        {
            string path = Path.Combine(configuration.GetSection("MainFolderPath").Value, createFileRequest.Path);

            long length = 0;
            using (FileStream fs = System.IO.File.Create(path))
            {
                length = fs.Length;
                Console.WriteLine("file created succesfully!");
            }

            Models.File newFile = new Models.File()
            {
                Type = Path.GetExtension(path),
                CreatedDate = DateTime.Now,
                Size = length,
                Path = path,
                FolderId = createFileRequest.ParentFolderId
            };

            return new FileResponse(fileRepository.Create(newFile));
        }

        public FileResponse Add(AddFileRequest addFileRequest)
        {
            string path = Path.Combine(configuration.GetSection("MainFolderPath").Value, addFileRequest.Path, addFileRequest.Content.FileName);
            addFileRequest.Content.CopyTo(new FileStream(path, FileMode.Create));

            Models.File newFile = new Models.File()
            {
                Name = addFileRequest.Content.FileName,
                Type = addFileRequest.Content.ContentType,
                CreatedDate = DateTime.Now,
                Size = addFileRequest.Content.Length,
                Path = path,
                FolderId = addFileRequest.FolderId
            };

            return new FileResponse(fileRepository.Create(newFile));
        }

        public bool DeleteById(int id)
        {
            Models.File foundFile = fileRepository.CheckById(id);
            if (foundFile != null)
            {
                fileRepository.Delete(foundFile);
                return true;
            }

            return false;
        }

    }

}
