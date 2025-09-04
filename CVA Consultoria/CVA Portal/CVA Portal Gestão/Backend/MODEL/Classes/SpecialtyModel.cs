using MODEL.Enumerators;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class SpecialtyModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.Specialty; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public SpecialtyTypeModel TipoEspecialidade { get; set; }
    }
}
