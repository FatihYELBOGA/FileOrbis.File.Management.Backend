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

        [HttpGet("/files/{id}")] 
        public IActionResult GetById(int id)
        {
            return fileService.GetById(id); 
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
