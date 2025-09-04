using CVA.AddOn.Common.Controllers;
using System;
using System.Globalization;

namespace CVA.Portal.Producao.Model.Producao
{
    public class ApontamentoHRModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_OPDocEntry")]
        public int OPDocEntry { get; set; }

        [ModelController(ColumnName = "U_OPDocNum")]
        public int OPDocNum { get; set; }

        [ModelController(ColumnName = "U_CodEtapa")]
        public int CodEtapa { get; set; }

        [ModelController(ColumnName = "U_NomeEtapa")]
        public string NomeEtapa { get; set; }

        [ModelController(ColumnName = "U_RecursoLineNum")]
        public int RecursoLineNum { get; set; }

        [ModelController(ColumnName = "U_RecursoCod")]
        public string RecursoCod { get; set; }

        [ModelController(ColumnName = "U_RecursoNome")]
        public string RecursoNome { get; set; }

        [ModelController(ColumnName = "U_CodUsuario")]
        public string CodUsuario { get; set; }

        [ModelController(ColumnName = "U_StartDateTime")]
        public string StartDateTime { get; set; }

        [ModelController(ColumnName = "U_EndDateTime")]
        public string EndDateTime { get; set; }

        [ModelController(ColumnName = "U_Duration")]
        public double Duration { get; set; }

        [ModelController(ColumnName = "U_OkQuantity")]
        public double OkQuantity { get; set; }

        [ModelController(ColumnName = "U_OcrCode")]
        public string OcrCode { get; set; }

        [ModelController(ColumnName = "U_Obs")]
        public string Obs { get; set; }

        public DateTime StartDateTimeFormat { get { return Convert.ToDateTime(StartDateTime, ptBR); } set { StartDateTime = value.ToString(ptBR); } }

        public DateTime EndDateTimeFormat { get { return Convert.ToDateTime(EndDateTime, ptBR); } set { EndDateTime = value.ToString(ptBR); } }

        public ProducaoModel OP { get; set; }

        private CultureInfo ptBR = new CultureInfo("pt-BR");
    }
}
