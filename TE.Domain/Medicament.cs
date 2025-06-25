using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
    
    public class Medicament
    {
        [Key]
        public long Id { get; set; } 
        public string Fabricant { get; set; } 
        public string PrincipeActif { get; set; } 
        public List<string> DosageDisponible { get; set; } = new();
        public string FormePharmaceutique { get; set; } 
        public List<string> ContreIndications { get; set; } = new();
        public List<string> EffetsSecondaires { get; set; } = new();
        public List<string> Interactions { get; set; } = new();
        public float Prix { get; set; } 
        public int DisponibiliteStock { get; set; }
        public List<PrescriptionMedicament>? Prescriptions { get; set; }
    }
}
