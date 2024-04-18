using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("/users")]
        public List<UserResponse> GetAll()
        {
            return userService.GetAll();
        }

        [HttpGet("/users/{id}")]
        public UserResponse GetById(int id)
        {
            return userService.GetById(id);
        }

        [HttpGet("/users/favorites/{userId}")]
        public UserResponse GetFavoritesById(int userId)
        {
            return userService.GetFavoritesById(userId);
        }

        [HttpGet("/users/check-file-in-trash")]
        public string CheckFileInTrash([FromQuery] int fileId, [FromQuery] string username)
        {
            return userService.CheckFileInTrash(fileId, username);
        }

        [HttpGet("/users/check-folder-in-trash")]
        public string CheckFolderInTrash([FromQuery] int folderId, [FromQuery] string username)
        {
            return userService.CheckFolderInTrash(folderId, username);
        }

        [HttpPost("/users/favorites/file")]
        public UserResponse AddFavoriteFile([FromForm] AddFavoriteFileRequest addFavoriteFile)
        {
            return userService.AddFavoriteFile(addFavoriteFile);
        }

        [HttpPost("/users/favorites/folder")]
        public UserResponse AddFavoriteFolder([FromForm] AddFavoriteFolderRequest addFavoriteFolder)
        {
            return userService.AddFavoriteFolder(addFavoriteFolder);
        }

        [HttpDelete("/users/favorites/file/{id}")]
        public bool DeleteFavoriteFilebyId(int id)
        {
            return userService.DeleteFavoriteFileById(id);
        }

        [HttpDelete("/users/favorites/folder/{id}")]
        public bool DeleteFavoriteFolderbyId(int id)
        {
            return userService.DeleteFavoriteFolderById(id);
        }

    }

}
