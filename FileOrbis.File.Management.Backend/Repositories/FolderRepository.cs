using FileOrbis.File.Management.Backend.Configurations.Database;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly Database database;

        public FolderRepository(Database database)
        {
            this.database = database;
        }

    }

}
