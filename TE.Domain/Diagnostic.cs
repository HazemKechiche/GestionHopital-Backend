using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
    
    public class Diagnostic
    {
        [Key]
        public long IdDiagnostic { get; set; } 
        public DateTime DateDiagnostic { get; set; } // Date du diagnostic
        public string TypeDiagnostic { get; set; } // Type de diagnostic (ex. diabète, hypertension, etc.)
        public string Details { get; set; } // Détails supplémentaires sur le diagnostic
        public NiveauGravite Gravite { get; set; } 
        public string Recommandations { get; set; } 
        public List<Examen> ExamensAssocies { get; set; } 
        public List<Medicament> TraitementsRecommandes { get; set; } 
        public Consultation? consultation { get; set; }
        public long ConsultationId { get; set; }
        public long? ficheMedicalId { get; set; }
        public FicheMedical? ficheMedical { get; set; }
    }
    public enum NiveauGravite
    {
        Faible,
        Modéré,
        Grave
    }

}
