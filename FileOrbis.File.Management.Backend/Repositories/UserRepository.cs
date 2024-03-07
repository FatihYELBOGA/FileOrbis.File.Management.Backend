using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.DTO.Responses;
using FileOrbis.File.Management.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Database database;

        public UserRepository(Database database)
        {
            this.database = database;
        }

        public List<User> GetAll()
        {
            return database.Users.ToList();
        }

        public User GetById(int id)
        {
            return database.Users
                .Where(u => u.Id == id)
                .Include(u =>u.RootFolder)
                    .ThenInclude(f => f.SubFolders)
                .Include(u => u.RootFolder)
                    .ThenInclude(f => f.SubFiles)
                .First();
        }

        public Folder AddNewFolder(Folder folder)
        {
            Folder newFolder = database.Folders.Add(folder).Entity;
            database.SaveChanges();
            
            return newFolder;
        }

        public Models.File AddFile(Models.File file)
        {
            Models.File newFile = database.Files.Add(file).Entity;
            database.SaveChanges();

            return newFile;
        }

    }

}
