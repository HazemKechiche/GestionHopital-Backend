using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TE.Data.Migrations
{
    /// <inheritdoc />
    public partial class timeConsu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Consultations_consultationId",
                table: "Consultations");

            migrationBuilder.AlterColumn<long>(
                name: "consultationId",
                table: "Consultations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DureeTotale",
                table: "Consultations",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalisee",
                table: "Consultations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_consultationId",
                table: "Consultations",
                column: "consultationId",
                unique: true,
                filter: "[consultationId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Consultations_consultationId",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "DureeTotale",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "IsFinalisee",
                table: "Consultations");

            migrationBuilder.AlterColumn<long>(
                name: "consultationId",
                table: "Consultations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_consultationId",
                table: "Consultations",
                column: "consultationId",
                unique: true);
        }
    }
}
