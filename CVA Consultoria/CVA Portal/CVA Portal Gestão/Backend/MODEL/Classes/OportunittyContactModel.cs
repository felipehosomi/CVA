using MODEL.Interface;

namespace MODEL.Classes
{
    public class OportunittyContactModel : IModel
    {
        public string CommercialContact { get; set; }
        public CollaboratorModel Vendor { get; set; }
        public CollaboratorModel Technical { get; set; }
        public CollaboratorModel ProjectManager { get; set; }
    }
}
