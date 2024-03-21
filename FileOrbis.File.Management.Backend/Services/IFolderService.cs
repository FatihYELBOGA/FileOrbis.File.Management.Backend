using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IFolderService
    {
        public FolderResponse GetById(int id);
        public FolderResponse GetByPath(string path);
        public FolderResponse Create(CreateFolderRequest createFolderRequest);
        public FolderResponse Rename(int id, string name);
        public FolderResponse Trash(int id);
        public FolderResponse Restore(int id);
        public bool DeleteById(int id);

    }

}
