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

        [HttpGet("/folders/check-exist/{id}")]
        public bool CheckExistById(int id)
        {
            return folderService.HasFile(id);
        }

        [HttpPost("/folders/create")]
        public FolderResponse Create([FromForm] CreateFolderRequest createFolderRequest)
        {
            return folderService.Create(createFolderRequest);
        }

        [HttpDelete("/folders/{id}")]
        public bool DeleteById(int id)
        {
            return folderService.DeleteById(id);
        }

    }

}
