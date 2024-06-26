﻿using FileOrbis.File.Management.Backend.Models;

namespace FileOrbis.File.Management.Backend.Repositories
{
    public interface IFolderRepository
    {
        public Folder GetFolderWithParentFolder(int folderId);
        public List<Folder> GetAll(string filterPath);
        public List<Folder> GetAllStartsWith(string startsWith);
        public Folder GetById(int id);
        public bool CheckNameExists(string name, int parentFolderId);
        public List<Folder> GetAllTrashes(string username);
        public Folder CheckById(int id);
        public Folder GetByPath(string path);
        public Folder Create(Folder newFolder);
        public Folder Update(Folder currentFolder);
        public void Delete(Folder folder);

    }

}
