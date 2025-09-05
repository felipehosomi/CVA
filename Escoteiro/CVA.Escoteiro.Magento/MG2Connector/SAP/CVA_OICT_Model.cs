using System.Collections.Generic;

namespace CVA.Escoteiro.Magento.Models.SAP
{
    public class Value
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int U_ID { get; set; }
        public string U_Name { get; set; }
        public string U_ItemCode { get; set; }
    }

    public class CVA_OICT_Model
    {
        public string OdataMetadata { get; set; }
        public List<Value> value { get; set; }
    }
}
