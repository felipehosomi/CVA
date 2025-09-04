using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model.Producao
{
    public class DocumentoMarketingModel
    {
        public int? DocEntry { get; set; }
        public string DocDate { get; set; }
        
        public int BPL_IDAssignedToInvoice { get; set; }
        public List<Documentline> DocumentLines { get; set; }
        public string Comments { get; set; }
        
    }

    public class Documentline
    {
        public double Quantity { get; set; }
        public int? LineNum { get; set; }
        
        public int? BaseType { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }

        public string ItemCode { get; set; }
        public string OcrCode { get; set; }

        public List<Serialnumber> SerialNumbers { get; set; }
        public List<Batchnumber> BatchNumbers { get; set; }
        public string CFOPCode { get; set; }

        public int? AgreementNo { get; set; }
        public int? Usage { get; set; }
    }

    public class Serialnumber
    {
        public int BaseLineNumber { get; set; }
        public string InternalSerialNumber { get; set; }
        public string ManufacturerSerialNumber { get; set; }
        public int SystemSerialNumber { get; set; }
        public double Quantity { get; set; }
        public DateTime? ManufactureDate { get; set; }
    }

    public class Batchnumber
    {
        public int BaseLineNumber { get; set; }
        public string BatchNumber { get; set; }
        public string ManufacturerSerialNumber { get; set; }
        public double Quantity { get; set; }
        public DateTime? ManufacturingDate { get; set; }
    }

}
