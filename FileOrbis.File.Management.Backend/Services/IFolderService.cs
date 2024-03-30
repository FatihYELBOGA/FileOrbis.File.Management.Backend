using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using System.IO.Compression;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IFolderService
    {
        public void AddFolderToZip(ZipArchive zipArchive, string folderPath, string parentFolder);
        public FolderResponse GetById(int id);
        public string GetNameById(int id);
        public string GetFolderPath(int id);
        public List<FolderResponse> GetAllTrashes();
        public FolderResponse GetByPath(string path);
        public FolderResponse Create(CreateFolderRequest createFolderRequest);
        public FolderResponse Rename(int id, string name);
        public FolderResponse Trash(int id);
        public FolderResponse Restore(int id);
        public bool DeleteById(int id);

    }

}
