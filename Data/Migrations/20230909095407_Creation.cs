using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations 
{
    /// <inheritdoc />
    public partial class Creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lyrics",
                columns: table => new
                {
                    LyricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmbededLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lyrics", x => x.LyricId);
                });

            migrationBuilder.CreateTable(
                name: "MininsterTypes",
                columns: table => new
                {
                    MininsterTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MininsterTypes", x => x.MininsterTypeId);
                });

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

            migrationBuilder.CreateTable(
                name: "Musics",
                columns: table => new
                {
                    MusicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmbededLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LyricId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.MusicId);
                    table.ForeignKey(
                        name: "FK_Musics_Lyrics_LyricId",
                        column: x => x.LyricId,
                        principalTable: "Lyrics",
                        principalColumn: "LyricId");
                });

            migrationBuilder.CreateTable(
                name: "Ministers",
                columns: table => new
                {
                    MinisterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MininsterTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ministers", x => x.MinisterId);
                    table.ForeignKey(
                        name: "FK_Ministers_MininsterTypes_MininsterTypeId",
                        column: x => x.MininsterTypeId,
                        principalTable: "MininsterTypes",
                        principalColumn: "MininsterTypeId");
                });

            migrationBuilder.CreateTable(
                name: "Sermons",
                columns: table => new
                {
                    SermonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioEmbededLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioDownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoEmbededLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoDownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinisterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sermons", x => x.SermonId);
                    table.ForeignKey(
                        name: "FK_Sermons_Ministers_MinisterId",
                        column: x => x.MinisterId,
                        principalTable: "Ministers",
                        principalColumn: "MinisterId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ministers_MininsterTypeId",
                table: "Ministers",
                column: "MininsterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_LyricId",
                table: "Musics",
                column: "LyricId");

            migrationBuilder.CreateIndex(
                name: "IX_Sermons_MinisterId",
                table: "Sermons",
                column: "MinisterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musics");

            migrationBuilder.DropTable(
                name: "MusicTypes");

            migrationBuilder.DropTable(
                name: "Sermons");

            migrationBuilder.DropTable(
                name: "Lyrics");

            migrationBuilder.DropTable(
                name: "Ministers");

            migrationBuilder.DropTable(
                name: "MininsterTypes");
        }
    }
}
