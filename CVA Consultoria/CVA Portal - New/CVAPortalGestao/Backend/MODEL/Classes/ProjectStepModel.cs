using MODEL.Enumerators;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class ProjectStepModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.ProjectStep; } }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsProjectStep { get; set; }
    }
}
