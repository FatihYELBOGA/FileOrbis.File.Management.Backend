namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IFileRepository
    {
        public List<Models.File> GetAll();
        public Models.File GetById(int id);
        public List<Models.File> GetAllTrashes(string username);
        public Models.File CheckById(int id);
        public Models.File Create(Models.File newFile);
        public Models.File Update(Models.File currentFile);
        public void Delete(Models.File file);

    }

}
