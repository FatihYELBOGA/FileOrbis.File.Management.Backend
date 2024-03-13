using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public FolderResponse Folders { get; set; }

        public UserResponse() { }

        public UserResponse(User user) 
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            if(user.RootFolder != null)
            {
                Folders = new FolderResponse(user.RootFolder);
            }

        }

    }

}
