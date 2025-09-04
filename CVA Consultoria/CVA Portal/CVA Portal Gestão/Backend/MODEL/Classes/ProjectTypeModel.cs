using MODEL.Interface;

namespace MODEL.Classes
{
    public class ProjectTypeModel : IModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string AMS { get; set; }
        public string Equipe { get; set; }
    }
}
