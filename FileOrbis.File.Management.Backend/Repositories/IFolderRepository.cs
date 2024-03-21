using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IFolderRepository
    {
        public Folder GetById(int id);
        public Folder CheckById(int id);
        public Folder GetByPath(string path);
        public Folder Create(Folder newFolder);
        public Folder Update(Folder currentFolder);
        public void Delete(Folder folder);

    }

}
