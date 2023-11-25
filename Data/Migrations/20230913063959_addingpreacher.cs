using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingpreacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preacher",
                table: "Sermons");

            migrationBuilder.AddColumn<int>(
                name: "MininsterId",
                table: "SermonType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MininsterTypeId",
                table: "SermonType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MininsterId",
                table: "Sermons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MininsterTypeId",
                table: "Sermons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SermonType_MininsterTypeId",
                table: "SermonType",
                column: "MininsterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sermons_MininsterTypeId",
                table: "Sermons",
                column: "MininsterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_MininsterTypes_MininsterTypeId",
                table: "Sermons",
                column: "MininsterTypeId",
                principalTable: "MininsterTypes",
                principalColumn: "MininsterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SermonType_MininsterTypes_MininsterTypeId",
                table: "SermonType",
                column: "MininsterTypeId",
                principalTable: "MininsterTypes",
                principalColumn: "MininsterTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_MininsterTypes_MininsterTypeId",
                table: "Sermons");

            migrationBuilder.DropForeignKey(
                name: "FK_SermonType_MininsterTypes_MininsterTypeId",
                table: "SermonType");

            migrationBuilder.DropIndex(
                name: "IX_SermonType_MininsterTypeId",
                table: "SermonType");

            migrationBuilder.DropIndex(
                name: "IX_Sermons_MininsterTypeId",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "MininsterId",
                table: "SermonType");

            migrationBuilder.DropColumn(
                name: "MininsterTypeId",
                table: "SermonType");

            migrationBuilder.DropColumn(
                name: "MininsterId",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "MininsterTypeId",
                table: "Sermons");

            migrationBuilder.AddColumn<string>(
                name: "Preacher",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
