using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class singlelinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioDownloadLink",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "AudioEmbededLink",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "VideoDownloadLink",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Lyrics");

            migrationBuilder.RenameColumn(
                name: "VideoEmbededLink",
                table: "Sermons",
                newName: "SermonMediaLink");

            migrationBuilder.RenameColumn(
                name: "EmbededLink",
                table: "Musics",
                newName: "MusicMediaLink");

            migrationBuilder.RenameColumn(
                name: "EmbededLink",
                table: "Lyrics",
                newName: "LyricMediaLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SermonMediaLink",
                table: "Sermons",
                newName: "VideoEmbededLink");

            migrationBuilder.RenameColumn(
                name: "MusicMediaLink",
                table: "Musics",
                newName: "EmbededLink");

            migrationBuilder.RenameColumn(
                name: "LyricMediaLink",
                table: "Lyrics",
                newName: "EmbededLink");

            migrationBuilder.AddColumn<string>(
                name: "AudioDownloadLink",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AudioEmbededLink",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoDownloadLink",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Musics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Lyrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
