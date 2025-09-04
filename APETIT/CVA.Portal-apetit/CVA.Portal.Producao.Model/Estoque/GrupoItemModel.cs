using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Estoque
{
    public class GrupoItemModel
    {
        [ModelController(ColumnName = "ItmsGrpCod")]
        public int Code { get; set; }

        [ModelController(ColumnName = "ItmsGrpNam")]
        public string Name { get; set; }
    }
}
