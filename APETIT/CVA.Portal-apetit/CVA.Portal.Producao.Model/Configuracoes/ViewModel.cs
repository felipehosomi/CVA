using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class ViewModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_View")]
        public string View { get; set; }

        [ModelController(ColumnName = "U_Descricao")]
        public string Descricao { get; set; }

        [ModelController(ColumnName = "U_Posicao")]
        public int Posicao { get; set; }

        [ModelController(ColumnName = "U_CodPai")]
        public string CodPai { get; set; }

        [ModelController(ColumnName = "U_Controller")]
        public string Controller { get; set; }

        [ModelController(ColumnName = "U_Action")]
        public string Action { get; set; }

        [ModelController(ColumnName = "U_Icone")]
        public string Icone { get; set; }
    }
}
