using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheThanh_WebAPI_Flight.Migrations
{
    public partial class DBUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "$2a$11$qse3/LUTWo8.qJwnMLI/0eRn.9tIrHP5nn/FUnFfrI7PZSY.yEXRi", "TheThanh29" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "Thanh123@", "TheThanh2" });
        }
    }
}
