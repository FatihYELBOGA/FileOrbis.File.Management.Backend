﻿using FileOrbis.File.Management.Backend.Configurations.Database;
using Microsoft.EntityFrameworkCore;

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
            return database.Files
                .Where(f => f.Id == id && f.Trashed == 0)
                .Include(f => f.Folder)
                .FirstOrDefault();
        }

        public Models.File CheckById(int id)
        {
            return database.Files
                .Where(f => f.Id == id)
                .Include(f => f.Folder)
                .FirstOrDefault();
        }

        public Models.File Create(Models.File newFile)
        {
            Models.File returnedFile = database.Files.Add(newFile).Entity;
            database.SaveChanges();

            return GetById(returnedFile.Id);
        }

        public Models.File Update(Models.File currentFile)
        {
            int fileId = database.Files.Update(currentFile).Entity.Id;
            database.SaveChanges();

            return GetById(fileId);
        }

        public void Delete(Models.File file)
        {
            database.Remove(file);
            database.SaveChanges();
        }

    }

}
