using CVA.AddOn.Common.Controllers;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class ModeloFichaItemModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_ID")]
        public int ID { get; set; }

        [ModelController(ColumnName = "U_CodModelo")]
        public string CodModelo { get; set; }

        [ModelController(ColumnName = "U_CodEspec")]
        public string CodEspec { get; set; }

        [ModelController(ColumnName = "U_VlrNominal")]
        public string VlrNominal { get; set; }

        [ModelController(ColumnName = "U_PadraoDe")]
        public string PadraoDe { get; set; }

        [ModelController(ColumnName = "U_PadraoAte")]
        public string PadraoAte { get; set; }

        [ModelController(ColumnName = "U_Analise")]
        public string Analise { get; set; }

        [ModelController(ColumnName = "U_Observacao")]
        public string Observacao { get; set; }

        [ModelController(ColumnName = "U_Metodo")]
        public string Metodo { get; set; }

        [ModelController(ColumnName = "U_Amostragem")]
        public string Amostragem { get; set; }

        [ModelController(ColumnName = "U_Link")]
        public string Link { get; set; }

        public int Deleted { get; set; }

        public string DescEspec { get; set; }

        public string TipoCampo { get; set; }

        public SelectList Especificacoes { get; set; }
    }
}
