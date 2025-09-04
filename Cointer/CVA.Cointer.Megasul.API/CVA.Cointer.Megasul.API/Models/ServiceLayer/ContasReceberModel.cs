using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.Models.ServiceLayer
{
    public class ContasReceberModel
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocType { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public object Remarks { get; set; }
        public string PaymentType { get; set; }
        public DateTime DueDate { get; set; }
        public int BPLID { get; set; }

        public string TransferAccount { get; set; }
        public DateTime? TransferDate { get; set; }
        public double? TransferSum { get; set; }

        public string CashAccount { get; set; }
        public double? CashSum { get; set; }

        //public string BoeAccount { get; set; }
        public double? BillOfExchangeAmount { get; set; }

        public List<Paymentinvoice> PaymentInvoices { get; set; }
        public List<Paymentcreditcard> PaymentCreditCards { get; set; }

        public BillOfExchange BillOfExchange { get; set; }
    }

    public class Paymentinvoice
    {
        public int DocEntry { get; set; }
        public string InvoiceType { get; set; }
        public int InstallmentId { get; set; }
    }

    public class BillOfExchange
    {
        public string PaymentMethodCode { get; set; }
    }

    public class Paymentcreditcard
    {
        public int LineNum { get; set; }
        public int CreditCard { get; set; }
        public string CreditAcct { get; set; }
        public string CreditCardNumber { get; set; }
        public DateTime CardValidUntil { get; set; }
        public string VoucherNum { get; set; }
        public object OwnerIdNum { get; set; }
        public object OwnerPhone { get; set; }
        public int PaymentMethodCode { get; set; }
        public int NumOfPayments { get; set; }
        public DateTime FirstPaymentDue { get; set; }
        public double FirstPaymentSum { get; set; }
        public double AdditionalPaymentSum { get; set; }
        public double CreditSum { get; set; }
        public string CreditCur { get; set; }
        public object ConfirmationNum { get; set; }
        public int NumOfCreditPayments { get; set; }
        public string CreditType { get; set; }

        public string SplitPayments { get; set; } = "tYES";
    }
}