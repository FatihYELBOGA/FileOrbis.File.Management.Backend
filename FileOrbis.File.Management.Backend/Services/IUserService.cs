using FileOrbis.File.Management.Backend.DTO.Responses;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IUserService
    {
        public List<UserResponse> GetAll();
        public UserResponse GetById(int id);

    }

}
