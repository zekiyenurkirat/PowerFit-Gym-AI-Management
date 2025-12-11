using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class TestVerileriEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SporSalonları",
                columns: new[] { "Id", "AcilisSaati", "Ad", "Adres", "KapanisSaati", "Telefon" },
                values: new object[] { 1, new TimeSpan(0, 7, 0, 0, 0), "Fitlife Spor Salonu", "İstanbul /Kadıköy", new TimeSpan(0, 22, 0, 0, 0), "530 000 00 00" });

            migrationBuilder.InsertData(
                table: "Antrenorler",
                columns: new[] { "Id", "Ad", "BaslangicSaati", "BitisSaati", "Soyad", "SporSalonuId", "UzmanlikAlani" },
                values: new object[,]
                {
                    { 1, "Ahmet", new TimeSpan(0, 9, 0, 0, 0), new TimeSpan(0, 17, 0, 0, 0), "Yılmaz", 1, "Fitness" },
                    { 2, "Elif", new TimeSpan(0, 9, 0, 0, 0), new TimeSpan(0, 17, 0, 0, 0), "Demir", 1, "Yoga" }
                });

            migrationBuilder.InsertData(
                table: "Hizmetler",
                columns: new[] { "Id", "Ad", "SporSalonuId", "SureDakika", "Ucret" },
                values: new object[,]
                {
                    { 1, "Fitness", 1, 60, 200m },
                    { 2, "Yoga", 1, 45, 180m }
                });

            migrationBuilder.InsertData(
                table: "AntrenorHizmetler",
                columns: new[] { "AntrenorId", "HizmetId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AntrenorHizmetler",
                keyColumns: new[] { "AntrenorId", "HizmetId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AntrenorHizmetler",
                keyColumns: new[] { "AntrenorId", "HizmetId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Antrenorler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Antrenorler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hizmetler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hizmetler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SporSalonları",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
