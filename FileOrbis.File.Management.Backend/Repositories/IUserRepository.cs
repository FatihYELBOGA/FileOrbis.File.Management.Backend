using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public User GetById(int id);
        public Folder AddNewFolder(Folder folder);
        public Models.File AddFile(Models.File file);

    }

}
