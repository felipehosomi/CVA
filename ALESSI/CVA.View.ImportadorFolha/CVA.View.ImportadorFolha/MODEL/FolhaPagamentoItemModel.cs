using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.MODEL
{
    public class FolhaPagamentoItemModel
    {
        [ModelController(ColumnName = "U_Posicao")]
        public int Posicao { get; set; }

        [ModelController(ColumnName = "U_Tipo_Campo")]
        public int TipoCampoLCM { get; set; }

        [ModelController(ColumnName = "U_Campo_LCM")]
        public string CampoLCM { get; set; }

        [ModelController(ColumnName = "U_Consulta")]
        public string Consulta { get; set; }

        //[ModelController(ColumnName = "U_Tabela")]
        //public string Tabela { get; set; }

        //[ModelController(ColumnName = "U_Campo_De")]
        //public string CampoDe { get; set; }

        //[ModelController(ColumnName = "U_Campo_Para")]
        //public string CampoPara { get; set; }

        public string ValorDe { get; set; }
        public string ValorPara { get; set; }
    }
}
