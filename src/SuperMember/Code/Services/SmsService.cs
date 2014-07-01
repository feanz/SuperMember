using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SuperMember.Sample.Code.Interfaces;

namespace SuperMember.Sample.Code.Services
{
    public class SmsService : ISmsIdentityService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}