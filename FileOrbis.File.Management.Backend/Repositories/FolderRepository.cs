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

        public Folder GetById(int id)
        {
            return database.Folders
                .Where(f => f.Id == id && f.Trashed == 0)
                .Include(f => f.SubFolders)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.Folder)
                .FirstOrDefault();
        }

        public Folder CheckById(int id)
        {
            return database.Folders
                .Where(f => f.Id == id)
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
            Folder returnedFolder = database.Folders.Add(newFolder).Entity;
            database.SaveChanges();

            return returnedFolder;
        }

        public Folder Update(Folder currentFolder)
        {
            Folder returnedFolder = database.Folders.Update(currentFolder).Entity;
            database.SaveChanges();

            return returnedFolder;
        }

        public void Delete(Folder folder)
        {
             database.Folders.Remove(folder);
             database.SaveChanges();
        }

    }

}
