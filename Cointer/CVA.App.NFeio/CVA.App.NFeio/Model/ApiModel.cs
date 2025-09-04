using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.App.NFeio.Model
{
    public class ApiModel
    {
        public Borrower borrower { get; set; }
        public string cityServiceCode { get; set; }
        public string federalServiceCode { get; set; }
        public string cnaeCode { get; set; }
        public string description { get; set; }
        public decimal servicesAmount { get; set; }
        public DateTime? issuedOn { get; set; }
    }

    public class Borrower
    {
        public string type { get; set; }
        public string name { get; set; }
        public long? federalTaxNumber { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }

    public class Address
    {
        public string country { get; set; }
        public string postalCode { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string additionalInformation { get; set; }
        public string district { get; set; }
        public City city { get; set; }
        public string state { get; set; }
    }

    public class City
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
