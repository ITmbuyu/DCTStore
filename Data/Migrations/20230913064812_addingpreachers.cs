using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DCTStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingpreachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_MininsterTypes_MininsterTypeId",
                table: "Sermons");

            migrationBuilder.DropForeignKey(
                name: "FK_SermonType_MininsterTypes_MininsterTypeId",
                table: "SermonType");

            migrationBuilder.DropColumn(
                name: "MininsterId",
                table: "SermonType");

            migrationBuilder.DropColumn(
                name: "MininsterId",
                table: "Sermons");

            migrationBuilder.RenameColumn(
                name: "MininsterTypeId",
                table: "SermonType",
                newName: "MinisterId");

            migrationBuilder.RenameIndex(
                name: "IX_SermonType_MininsterTypeId",
                table: "SermonType",
                newName: "IX_SermonType_MinisterId");

            migrationBuilder.RenameColumn(
                name: "MininsterTypeId",
                table: "Sermons",
                newName: "MinisterId");

            migrationBuilder.RenameIndex(
                name: "IX_Sermons_MininsterTypeId",
                table: "Sermons",
                newName: "IX_Sermons_MinisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_Ministers_MinisterId",
                table: "Sermons",
                column: "MinisterId",
                principalTable: "Ministers",
                principalColumn: "MinisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SermonType_Ministers_MinisterId",
                table: "SermonType",
                column: "MinisterId",
                principalTable: "Ministers",
                principalColumn: "MinisterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_Ministers_MinisterId",
                table: "Sermons");

            migrationBuilder.DropForeignKey(
                name: "FK_SermonType_Ministers_MinisterId",
                table: "SermonType");

            migrationBuilder.RenameColumn(
                name: "MinisterId",
                table: "SermonType",
                newName: "MininsterTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SermonType_MinisterId",
                table: "SermonType",
                newName: "IX_SermonType_MininsterTypeId");

            migrationBuilder.RenameColumn(
                name: "MinisterId",
                table: "Sermons",
                newName: "MininsterTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Sermons_MinisterId",
                table: "Sermons",
                newName: "IX_Sermons_MininsterTypeId");

            migrationBuilder.AddColumn<int>(
                name: "MininsterId",
                table: "SermonType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MininsterId",
                table: "Sermons",
                type: "int",
                nullable: true);

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
    }
}
