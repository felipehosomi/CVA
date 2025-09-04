using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.Models
{
    public class LcmModel
    {
        public List<LcmLineModel> JournalEntryLines { get; set; }
    }

    public class LcmLineModel
    {
        [JsonIgnore]
        public int TransId { get; set; }
        public int Line_ID { get; set; }
        public string AdditionalReference { get; set; }
    }
}