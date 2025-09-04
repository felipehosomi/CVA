using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrder.Models.SAP
{
    public class Metadata_IncomingPayment
    {
        public class IncomingPayment
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
            public double CashSum { get; set; }
            public string TransferAccount { get; set; }
            public double TransferSum { get; set; }

            public int? BPLID { get; set; }
            public List<Paymentinvoice> PaymentInvoices { get; set; }
            public List<Paymentcreditcard> PaymentCreditCards { get; set; }
        }

        public class Paymentinvoice
        {
            public int DocEntry { get; set; }
            public double SumApplied { get; set; }
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
            public double CreditSum { get; set; }
            public double FirstPaymentSum { get; set; }
            public double AdditionalPaymentSum { get; set; }
            public string SplitPayments { get; set; }
        }
    }
}
