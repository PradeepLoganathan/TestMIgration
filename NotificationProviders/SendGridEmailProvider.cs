using System;
using System.Collections.Generic;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;
namespace NotificationProviders
{
    public class SendGridEmailProvider
    {
        
        public bool SendMail(string emailId, string userName, string subject, string msg,int msgType)
        {
            bool isMailSend = false;
            try
            {

                string body = constructMsg(msgType, msg);
                // encode the link and Name
                body = body.Replace("$USERNAME$", userName);

                body = body.Replace("$MSG$", msg);

                Execute(emailId, userName, subject, body).Wait();
                isMailSend = true;
            }
            catch (Exception)
            {
                return false;
            }

            return isMailSend;
        }
        public bool SendMail(string emailId,  string subject, string msg, int msgType)
        {
            bool isMailSend = false;
            try
            {

                string body = constructMsg(msgType, msg);
                // encode the link and Name

                body = body.Replace("$USERNAME$", "");
                body = body.Replace("$MSG$", msg);

                Execute(emailId, "", subject, body).Wait();
                isMailSend = true;
            }
            catch (Exception)
            {
                return false;
            }

            return isMailSend;
        }
        private string constructMsg(int msgType, string msg)
        {
            string bodymsg = string.Empty;
            switch(msgType)
            {
                case 1:
                    bodymsg = "Hi $USERNAME$, <br/> Click on the below link to confirm your registration,<br /> <br /> $MSG$ <br /> <p> Thanks, <br /> Proofring Team .<br />";
                    break;
                case 2:
                    bodymsg = "Hi $USERNAME$, <br/>  $MSG$ <br /> <p> Thanks, <br /> Proofring Team .<br />";
                    break;
                case 3:
                    bodymsg = "Hi $USERNAME$, <br/>  $MSG$ <br /> <p> Thanks, <br /> Proofring Team .<br />";
                    break;
                default:
                    bodymsg = "Hi $USERNAME$, <br/>  $MSG$ <br /> <p> Thanks, <br /> Proofring Team .<br />";
                    break;

            }

            return bodymsg;
        }
        private static async Task Execute(string toMailId, string toName, string subject, string body)
        {
            string apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("thiru@bootminds.com", "Example User");
            //var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(toMailId, toName);
            var plainTextContent = body;
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        private static string cretaeHash256Password(string password)
        {
            //SHA256Managed crypt = new SHA256Managed();
            //string enrPassword = String.Empty;
            //byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(password), 0, Encoding.ASCII.GetByteCount(password));
            //foreach (byte theByte in crypto)
            //{
            //    enrPassword += theByte.ToString("x2");
            //}
            //return enrPassword;
            // SHA256 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
