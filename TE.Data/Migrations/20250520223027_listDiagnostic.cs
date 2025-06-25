using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TE.Data.Migrations
{
    /// <inheritdoc />
    public partial class listDiagnostic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Diagnostics_ConsultationId",
                table: "Diagnostics");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_ConsultationId",
                table: "Diagnostics",
                column: "ConsultationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Diagnostics_ConsultationId",
                table: "Diagnostics");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_ConsultationId",
                table: "Diagnostics",
                column: "ConsultationId",
                unique: true);
        }
    }
}
