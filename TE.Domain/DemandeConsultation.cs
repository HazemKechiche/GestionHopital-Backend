using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
   
    public  class DemandeConsultation
    {
        [Key] 
        public long  idDemande { get; set; }
        public DateTime dateDemande { get; set; }
        public string Motif {  get; set; }
        public DemandeType demandeType { get; set; }
        public Patient patient { get; set; }
        [AllowNull]
        public Medecin MedecinSuggeré { get; set; }
        public long MedecinId { get; set; }
        public long PatientId { get; set; }
    }
    
    public enum DemandeType
    {
        ENATTENTE,MEDECINSUGGERE,ACCEPTE,REFUSE
    }
}
