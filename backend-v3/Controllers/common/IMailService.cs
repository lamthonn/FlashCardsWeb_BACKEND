using Org.BouncyCastle.Asn1.Pkcs;

namespace backend_v3.Controllers.common
{
    public interface IMailService
    {
        public Task SendMail(MailData mailData);
    }
}
