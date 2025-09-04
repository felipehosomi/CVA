using MODEL.Interface;

namespace MODEL.Classes
{
    public class MemberModel : IModel
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Departamento { get; set; }
    }
}
