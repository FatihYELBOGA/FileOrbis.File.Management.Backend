namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IFileRepository
    {
        public Models.File GetById(int id);
        public Models.File CheckById(int id);
        public Models.File Create(Models.File newFile);
        public void Delete(Models.File file);

    }

}
