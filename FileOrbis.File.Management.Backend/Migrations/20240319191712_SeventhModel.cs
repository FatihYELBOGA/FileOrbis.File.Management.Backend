using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.File.Management.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeventhModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Trashed",
                table: "Folders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Trashed",
                table: "Files",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trashed",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "Trashed",
                table: "Files");
        }
    }
}
