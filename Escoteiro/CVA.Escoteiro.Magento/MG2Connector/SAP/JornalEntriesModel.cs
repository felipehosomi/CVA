using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.Models.SAP
{
    public class JornalEntriesModel
    {
        public List<JornalEntriesLineModel> JournalEntryLines { get; set; }
    }

    public class JornalEntriesLineModel
    {
        [JsonIgnore]
        public int TransId { get; set; }
        public int Line_ID { get; set; }
        public string AdditionalReference { get; set; }
    }
}
