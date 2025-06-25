using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
    
    public class Ordenance
    {
        [Key]
        public long IdOrdonnance { get; set; }
        public DateTime Date { get; set; }
        public Patient? Patient { get; set; }
        public Medecin? Medecin { get; set; }
        public Consultation? Consultation { get; set; }
        public long? ConsultationId { get; set; }

        public string Remarques { get; set; }
        public DateTime DateExpiration { get; set; }
        public List<PrescriptionMedicament>? ListeMedicaments { get; set; }
        public long? ficheMedicalId { get; set; }
        public long? patientId { get; set; }
        public long? medecinId { get; set; }
        public FicheMedical? ficheMedical { get; set; }
        public List<PrescriptionMedicament>? Prescriptions { get; set; }
    }
}
