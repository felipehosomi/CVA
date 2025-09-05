using System.Collections.Generic;

namespace Escoteiro.Magento.Models
{
    public class BusinessPartnersModel
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public int? GroupCode { get; set; }
        public string ShipToDefault { get; set; }
        public string Cellular { get; set; }
        public string CardForeignName { get; set; }
        public object EmailAddress { get; set; }
        public int? Series { get; set; }
        public string BPCode { get; set; }
        public int? U_CVA_EntityId { get; set; }
        public List<Bpaddress> BPAddresses { get; set; }
        public List<Bpfiscaltaxidcollection> BPFiscalTaxIDCollection { get; set; }
        public List<BPPaymentMethods> BPPaymentMethods { get; set; }
    }

    public class Bpaddress
    {
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public object FederalTaxID { get; set; }
        public object TaxCode { get; set; }
        public string BuildingFloorRoom { get; set; }
        public string AddressType { get; set; }
        public object AddressName2 { get; set; }
        public object AddressName3 { get; set; }
        public string TypeOfAddress { get; set; }
        public string StreetNo { get; set; }
        public string BPCode { get; set; }
        public int? RowNum { get; set; }
    }

    public class Bpfiscaltaxidcollection
    {
        public string Address { get; set; }
        public int? CNAECode { get; set; }
        public string TaxId0 { get; set; }
        public string TaxId1 { get; set; }
        public string TaxId2 { get; set; }
        public string TaxId3 { get; set; }
        public string TaxId4 { get; set; }
        public string TaxId5 { get; set; }
        public string TaxId6 { get; set; }
        public string TaxId7 { get; set; }
        public string TaxId8 { get; set; }
        public object TaxId9 { get; set; }
        public object TaxId10 { get; set; }
        public object TaxId11 { get; set; }
        public string BPCode { get; set; }
        public string AddrType { get; set; }
        public string TaxId12 { get; set; }
        public object TaxId13 { get; set; }
    }

    public class BPPaymentMethods
    {
        public string PaymentMethodCode { get; set; }
    }
}
