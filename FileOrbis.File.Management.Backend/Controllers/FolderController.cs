using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

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

        [HttpGet("/folders")]
        public FolderResponse GetById([FromQuery] int folderId, [FromQuery] int userId)
        {
            return folderService.GetById(folderId, userId);
        }

        [HttpGet("/folders/check-name-exists")]
        public string CheckExists([FromQuery] string name, [FromQuery] int parentFolderId)
        {
            return folderService.CheckNameExists(name, parentFolderId) ? "true" : "false";
        }

        [HttpGet("/folders/name/{id}")]
        public ActionResult<string> GetNameById(int id)
        {
            return Ok(folderService.GetNameById(id));
        }

        [HttpGet("/folders/zip/{id}")]
        public IActionResult GetZipFolderById(int id)
        {
            string folderPath = folderService.GetFolderPath(id);

            bool hasContent = Directory.GetFiles(folderPath).Length > 0 || Directory.GetDirectories(folderPath).Length > 0;
            if (!hasContent)
                return StatusCode(204, null);

            string zipFileName = folderPath.Split("/")[^1] + ".zip"; 
            try
            {
                using MemoryStream memoryStream = new();
                using (ZipArchive zipArchive = new(memoryStream, ZipArchiveMode.Create, true))
                {
                    folderService.AddFolderToZip(zipArchive, folderPath, "");
                }

                return File(memoryStream.ToArray(), "application/zip", zipFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/folders/trash/{username}")]
        public List<FolderResponse> GetAllTrashes(string username)
        {
            return folderService.GetAllTrashes(username);
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
