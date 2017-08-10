using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationProviders;
namespace testmigration.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message, int msgType)
        {
            // Plug in your email service here to send an email.
            SendGridEmailProvider sendGridEmailProvider = new SendGridEmailProvider();
            sendGridEmailProvider.SendMail(email, subject, message, msgType);
            return Task.FromResult(0);
        }
        
        public Task SendEmailAsync(string email,string userName, string subject, string message,int msgtype)
        {
            // Plug in your email service here to send an email.
            SendGridEmailProvider sendGridEmailProvider = new SendGridEmailProvider();
            sendGridEmailProvider.SendMail(email, userName, subject, message,msgtype);
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            SendSMS sendSMS = new SendSMS();
            sendSMS.SendMsg(number, message);
            return Task.FromResult(0);
        }
    }
}
