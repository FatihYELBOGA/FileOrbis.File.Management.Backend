using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.File.Management.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedFavoritesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteFolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteFolders_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteFolders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteFiles_FileId",
                table: "FavoriteFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteFiles_UserId",
                table: "FavoriteFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteFolders_FolderId",
                table: "FavoriteFolders",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteFolders_UserId",
                table: "FavoriteFolders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteFiles");

            migrationBuilder.DropTable(
                name: "FavoriteFolders");
        }
    }
}
