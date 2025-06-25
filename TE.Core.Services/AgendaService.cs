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
    public class AgendaService
    {
        private readonly TEContext _context;
        private readonly NotificationService _notificationService;
        public AgendaService(TEContext context,NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public Agenda addMedecinAgenda(long idMedecin , Agenda agenda)
        {
            agenda.UtilisateurId = idMedecin;
           var result = _context.Add(agenda);
            _context.SaveChanges();
            return result.Entity;
        }
        public void CreerDisponibilites30Min(long medecinId, DateTime date, TimeSpan heureDebut, TimeSpan heureFin, string description)
        {
            var duree = TimeSpan.FromMinutes(30);

            if (heureDebut >= heureFin)
                throw new ArgumentException("Heure début doit être avant heure fin");

            var medecin = _context.Users.FirstOrDefault(u => u.Id == medecinId);
            if (medecin == null)
                throw new Exception("Médecin non trouvé");

            var currentStart = heureDebut;

            while (currentStart + duree <= heureFin)
            {
                var agenda = new Agenda
                {
                    Utilisateur = _context.Users.Find(medecinId),
                    UtilisateurId = medecinId,
                    Date = date.Date,
                    HeureDebut = currentStart,
                    HeureFin = currentStart + duree,
                    Type = AgendaType.DISPONIBILITE,
                    Description = description,
                    IsBooked = false
                };
                _context.agendas.Add(agenda);
                currentStart += duree;
            }
            _context.SaveChanges();
        }
        public List<Agenda> GetDispoMedecin(long medecinId, DateTime date)
        {
            return _context.agendas
                .Where(a => a.UtilisateurId == medecinId
                            && a.Date == date.Date
                            && a.Type == AgendaType.DISPONIBILITE
                            && !a.IsBooked)
                .OrderBy(a => a.HeureDebut)
                .ToList();
        }
        ///
        public RendezVous? ReserverRendezVous(long patientId, long agendaId, RendezVous request)
        {
            var agenda = _context.agendas
                .Include(a => a.RendezVous)
                .Include(a => a.Utilisateur)
                .FirstOrDefault(a => a.Id == agendaId);

            if (agenda == null || agenda.Type != AgendaType.DISPONIBILITE || agenda.RendezVous != null)
                return null;

            var patient = _context.Patients.Find(patientId);
            if (patient == null)
                return null;

            var medecin = _context.Medecins.FirstOrDefault(m => m.Id == agenda.UtilisateurId);
            if (medecin == null)
                return null;

            var rdv = new RendezVous
            {
                date = agenda.Date,
                heure = TimeOnly.FromTimeSpan(agenda.HeureDebut),
                type = request.type,
                statut = StatutType.PREVU,
                Patient = patient,
                Medecin = medecin,
                Agenda = agenda,
                PatientId = patientId,
                MedecinId=agenda.UtilisateurId,
                AgendaId=agendaId

            };
            

            agenda.Type = AgendaType.RENDEZ_VOUS;
            agenda.RendezVous = rdv;

            _context.SaveChanges();
            _notificationService.sendNotif(rdv.PatientId, "vous avez reserver votre rendez vous avec succes  ", "RESERVATION RDV", TypeNotification.Info);
            return rdv;
        }


    }
}
