using System.Collections.Generic;

namespace Escoteiro.Magento.Models
{
    public class IncomingPaymentsModel
    {
        public string odatametadata { get; set; }
        public int? DocEntry { get; set; }
        public string ObjectType { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string CardCode { get; set; }
        public string TaxDate { get; set; }
        public string DocTypte { get; set; }
        public string DueDate { get; set; }

        public string CashAccount { get; set; }
        public float CashSum { get; set; }

        public int BPLID { get; set; }
        public int U_CVA_EntityId { get; set; }
        public int U_CVA_Increment_id { get; set; }
        public List<Paymentinvoice> PaymentInvoices { get; set; }
        public List<Paymentcreditcard> PaymentCreditCards { get; set; }
    }

    public class Paymentinvoice
    {
        public int DocEntry { get; set; }
        public float SumApplied { get; set; }
        public string InvoiceType { get; set; }
        public int InstallmentId { get; set; }
    }

    public class Paymentcreditcard
    {
        public int CreditCard { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardValidUntil { get; set; }
        public string VoucherNum { get; set; }
        public string OwnerIdNum { get; set; }
        public int PaymentMethodCode { get; set; }
        public int NumOfPayments { get; set; }
        public float CreditSum { get; set; }
        public float FirstPaymentSum { get; set; }
        public float AdditionalPaymentSum { get; set; }
        public string SplitPayments { get; set; }
    }


}
