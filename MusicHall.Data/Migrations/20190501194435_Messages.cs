using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MusicHall.Data.Migrations
{
    public partial class Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "email_account",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    host = table.Column<string>(nullable: true),
                    port = table.Column<int>(nullable: false),
                    user_name = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    enable_ssl = table.Column<bool>(nullable: false),
                    use_default_credentials = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "message_template",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    bcc_email_addresses = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true),
                    is_active = table.Column<bool>(nullable: false),
                    delay_before_send = table.Column<int>(nullable: true),
                    delay_period_id = table.Column<int>(nullable: false),
                    attached_download_id = table.Column<int>(nullable: false),
                    email_account_id = table.Column<int>(nullable: false),
                    DelayPeriod = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_template", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "queued_email",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    priority_id = table.Column<int>(nullable: false),
                    from = table.Column<string>(nullable: true),
                    from_name = table.Column<string>(nullable: true),
                    to = table.Column<string>(nullable: true),
                    to_name = table.Column<string>(nullable: true),
                    reply_to = table.Column<string>(nullable: true),
                    reply_to_name = table.Column<string>(nullable: true),
                    cc = table.Column<string>(nullable: true),
                    bcc = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true),
                    attachment_file_path = table.Column<string>(nullable: true),
                    attachment_file_name = table.Column<string>(nullable: true),
                    attachment_download_id = table.Column<int>(nullable: false),
                    created_on_utc = table.Column<DateTime>(nullable: false),
                    dont_send_before_date_utc = table.Column<DateTime>(nullable: true),
                    sent_tries = table.Column<int>(nullable: false),
                    sent_on_utc = table.Column<DateTime>(nullable: true),
                    email_account_id = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_queued_email", x => x.id);
                    table.ForeignKey(
                        name: "FK_queued_email_email_account_email_account_id",
                        column: x => x.email_account_id,
                        principalTable: "email_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_queued_email_email_account_id",
                table: "queued_email",
                column: "email_account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_template");

            migrationBuilder.DropTable(
                name: "queued_email");

            migrationBuilder.DropTable(
                name: "email_account");
        }
    }
}
