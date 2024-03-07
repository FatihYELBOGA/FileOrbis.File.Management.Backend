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

    }

}
