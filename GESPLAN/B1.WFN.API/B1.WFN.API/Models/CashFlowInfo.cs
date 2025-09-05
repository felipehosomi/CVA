using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace B1.WFN.API.Models
{
    enum PartType
    {
        Header = 'H',
        Financial = 'F',
        Accounting = 'C'
    }

    public class CashFlowInfoHeader
    {
        [JsonProperty(PropertyName = "items")]
        public List<Header> Headers { get; set; }
    }

    public class CashFlowInfoFinancialOpening
    {
        [JsonProperty(PropertyName = "items")]
        public List<FinancialOpening> FinancialOpenings { get; set; }
    }

    public class CashFlowInfoAccountingOpening
    {
        [JsonProperty(PropertyName = "items")]
        public List<AccountingOpening> AccountingOpenings { get; set; }
    }

    public class Header
    {
        [JsonProperty(PropertyName = "integrationType", NullValueHandling = NullValueHandling.Ignore)]
        public string IntegrationType { get; set; }

        [JsonProperty(PropertyName = "externalCode")]
        public string DocEntry { get; set; }

        [JsonProperty(PropertyName = "businessUnit")]
        public string BranchId { get; set; }

        [JsonProperty(PropertyName = "originSystem")]
        public string TransType { get; set; }

        [JsonProperty(PropertyName = "dateOfIssue")]
        public string DocDate { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string JournalRemarks { get; set; }

        [JsonProperty(PropertyName = "documentType")]
        public string DocType { get; set; }

        [JsonProperty(PropertyName = "documentNumber")]
        public string Serial { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string DocCurrency { get; set; }

        [JsonProperty(PropertyName = "scenario", NullValueHandling = NullValueHandling.Ignore)]
        public string Scenario { get; set; }

        [JsonProperty(PropertyName = "flexField001", NullValueHandling = NullValueHandling.Ignore)]
        public string InstallmentNum { get; set; }

        [JsonProperty(PropertyName = "flexField002", NullValueHandling = NullValueHandling.Ignore)]
        public string DocTotal { get; set; }

        [JsonProperty(PropertyName = "flexField003", NullValueHandling = NullValueHandling.Ignore)]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "flexField004", NullValueHandling = NullValueHandling.Ignore)]
        public string U_GrupoDespesa { get; set; }
        [JsonProperty(PropertyName = "flexField005", NullValueHandling = NullValueHandling.Ignore)]
        public string U_SubGrupoDespesa { get; set; }

    }

    public class FinancialOpening
    {
        [JsonProperty(PropertyName = "integrationType", NullValueHandling = NullValueHandling.Ignore)]
        public string IntegrationType { get; set; }

        [JsonProperty(PropertyName = "externalCode")]
        public string InstallmentId { get; set; }

        [JsonProperty(PropertyName = "cashPosting")]
        public string DocEntry { get; set; }

        [JsonProperty(PropertyName = "dueDate")]
        public string InstDueDate { get; set; }

        [JsonProperty(PropertyName = "payday")]
        public string PaymentDate { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "currentAccount")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "scenario", NullValueHandling = NullValueHandling.Ignore)]
        public string Scenario { get; set; }

        [JsonProperty(PropertyName = "beneficiary", NullValueHandling = NullValueHandling.Ignore)]
        public string CardCode { get; set; }

        [JsonProperty(PropertyName = "beneficiaryType", NullValueHandling = NullValueHandling.Ignore)]
        public string CardType { get; set; }

        [JsonProperty(PropertyName = "motionWay", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        [JsonProperty(PropertyName = "flexField001", NullValueHandling = NullValueHandling.Ignore)]
        public string BoeNum { get; set; }

        [JsonProperty(PropertyName = "flexField002", NullValueHandling = NullValueHandling.Ignore)]
        public string PaidSum { get; set; }
    }

    public class AccountingOpening
    {
        [JsonProperty(PropertyName = "integrationType", NullValueHandling = NullValueHandling.Ignore)]
        public string IntegrationType { get; set; }

        [JsonProperty(PropertyName = "externalCode")]
        public string Line_ID { get; set; }

        [JsonProperty(PropertyName = "cashPosting")]
        public string DocEntry { get; set; }

        [JsonProperty(PropertyName = "scenario", NullValueHandling = NullValueHandling.Ignore)]
        public string Scenario { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "accountingAccount")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "costCenter")]
        public string CostCenter { get; set; }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        [JsonProperty(PropertyName = "flexField001", NullValueHandling = NullValueHandling.Ignore)]
        public string RefDate { get; set; }

        [JsonProperty(PropertyName = "flexField002", NullValueHandling = NullValueHandling.Ignore)]
        public string DueDate { get; set; }

        [JsonProperty(PropertyName = "flexField003", NullValueHandling = NullValueHandling.Ignore)]
        public string ItemCode { get; set; }
    }
}
