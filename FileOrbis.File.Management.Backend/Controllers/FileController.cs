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

        [HttpPost("/files/create")]
        public FileResponse CreateFile([FromForm] CreateFileRequest createFileRequest)
        {
            return fileService.Create(createFileRequest);
        }

        [HttpPost("/files/add")]
        public FileResponse Add([FromForm] AddFileRequest addFileRequest)
        {
            return fileService.Add(addFileRequest);
        }

        [HttpDelete("/files/{id}")]
        public bool DeleteById(int id)
        {
            return fileService.DeleteById(id);
        }

    }

}
