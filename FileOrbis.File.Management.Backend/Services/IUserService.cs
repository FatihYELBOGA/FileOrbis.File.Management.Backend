using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IUserService
    {
        public List<UserResponse> GetAll();
        public UserResponse GetById(int id);
        public FolderResponse AddNewFolder(FolderRequest folder, int userId);
        public FileResponse AddFile(FileRequest file, int userId);

    }

}
