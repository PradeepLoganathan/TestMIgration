using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationProviders;
namespace testmigration.Services
{
    public interface IEmailSender
    {
        
        
        Task SendEmailAsync(string email, string subject, string message,int msgType);

        Task SendEmailAsync(string email,string username, string subject, string message, int msgTyp);
    }
}
