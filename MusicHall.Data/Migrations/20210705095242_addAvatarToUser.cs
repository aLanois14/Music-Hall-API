using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicHall.Data.Migrations
{
    public partial class addAvatarToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_publications_bands_band_id",
                table: "publications");

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "users",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "band_id",
                table: "publications",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "publications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_publications_user_id",
                table: "publications",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_publications_bands_band_id",
                table: "publications",
                column: "band_id",
                principalTable: "bands",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_publications_users_user_id",
                table: "publications",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_publications_bands_band_id",
                table: "publications");

            migrationBuilder.DropForeignKey(
                name: "FK_publications_users_user_id",
                table: "publications");

            migrationBuilder.DropIndex(
                name: "IX_publications_user_id",
                table: "publications");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "publications");

            migrationBuilder.AlterColumn<int>(
                name: "band_id",
                table: "publications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_publications_bands_band_id",
                table: "publications",
                column: "band_id",
                principalTable: "bands",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
