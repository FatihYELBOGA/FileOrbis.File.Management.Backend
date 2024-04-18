using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.DTO.Requests;
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

        public Folder GetFolderWithParentFolder(int folderId)
        {
            return database.Folders
                .Where(f => f.Id == folderId)
                .Include(f => f.ParentFolder)
                .FirstOrDefault();
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
                    .ThenInclude(f => f.InFavorites)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.Folder)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.InFavorites)
                .FirstOrDefault();
        }
        public bool CheckNameExists(string name, int parentFolderId)
        {
            Folder folder = database.Folders
                .Where(f => f.Id == parentFolderId)
                .Include(f => f.SubFolders)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.Folder)
                .FirstOrDefault();

            if(folder.SubFolders != null)
            {
                foreach (var item in folder.SubFolders)
                {
                    if (item.Name.Equals(name.Trim()))
                    {
                        return true;
                    }
                }
            }
            if(folder.SubFiles != null)
            {
                foreach (var item in folder.SubFiles)
                {
                    if (item.Name.Equals(name.Trim()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<Folder> GetAllTrashes(string username)
        {
            return database.Folders
                .Where(f => f.Trashed == 1 && f.Path.StartsWith(username))
                .Include(f => f.ParentFolder)
                .ToList();
        }

        public Folder CheckById(int id)
        {
            return database.Folders
                .Where(f => f.Id == id)
                .Include(f => f.SubFolders)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.Folder)
                .Include(f => f.SubFiles)
                    .ThenInclude(f => f.InFavorites)
                .Include(f => f.InFavorites)
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
