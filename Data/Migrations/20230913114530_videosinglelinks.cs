using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class videosinglelinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SermonVideoLink",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SermonVideoLink",
                table: "Sermons");
        }
    }
}
