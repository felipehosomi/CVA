using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model
{
    public class CVA_APTO_TERCEIROSModel
    {
        public class CVA_APTO_Terceiros
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string U_FILIAL { get; set; }
            public string U_DATA { get; set; }
            public string U_TURNO { get; set; }
            public string U_SERVICO { get; set; }
            public float U_QTYREF { get; set; }
            public float U_QTYPLAN { get; set; }
            public string U_USERPORTAL { get; set; }
        }

        public class SLModel : CVA_APTO_Terceiros
        {
            public SLModel()
            {
                CVA_APTO_TERCEIROS1Collection = new List<CVA_APTO_Terceiros1collection>();
            }

            public int DocEntry { get; set; }
            public List<CVA_APTO_Terceiros1collection> CVA_APTO_TERCEIROS1Collection { get; set; }
            
        }

        public class CVA_APTO_Terceiros1collection
        {
            public int LineId { get; set; }
            public string U_CARDCODE { get; set; }
            public string U_CARDNAME { get; set; }
            public float U_QTYAPT { get; set; }
            
        }

        public class CVA_APTO_TerceirosSAP
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }

            public string U_CVA_CNPJ { get; set; }
        }

        public class CVA_APTO_TerceirosSQL 
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string U_FILIAL { get; set; }
            public DateTime? U_DATA { get; set; }
            public string U_TURNO { get; set; }
            public string U_SERVICO { get; set; }
            public double U_QTYREF { get; set; }
            public double U_QTYPLAN { get; set; }
            public string U_USERPORTAL { get; set; }
            
        }

        public class CVA_APTO_Terceiros1collectionSQL
        {
            public int LineId { get; set; }
            public string U_CARDCODE { get; set; }
            public string U_CARDNAME { get; set; }
            public double U_QTYAPT { get; set; }

        }

        public class APIModel : CVA_APTO_TerceirosSQL
        {
            public APIModel()
            {
                CVA_APTO_TERCEIROS1Collection = new List<CVA_APTO_Terceiros1collectionSQL>();
            }

            public List<CVA_APTO_Terceiros1collectionSQL> CVA_APTO_TERCEIROS1Collection { get; set; }

        }

    }


}
