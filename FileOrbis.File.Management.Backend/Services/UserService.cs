using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using FileOrbis.File.Management.Backend.Repositories;

namespace FileOrbis.File.Management.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public List<UserResponse> GetAll()
        {
            List<UserResponse> userResponses = new List<UserResponse>();
            foreach (var user in userRepository.GetAll())
            {
                userResponses.Add(new UserResponse(user));
            }

            return userResponses;
        }

        public UserResponse GetById(int id)
        {
            return new UserResponse(userRepository.GetById(id));
        }

        public FolderResponse AddNewFolder(FolderRequest folder, int userId)
        {
            throw new NotImplementedException();
        }

        public FileResponse AddFile(FileRequest file, int userId)
        {
            string path = Path.Combine("C:\\file_management_system\\", file.Email, file.Path);
            file.Content.CopyTo(new FileStream(path, FileMode.Create));

            Models.File newFile = new Models.File()
            {
                Name = file.Content.FileName,
                Type = file.Content.ContentType,
                CreatedDate = DateTime.Now,
                Size = file.Content.Length,
                Path = path,
                FolderId = file.FolderId,
                UserId = userId
            };

            return new FileResponse(userRepository.AddFile(newFile));
        }

    }

}
