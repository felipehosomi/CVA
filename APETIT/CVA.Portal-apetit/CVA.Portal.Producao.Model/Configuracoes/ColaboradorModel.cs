using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class ColaboradorModel
    {
        [ModelController]
        public string Codigo { get; set; }

        [ModelController]
        public string Nome { get; set; }
    }

    public class Colaborador_PCP_GERENTE_Model
    {
        public string DEP { get; set; }
        public string CODE { get; set; }
        public string EMAIL { get; set; }
    }
}
