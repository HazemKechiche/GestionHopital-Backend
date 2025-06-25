using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Domain;

namespace TE.Core.Domain
{
   
    public class RendezVous
    {
        [Key] 
        public long idRdv { get; set; }
        public DateTime date{ get; set; }
        public TimeOnly heure{ get; set; }
        public RdvType type{ get; set; }
        public StatutType statut{ get; set; }
        public long PatientId { get; set; }
        public Patient? Patient { get; set; }

        public long MedecinId { get; set; }
        public Medecin? Medecin { get; set; }

        public long AgendaId { get; set; }
        public Agenda? Agenda { get; set; }

    } 
  public enum RdvType
{
    PRESENTIEL,ENLIGNE
}
public enum StatutType
{
        TERMINE,PREVU,CONFIRME,ANNULE
    }
}
