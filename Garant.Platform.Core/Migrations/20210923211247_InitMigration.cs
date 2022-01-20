using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garant.Platform.Core.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserPassword = table.Column<string>(type: "varchar(100)", nullable: true),
                    UserRole = table.Column<string>(type: "varchar(1)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Patronymic = table.Column<string>(type: "varchar(100)", nullable: true),
                    UserIcon = table.Column<string>(type: "text", nullable: true),
                    DateRegister = table.Column<DateTime>(type: "timestamp", nullable: false),
                    City = table.Column<string>(type: "varchar(200)", nullable: true),
                    RememberMe = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
