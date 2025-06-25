using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
   
    public class FicheMedical
    {
        [Key] public long idFiche {  get; set; }
        
        public float? poids { get; set; }
        public float? taille {  get; set; }
        public float? IMC { get; set; }
        public int? age { get; set; }
        public string sexe { get; set; }
        public List<string>? antecedentsFamiliaux { get; set; } = new();
        public List<string>? antecedentsMedicaux { get; set; } = new();
        public List<string>? traitementChroniques { get; set; } = new();
        public List<string>? AllergiesConnus { get; set; } = new();
        public Boolean?   Fummeur {  get; set; }
        public Boolean? Alcolique { get; set; }
        public TypeSang? typeSang { get; set; } // nullable enum
        public Patient? patient { get; set; } // rendre optionnel pour tester sans erreur
        public List<Consultation>? Consultations { get; set; }
        public List<Examen>? examens { get; set; }
        public List<Diagnostic>? diagnostics { get; set; }


    }
    public enum TypeSang
    {
        A_POSITIF,
        A_NEGATIF,
        B_POSITIF,
        B_NEGATIF,
        AB_POSITIF,
        AB_NEGATIF,
        O_POSITIF,
        O_NEGATIF
    }

}
