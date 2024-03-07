using FileOrbis.File.Management.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FileOrbis.File.Management.Backend.Configurations.Database
{
    public class Database : DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Models.File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.User)
                .WithMany(u => u.Folders)
                .HasForeignKey(f => f.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<Models.File>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.SubFiles)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<Models.File>()
                .HasOne(f => f.User)
                .WithMany(u => u.Files)
                .HasForeignKey(f => f.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.RootFolder)
                .WithOne(f => f.RootFolderUser)
                .HasForeignKey<User>(u => u.RootFolderId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
        }

        private static User[] users =
        {
            new User
            {
                FirstName = "Fatih",
                LastName = "YELBOGA",
                Email = "fatihyelbogaaa@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("fatih123"))
            },
            new User
            {
                FirstName = "Osman",
                LastName = "ALTUNAY",
                Email = "osmanaltunay@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("osman123"))
            },
            new User
            {
                FirstName = "Berkay",
                LastName = "BAYRAK",
                Email = "berkaybayrak@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("berkay123"))
            }
        };

        public static void Seed(Database database)
        {
            if (database.Database.GetPendingMigrations().Count() == 0)
            {
                if (database.Users.Count() == 0)
                {
                    database.Users.AddRange(users);
                    database.SaveChanges();
                }
            }
        }

    }

}
