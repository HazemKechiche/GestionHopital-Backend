using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TE.Core.Domain
{
    

    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public long phoneNumber     { get; set; }
        public string Password { get; set; }
        public string sexe { get; set; }
        public string adresse { get; set; }
        public DateTime dateNaissance { get; set; }
        

    }
    public class Medecin : User
    {
        public string specialite { get; set; }
        private string numLicence { get; set; }
        public List<Consultation>? consultationList { get; set; }
         

    }
    public class Patient : User
    {
       
       
      
        public List<Consultation>? consultationList { get; set; }
      
        public FicheMedical? ficheMedical { get; set; }
        public long? ficheMedicalId { get; set; }



    }
    public class Admin : User
    {
        public string idAdmin { get; set; }

    }

}