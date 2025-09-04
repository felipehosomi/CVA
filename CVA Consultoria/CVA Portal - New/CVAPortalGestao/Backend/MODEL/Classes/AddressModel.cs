using MODEL.Enumerators;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class AddressModel : IModel
    {
        public static int oObjectType { get {return (int)ObjectType.Address;} }
        public string Street { get; set; }
        public string StreetNo { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public UfModel Uf { get; set; }
        public int Uf_Id { get; set; }
        public string City { get; set; }
    }
}
