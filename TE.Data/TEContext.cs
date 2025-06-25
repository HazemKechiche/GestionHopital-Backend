using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using TE.Core.Domain;
using System.Collections.Generic;

namespace TE.Data
{
    public class TEContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;
                                        Initial Catalog = MyDB; 
                                        Integrated Security = true");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<FicheMedical> FichesMedicales { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }
        public DbSet<Examen> Examens { get; set; }
        public DbSet<Ordenance> Ordonnances { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PrescriptionMedicament> prescriptionMedicaments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<DemandeConsultation> demandes { get; set; }
        public DbSet<Agenda> agendas { get; set; }
        public DbSet<RendezVous> RendezVous { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity => {
                entity.ToTable("Users");
                entity.HasDiscriminator<string>("Role")
                      .HasValue<Admin>("Admin")
                      .HasValue<Medecin>("Medecin")
                      .HasValue<Patient>("Patient");
            });

            modelBuilder.Entity<FicheMedical>(entity => {
                entity.ToTable("FichesMedicales");
                entity.Property(d => d.typeSang).HasConversion<string>();
            });

            modelBuilder.Entity<Consultation>(entity => {
                entity.ToTable("Consultations");
            });

            modelBuilder.Entity<Diagnostic>(entity => {
                entity.ToTable("Diagnostics");
                entity.Property(d => d.Gravite).HasConversion<string>();
            });

            modelBuilder.Entity<Examen>(entity => {
                entity.ToTable("Examens");
                entity.Property(e => e.TypeExamen).HasConversion<string>();
                entity.Property(e => e.StatutExamen).HasConversion<string>();
            });

            modelBuilder.Entity<Ordenance>(entity => {
                entity.ToTable("Ordonnances");
            });

            modelBuilder.Entity<Medicament>(entity => {
                entity.ToTable("Medicaments");
            });

            modelBuilder.Entity<PrescriptionMedicament>(entity => {
                entity.ToTable("PrescriptionMedicaments");
            });

            modelBuilder.Entity<Notification>(entity => {
                entity.ToTable("Notifications");
                entity.Property(n => n.Type).HasConversion<string>();
            });

            modelBuilder.Entity<DemandeConsultation>(entity => {
                entity.ToTable("DemandesConsultation");
                entity.Property(d => d.demandeType).HasConversion<string>();
            });

            modelBuilder.Entity<Agenda>(entity => {
                entity.ToTable("Agendas");
                entity.Property(a => a.Type).HasConversion<string>();
            });

            modelBuilder.Entity<RendezVous>(entity => {
                entity.ToTable("RendezVous");
                entity.Property(r => r.type).HasConversion<string>();
                entity.Property(r => r.statut).HasConversion<string>();
            });

            // Consultation → Medecin
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.medecin)
                .WithMany(m => m.consultationList)
                .HasForeignKey(c => c.MedecinId)
                .OnDelete(DeleteBehavior.Restrict);

