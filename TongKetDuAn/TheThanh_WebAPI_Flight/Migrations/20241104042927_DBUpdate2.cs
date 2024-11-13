using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheThanh_WebAPI_Flight.Migrations
{
    public partial class DBUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_User_CreatorID",
                table: "RoleUser");

            migrationBuilder.DropIndex(
                name: "IX_RoleUser_CreatorID",
                table: "RoleUser");

            migrationBuilder.DropColumn(
                name: "CreatorID",
                table: "RoleUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorID",
                table: "RoleUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "RoleUser",
                keyColumns: new[] { "RoleID", "UserID" },
                keyValues: new object[] { 1, 1 },
                column: "CreatorID",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_CreatorID",
                table: "RoleUser",
                column: "CreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_User_CreatorID",
                table: "RoleUser",
                column: "CreatorID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
