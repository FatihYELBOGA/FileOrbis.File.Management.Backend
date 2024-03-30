using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly Database database;

        public FolderRepository(Database database)
        {
            this.database = database;
        }

        public List<Folder> GetAll(string filterPath)
        {
            return database.Folders
                .Where(f => f.Trashed == 0 && f.Path.StartsWith(filterPath))
                .ToList();
        }

        public List<Folder> GetAllStartsWith(string startsWith)
        {
            return database.Folders
                .Where(f => f.Path.StartsWith(startsWith))
                .ToList();
        }

        public Folder GetById(int id)
        {
            return database.Folders
                .Where(f => f.Id == id && f.Trashed == 0)
                .Include(f => f.SubFolders)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.Folder)
                .FirstOrDefault();
        }

        public List<Folder> GetAllTrashes()
        {
            return database.Folders
                .Where(f => f.Trashed == 1)
                .ToList();
        }

        public Folder CheckById(int id)
        {
            return database.Folders
                .Where(f => f.Id == id)
                .Include(f => f.SubFolders)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.Folder)
                .FirstOrDefault();
        }

        public Folder GetByPath(string path)
        {
            return database.Folders
                .Where(f => f.Path == path)
                .FirstOrDefault();
        }

        public Folder Create(Folder newFolder)
        {
            Folder returnedFolder = database.Folders.Update(newFolder).Entity;
            database.SaveChanges();

            return GetById(returnedFolder.Id);
        }

        public Folder Update(Folder currentFolder)
        {
            Folder returnedFolder = database.Folders.Update(currentFolder).Entity;
            database.SaveChanges();

            return CheckById(returnedFolder.Id);
        }

        public void Delete(Folder folder)
        {
             database.Folders.Remove(folder);
             database.SaveChanges();
        }

    }

}
