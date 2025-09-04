using SBO.Hub.Attributes;
using System;

namespace CVA.Cointer.Core.Model
{
    public class ReturnInvoiceModel
    {
        [HubModel(UserDataSource = "ud_DocDate", Description = "Data Lançamento", MandatoryYN = true)]
        public DateTime DocDate { get; set; }
        [HubModel(UserDataSource = "ud_DueDate", Description = "Data Vencimento", MandatoryYN = true)]
        public DateTime DueDate { get; set; }
        [HubModel(UserDataSource = "ud_TaxDate", Description = "Data Documento", MandatoryYN = true)]
        public DateTime TaxDate { get; set; }
        [HubModel(UserDataSource = "ud_Group")]
        public string GroupItensYN { get; set; }
    }
}
