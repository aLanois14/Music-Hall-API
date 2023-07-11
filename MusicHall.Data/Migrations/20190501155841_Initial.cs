using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MusicHall.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrators",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    guid = table.Column<Guid>(nullable: false),
                    login = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    first_name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    disabled = table.Column<bool>(nullable: false),
                    password_recovery_token = table.Column<string>(nullable: true),
                    password_recovery_token_date_generated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "civilities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_civilities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "administrator_passwords",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    administrator_id = table.Column<int>(nullable: false),
                    password = table.Column<string>(nullable: true),
                    password_format_id = table.Column<int>(nullable: false),
                    password_salt = table.Column<string>(nullable: true),
                    password_format = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrator_passwords", x => x.id);
                    table.ForeignKey(
                        name: "FK_administrator_passwords_administrators_administrator_id",
                        column: x => x.administrator_id,
                        principalTable: "administrators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    guid = table.Column<Guid>(nullable: false),
                    civility_id = table.Column<int>(nullable: false),
                    last_name = table.Column<string>(nullable: true),
                    first_name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    mobile = table.Column<string>(nullable: true),
                    confirmed = table.Column<bool>(nullable: false),
                    disabled = table.Column<bool>(nullable: false),
                    password_recovery_token = table.Column<string>(nullable: true),
                    password_recovery_token_date_generated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_civilities_civility_id",
                        column: x => x.civility_id,
                        principalTable: "civilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_passwords",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    password = table.Column<string>(nullable: true),
                    password_format_id = table.Column<int>(nullable: false),
                    password_salt = table.Column<string>(nullable: true),
                    password_format = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_passwords", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_passwords_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_administrator_passwords_administrator_id",
                table: "administrator_passwords",
                column: "administrator_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_passwords_user_id",
                table: "user_passwords",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_civility_id",
                table: "users",
                column: "civility_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrator_passwords");

            migrationBuilder.DropTable(
                name: "user_passwords");

            migrationBuilder.DropTable(
                name: "administrators");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "civilities");
        }
    }
}
