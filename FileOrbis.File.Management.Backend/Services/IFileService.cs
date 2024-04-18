using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IFileService
    {
        public List<StorageResponse> GetStorageDetails(string username);
        public Models.File GetById(int id);
        public string GetPathById(int id);
        public IActionResult GetDownloadFileById(int id);
        public List<FileResponse> GetAllRecents(string username);
        public string GetNameById(int recentId);
        public int? GetIdByPath(string path);
        public List<FileResponse> GetAllTrashes(string username);
        public FileResponse Add(AddFileRequest addFileRequests);
        public FileResponse UpdateRecentDate(int id);
        public FileResponse Rename(int id, string name);
        public FileResponse Trash(int id);
        public FileResponse Restore(int id);
        public bool DeleteById(int id);

    }

}
