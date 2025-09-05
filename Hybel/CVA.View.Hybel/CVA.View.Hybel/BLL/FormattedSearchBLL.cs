using CVA.AddOn.Common.Util;
using CVA.View.Hybel.DAO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.BLL
{
    public class FormattedSearchBLL
    {
        public static void CreateFormattedSeaches()
        {
            FormattedSearchUtil formattedSearchUtil = new FormattedSearchUtil();
            formattedSearchUtil.AssignFormattedSearch("Concorrentes", SQL.Concorrente_GetList, "2000003031", "et_Code");
            formattedSearchUtil.AssignFormattedSearch("Item - Engenharia", SQL.Engenharia_Get, "139", "38", "U_CVA_Engenharia");
            formattedSearchUtil.AssignFormattedSearch("Item - Engenharia", SQL.Engenharia_Get, "149", "38", "U_CVA_Engenharia");

            formattedSearchUtil.AssignFormattedSearch("Produto Montadora", SQL.ProdutoMontadora_Get, "2000003036", "et_Prod");
            formattedSearchUtil.AssignFormattedSearch("Tipo Máquina", SQL.TipoMaquina_Get, "2000003036", "et_Tipo");

            formattedSearchUtil.AssignFormattedSearch("Cód. Concorrente", SQL.CodConcorrente_Get, "2000003037", "et_Conc");
            formattedSearchUtil.AssignFormattedSearch("Produto Montadora", SQL.ProdutoMontadora_Get, "2000003037", "et_Prod");
            formattedSearchUtil.AssignFormattedSearch("Tipo Máquina", SQL.TipoMaquina_Get, "2000003037", "et_Tipo");
            //formattedSearchUtil.AssignFormattedSearch("Imposto", SQL.TipoMaquina_Get, "2000003038", "gr_Item", "Imposto");
        }
    }
}
