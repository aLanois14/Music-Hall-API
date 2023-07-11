using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicHall.Data.Migrations
{
    public partial class Initial_Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "administrators",
                columns: new[] { "id", "created_at", "disabled", "email", "first_name", "guid", "last_name", "login", "password_recovery_token", "password_recovery_token_date_generated", "updated_at" },
                values: new object[] { 1, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "admin@admin.fr", "admin", new Guid("00000000-0000-0000-0000-000000000000"), "admin", "admin@admin.fr", null, null, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "administrator_passwords",
                columns: new[] { "id", "administrator_id", "created_at", "password", "password_format", "password_format_id", "password_salt", "updated_at" },
                values: new object[] { 1, 1, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin", 0, 0, null, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "administrator_passwords",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "administrators",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
