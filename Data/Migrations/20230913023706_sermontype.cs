using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class sermontype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_Ministers_MinisterId",
                table: "Sermons");

            migrationBuilder.RenameColumn(
                name: "MinisterId",
                table: "Sermons",
                newName: "SermonTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Sermons_MinisterId",
                table: "Sermons",
                newName: "IX_Sermons_SermonTypeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePreached",
                table: "Sermons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Preacher",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SermonType",
                columns: table => new
                {
                    SermonTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SermonType", x => x.SermonTypeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_SermonType_SermonTypeId",
                table: "Sermons",
                column: "SermonTypeId",
                principalTable: "SermonType",
                principalColumn: "SermonTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_SermonType_SermonTypeId",
                table: "Sermons");

            migrationBuilder.DropTable(
                name: "SermonType");

            migrationBuilder.DropColumn(
                name: "DatePreached",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "Preacher",
                table: "Sermons");

            migrationBuilder.RenameColumn(
                name: "SermonTypeId",
                table: "Sermons",
                newName: "MinisterId");

            migrationBuilder.RenameIndex(
                name: "IX_Sermons_SermonTypeId",
                table: "Sermons",
                newName: "IX_Sermons_MinisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_Ministers_MinisterId",
                table: "Sermons",
                column: "MinisterId",
                principalTable: "Ministers",
                principalColumn: "MinisterId");
        }
    }
}
