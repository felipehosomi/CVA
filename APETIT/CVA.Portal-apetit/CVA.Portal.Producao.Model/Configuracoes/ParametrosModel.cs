using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class ParametrosModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Parcial")]
        public int PermiteParcialInt { get; set; }

        public bool PermiteParcial { get; set; }

        [ModelController(ColumnName = "U_InspecaoMP")]
        public string InspecaoMP { get; set; } = "PCH";
    }
}
