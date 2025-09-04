
using MODEL.Interface;

namespace MODEL.Classes
{
    public class CollaboratorTypeModel : IModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Fixed { get; set; }
        public int CnpjRequired { get; set; }
    }
}
