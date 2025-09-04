using MODEL.Interface;

namespace MODEL.Classes
{
    public class SpecialtyRuleModel : IModel
    {
        public int CollaboratorId { get; set; }
        public string CollaboratorName { get; set; }
        public int SpecialtyId { get; set; }
        public string SpecialtyName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
