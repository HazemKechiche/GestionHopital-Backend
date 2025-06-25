using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
    
    public class PrescriptionMedicament
    {
        [Key]
        public long Id { get; set; }
        public Medicament? Medicament { get; set; } // Le médicament prescrit
        public string Posologie { get; set; } // La posologie du médicament
        public string Duree { get; set; } // La durée du traitement
        public string Instructions { get; set; } // Instructions supplémentaires pour le médicament
        public bool AvantRepas { get; set; } // Indique si le médicament doit être pris avant les repas
        public long? OrdonnanceId { get; set; }
        public Ordenance? Ordonnance { get; set; }

        public long? MedicamentId { get; set; }
    }
}
