using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Domain;
using TE.Data;

namespace TE.Core.Services
{
    public class NotificationService
    {
        private readonly TEContext _context;
        public NotificationService(TEContext context)
        {
            _context = context;
        }

        public Boolean sendNotif(long userId, string message, string titre, TypeNotification typeNotification)
        {
            var User = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (User == null)
            {
                return false;
            }
            Notification notification = new Notification();
            notification.Message = message;
            notification.Type = typeNotification;
            notification.Titre = titre;
            notification.Destinataire = User;
            notification.DestinataireId = userId;
            notification.EstLue = false;
            notification.DateEnvoi = DateTime.Now;
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return true;
        }
    }
}
