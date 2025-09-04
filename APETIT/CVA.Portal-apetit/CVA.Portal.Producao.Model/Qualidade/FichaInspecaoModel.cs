using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class FichaInspecaoModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Ano")]
        public int Ano { get; set; }

        [ModelController(ColumnName = "U_ID")]
        public int ID { get; set; }

        [ModelController(ColumnName = "U_QtdeAmostra")]
        public int QtdeAmostra { get; set; }

        [ModelController(ColumnName = "U_Sequencia")]
        public int Sequencia { get; set; }

        [ModelController(ColumnName = "U_DataInsp")]
        public DateTime DataInsp { get; set; }

        [ModelController(ColumnName = "U_DataDoc")]
        public DateTime DataDoc { get; set; }

        [ModelController(ColumnName = "U_CodItem")]
        public string CodItem { get; set; }

        [ModelController(ColumnName = "U_DescItem")]
        public string DescItem { get; set; }

        [ModelController(ColumnName = "U_DocEntry")]
        public int DocEntry { get; set; }

        [ModelController(ColumnName = "U_DocNum")]
        public int DocNum { get; set; }

        [ModelController(ColumnName = "U_LineNum")]
        public int LineNum { get; set; }

        [ModelController(ColumnName = "U_TipoDoc")]
        public string TipoDoc { get; set; }

        [ModelController(ColumnName = "U_Quantidade")]
        public double Quantidade { get; set; }

        [ModelController(ColumnName = "U_LoteSerie")]
        public string LoteSerie { get; set; }

        [ModelController(ColumnName = "U_CodModelo")]
        public string CodModelo { get; set; }

        [ModelController(ColumnName = "U_QtdeAnalisada")]
        public double QtdeAnalisada { get; set; }

        [ModelController(ColumnName = "U_Status")]
        public string Status { get; set; }

        [ModelController(ColumnName = "U_CodUsuario")]
        public string CodUsuario { get; set; }

        //[ModelController(ColumnName = "U_NomeUsuario")]
        //public string NomeUsuario { get; set; }

        [ModelController(ColumnName = "U_CodPN")]
        public string CodPN { get; set; }

        [ModelController(ColumnName = "U_NomePN")]
        public string NomePN { get; set; }

        [ModelController(ColumnName = "U_NrNF")]
        public int NrNF { get; set; }

        [ModelController(ColumnName = "U_CodEtapa")]
        public string CodEtapa { get; set; }

        [ModelController(ColumnName = "U_StatusLote")]
        public string StatusLote { get; set; }

        public string DataDocStr
        {
            get
            {
                return DataDoc.ToString("dd/MM/yyyy");
            }
        }

        public string DataInspStr { get; set; }

        public string PN
        {
            get
            {
                return CodPN + " - " + NomePN;
            }
        }

        public string Item
        {
            get
            {
                return CodItem + " - " + DescItem;
            }
        }

        public string DescModelo { get; set; }
        public string DescEtapa { get; set; }

        public double QtdeSeq { get; set; }
        public int SequenciaParcial { get; set; }

        public string SeqDesc
        {
            get
            {
                return SequenciaParcial + "/" + Convert.ToInt32(QtdeSeq);
            }
        }

        public List<FichaInspecaoAmostraModel> AmostraList { get; set; }
        public List<FichaInspecaoItemModel> ItemList { get; set; }
    }
}
