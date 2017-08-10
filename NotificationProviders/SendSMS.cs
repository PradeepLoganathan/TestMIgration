using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace NotificationProviders
{
    public class SendSMS
    {
        public void SendMsg(string toNumber, string messageBody)
        {
            // Find your Account Sid and Auth Token at twilio.com/console
            const string accountSid = "ACedd03a99029b860e58cfc56bc3480b78";
            const string authToken = "4b16f759b7511075cad6c980ea54bc60";
            TwilioClient.Init(accountSid, authToken);
            var to = new PhoneNumber("+91" + toNumber);
            var message = MessageResource.Create(
              to,
              from: new PhoneNumber("+15612500996 "),
              body: messageBody
            );
            Console.WriteLine(message.Sid);
        }
    }
}
