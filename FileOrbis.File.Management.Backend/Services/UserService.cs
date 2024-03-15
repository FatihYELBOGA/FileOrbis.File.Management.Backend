using FileOrbis.File.Management.Backend.DTO.Responses;
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

    }

}
