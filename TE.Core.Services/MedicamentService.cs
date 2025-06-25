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
    public class MedicamentService
    {
        private readonly TEContext _context;

        public MedicamentService(TEContext context)
        {
            _context = context;
        }

        public async Task<List<Medicament>> GetAllAsync()
        {
            return await _context.Medicaments.ToListAsync();
        }

        public async Task<Medicament?> GetByIdAsync(long id)
        {
            return await _context.Medicaments.FindAsync(id);
        }

        public async Task<Medicament> CreateAsync(Medicament medicament)
        {
            _context.Medicaments.Add(medicament);
            await _context.SaveChangesAsync();
            return medicament;
        }

        public async Task<bool> UpdateAsync(Medicament medicament)
        {
            var existing = await _context.Medicaments.FindAsync(medicament.Id);
            if (existing == null) return false;

            // Mettre à jour les champs
            _context.Entry(existing).CurrentValues.SetValues(medicament);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var medicament = await _context.Medicaments.FindAsync(id);
            if (medicament == null) return false;

            _context.Medicaments.Remove(medicament);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
