using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class lyrictype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LyricTypeId",
                table: "Lyrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LyricTypes",
                columns: table => new
                {
                    LyricTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LyricTypes", x => x.LyricTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lyrics_LyricTypeId",
                table: "Lyrics",
                column: "LyricTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lyrics_LyricTypes_LyricTypeId",
                table: "Lyrics",
                column: "LyricTypeId",
                principalTable: "LyricTypes",
                principalColumn: "LyricTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lyrics_LyricTypes_LyricTypeId",
                table: "Lyrics");

            migrationBuilder.DropTable(
                name: "LyricTypes");

            migrationBuilder.DropIndex(
                name: "IX_Lyrics_LyricTypeId",
                table: "Lyrics");

            migrationBuilder.DropColumn(
                name: "LyricTypeId",
                table: "Lyrics");
        }
    }
}
