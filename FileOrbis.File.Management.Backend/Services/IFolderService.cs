using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IFolderService
    {
        public FolderResponse GetById(int id);
        public FolderResponse GetByPath(string path);
        public FolderResponse Create(CreateFolderRequest createFolderRequest);
        public bool HasFile(int id);
        public bool DeleteById(int id);

    }

}
