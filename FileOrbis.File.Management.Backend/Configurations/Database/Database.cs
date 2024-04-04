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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<FavoriteFiles> FavoriteFiles { get; set; }
        public DbSet<FavoriteFolders> FavoriteFolders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<User>()
                .HasOne(u => u.RootFolder)
                .WithOne(f => f.RootFolderUser)
                .HasForeignKey<User>(u => u.RootFolderId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(rt => rt.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteFiles>()
                .HasOne(ff => ff.File)
                .WithMany(f => f.InFavorites)
                .HasForeignKey(f => f.FileId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteFiles>()
                .HasOne(ff => ff.User)
                .WithMany(u => u.FavoriteFiles)
                .HasForeignKey(f => f.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteFolders>()
                .HasOne(ff => ff.Folder)
                .WithMany(f => f.InFavorites)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

            modelBuilder.Entity<FavoriteFolders>()
                .HasOne(ff => ff.User)
                .WithMany(u => u.FavoriteFolders)
                .HasForeignKey(f => f.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
        }

        private static User[] users =
        {   
            new User
            {
                FirstName = "Korhan",
                LastName = "KONARAY",
                Email = "korhankonaray@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("korhan123")),
                Role = Enumerations.Role.USER,
                RootFolder = new Folder()
                {
                    Name = "korhankonaray@gmail.com",
                    CreatedDate = DateTime.Now,
                    ParentFolder = null,
                    Path = "korhankonaray@gmail.com",
                    Trashed = 0
                }
            },
            new User
            {
                FirstName = "Fatih",
                LastName = "YELBOGA",
                Email = "fatihyelboga@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("fatih123")),
                Role = Enumerations.Role.USER,
                RootFolder = new Folder()
                {
                    Name = "fatihyelboga@gmail.com",
                    CreatedDate = DateTime.Now,
                    ParentFolder = null,
                    Path = "fatihyelboga@gmail.com",
                    Trashed = 0
                }
            },
            new User
            {
                FirstName = "Osman",
                LastName = "ALTUNAY",
                Email = "osmanaltunay@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("osman123")),
                Role = Enumerations.Role.USER,
                RootFolder = new Folder()
                {
                    Name = "osmanaltunay@gmail.com",
                    CreatedDate = DateTime.Now,
                    ParentFolder = null,
                    Path = "osmanaltunay@gmail.com",
                    Trashed = 0
                }
            },
            new User
            {
                FirstName = "Enes",
                LastName = "DEMIREL",
                Email = "enesdemirel@gmail.com",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("berkay123")),
                Role = Enumerations.Role.USER,
                RootFolder = new Folder()
                {
                    Name = "enesdemirel@gmail.com",
                    CreatedDate = DateTime.Now,
                    ParentFolder = null,
                    Path = "enesdemirel@gmail.com",
                    Trashed = 0
                }
            }
        };

        public static void Seed(Database database, IConfiguration configuration)
        {
            if (database.Database.GetPendingMigrations().Count() == 0)
            {
                if (database.Users.Count() == 0)
                {
                    database.Users.AddRange(users);
                    database.SaveChanges();

                    foreach (var user in users)
                    {
                        Directory.CreateDirectory(configuration.GetSection("MainFolderPath").Value + "/" + user.RootFolder.Path);
                    }
                }
            }
        }

    }

}
