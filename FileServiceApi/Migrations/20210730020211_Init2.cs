using Microsoft.EntityFrameworkCore.Migrations;

namespace FileServiceApi.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilePathId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FilePathId",
                table: "Users",
                column: "FilePathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FilePaths_FilePathId",
                table: "Users",
                column: "FilePathId",
                principalTable: "FilePaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_FilePaths_FilePathId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FilePathId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FilePathId",
                table: "Users");
        }
    }
}
