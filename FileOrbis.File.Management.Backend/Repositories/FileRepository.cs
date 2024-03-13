using FileOrbis.File.Management.Backend.Configurations.Database;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly Database database;

        public FileRepository(Database database)
        {
            this.database = database;
        }

        public Models.File GetById(int id)
        {
            return database.Files.Where(f => f.Id == id).FirstOrDefault();
        }

        public Models.File Create(Models.File newFile)
        {
            Models.File returnedFile = database.Files.Add(newFile).Entity;
            database.SaveChanges();

            return returnedFile;
        }

        public Models.File CheckById(int id)
        {
            return database.Files.Where(f => f.Id == id).FirstOrDefault();
        }

        public void Delete(Models.File file)
        {
            database.Remove(file);
            database.SaveChanges();
        }

    }

}
