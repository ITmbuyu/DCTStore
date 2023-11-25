using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class thumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyricThumbnail",
                table: "Lyrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyricThumbnail",
                table: "Lyrics");
        }
    }
}
