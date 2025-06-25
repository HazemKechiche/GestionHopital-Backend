using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TE.Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FichesMedicales",
                columns: table => new
                {
                    idFiche = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupeSanguin = table.Column<float>(type: "real", nullable: false),
                    poids = table.Column<float>(type: "real", nullable: false),
                    taille = table.Column<float>(type: "real", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    sexe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    antecedentsFamiliaux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    antecedentsMedicaux = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    traitementChroniques = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllergiesConnus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fummeur = table.Column<bool>(type: "bit", nullable: false),
                    Alcolique = table.Column<bool>(type: "bit", nullable: false),
                    typeSang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichesMedicales", x => x.idFiche);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idAdmin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specialite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateNaissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sexe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ficheMedicalId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_FichesMedicales_ficheMedicalId",
                        column: x => x.ficheMedicalId,
                        principalTable: "FichesMedicales",
                        principalColumn: "idFiche",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilisateurId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    HeureFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agendas_Users_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemandesConsultation",
                columns: table => new
                {
                    idDemande = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateDemande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    demandeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedecinId = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesConsultation", x => x.idDemande);
                    table.ForeignKey(
                        name: "FK_DemandesConsultation_Users_MedecinId",
                        column: x => x.MedecinId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandesConsultation_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstLue = table.Column<bool>(type: "bit", nullable: false),
                    DateEnvoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinataireId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_DestinataireId",
                        column: x => x.DestinataireId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ordonnances",
                columns: table => new
                {
                    IdOrdonnance = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConsultationId = table.Column<long>(type: "bigint", nullable: false),
                    Remarques = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ficheMedicalId = table.Column<long>(type: "bigint", nullable: false),
                    patientId = table.Column<long>(type: "bigint", nullable: false),
                    medecinId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordonnances", x => x.IdOrdonnance);
                    table.ForeignKey(
                        name: "FK_Ordonnances_FichesMedicales_ficheMedicalId",
                        column: x => x.ficheMedicalId,
                        principalTable: "FichesMedicales",
                        principalColumn: "idFiche",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Users_medecinId",
                        column: x => x.medecinId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Users_patientId",
                        column: x => x.patientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RendezVous",
                columns: table => new
                {
                    idRdv = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    heure = table.Column<TimeSpan>(type: "time", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    MedecinId = table.Column<long>(type: "bigint", nullable: false),
                    AgendaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendezVous", x => x.idRdv);
                    table.ForeignKey(
                        name: "FK_RendezVous_Agendas_AgendaId",
                        column: x => x.AgendaId,
                        principalTable: "Agendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RendezVous_Users_MedecinId",
                        column: x => x.MedecinId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RendezVous_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    IdConsultation = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateHeureEffectif = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Symptomes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedecinId = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    ficheMedicalId = table.Column<long>(type: "bigint", nullable: false),
                    rdvId = table.Column<long>(type: "bigint", nullable: false),
                    consultationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.IdConsultation);
                    table.ForeignKey(
                        name: "FK_Consultations_FichesMedicales_ficheMedicalId",
                        column: x => x.ficheMedicalId,
                        principalTable: "FichesMedicales",
                        principalColumn: "idFiche",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultations_Ordonnances_consultationId",
                        column: x => x.consultationId,
                        principalTable: "Ordonnances",
                        principalColumn: "IdOrdonnance",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultations_RendezVous_rdvId",
                        column: x => x.rdvId,
                        principalTable: "RendezVous",
                        principalColumn: "idRdv",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultations_Users_MedecinId",
                        column: x => x.MedecinId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultations_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    IdDiagnostic = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDiagnostic = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeDiagnostic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gravite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recommandations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultationId = table.Column<long>(type: "bigint", nullable: false),
                    ficheMedicalId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.IdDiagnostic);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "IdConsultation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnostics_FichesMedicales_ficheMedicalId",
                        column: x => x.ficheMedicalId,
                        principalTable: "FichesMedicales",
                        principalColumn: "idFiche",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Examens",
                columns: table => new
                {
                    IdExamen = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeExamen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateExamen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resultats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaboId = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatutExamen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ficheMedicalId = table.Column<long>(type: "bigint", nullable: false),
                    medecinId = table.Column<long>(type: "bigint", nullable: false),
                    patientId = table.Column<long>(type: "bigint", nullable: false),
                    DiagnosticIdDiagnostic = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examens", x => x.IdExamen);
                    table.ForeignKey(
                        name: "FK_Examens_Diagnostics_DiagnosticIdDiagnostic",
                        column: x => x.DiagnosticIdDiagnostic,
                        principalTable: "Diagnostics",
                        principalColumn: "IdDiagnostic");
                    table.ForeignKey(
                        name: "FK_Examens_FichesMedicales_ficheMedicalId",
                        column: x => x.ficheMedicalId,
                        principalTable: "FichesMedicales",
                        principalColumn: "idFiche",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Examens_Users_LaboId",
                        column: x => x.LaboId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Examens_Users_medecinId",
                        column: x => x.medecinId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Examens_Users_patientId",
                        column: x => x.patientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Medicaments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fabricant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrincipeActif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosageDisponible = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormePharmaceutique = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContreIndications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffetsSecondaires = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interactions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prix = table.Column<float>(type: "real", nullable: false),
                    DisponibiliteStock = table.Column<int>(type: "int", nullable: false),
                    DiagnosticIdDiagnostic = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicaments_Diagnostics_DiagnosticIdDiagnostic",
                        column: x => x.DiagnosticIdDiagnostic,
                        principalTable: "Diagnostics",
                        principalColumn: "IdDiagnostic");
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionMedicaments",
                columns: table => new
                {
                    OrdonnanceId = table.Column<long>(type: "bigint", nullable: false),
                    MedicamentId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Posologie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvantRepas = table.Column<bool>(type: "bit", nullable: false),
                    OrdenanceIdOrdonnance = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedicaments", x => new { x.OrdonnanceId, x.MedicamentId });
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicaments_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicaments_Ordonnances_OrdenanceIdOrdonnance",
                        column: x => x.OrdenanceIdOrdonnance,
                        principalTable: "Ordonnances",
                        principalColumn: "IdOrdonnance");
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicaments_Ordonnances_OrdonnanceId",
                        column: x => x.OrdonnanceId,
                        principalTable: "Ordonnances",
                        principalColumn: "IdOrdonnance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_UtilisateurId",
                table: "Agendas",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_consultationId",
                table: "Consultations",
                column: "consultationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_ficheMedicalId",
                table: "Consultations",
                column: "ficheMedicalId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_MedecinId",
                table: "Consultations",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_PatientId",
                table: "Consultations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_rdvId",
                table: "Consultations",
                column: "rdvId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DemandesConsultation_MedecinId",
                table: "DemandesConsultation",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesConsultation_PatientId",
                table: "DemandesConsultation",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_ConsultationId",
                table: "Diagnostics",
                column: "ConsultationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_ficheMedicalId",
                table: "Diagnostics",
                column: "ficheMedicalId");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_DiagnosticIdDiagnostic",
                table: "Examens",
                column: "DiagnosticIdDiagnostic");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_ficheMedicalId",
                table: "Examens",
                column: "ficheMedicalId");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_LaboId",
                table: "Examens",
                column: "LaboId");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_medecinId",
                table: "Examens",
                column: "medecinId");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_patientId",
                table: "Examens",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_DiagnosticIdDiagnostic",
                table: "Medicaments",
                column: "DiagnosticIdDiagnostic");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DestinataireId",
                table: "Notifications",
                column: "DestinataireId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_ficheMedicalId",
                table: "Ordonnances",
                column: "ficheMedicalId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_medecinId",
                table: "Ordonnances",
                column: "medecinId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_patientId",
                table: "Ordonnances",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicaments_MedicamentId",
                table: "PrescriptionMedicaments",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicaments_OrdenanceIdOrdonnance",
                table: "PrescriptionMedicaments",
                column: "OrdenanceIdOrdonnance");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_AgendaId",
                table: "RendezVous",
                column: "AgendaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_MedecinId",
                table: "RendezVous",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_PatientId",
                table: "RendezVous",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ficheMedicalId",
                table: "Users",
                column: "ficheMedicalId",
                unique: true,
                filter: "[ficheMedicalId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandesConsultation");

            migrationBuilder.DropTable(
                name: "Examens");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PrescriptionMedicaments");

            migrationBuilder.DropTable(
                name: "Medicaments");

            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "Ordonnances");

            migrationBuilder.DropTable(
                name: "RendezVous");

            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FichesMedicales");
        }
    }
}
