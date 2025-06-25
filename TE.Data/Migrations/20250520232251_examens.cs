using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TE.Data.Migrations
{
    /// <inheritdoc />
    public partial class examens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resultats",
                table: "Examens",
                newName: "ModeRealisation");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateExamen",
                table: "Examens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CodeExamen",
                table: "Examens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Commentaires",
                table: "Examens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDemande",
                table: "Examens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateResultatRecu",
                table: "Examens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FichierResultats",
                table: "Examens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsResultatDisponible",
                table: "Examens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LieuExamen",
                table: "Examens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Priorite",
                table: "Examens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ficheMedicalId",
                table: "Diagnostics",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeExamen",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "Commentaires",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "DateDemande",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "DateResultatRecu",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "FichierResultats",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "IsResultatDisponible",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "LieuExamen",
                table: "Examens");

            migrationBuilder.DropColumn(
                name: "Priorite",
                table: "Examens");

            migrationBuilder.RenameColumn(
                name: "ModeRealisation",
                table: "Examens",
                newName: "Resultats");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateExamen",
                table: "Examens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ficheMedicalId",
                table: "Diagnostics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
