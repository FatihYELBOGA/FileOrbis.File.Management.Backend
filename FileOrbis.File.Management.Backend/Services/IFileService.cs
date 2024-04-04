using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IFileService
    {
        public IActionResult GetById(int id);
        public string GetNameById(int id);
        public int? GetIdByPath(string path);
        public List<FileResponse> GetAllTrashes(string username);
        public FileResponse Add(AddFileRequest addFileRequests);
        public FileResponse Rename(int id, string name);
        public FileResponse Trash(int id);
        public FileResponse Restore(int id);
        public bool DeleteById(int id);

    }

}
