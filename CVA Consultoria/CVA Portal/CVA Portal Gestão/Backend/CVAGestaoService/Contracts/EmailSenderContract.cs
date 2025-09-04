using BLL.Classes;
using MODEL.Classes;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class EmailSenderContract
    {
        public EmailSenderBLL _emailSenderBLL { get; set; }

        public EmailSenderContract()
        {
            this._emailSenderBLL = new EmailSenderBLL();
        }

        public MessageModel SendEmail(string emailAddress)
        {
            return _emailSenderBLL.SendEmail(emailAddress);
        }
    }        
}