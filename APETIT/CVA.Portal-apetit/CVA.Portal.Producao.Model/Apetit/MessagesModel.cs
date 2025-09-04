using System.Collections.Generic;

namespace CVA.Portal.Producao.Model
{
    public class MessagesModel
    {
        public MessagesModel()
        {
            MessageDataColumns = new List<Messagedatacolumn>();
            RecipientCollection = new List<Recipientcollection>();
        }
        public int Code { get; set; }
        public int User { get; set; }
        public string Priority { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public object Attachment { get; set; }
        public List<Messagedatacolumn> MessageDataColumns { get; set; }
        public List<Recipientcollection> RecipientCollection { get; set; }

        public class Messagedatacolumn
        {
            public Messagedatacolumn()
            {
                MessageDataLines = new List<Messagedataline>();
            }

            public string ColumnName { get; set; }
            public string Link { get; set; }
            public List<Messagedataline> MessageDataLines { get; set; }
        }

        public class Messagedataline
        {
            public string Value { get; set; }
            public string Object { get; set; }
            public string ObjectKey { get; set; }
        }

        public class Recipientcollection
        {
            public string UserCode { get; set; }
            public string UserType { get; set; }
            public string NameTo { get; set; }
            public string SendEmail { get; set; }
            public string EmailAddress { get; set; }
            public string SendSMS { get; set; }
            public string CellularNumber { get; set; }
            public string SendFax { get; set; }
            public string FaxNumber { get; set; }
            public string SendInternal { get; set; }
        }
    }

}
