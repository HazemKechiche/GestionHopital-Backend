using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Domain;
using TE.Data;

namespace TE.Core.Services
{
    public class ConsultationService
    {
        private readonly TEContext _context;
        public ConsultationService(TEContext context    )
        {
            _context = context;
        }
        public Consultation? DemarrerConsultation(long rdvId)
        {
            var rdv = _context.RendezVous
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .FirstOrDefault(r => r.idRdv == rdvId);

            if (rdv == null || rdv.statut != StatutType.CONFIRME)
                return null;
            var patient = _context.Patients.FirstOrDefault(f => f.Id == rdv.PatientId);
           if(patient==null)
            { return null; }
            var idFiche = patient.ficheMedicalId;

            var fiche = _context.FichesMedicales.FirstOrDefault(f => f.idFiche == idFiche);
            if (fiche == null)
                return null;



            var consultation = new Consultation
            {
                DateHeureEffectif = DateTime.Now,
                Symptomes = "",
                Prescription = "",
                Rdv = rdv,
                rdvId = rdv.idRdv,
                MedecinId = rdv.MedecinId,
                PatientId = rdv.PatientId,
                ficheMedicalId = fiche.idFiche,
                ficheMedical = fiche,
                IsFinalisee=true,

            };

            _context.Consultations.Add(consultation);
            _context.SaveChanges();

            // Mettre à jour le statut du rendez-vous si besoin
            rdv.statut = StatutType.TERMINE;

            _context.SaveChanges();

            return consultation;
        }
        public async Task<Consultation?> FinaliserConsultationAsync(long consultationId)
        {
            var consultation = await _context.Consultations
                .FirstOrDefaultAsync(c => c.IdConsultation == consultationId);

            if (consultation == null || consultation.IsFinalisee)
                return null;

            consultation.IsFinalisee = true;
            consultation.DureeTotale = DateTime.Now - consultation.DateHeureEffectif;

            await _context.SaveChangesAsync();
            return consultation;
        }
        public async Task AjouterDiagnosticsAsync(long consultationId, List<Diagnostic> diagnostics)
        {
            // Récupérer la consultation avec sa fiche médicale (car on a besoin du lien avec la FicheMedical)
            var consultation = await _context.Consultations
                .Include(c => c.diagnostic)
                .Include(c => c.ficheMedical)
                .FirstOrDefaultAsync(c => c.IdConsultation == consultationId);
            
            if (consultation == null)
                throw new Exception("Consultation introuvable.");

            foreach (var diagnostic in diagnostics)
            {
                diagnostic.DateDiagnostic = DateTime.Now;
                diagnostic.ConsultationId = consultation.IdConsultation;
                diagnostic.consultation = consultation;
                diagnostic.ficheMedicalId = consultation.ficheMedical.idFiche;
                diagnostic.ficheMedical = consultation.ficheMedical;
                Console.WriteLine(consultation.ficheMedical);
                consultation.diagnostic.Add(diagnostic);
                foreach (var examen in diagnostic.ExamensAssocies)
                {
                    examen.ficheMedicalId = consultation.ficheMedical.idFiche;
                    examen.ficheMedical = consultation.ficheMedical;
                    examen.medecinId = consultation.MedecinId;// ou MedecinId selon ta structure
                    examen.patientId = consultation.PatientId;
                    examen.DateExamen = DateTime.MinValue; // en attente
                    examen.DateResultatRecu = null;
                    examen.IsResultatDisponible = false;
                }
                _context.Diagnostics.Add(diagnostic); // facultatif si EF suit automatiquement
            }

            await _context.SaveChangesAsync();
        }



    }
}
