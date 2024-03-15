﻿namespace FileOrbis.File.Management.Backend.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Path { get; set; }
        public int? FolderId { get; set; }
        public Folder? Folder { get; set; }

    }

}
