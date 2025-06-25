using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Domain;
using TE.Data;

namespace TE.Core.Services
{
    public class MedecinService
    {
        private readonly TEContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MedecinService(TEContext context, IHttpContextAccessor httpContextAccessor) { 
        _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IList<Medecin> getAllMedecins()
        {
            return _context.Medecins.ToList();
        }
        public List<Agenda> GetDisponibilitesParMedecin(long medecinId)
        {
            return _context.agendas
                .Where(a => a.UtilisateurId == medecinId && a.Type == AgendaType.DISPONIBILITE && a.Date >= DateTime.Today)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.HeureDebut)
                .ToList();
        }
        public async Task<object> GetProfile()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            if (!long.TryParse(userIdClaim.Value, out long userId))
                throw new ArgumentException("Invalid user ID");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Check if user is a Medecin
            var medecin = await _context.Medecins.FirstOrDefaultAsync(m => m.Id == userId);
            if (medecin == null)
                throw new InvalidOperationException("L'utilisateur connecté n'est pas un médecin.");

            var profileDto = new
            {
                medecin.Id,
                Nom = medecin.Name,
                Prenom = medecin.Surname,
                medecin.Email,
                Telephone = medecin.phoneNumber,
                Specialite = medecin.specialite,
                Biographie = "",
                AvatarUrl = ""
            };

            return profileDto;
        }
        public class PatientDto
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public long phoneNumber { get; set; }
            public string sexe { get; set; }
            public string adresse { get; set; }
            public DateTime dateNaissance { get; set; }
        }

        public async Task<List<PatientDto>> GetPatientsWithFinalisedConsultationsAsync(long medecinId)
        {
            return await _context.Consultations
                .Where(c => c.MedecinId == medecinId && c.IsFinalisee)
                .Select(c => new PatientDto
                {
                    Id = c.patient.Id,
                    Name = c.patient.Name,
                    Surname = c.patient.Surname,
                    Email = c.patient.Email,
                    phoneNumber = c.patient.phoneNumber,
                    sexe = c.patient.sexe,
                    adresse = c.patient.adresse,
                    dateNaissance = c.patient.dateNaissance
                })
                .Distinct()
                .ToListAsync();
        }


    }
}
