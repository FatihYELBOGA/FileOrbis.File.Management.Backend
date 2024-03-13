using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IFolderRepository
    {
        public Folder CheckById(int id); 
        public Folder GetById(int id);
        public Folder Create(Folder newFolder);
        public void Delete(Folder folder);

    }

}
