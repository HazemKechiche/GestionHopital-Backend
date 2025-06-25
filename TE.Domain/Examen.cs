using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{

    public class Examen
    {
        [Key]
        public long IdExamen { get; set; }

        public TypeExamen TypeExamen { get; set; }
        public DateTime DateDemande { get; set; }

        public DateTime? DateExamen { get; set; } = null;           
        public DateTime? DateResultatRecu { get; set; } = null;
        public bool IsResultatDisponible { get; set; } = false;

        
        public string? Commentaires { get; set; } = null;
        
        public string? FichierResultats { get; set; } = null;

        public PrioriteExamen Priorite { get; set; }
        public string CodeExamen { get; set; }
        public string ModeRealisation { get; set; }
        public string LieuExamen { get; set; }

        public StatutExamen StatutExamen { get; set; } = StatutExamen.EnAttente;

        public List<string> Images { get; set; } = new();

        public long? ficheMedicalId { get; set; }
        public FicheMedical? ficheMedical { get; set; }

        public long? medecinId { get; set; }
        public Medecin? Medecin { get; set; }

        public long? patientId { get; set; }
        public Patient? Patient { get; set; }

        public Admin? Labo { get; set; }
        public long? LaboId { get; set; }
    }


    public enum TypeExamen
    {
        ExamenPhysiqueGeneral,
        Radiographie,
        Echographie,
        Scanner,
        IRM,
        NFS,
        Glycemie,
        BilanHepatique,
        AnalyseUrine,
        ECG,
        EEG,
        EMG,
        Frottis,
        SerologieVIH,
        TestGrossesse,
        Spirometrie,
        Audiometrie,
        Endoscopie,
        Mammographie,
        MonitoringFoetal,
        Amniocentese
    }

    public enum StatutExamen
    {
        EnAttente,
        Terminé,
        EnCours
    }
    public enum PrioriteExamen
    {
        Basse,
        Normale,
        Urgente
    }
}
