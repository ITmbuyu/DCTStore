using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class MediaType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicTypes");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Musics");

            migrationBuilder.AddColumn<int>(
                name: "MediaTypeId",
                table: "Sermons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaTypeId",
                table: "Musics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaTypes",
                columns: table => new
                {
                    MediaTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaTypes", x => x.MediaTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sermons_MediaTypeId",
                table: "Sermons",
                column: "MediaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_MediaTypeId",
                table: "Musics",
                column: "MediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musics_MediaTypes_MediaTypeId",
                table: "Musics",
                column: "MediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "MediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_MediaTypes_MediaTypeId",
                table: "Sermons",
                column: "MediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "MediaTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musics_MediaTypes_MediaTypeId",
                table: "Musics");

            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_MediaTypes_MediaTypeId",
                table: "Sermons");

            migrationBuilder.DropTable(
                name: "MediaTypes");

            migrationBuilder.DropIndex(
                name: "IX_Sermons_MediaTypeId",
                table: "Sermons");

            migrationBuilder.DropIndex(
                name: "IX_Musics_MediaTypeId",
                table: "Musics");

            migrationBuilder.DropColumn(
                name: "MediaTypeId",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "MediaTypeId",
                table: "Musics");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Musics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MusicTypes",
                columns: table => new
                {
                    MusicTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicTypes", x => x.MusicTypeId);
                });
        }
    }
}
