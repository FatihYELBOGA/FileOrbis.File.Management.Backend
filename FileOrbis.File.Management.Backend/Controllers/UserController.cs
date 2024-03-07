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

        [HttpPost("/users/add-folder/{id}")]
        public FolderResponse AddNewFolder([FromForm]FolderRequest folder, int id)
        {
            return userService.AddNewFolder(folder, id);
        }

        [HttpPost("/users/add-file/{id}")]
        public FileResponse AddFile([FromForm] FileRequest file,int id)
        {
            return userService.AddFile(file, id);   
        }

    }

}
