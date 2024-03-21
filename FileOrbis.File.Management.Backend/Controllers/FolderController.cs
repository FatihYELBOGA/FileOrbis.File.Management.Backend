using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService folderService;

        public FolderController(IFolderService folderService)
        {
            this.folderService = folderService;
        }

        [HttpGet("/folders/{id}")]
        public FolderResponse GetById(int id)
        {
            return folderService.GetById(id);
        }

        [HttpGet("/folders/path")]
        public FolderResponse GetByPath([FromQuery] string path)
        {
            return folderService.GetByPath(path);
        }

        [HttpPost("/folders/create")]
        public FolderResponse Create([FromForm] CreateFolderRequest createFolderRequest)
        {
            return folderService.Create(createFolderRequest);
        }

        [HttpPut("/folders/rename/{id}")]
        public FolderResponse Rename(int id, [FromQuery] string name)
        {
            return folderService.Rename(id, name);
        }

        [HttpPut("/folders/trash/{id}")]
        public FolderResponse Trash(int id)
        {
            return folderService.Trash(id);
        }

        [HttpPut("/folders/restore/{id}")]
        public FolderResponse Restore(int id)
        {
            return folderService.Restore(id);
        }

        [HttpDelete("/folders/{id}")]
        public bool DeleteById(int id)
        {
            return folderService.DeleteById(id);
        }

    }

}
