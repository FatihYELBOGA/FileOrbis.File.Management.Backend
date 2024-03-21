using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Repositories;

namespace FileOrbis.File.Management.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public List<UserResponse> GetAll()
        {
            List<UserResponse> userResponses = new List<UserResponse>();
            foreach (var user in userRepository.GetAll())
            {
                userResponses.Add(new UserResponse(user, configuration));
            }

            return userResponses;
        }

        public UserResponse GetById(int id)
        {
            return new UserResponse(userRepository.GetById(id), configuration);
        }

    }

}
