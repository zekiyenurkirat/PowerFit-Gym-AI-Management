using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class RandevuTanlosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorId",
                table: "Randevular",
                column: "AntrenorId",
                principalTable: "Antrenorler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular",
                column: "HizmetId",
                principalTable: "Hizmetler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular",
                column: "UyeId",
                principalTable: "Uyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Antrenorler_AntrenorId",
                table: "Randevular",
                column: "AntrenorId",
                principalTable: "Antrenorler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Hizmetler_HizmetId",
                table: "Randevular",
                column: "HizmetId",
                principalTable: "Hizmetler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Uyeler_UyeId",
                table: "Randevular",
                column: "UyeId",
                principalTable: "Uyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
