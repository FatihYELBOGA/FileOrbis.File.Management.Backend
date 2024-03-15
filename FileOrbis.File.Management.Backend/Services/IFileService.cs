using FileOrbis.File.Management.Backend.DTO.Requests;
using FileOrbis.File.Management.Backend.DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbis.File.Management.Backend.Services
{
    public interface IFileService
    {
        public IActionResult GetById(int id);
        public FileResponse Add(AddFileRequest addFileRequests);
        public bool DeleteById(int id);

    }

}
