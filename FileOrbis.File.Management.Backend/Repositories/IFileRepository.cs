namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IFileRepository
    {
        public Models.File GetFileWithParentFolder(int fileId);
        public List<Models.File> GetAll();
        public List<Models.File> GetAllByUsername(string username);
        public List<Models.File> GetAllRecents(string username);
        public Models.File GetById(int id);
        public List<Models.File> GetAllTrashes(string username);
        public Models.File CheckById(int id);
        public Models.File Create(Models.File newFile);
        public Models.File Update(Models.File currentFile);
        public void Delete(Models.File file);

    }

}
