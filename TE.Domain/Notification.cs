using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE.Core.Domain
{
   
    public class Notification
    {
        [Key]
        public long Id { get; set; }

        public string Titre { get; set; }
        public string Message { get; set; }

        public bool EstLue { get; set; }

        public DateTime DateEnvoi { get; set; }

        public TypeNotification Type { get; set; }

        public long DestinataireId { get; set; }
        public User Destinataire { get; set; }
    }
    public enum TypeNotification
    {
        Info,
        Alerte,
        Systeme
    }
}
