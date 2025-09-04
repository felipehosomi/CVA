using CVA.AddOn.Common.Controllers;
using System.Collections.Generic;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class PerfilModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Descricao")]
        public string Descricao { get; set; }

        [ModelController(ColumnName = "U_Ativo")]
        public int AtivoInt { get; set; }

        public bool Ativo { get; set; }

        public List<string> SelectedViews { get; set; }

        public List<PerfilViewModel> ViewList { get; set; }
    }
}
