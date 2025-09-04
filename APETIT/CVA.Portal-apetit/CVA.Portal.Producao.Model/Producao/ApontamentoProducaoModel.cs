using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Producao
{
    public class ApontamentoProducaoModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_DocNum")]
        public int DocNum { get; set; }

        [ModelController(ColumnName = "U_CodEtapa")]
        public string CodEtapa { get; set; }

        [ModelController(ColumnName = "U_QtdeApontada")]
        public double QtdeApontada { get; set; }

        [ModelController(ColumnName = "U_QtdeCQ")]
        public double QtdeCQ { get; set; }
    }
}
