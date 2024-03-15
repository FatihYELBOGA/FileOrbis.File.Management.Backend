using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileOrbis.File.Management.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SixthModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Size",
                table: "Files",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
