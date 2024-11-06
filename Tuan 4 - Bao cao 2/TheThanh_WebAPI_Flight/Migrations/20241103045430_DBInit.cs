using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheThanh_WebAPI_Flight.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointLoad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointUnload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleID, x.PermissionID });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "PermissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.TypeID);
                    table.ForeignKey(
                        name: "FK_DocumentType_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IsssueAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    CreatorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RoleID, x.UserID });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    FlightID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocID);
                    table.ForeignKey(
                        name: "FK_Document_DocumentType_TypeID",
                        column: x => x.TypeID,
                        principalTable: "DocumentType",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_Flight_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flight",
                        principalColumn: "FlightID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleDocumentType",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                    TypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDocumentType", x => new { x.RoleID, x.PermissionID, x.TypeID });
                    table.ForeignKey(
                        name: "FK_RoleDocumentType_DocumentType_TypeID",
                        column: x => x.TypeID,
                        principalTable: "DocumentType",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleDocumentType_RolePermission_RoleID_PermissionID",
                        columns: x => new { x.RoleID, x.PermissionID },
                        principalTable: "RolePermission",
                        principalColumns: new[] { "RoleID", "PermissionID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleDocument",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                    DocID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDocument", x => new { x.RoleID, x.PermissionID, x.DocID });
                    table.ForeignKey(
                        name: "FK_RoleDocument_Document_DocID",
                        column: x => x.DocID,
                        principalTable: "Document",
                        principalColumn: "DocID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleDocument_RolePermission_RoleID_PermissionID",
                        columns: x => new { x.RoleID, x.PermissionID },
                        principalTable: "RolePermission",
                        principalColumns: new[] { "RoleID", "PermissionID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionID", "Name" },
                values: new object[,]
                {
                    { 1, "Read Only" },
                    { 2, "Read and modify" },
                    { 3, "Full permission" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleID", "Name", "Note" },
                values: new object[] { 1, "Admin", "Dành cho quản trị viên" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Email", "Password", "Phone", "UserName" },
                values: new object[] { 1, "thanh@vietjetair.com", "Thanh123@", "0147852369", "TheThanh2" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[] { 3, 1 });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RoleID", "UserID", "CreatorID" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Document_FlightID",
                table: "Document",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_TypeID",
                table: "Document",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserID",
                table: "Document",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_UserID",
                table: "DocumentType",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_FlightNo",
                table: "Flight",
                column: "FlightNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDocument_DocID",
                table: "RoleDocument",
                column: "DocID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDocumentType_TypeID",
                table: "RoleDocumentType",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionID",
                table: "RolePermission",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_CreatorID",
                table: "RoleUser",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserID",
                table: "RoleUser",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RoleDocument");

            migrationBuilder.DropTable(
                name: "RoleDocumentType");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
