using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly Database database;

        public FileRepository(Database database)
        {
            this.database = database;
        }

        public Models.File GetFileWithParentFolder(int fileId)
        {
            return database.Files
                .Where(f => f.Id == fileId)
                .Include(f => f.Folder)
                    .ThenInclude(f => f.ParentFolder)
                .FirstOrDefault();
        }

        public List<Models.File> GetAll()
        {
            return database.Files
                .Where(f => f.Trashed == 0)
                .Include(f => f.Folder)
                .ToList();
        }
        public List<Models.File> GetAllByUsername(string username)
        {
            return database.Files
                .Include(f => f.Folder)
                    .ThenInclude(f => f.ParentFolder)
                .Where(f => f.Folder.Path.StartsWith(username))
                .ToList();
        }

        public List<Models.File> GetAllRecents(string username)
        {
            return database.Files
                .Where(f =>f.RecentDate != null)
                .Include(f => f.Folder)
                .Where(f => f.Folder.Path.StartsWith(username))
                .ToList();
        }

        public Models.File GetById(int id)
        {
            return database.Files
                .Where(f => f.Id == id)
                .Include(f => f.Folder)
                .FirstOrDefault();
        }

        public List<Models.File> GetAllTrashes(string username) 
        {
            return database.Files
                .Where(f => f.Trashed == 1)
                .Include(f => f.Folder)
                    .ThenInclude(f => f.ParentFolder)
                .Where(f => f.Folder.Path.StartsWith(username))
                .ToList();
        }

        public Models.File CheckById(int id)
        {
            return database.Files
                .Where(f => f.Id == id)
                .Include(f => f.Folder)
                .Include(f => f.InFavorites)
                .FirstOrDefault();
        }

        public Models.File Create(Models.File newFile)
        {
            Models.File returnedFile = database.Files.Add(newFile).Entity;
            database.SaveChanges();

            return GetById(returnedFile.Id);
        }

        public Models.File Update(Models.File currentFile)
        {
            Models.File returnedFile = database.Files.Update(currentFile).Entity;
            database.SaveChanges();

            return GetById(returnedFile.Id);
        }

        public void Delete(Models.File file)
        {
            database.Remove(file);
            database.SaveChanges();
        }

    }

}
