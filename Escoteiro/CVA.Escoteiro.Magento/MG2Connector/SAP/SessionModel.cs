using System;

namespace Escoteiro.Magento.Models
{
    public class SessionModel
    {
        public string SessionId { get; set; }
        public int SessionTimeout;
    }

    public class SessionDataBaseModel
    {
        public string DataBase;
        public DateTime SessionFinish;
        public SessionModel Session;
    }
}
