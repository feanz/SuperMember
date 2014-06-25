using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace SuperMember.Sample.Areas.Admin.Services
{
    public class SendGridIdentityEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridasync(message);
        }

        private Task ConfigSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(
                                ConfigurationManager.AppSettings["email.fromaddress"], ConfigurationManager.AppSettings["email.fromdisplay"]);

            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["email.username"],
                       ConfigurationManager.AppSettings["email.password"]
                       );

            var transportWeb = new Web(credentials);
            
            return transportWeb.DeliverAsync(myMessage);
        }
    }
}