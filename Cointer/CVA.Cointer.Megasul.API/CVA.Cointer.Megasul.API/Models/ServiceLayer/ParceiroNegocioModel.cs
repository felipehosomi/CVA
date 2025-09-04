using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models.ServiceLayer
{
    public class ParceiroNegocioModel
    {
        public int Series { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardForeignName { get; set; }
        public string CardType { get; set; }
        public int GroupCode { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Cellular { get; set; }


        public List<Bpaddress> BPAddresses { get; set; }
        public List<Bpbranchassignment> BPBranchAssignment { get; set; }
        public List<Bpfiscaltaxidcollection> BPFiscalTaxIDCollection { get; set; }
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
        public object BuildingFloorRoom { get; set; }
        public string AddressType { get; set; }
        public object AddressName2 { get; set; }
        public object AddressName3 { get; set; }
        public string TypeOfAddress { get; set; }
        public string StreetNo { get; set; }
        public string BPCode { get; set; }
        public int RowNum { get; set; }
        public object GlobalLocationNumber { get; set; }
        public object Nationality { get; set; }
        public object TaxOffice { get; set; }
        public object GSTIN { get; set; }
        public object GstType { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public object MYFType { get; set; }
        public string TaasEnabled { get; set; }
        public object U_TX_IE { get; set; }
    }

    public class Bpbranchassignment
    {
        public string BPCode { get; set; }
        public int BPLID { get; set; }
        public string DisabledForBP { get; set; }
    }

    public class Bpfiscaltaxidcollection
    {
        public string Address { get; set; }
        public object CNAECode { get; set; }
        public object TaxId0 { get; set; }
        public string TaxId1 { get; set; }
        public object TaxId2 { get; set; }
        public object TaxId3 { get; set; }
        public string TaxId4 { get; set; }
        public object TaxId5 { get; set; }
        public object TaxId6 { get; set; }
        public object TaxId7 { get; set; }
        public object TaxId8 { get; set; }
        public object TaxId9 { get; set; }
        public object TaxId10 { get; set; }
        public object TaxId11 { get; set; }
        public string BPCode { get; set; }
        public string AddrType { get; set; }
        public string TaxId12 { get; set; }
        public object TaxId13 { get; set; }
    }
}