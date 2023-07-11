using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MusicHall.Data.Migrations
{
    public partial class addTypeAndNameToFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "publication_photos");

            migrationBuilder.DropColumn(
                name: "audio_file",
                table: "publications");

            migrationBuilder.CreateTable(
                name: "publication_files",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    guid = table.Column<Guid>(nullable: false),
                    file = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    publication_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publication_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_publication_files_publications_publication_id",
                        column: x => x.publication_id,
                        principalTable: "publications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_publication_files_publication_id",
                table: "publication_files",
                column: "publication_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "publication_files");

            migrationBuilder.AddColumn<string>(
                name: "audio_file",
                table: "publications",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "publication_photos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    file = table.Column<string>(type: "text", nullable: true),
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    publication_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publication_photos", x => x.id);
                    table.ForeignKey(
                        name: "FK_publication_photos_publications_publication_id",
                        column: x => x.publication_id,
                        principalTable: "publications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_publication_photos_publication_id",
                table: "publication_photos",
                column: "publication_id");
        }
    }
}
