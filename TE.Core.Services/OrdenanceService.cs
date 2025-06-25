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
    public class OrdenanceService
    {
        private readonly TEContext _context;

        public OrdenanceService(TEContext context)
        {
            _context = context;
        }

        public async Task<List<Ordenance>> GetAllAsync()
        {
            return await _context.Ordonnances
                .Include(o => o.Patient)
                .Include(o => o.Medecin)
                .Include(o => o.Consultation)
                .Include(o => o.Prescriptions)
                    .ThenInclude(p => p.Medicament)
                .ToListAsync();
        }

        public async Task<Ordenance?> GetByIdAsync(long id)
        {
            return await _context.Ordonnances
                .Include(o => o.Patient)
                .Include(o => o.Medecin)
                .Include(o => o.Consultation)
                .Include(o => o.Prescriptions)
                    .ThenInclude(p => p.Medicament)
                .FirstOrDefaultAsync(o => o.IdOrdonnance == id);
        }

        public async Task<Ordenance> CreateAsync(long consultationId, Ordenance ordonnance)
        {
            var consultation = await _context.Consultations
                .Include(c => c.medecin)
                .Include(c => c.patient)
                .Include(c => c.ficheMedical)
                .FirstOrDefaultAsync(c => c.IdConsultation == consultationId);

            if (consultation == null)
                return null;

            ordonnance.Date = DateTime.Now;
            ordonnance.Consultation = consultation;
            ordonnance.ConsultationId = consultation.IdConsultation;
            ordonnance.Patient = consultation.patient;
            ordonnance.Medecin = consultation.medecin;
            ordonnance.ficheMedical = consultation.ficheMedical;
            ordonnance.patientId = consultation.PatientId;
            ordonnance.medecinId = consultation.MedecinId;
            ordonnance.ficheMedicalId = consultation.ficheMedicalId;

            if (ordonnance.Prescriptions != null)
            {
                foreach (var pres in ordonnance.Prescriptions)
                {
                    
                    pres.Ordonnance = ordonnance;
                }
            }

            _context.Ordonnances.Add(ordonnance);
            await _context.SaveChangesAsync();

            return ordonnance;
        }

        public async Task<bool> UpdateAsync(Ordenance updated)
        {
            var existing = await _context.Ordonnances
                .Include(o => o.Prescriptions)
                .FirstOrDefaultAsync(o => o.IdOrdonnance == updated.IdOrdonnance);

            if (existing == null) return false;

            existing.DateExpiration = updated.DateExpiration;
            existing.Remarques = updated.Remarques;
            existing.medecinId = updated.medecinId;
            existing.patientId = updated.patientId;
            existing.ConsultationId = updated.ConsultationId;
            existing.ficheMedicalId = updated.ficheMedicalId;

            _context.prescriptionMedicaments.RemoveRange(existing.Prescriptions);
            existing.Prescriptions = updated.Prescriptions;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var ordonnance = await _context.Ordonnances
                .Include(o => o.Prescriptions)
                .FirstOrDefaultAsync(o => o.IdOrdonnance == id);

            if (ordonnance == null) return false;

            _context.prescriptionMedicaments.RemoveRange(ordonnance.Prescriptions);
            _context.Ordonnances.Remove(ordonnance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
    }
