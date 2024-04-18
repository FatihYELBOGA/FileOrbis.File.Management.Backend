using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet("/files/storage/{username}")]
        public List<StorageResponse> GetStorageDetails(string username)
        {
            return fileService.GetStorageDetails(username);
        }

        [HttpGet("/files/recent/{username}")]
        public List<FileResponse> GetAllRecents(string username)
        {
            return fileService.GetAllRecents(username);
        } 

        [HttpGet("/files/download/{id}")] 
        public IActionResult GetDownloadFileById(int id)
        {
            return fileService.GetDownloadFileById(id); 
        }

        [HttpGet("/files/stream/{id}")]
        public IActionResult GetStreamFileById(int id)
        {
            string[] splitNamesByDot = fileService.GetById(id).Name.Split(".");
            if(splitNamesByDot.Length > 0)
            {
                string extension = splitNamesByDot[splitNamesByDot.Length - 1];
                FileStream fileStream = new FileStream(fileService.GetPathById(id), FileMode.Open, FileAccess.Read);
                switch (extension)
                {
                    case "jpg":
                        return File(fileStream, "image/jpeg");
                    case "jpeg":
                        return File(fileStream, "image/jpeg");
                    case "png":
                        return File(fileStream, "image/png");
                    case "pdf":
                        return File(fileStream, "application/pdf");
                }
            }
            return null;
        }

        [HttpGet("/files/name/{id}")]
        public ActionResult<string> GetNameById(int id)
        {
            return Ok(fileService.GetNameById(id));
        }

        [HttpGet("/files/trash/{username}")]
        public List<FileResponse> GetAllTrashes(string username)
        {
            return fileService.GetAllTrashes(username);
        }

        [HttpPost("/files/add")]
        public FileResponse Add([FromForm] AddFileRequest addFileRequest)
        {
            return fileService.Add(addFileRequest);
        }

        [HttpPut("/files/recent/{id}")]
        public FileResponse UpdateRecentDate(int id)
        {
            return fileService.UpdateRecentDate(id);
        }

        [HttpPut("/files/rename/{id}")]
        public FileResponse Rename(int id, [FromQuery] string name)
        {
            return fileService.Rename(id, name);
        }

        [HttpPut("/files/trash/{id}")]
        public FileResponse Trash(int id)
        {
            return fileService.Trash(id);
        }

        [HttpPut("/files/restore/{id}")]
        public FileResponse Restore(int id)
        {
            return fileService.Restore(id);
        }

        [HttpDelete("/files/{id}")]
        public bool DeleteById(int id)
        {
            return fileService.DeleteById(id);
        }

    }

}
