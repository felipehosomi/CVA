using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class PesquisaModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Desc")]
        public string Desc { get; set; }

        [ModelController(ColumnName = "U_Ativa")]
        public string Ativa { get; set; }
    }
}
