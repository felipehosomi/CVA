using CVA.AddOn.Common.Controllers;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class FichaInspecaoItemModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; } 

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_CodFicha")]
        public string CodFicha { get; set; }

        //[ModelController(ColumnName = "U_Ano")]
        //public int Ano { get; set; }

        [ModelController(ColumnName = "U_ID")]
        public int ID { get; set; }

        [ModelController(ColumnName = "U_IdAmostra")]
        public int IdAmostra { get; set; }

        [ModelController(ColumnName = "U_CodEspec")]
        public string CodEspec { get; set; }

        public string VlrNominalStr
        {
            get
            {
                DateTime date;
                if (DateTime.TryParseExact(VlrNominal, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    return date.ToString("dd/MM/yyyy");
                }
                else
                {
                    return VlrNominal;
                }
            }
        }

        public string PadraoDeStr
        {
            get
            {
                DateTime date;
                if (DateTime.TryParseExact(PadraoDe, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    return date.ToString("dd/MM/yyyy");
                }
                else
                {
                    return PadraoDe;
                }
            }
        }

        public string PadraoAteStr
        {
            get
            {
                DateTime date;
                if (DateTime.TryParseExact(PadraoAte, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    return date.ToString("dd/MM/yyyy");
                }
                else
                {
                    return PadraoAte;
                }
            }
        }

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

        //[ModelController(ColumnName = "U_Link")]
        //public string Link { get; set; }

        [ModelController(ColumnName = "U_CodRecurso")]
        public string CodRecurso { get; set; }

        [ModelController(ColumnName = "U_Resultado")]
        public string Resultado { get; set; }

        public string DescEspec { get; set; }
        public string TipoCampo { get; set; }

        public SelectList Recursos { get; set; }
    }
}
