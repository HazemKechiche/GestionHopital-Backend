using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
    
    public class Consultation
    {
        [Key]
        public long IdConsultation { get; set; }
        public DateTime DateHeureEffectif { get; set; }
        public string Symptomes { get; set; }
        
        public string Prescription { get; set; }
        public RendezVous Rdv { get; set; } // Lié à un rendez-vous
        public Ordenance? Ordonnance { get; set; } // Lié à une ordonnance
        public Medecin medecin { get; set; }
        public long MedecinId { get; set; }
        public Patient patient { get; set; }
        public long PatientId { get; set; }
        public IList<Diagnostic>? diagnostic { get; set; }
        public long ficheMedicalId { get; set; }
        public long rdvId  { get; set; }
        public long? consultationId { get; set; }

        public FicheMedical ficheMedical { get; set; } 
        public bool IsFinalisee { get; set; } = false;  // Marqueur finalisation

        public TimeSpan? DureeTotale { get; set; }

    }
}
