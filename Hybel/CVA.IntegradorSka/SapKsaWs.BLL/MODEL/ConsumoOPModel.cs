using DelimitedDataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapKsaWs.BLL.MODEL
{
    public class ConsumoOPModel
    {
        public string TransactionType { get; set; }
        public string DocumentValidDate { get; set; }
        public string DocumentRemarks { get; set; }
        public string BusinessPartner { get; set; }
        public string Project { get; set; }
        public string LinkDocumentType { get; set; }
        public string LinkDocument { get; set; }
        public int LinkLine1 { get; set; }
        public int LinkLine2 { get; set; }
        public string Itemcode { get; set; }
        public decimal Quantity { get; set; }
        public string Warehouse { get; set; }
        public string Bincode { get; set; }
        public string RFID { get; set; }
        public string Version { get; set; }
        public string Batchnumber { get; set; }
        public string SerialNumberInternal { get; set; }
        public string BatchAttribute1 { get; set; }
        public string BatchAttribute2 { get; set; }
        public string DateOfEntry { get; set; }
        public string ManufacturingDate { get; set; }
        public string ExpirDate { get; set; }
        public string PersonnelNumber { get; set; }
        public string UserWEBUser { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string WorkStation { get; set; }
        public string Userfield1 { get; set; }
        public string Userfield2 { get; set; }
        public string Userfield3 { get; set; }
        public string Userfield4 { get; set; }
        public string Freetext { get; set; }
        [FileConfig(Ignore = true)]
        public decimal BatchQuantity { get; set; }
    }
}
