using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.File.Management.Backend.Migrations
{
    /// <inheritdoc />
    public partial class version_08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
