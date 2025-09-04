using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class UsuarioEtapaModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_CodUsuario")]
        public string CodUsuario { get; set; }

        [ModelController(ColumnName = "U_CodEtapa")]
        public string CodEtapa { get; set; }

        public int SelectedInt { get; set; }

        public bool Selected { get; set; }
    }
}
