using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TE.Core.Domain
{
  
    public class Agenda
    {
        [Key]
        public long Id { get; set; }

        public long UtilisateurId { get; set; }
        public User Utilisateur { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }

        public AgendaType Type { get; set; }

        public string Description { get; set; }

        public RendezVous? RendezVous { get; set; }
        public bool IsBooked { get; set; } = false;
    }
    public enum AgendaType
    {
        DISPONIBILITE,
        PLAGE_BLOQUEE,
        RENDEZ_VOUS,
        AUTRE
    }
}