using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class PerfilViewModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_CodPerfil")]
        public string CodPerfil { get; set; }

        [ModelController(ColumnName = "U_CodView")]
        public string CodView { get; set; }

        public string DescView { get; set; }

        public string CodPai { get; set; }

        public int SelectedInt { get; set; }

        public bool Selected { get; set; }

        public int Posicao { get; set; }
    }
}
