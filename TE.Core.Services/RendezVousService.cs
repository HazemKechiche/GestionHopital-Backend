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
    public class RendezVousService
    {
        private readonly TEContext _tecontext;
        private readonly NotificationService _notificationService;
        public RendezVousService(TEContext tecontext,NotificationService notificationService)
        {
            _tecontext = tecontext;
            _notificationService = notificationService;
        }
        public IList<RendezVous> getRendezVousPerPatient(long idPatient) {
            var patient = _tecontext.RendezVous.FirstOrDefault(x => x.PatientId != null && x.PatientId == idPatient);
              

            if (patient == null)
                return null;
            IList<RendezVous>  rdvs = _tecontext.RendezVous.Where(a=>a.PatientId == idPatient).OrderBy(a=>a.date).ToList();
            return rdvs;

        }
        public IList<RendezVousDto> getRendezVousPerMedecin(long idMedecin)
        {
            var medecin = _tecontext.RendezVous.FirstOrDefault(x => x.MedecinId != null && x.MedecinId == idMedecin);

            if (medecin == null)
                return null;

            IList<RendezVousDto> rdvs = _tecontext.RendezVous
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .Where(r => r.MedecinId == idMedecin)
                .Select(r => new RendezVousDto
                {
                    IdRdv = r.idRdv,
                    Date = r.date,
                    Heure = r.heure,
                    PatientNom = r.Patient.Name,
                    PatientPrenom = r.Patient.Surname,
                    Sexe = r.Patient.sexe,
                    Telephone = r.Patient.phoneNumber,
                    DateNaissance = r.Patient.dateNaissance,
                    MedecinNom = r.Medecin.Name,
                    MedecinPrenom = r.Medecin.Surname,
                    Statut = r.statut,
                    Type = r.type
                })
                .ToList();

            return rdvs;
        }

        public Boolean confirmerRendezVous(long idRendezVous)
        {
            var rdv = _tecontext.RendezVous.FirstOrDefault(x => x.idRdv == idRendezVous);
            var agenda = _tecontext.agendas.FirstOrDefault(a => a.Id == rdv.AgendaId);

            if (rdv == null || agenda == null) { return false; }
            rdv.statut = StatutType.CONFIRME;
            agenda.IsBooked = true;
            _tecontext.SaveChanges();

            _notificationService.sendNotif(rdv.PatientId, "votre rdv a ete confirmé", "CONFIRMATION RDV", TypeNotification.Info);
            return true;
        }
        public Boolean annulerRendezVous(long idRendezVous)
        {
            var rdv = _tecontext.RendezVous.FirstOrDefault(x => x.idRdv == idRendezVous);
            var agenda = _tecontext.agendas.FirstOrDefault(a => a.Id == rdv.AgendaId);
            if (rdv == null || agenda==null) { return false; }
            rdv.statut = StatutType.ANNULE;
            agenda.Type = AgendaType.DISPONIBILITE;
            agenda.IsBooked = false;
            _tecontext.SaveChanges();
            _notificationService.sendNotif(rdv.PatientId, "votre rdv a ete annulé", "ANNULATION RDV", TypeNotification.Info);
            return true;
        }
        public class RendezVousDto
        {
            public long IdRdv { get; set; }
            public DateTime Date { get; set; }
            public TimeOnly Heure { get; set; }
            public string PatientNom { get; set; }
            public string PatientPrenom { get; set; }
            public string MedecinNom { get; set; }
            public string MedecinPrenom { get; set; }
            public string Sexe { get; set; }
            public long Telephone { get; set; }
            public DateTime DateNaissance { get; set; }
            public StatutType Statut { get; set; }
            public RdvType Type { get; set; }
        }


    }
}
