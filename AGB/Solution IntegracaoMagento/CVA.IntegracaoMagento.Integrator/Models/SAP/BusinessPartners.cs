using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.Integrator.Models.SAP
{
    public partial class BusinessPartners
    {
        [JsonProperty("BusinessPartners")]
        public BusinessPartners BusinessPartner { get; set; }

        [JsonProperty("BP Code")]
        public string BPCode { get; set; }

        [JsonProperty("CardCode")]
        public string CardCode { get; set; }

        [JsonProperty("CardName")]
        public string CardName { get; set; }

        [JsonProperty("CardType")]
        public string CardType { get; set; }

        [JsonProperty("GroupCode")]
        public int? GroupCode { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("ZipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("MailAddress")]
        public string MailAddress { get; set; }

        [JsonProperty("MailZipCode")]
        public string MailZipCode { get; set; }

        [JsonProperty("Phone1")]
        public string Phone1 { get; set; }

        [JsonProperty("Phone2")]
        public string Phone2 { get; set; }

        [JsonProperty("Fax")]
        public object Fax { get; set; }

        [JsonProperty("ContactPerson")]
        public object ContactPerson { get; set; }

        [JsonProperty("Notes")]
        public object Notes { get; set; }

        [JsonProperty("PayTermsGrpCode")]
        public long? PayTermsGrpCode { get; set; }

        [JsonProperty("CreditLimit")]
        public long? CreditLimit { get; set; }

        [JsonProperty("MaxCommitment")]
        public long? MaxCommitment { get; set; }

        [JsonProperty("DiscountPercent")]
        public long? DiscountPercent { get; set; }

        [JsonProperty("VatLiable")]
        public string VatLiable { get; set; }

        [JsonProperty("FederalTaxID")]
        public object FederalTaxId { get; set; }

        [JsonProperty("DeductibleAtSource")]
        public string DeductibleAtSource { get; set; }

        [JsonProperty("DeductionPercent")]
        public long? DeductionPercent { get; set; }

        [JsonProperty("DeductionValidUntil")]
        public object DeductionValidUntil { get; set; }

        [JsonProperty("PriceListNum")]
        public long? PriceListNum { get; set; }

        [JsonProperty("IntrestRatePercent")]
        public long? IntrestRatePercent { get; set; }

        [JsonProperty("CommissionPercent")]
        public long? CommissionPercent { get; set; }

        [JsonProperty("CommissionGroupCode")]
        public long? CommissionGroupCode { get; set; }

        [JsonProperty("FreeText")]
        public object FreeText { get; set; }

        [JsonProperty("SalesPersonCode")]
        public long? SalesPersonCode { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("RateDiffAccount")]
        public object RateDiffAccount { get; set; }

        [JsonProperty("Cellular")]
        public object Cellular { get; set; }

        [JsonProperty("AvarageLate")]
        public object AvarageLate { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("MailCity")]
        public string MailCity { get; set; }

        [JsonProperty("MailCounty")]
        public string MailCounty { get; set; }

        [JsonProperty("MailCountry")]
        public string MailCountry { get; set; }

        [JsonProperty("EmailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("Picture")]
        public object Picture { get; set; }

        [JsonProperty("DefaultAccount")]
        public object DefaultAccount { get; set; }

        [JsonProperty("DefaultBranch")]
        public object DefaultBranch { get; set; }

        [JsonProperty("DefaultBankCode")]
        public string DefaultBankCode { get; set; }

        [JsonProperty("AdditionalID")]
        public object AdditionalId { get; set; }

        [JsonProperty("Pager")]
        public object Pager { get; set; }

        [JsonProperty("FatherCard")]
        public object FatherCard { get; set; }

        [JsonProperty("CardForeignName")]
        public object CardForeignName { get; set; }

        [JsonProperty("FatherType")]
        public string FatherType { get; set; }

        [JsonProperty("DeductionOffice")]
        public object DeductionOffice { get; set; }

        [JsonProperty("ExportCode")]
        public object ExportCode { get; set; }

        [JsonProperty("MinIntrest")]
        public long? MinIntrest { get; set; }

        [JsonProperty("CurrentAccountBalance")]
        public long? CurrentAccountBalance { get; set; }

        [JsonProperty("OpenDeliveryNotesBalance")]
        public long? OpenDeliveryNotesBalance { get; set; }

        [JsonProperty("OpenOrdersBalance")]
        public long? OpenOrdersBalance { get; set; }

        [JsonProperty("VatGroup")]
        public object VatGroup { get; set; }

        [JsonProperty("ShippingType")]
        public object ShippingType { get; set; }

        [JsonProperty("Password")]
        public object Password { get; set; }

        [JsonProperty("Indicator")]
        public object Indicator { get; set; }

        [JsonProperty("IBAN")]
        public object Iban { get; set; }

        [JsonProperty("CreditCardCode")]
        public long? CreditCardCode { get; set; }

        [JsonProperty("CreditCardNum")]
        public object CreditCardNum { get; set; }

        [JsonProperty("CreditCardExpiration")]
        public object CreditCardExpiration { get; set; }

        [JsonProperty("DebitorAccount")]
        public string DebitorAccount { get; set; }

        [JsonProperty("OpenOpportunities")]
        public object OpenOpportunities { get; set; }

        [JsonProperty("Valid")]
        public string Valid { get; set; }

        [JsonProperty("ValidFrom")]
        public object ValidFrom { get; set; }

        [JsonProperty("ValidTo")]
        public object ValidTo { get; set; }

        [JsonProperty("ValidRemarks")]
        public object ValidRemarks { get; set; }

        [JsonProperty("Frozen")]
        public string Frozen { get; set; }

        [JsonProperty("FrozenFrom")]
        public object FrozenFrom { get; set; }

        [JsonProperty("FrozenTo")]
        public object FrozenTo { get; set; }

        [JsonProperty("FrozenRemarks")]
        public object FrozenRemarks { get; set; }

        [JsonProperty("Block")]
        public string Block { get; set; }

        [JsonProperty("BillToState")]
        public string BillToState { get; set; }

        [JsonProperty("ExemptNum")]
        public object ExemptNum { get; set; }

        [JsonProperty("Priority")]
        public long? Priority { get; set; }

        [JsonProperty("FormCode1099")]
        public object FormCode1099 { get; set; }

        [JsonProperty("Box1099")]
        public object Box1099 { get; set; }

        [JsonProperty("PeymentMethodCode")]
        public object PeymentMethodCode { get; set; }

        [JsonProperty("BackOrder")]
        public string BackOrder { get; set; }

        [JsonProperty("PartialDelivery")]
        public string PartialDelivery { get; set; }

        [JsonProperty("BlockDunning")]
        public string BlockDunning { get; set; }

        [JsonProperty("BankCountry")]
        public object BankCountry { get; set; }

        [JsonProperty("HouseBank")]
        public object HouseBank { get; set; }

        [JsonProperty("HouseBankCountry")]
        public string HouseBankCountry { get; set; }

        [JsonProperty("HouseBankAccount")]
        public object HouseBankAccount { get; set; }

        [JsonProperty("ShipToDefault")]
        public string ShipToDefault { get; set; }

        [JsonProperty("DunningLevel")]
        public object DunningLevel { get; set; }

        [JsonProperty("DunningDate")]
        public object DunningDate { get; set; }

        [JsonProperty("CollectionAuthorization")]
        public string CollectionAuthorization { get; set; }

        [JsonProperty("DME")]
        public object Dme { get; set; }

        [JsonProperty("InstructionKey")]
        public object InstructionKey { get; set; }

        [JsonProperty("SinglePayment")]
        public string SinglePayment { get; set; }

        [JsonProperty("ISRBillerID")]
        public object IsrBillerId { get; set; }

        [JsonProperty("PaymentBlock")]
        public string PaymentBlock { get; set; }

        [JsonProperty("ReferenceDetails")]
        public object ReferenceDetails { get; set; }

        [JsonProperty("HouseBankBranch")]
        public object HouseBankBranch { get; set; }

        [JsonProperty("OwnerIDNumber")]
        public object OwnerIdNumber { get; set; }

        [JsonProperty("PaymentBlockDescription")]
        public long? PaymentBlockDescription { get; set; }

        [JsonProperty("TaxExemptionLetterNum")]
        public object TaxExemptionLetterNum { get; set; }

        [JsonProperty("MaxAmountOfExemption")]
        public long? MaxAmountOfExemption { get; set; }

        [JsonProperty("ExemptionValidityDateFrom")]
        public object ExemptionValidityDateFrom { get; set; }

        [JsonProperty("ExemptionValidityDateTo")]
        public object ExemptionValidityDateTo { get; set; }

        [JsonProperty("LinkedBusinessPartner")]
        public object LinkedBusinessPartner { get; set; }

        [JsonProperty("LastMultiReconciliationNum")]
        public object LastMultiReconciliationNum { get; set; }

        [JsonProperty("DeferredTax")]
        public string DeferredTax { get; set; }

        [JsonProperty("Equalization")]
        public string Equalization { get; set; }

        [JsonProperty("SubjectToWithholdingTax")]
        public string SubjectToWithholdingTax { get; set; }

        [JsonProperty("CertificateNumber")]
        public object CertificateNumber { get; set; }

        [JsonProperty("ExpirationDate")]
        public object ExpirationDate { get; set; }

        [JsonProperty("NationalInsuranceNum")]
        public object NationalInsuranceNum { get; set; }

        [JsonProperty("AccrualCriteria")]
        public string AccrualCriteria { get; set; }

        [JsonProperty("WTCode")]
        public object WtCode { get; set; }

        [JsonProperty("BillToBuildingFloorRoom")]
        public string BillToBuildingFloorRoom { get; set; }

        [JsonProperty("DownPaymentClearAct")]
        public string DownPaymentClearAct { get; set; }

        [JsonProperty("ChannelBP")]
        public object ChannelBp { get; set; }

        [JsonProperty("DefaultTechnician")]
        public object DefaultTechnician { get; set; }

        [JsonProperty("BilltoDefault")]
        public string BilltoDefault { get; set; }

        [JsonProperty("CustomerBillofExchangDisc")]
        public object CustomerBillofExchangDisc { get; set; }

        [JsonProperty("Territory")]
        public object Territory { get; set; }

        [JsonProperty("ShipToBuildingFloorRoom")]
        public string ShipToBuildingFloorRoom { get; set; }

        [JsonProperty("CustomerBillofExchangPres")]
        public object CustomerBillofExchangPres { get; set; }

        [JsonProperty("ProjectCode")]
        public object ProjectCode { get; set; }

        [JsonProperty("VatGroupLatinAmerica")]
        public object VatGroupLatinAmerica { get; set; }

        [JsonProperty("DunningTerm")]
        public object DunningTerm { get; set; }

        [JsonProperty("Website")]
        public object Website { get; set; }

        [JsonProperty("OtherReceivablePayable")]
        public object OtherReceivablePayable { get; set; }

        [JsonProperty("BillofExchangeonCollection")]
        public object BillofExchangeonCollection { get; set; }

        [JsonProperty("CompanyPrivate")]
        public string CompanyPrivate { get; set; }

        [JsonProperty("LanguageCode")]
        public long? LanguageCode { get; set; }

        [JsonProperty("UnpaidBillofExchange")]
        public object UnpaidBillofExchange { get; set; }

        [JsonProperty("WithholdingTaxDeductionGroup")]
        public long? WithholdingTaxDeductionGroup { get; set; }

        [JsonProperty("ClosingDateProcedureNumber")]
        public object ClosingDateProcedureNumber { get; set; }

        [JsonProperty("Profession")]
        public object Profession { get; set; }

        [JsonProperty("BankChargesAllocationCode")]
        public object BankChargesAllocationCode { get; set; }

        [JsonProperty("TaxRoundingRule")]
        public string TaxRoundingRule { get; set; }

        [JsonProperty("Properties1")]
        public string Properties1 { get; set; }

        [JsonProperty("Properties2")]
        public string Properties2 { get; set; }

        [JsonProperty("Properties3")]
        public string Properties3 { get; set; }

        [JsonProperty("Properties4")]
        public string Properties4 { get; set; }

        [JsonProperty("Properties5")]
        public string Properties5 { get; set; }

        [JsonProperty("Properties6")]
        public string Properties6 { get; set; }

        [JsonProperty("Properties7")]
        public string Properties7 { get; set; }

        [JsonProperty("Properties8")]
        public string Properties8 { get; set; }

        [JsonProperty("Properties9")]
        public string Properties9 { get; set; }

        [JsonProperty("Properties10")]
        public string Properties10 { get; set; }

        [JsonProperty("Properties11")]
        public string Properties11 { get; set; }

        [JsonProperty("Properties12")]
        public string Properties12 { get; set; }

        [JsonProperty("Properties13")]
        public string Properties13 { get; set; }

        [JsonProperty("Properties14")]
        public string Properties14 { get; set; }

        [JsonProperty("Properties15")]
        public string Properties15 { get; set; }

        [JsonProperty("Properties16")]
        public string Properties16 { get; set; }

        [JsonProperty("Properties17")]
        public string Properties17 { get; set; }

        [JsonProperty("Properties18")]
        public string Properties18 { get; set; }

        [JsonProperty("Properties19")]
        public string Properties19 { get; set; }

        [JsonProperty("Properties20")]
        public string Properties20 { get; set; }

        [JsonProperty("Properties21")]
        public string Properties21 { get; set; }

        [JsonProperty("Properties22")]
        public string Properties22 { get; set; }

        [JsonProperty("Properties23")]
        public string Properties23 { get; set; }

        [JsonProperty("Properties24")]
        public string Properties24 { get; set; }

        [JsonProperty("Properties25")]
        public string Properties25 { get; set; }

        [JsonProperty("Properties26")]
        public string Properties26 { get; set; }

        [JsonProperty("Properties27")]
        public string Properties27 { get; set; }

        [JsonProperty("Properties28")]
        public string Properties28 { get; set; }

        [JsonProperty("Properties29")]
        public string Properties29 { get; set; }

        [JsonProperty("Properties30")]
        public string Properties30 { get; set; }

        [JsonProperty("Properties31")]
        public string Properties31 { get; set; }

        [JsonProperty("Properties32")]
        public string Properties32 { get; set; }

        [JsonProperty("Properties33")]
        public string Properties33 { get; set; }

        [JsonProperty("Properties34")]
        public string Properties34 { get; set; }

        [JsonProperty("Properties35")]
        public string Properties35 { get; set; }

        [JsonProperty("Properties36")]
        public string Properties36 { get; set; }

        [JsonProperty("Properties37")]
        public string Properties37 { get; set; }

        [JsonProperty("Properties38")]
        public string Properties38 { get; set; }

        [JsonProperty("Properties39")]
        public string Properties39 { get; set; }

        [JsonProperty("Properties40")]
        public string Properties40 { get; set; }

        [JsonProperty("Properties41")]
        public string Properties41 { get; set; }

        [JsonProperty("Properties42")]
        public string Properties42 { get; set; }

        [JsonProperty("Properties43")]
        public string Properties43 { get; set; }

        [JsonProperty("Properties44")]
        public string Properties44 { get; set; }

        [JsonProperty("Properties45")]
        public string Properties45 { get; set; }

        [JsonProperty("Properties46")]
        public string Properties46 { get; set; }

        [JsonProperty("Properties47")]
        public string Properties47 { get; set; }

        [JsonProperty("Properties48")]
        public string Properties48 { get; set; }

        [JsonProperty("Properties49")]
        public string Properties49 { get; set; }

        [JsonProperty("Properties50")]
        public string Properties50 { get; set; }

        [JsonProperty("Properties51")]
        public string Properties51 { get; set; }

        [JsonProperty("Properties52")]
        public string Properties52 { get; set; }

        [JsonProperty("Properties53")]
        public string Properties53 { get; set; }

        [JsonProperty("Properties54")]
        public string Properties54 { get; set; }

        [JsonProperty("Properties55")]
        public string Properties55 { get; set; }

        [JsonProperty("Properties56")]
        public string Properties56 { get; set; }

        [JsonProperty("Properties57")]
        public string Properties57 { get; set; }

        [JsonProperty("Properties58")]
        public string Properties58 { get; set; }

        [JsonProperty("Properties59")]
        public string Properties59 { get; set; }

        [JsonProperty("Properties60")]
        public string Properties60 { get; set; }

        [JsonProperty("Properties61")]
        public string Properties61 { get; set; }

        [JsonProperty("Properties62")]
        public string Properties62 { get; set; }

        [JsonProperty("Properties63")]
        public string Properties63 { get; set; }

        [JsonProperty("Properties64")]
        public string Properties64 { get; set; }

        [JsonProperty("CompanyRegistrationNumber")]
        public object CompanyRegistrationNumber { get; set; }

        [JsonProperty("VerificationNumber")]
        public object VerificationNumber { get; set; }

        [JsonProperty("DiscountBaseObject")]
        public string DiscountBaseObject { get; set; }

        [JsonProperty("DiscountRelations")]
        public string DiscountRelations { get; set; }

        [JsonProperty("TypeReport")]
        public string TypeReport { get; set; }

        [JsonProperty("ThresholdOverlook")]
        public string ThresholdOverlook { get; set; }

        [JsonProperty("SurchargeOverlook")]
        public string SurchargeOverlook { get; set; }

        [JsonProperty("DownPaymentInterimAccount")]
        public object DownPaymentInterimAccount { get; set; }

        [JsonProperty("OperationCode347")]
        public string OperationCode347 { get; set; }

        [JsonProperty("InsuranceOperation347")]
        public string InsuranceOperation347 { get; set; }

        [JsonProperty("HierarchicalDeduction")]
        public string HierarchicalDeduction { get; set; }

        [JsonProperty("ShaamGroup")]
        public string ShaamGroup { get; set; }

        [JsonProperty("WithholdingTaxCertified")]
        public string WithholdingTaxCertified { get; set; }

        [JsonProperty("BookkeepingCertified")]
        public string BookkeepingCertified { get; set; }

        [JsonProperty("PlanningGroup")]
        public object PlanningGroup { get; set; }

        [JsonProperty("Affiliate")]
        public string Affiliate { get; set; }

        [JsonProperty("Industry")]
        public object Industry { get; set; }

        [JsonProperty("VatIDNum")]
        public object VatIdNum { get; set; }

        [JsonProperty("DatevAccount")]
        public object DatevAccount { get; set; }

        [JsonProperty("DatevFirstDataEntry")]
        public string DatevFirstDataEntry { get; set; }

        [JsonProperty("UseShippedGoodsAccount")]
        public string UseShippedGoodsAccount { get; set; }

        [JsonProperty("GTSRegNo")]
        public object GtsRegNo { get; set; }

        [JsonProperty("GTSBankAccountNo")]
        public object GtsBankAccountNo { get; set; }

        [JsonProperty("GTSBillingAddrTel")]
        public object GtsBillingAddrTel { get; set; }

        [JsonProperty("ETaxWebSite")]
        public object ETaxWebSite { get; set; }

        [JsonProperty("HouseBankIBAN")]
        public string HouseBankIban { get; set; }

        [JsonProperty("VATRegistrationNumber")]
        public object VatRegistrationNumber { get; set; }

        [JsonProperty("RepresentativeName")]
        public object RepresentativeName { get; set; }

        [JsonProperty("IndustryType")]
        public object IndustryType { get; set; }

        [JsonProperty("BusinessType")]
        public object BusinessType { get; set; }

        [JsonProperty("Series")]
        public long? Series { get; set; }

        [JsonProperty("AutomaticPosting")]
        public string AutomaticPosting { get; set; }

        [JsonProperty("InterestAccount")]
        public object InterestAccount { get; set; }

        [JsonProperty("FeeAccount")]
        public object FeeAccount { get; set; }

        [JsonProperty("CampaignNumber")]
        public object CampaignNumber { get; set; }

        [JsonProperty("AliasName")]
        public string AliasName { get; set; }

        [JsonProperty("DefaultBlanketAgreementNumber")]
        public object DefaultBlanketAgreementNumber { get; set; }

        [JsonProperty("EffectiveDiscount")]
        public string EffectiveDiscount { get; set; }

        [JsonProperty("NoDiscounts")]
        public string NoDiscounts { get; set; }

        [JsonProperty("EffectivePrice")]
        public string EffectivePrice { get; set; }

        [JsonProperty("GlobalLocationNumber")]
        public object GlobalLocationNumber { get; set; }

        [JsonProperty("EDISenderID")]
        public object EdiSenderId { get; set; }

        [JsonProperty("EDIRecipientID")]
        public object EdiRecipientId { get; set; }

        [JsonProperty("ResidenNumber")]
        public string ResidenNumber { get; set; }

        [JsonProperty("RelationshipCode")]
        public object RelationshipCode { get; set; }

        [JsonProperty("RelationshipDateFrom")]
        public object RelationshipDateFrom { get; set; }

        [JsonProperty("RelationshipDateTill")]
        public object RelationshipDateTill { get; set; }

        [JsonProperty("UnifiedFederalTaxID")]
        public object UnifiedFederalTaxId { get; set; }

        [JsonProperty("AttachmentEntry")]
        public object AttachmentEntry { get; set; }

        [JsonProperty("TypeOfOperation")]
        public object TypeOfOperation { get; set; }

        [JsonProperty("EndorsableChecksFromBP")]
        public string EndorsableChecksFromBp { get; set; }

        [JsonProperty("AcceptsEndorsedChecks")]
        public string AcceptsEndorsedChecks { get; set; }

        [JsonProperty("OwnerCode")]
        public object OwnerCode { get; set; }

        [JsonProperty("BlockSendingMarketingContent")]
        public string BlockSendingMarketingContent { get; set; }

        [JsonProperty("AgentCode")]
        public object AgentCode { get; set; }

        [JsonProperty("PriceMode")]
        public object PriceMode { get; set; }

        [JsonProperty("EDocGenerationType")]
        public object EDocGenerationType { get; set; }

        [JsonProperty("EDocStreet")]
        public object EDocStreet { get; set; }

        [JsonProperty("EDocStreetNumber")]
        public object EDocStreetNumber { get; set; }

        [JsonProperty("EDocBuildingNumber")]
        public object EDocBuildingNumber { get; set; }

        [JsonProperty("EDocZipCode")]
        public object EDocZipCode { get; set; }

        [JsonProperty("EDocCity")]
        public object EDocCity { get; set; }

        [JsonProperty("EDocCountry")]
        public object EDocCountry { get; set; }

        [JsonProperty("EDocDistrict")]
        public object EDocDistrict { get; set; }

        [JsonProperty("EDocRepresentativeFirstName")]
        public object EDocRepresentativeFirstName { get; set; }

        [JsonProperty("EDocRepresentativeSurname")]
        public object EDocRepresentativeSurname { get; set; }

        [JsonProperty("EDocRepresentativeCompany")]
        public object EDocRepresentativeCompany { get; set; }

        [JsonProperty("EDocRepresentativeFiscalCode")]
        public object EDocRepresentativeFiscalCode { get; set; }

        [JsonProperty("EDocRepresentativeAdditionalId")]
        public object EDocRepresentativeAdditionalId { get; set; }

        [JsonProperty("EDocPECAddress")]
        public object EDocPecAddress { get; set; }

        [JsonProperty("IPACodeForPA")]
        public object IpaCodeForPa { get; set; }

        [JsonProperty("UpdateDate")]
        public object UpdateDate { get; set; }

        [JsonProperty("UpdateTime")]
        public object UpdateTime { get; set; }

        [JsonProperty("ExemptionMaxAmountValidationType")]
        public string ExemptionMaxAmountValidationType { get; set; }

        [JsonProperty("ECommerceMerchantID")]
        public object ECommerceMerchantId { get; set; }

        [JsonProperty("UseBillToAddrToDetermineTax")]
        public string UseBillToAddrToDetermineTax { get; set; }

        [JsonProperty("CreateDate")]
        public object CreateDate { get; set; }

        [JsonProperty("CreateTime")]
        public object CreateTime { get; set; }

        [JsonProperty("DefaultTransporterEntry")]
        public object DefaultTransporterEntry { get; set; }

        [JsonProperty("DefaultTransporterLineNumber")]
        public object DefaultTransporterLineNumber { get; set; }

        [JsonProperty("FCERelevant")]
        public string FceRelevant { get; set; }

        /*
        [JsonProperty("U_CVA_FlexyIdNum")]
        public string U_CVA_FlexyIdNum { get; set; }

        [JsonProperty("U_CVA_Source")]
        public string U_CVA_Source { get; set; }
        */

        [JsonProperty("BPAddresses")]
        public List<BPAddresses> BPAddresses { get; set; }

        [JsonProperty("ContactEmployees")]
        public object[] ContactEmployees { get; set; }

        [JsonProperty("BPAccountReceivablePaybleCollection")]
        public BpAccountReceivablePaybleCollection[] BpAccountReceivablePaybleCollection { get; set; }

        [JsonProperty("BPPaymentMethods")]
        public object[] BpPaymentMethods { get; set; }

        [JsonProperty("BPWithholdingTaxCollection")]
        public object[] BpWithholdingTaxCollection { get; set; }

        [JsonProperty("BPPaymentDates")]
        public object[] BpPaymentDates { get; set; }

        [JsonProperty("BPBranchAssignment")]
        public BpBranchAssignment[] BpBranchAssignment { get; set; }

        [JsonProperty("BPBankAccounts")]
        public object[] BpBankAccounts { get; set; }

        [JsonProperty("BPFiscalTaxIDCollection")]
        public List<BPFiscalTaxIdCollection> BPFiscalTaxIDCollection { get; set; }

        [JsonProperty("DiscountGroups")]
        public object[] DiscountGroups { get; set; }

        [JsonProperty("BPIntrastatExtension")]
        public BpIntrastatExtension BpIntrastatExtension { get; set; }

        [JsonProperty("BPBlockSendingMarketingContents")]
        public object[] BpBlockSendingMarketingContents { get; set; }
    }

    public partial class BpAccountReceivablePaybleCollection
    {
        [JsonProperty("AccountType")]
        public string AccountType { get; set; }

        [JsonProperty("AccountCode")]
        public string AccountCode { get; set; }

        [JsonProperty("BPCode")]
        public string BpCode { get; set; }
    }

    public partial class BPAddresses
    {
        [JsonProperty("AddressName")]
        public string AddressName { get; set; }

        [JsonProperty("Street")]
        public string Street { get; set; }

        [JsonProperty("Block")]
        public string Block { get; set; }

        [JsonProperty("ZipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("FederalTaxID")]
        public object FederalTaxId { get; set; }

        [JsonProperty("TaxCode")]
        public object TaxCode { get; set; }

        [JsonProperty("BuildingFloorRoom")]
        public string BuildingFloorRoom { get; set; }

        [JsonProperty("AddressType")]
        public string AddressType { get; set; }

        [JsonProperty("AddressName2")]
        public object AddressName2 { get; set; }

        [JsonProperty("AddressName3")]
        public object AddressName3 { get; set; }

        [JsonProperty("TypeOfAddress")]
        public string TypeOfAddress { get; set; }

        [JsonProperty("StreetNo")]
        public string StreetNo { get; set; }

        [JsonProperty("BPCode")]
        public string BpCode { get; set; }

        [JsonProperty("RowNum")]
        public long? RowNum { get; set; }

        [JsonProperty("GlobalLocationNumber")]
        public object GlobalLocationNumber { get; set; }

        [JsonProperty("Nationality")]
        public object Nationality { get; set; }

        [JsonProperty("TaxOffice")]
        public object TaxOffice { get; set; }

        [JsonProperty("GSTIN")]
        public object Gstin { get; set; }

        [JsonProperty("GstType")]
        public object GstType { get; set; }

        [JsonProperty("CreateDate")]
        public object CreateDate { get; set; }

        [JsonProperty("CreateTime")]
        public object CreateTime { get; set; }

        /*
        [JsonProperty("U_CVA_AddresRef")]
        public object UCvaAddresRef { get; set; }
        */
    }

    public partial class BpBranchAssignment
    {
        [JsonProperty("BPCode")]
        public string BpCode { get; set; }

        [JsonProperty("BPLID")]
        public long? Bplid { get; set; }

        [JsonProperty("DisabledForBP")]
        public string DisabledForBp { get; set; }
    }

    public partial class BPFiscalTaxIdCollection
    {
        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("CNAECode")]
        public long? CnaeCode { get; set; }

        [JsonProperty("TaxId0")]
        public string TaxId0 { get; set; }

        [JsonProperty("TaxId1")]
        public string TaxId1 { get; set; }

        [JsonProperty("TaxId2")]
        public string TaxId2 { get; set; }

        [JsonProperty("TaxId3")]
        public string TaxId3 { get; set; }

        [JsonProperty("TaxId4")]
        public string TaxId4 { get; set; }

        [JsonProperty("TaxId5")]
        public string TaxId5 { get; set; }

        [JsonProperty("TaxId6")]
        public string TaxId6 { get; set; }

        [JsonProperty("TaxId7")]
        public string TaxId7 { get; set; }

        [JsonProperty("TaxId8")]
        public string TaxId8 { get; set; }

        [JsonProperty("TaxId9")]
        public object TaxId9 { get; set; }

        [JsonProperty("TaxId10")]
        public object TaxId10 { get; set; }

        [JsonProperty("TaxId11")]
        public object TaxId11 { get; set; }

        [JsonProperty("BPCode")]
        public string BpCode { get; set; }

        [JsonProperty("AddrType")]
        public string AddrType { get; set; }

        [JsonProperty("TaxId12")]
        public string TaxId12 { get; set; }

        [JsonProperty("TaxId13")]
        public object TaxId13 { get; set; }
    }

    public partial class BpIntrastatExtension
    {
    }
}