            // Consultation → Patient
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.patient)
                .WithMany(m => m.consultationList)
                .HasForeignKey(c => c.PatientId).OnDelete(DeleteBehavior.Restrict);

            // Diagnostic → Consultation
            modelBuilder.Entity<Diagnostic>()
                .HasOne(d => d.consultation)
                .WithMany(c => c.diagnostic)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.Cascade); // ceci est correct

            // Consultation → Ordonnance
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.Ordonnance)
                .WithOne(o => o.Consultation)
                .HasForeignKey<Consultation>(c => c.consultationId)
                .OnDelete(DeleteBehavior.Restrict);
            // Consultation → Ordonnance
            modelBuilder.Entity<Ordenance>()
                .HasOne(c => c.Patient)
                .WithMany().HasForeignKey(c => c.patientId).OnDelete(DeleteBehavior.Restrict);
                
                
          
           

            // Consultation → RendezVous
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.Rdv)
                .WithOne()
                .HasForeignKey<Consultation>(c => c.rdvId)
                .OnDelete(DeleteBehavior.Restrict);

            // FicheMedicale → Patient
            modelBuilder.Entity<FicheMedical>()
                .HasOne(f => f.patient)
                .WithOne(f => f.ficheMedical)
                .HasForeignKey<Patient>(p => p.ficheMedicalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Consultation → FicheMedicale
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.ficheMedical)
                .WithMany(f => f.Consultations)
                .HasForeignKey(c => c.ficheMedicalId)
                .OnDelete(DeleteBehavior.Restrict);

            // FicheMedicale → Examens
            modelBuilder.Entity<FicheMedical>()
                .HasMany(f => f.examens)
                .WithOne(e => e.ficheMedical)
                .HasForeignKey(e => e.ficheMedicalId)
                .OnDelete(DeleteBehavior.Cascade);
            // examen medecin
            modelBuilder.Entity<Examen>().HasOne(e=>e.Medecin).WithMany().HasForeignKey(e=>e.medecinId).OnDelete(DeleteBehavior.Restrict);
            //
            modelBuilder.Entity<Examen>().HasOne(e => e.Patient).WithMany().HasForeignKey(e => e.patientId).OnDelete(DeleteBehavior.Restrict);

            // Diagnostic → FicheMedicale

            modelBuilder.Entity<Diagnostic>()
                .HasOne(d => d.ficheMedical)
                .WithMany(f => f.diagnostics)
                .HasForeignKey(d => d.ficheMedicalId)
                .OnDelete(DeleteBehavior.Restrict);

            // PrescriptionMedicament (composite clé)
            modelBuilder.Entity<PrescriptionMedicament>()
                .HasKey(pm => new { pm.OrdonnanceId, pm.MedicamentId });

            // PrescriptionMedicament → Ordonnance
            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Ordonnance)
                .WithMany(o => o.Prescriptions)
                .HasForeignKey(pm => pm.OrdonnanceId)
                .OnDelete(DeleteBehavior.Cascade);

            // PrescriptionMedicament → Medicament
            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Agenda → Utilisateur
            modelBuilder.Entity<Agenda>()
                .HasOne(a => a.Utilisateur)
                .WithMany()
                .HasForeignKey(a => a.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Agenda → RendezVous
            modelBuilder.Entity<Agenda>()
                .HasOne(a => a.RendezVous)
                .WithOne(r => r.Agenda)
                .HasForeignKey<RendezVous>(r => r.AgendaId)
                .OnDelete(DeleteBehavior.Restrict);

            // RendezVous → Patient
            modelBuilder.Entity<RendezVous>()
                .HasOne(r => r.Patient)
                .WithMany()
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // RendezVous → Medecin
            modelBuilder.Entity<RendezVous>()
                .HasOne(r => r.Medecin)
                .WithMany()
                .HasForeignKey(r => r.MedecinId)
                .OnDelete(DeleteBehavior.Restrict);

            // DemandeConsultation → Patient
            modelBuilder.Entity<DemandeConsultation>()
                .HasOne(d => d.patient)
                .WithMany()
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // DemandeConsultation → MedecinSuggeré
            modelBuilder.Entity<DemandeConsultation>()
                .HasOne(d => d.MedecinSuggeré)
                .WithMany()
                .HasForeignKey(d => d.MedecinId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification → Destinataire
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Destinataire)
                .WithMany()
                .HasForeignKey(n => n.DestinataireId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<RendezVous>()
                .Property(r => r.heure)
                .HasConversion(
                    t => t.ToTimeSpan(),
                    ts => TimeOnly.FromTimeSpan(ts));

            // Collection Value Converters with Comparers
            modelBuilder.Entity<FicheMedical>()
                .Property(f => f.antecedentsMedicaux)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<FicheMedical>()
                .Property(f => f.antecedentsFamiliaux)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<FicheMedical>()
                .Property(f => f.AllergiesConnus)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<FicheMedical>()
                .Property(f => f.traitementChroniques)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<Medicament>()
                .Property(f => f.ContreIndications)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<Medicament>()
                .Property(f => f.DosageDisponible)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<Medicament>()
                .Property(f => f.EffetsSecondaires)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<Medicament>()
                .Property(f => f.Interactions)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            modelBuilder.Entity<Examen>()
                .Property(f => f.Images)
                .HasConversion(
                    v => string.Join(";", v ?? new List<string>()),
                    v => v != null ? new List<string>(v.Split(new[] { ';' }, StringSplitOptions.None)) : new List<string>()
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        }
    }
}