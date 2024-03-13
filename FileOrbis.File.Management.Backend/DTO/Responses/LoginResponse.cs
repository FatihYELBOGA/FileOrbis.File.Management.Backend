using FileOrbis.File.Management.Backend.Enumerations;

namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? JWTToken { get; set; }
        public string? RefreshToken { get; set; }
        public Role? Role { get; set; }
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }
        public int? RootFolderId { get; set; }

    }

}
