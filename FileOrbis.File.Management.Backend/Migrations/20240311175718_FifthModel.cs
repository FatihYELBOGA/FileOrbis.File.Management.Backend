using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.File.Management.Backend.Migrations
{
    /// <inheritdoc />
    public partial class FifthModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Folders");
        }
    }
}
