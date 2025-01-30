using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserNotes.Data.Migrations
{
    /// <inheritdoc />
    public partial class NovataMigraciq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId1",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId1",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId",
                table: "Notes");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId1",
                table: "Notes",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId1",
                table: "Notes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
