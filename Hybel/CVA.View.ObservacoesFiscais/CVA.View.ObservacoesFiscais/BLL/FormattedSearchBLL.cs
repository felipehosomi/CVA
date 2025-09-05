using CVA.AddOn.Common.Util;
using CVA.View.ObservacoesFiscais.DAO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ObservacoesFiscais.BLL
{
    public class FormattedSearchBLL
    {
        public static void CreateFormattedSeaches()
        {
            FormattedSearchUtil formattedSearchUtil = new FormattedSearchUtil();
            formattedSearchUtil.AssignFormattedSearch("Imposto", SQL.Tax_Get, "2000008200", "mt_Item", "cl_Tax");
            formattedSearchUtil.AssignFormattedSearch("CFOP", SQL.CFOP_Get, "2000008200", "mt_Item", "cl_CFOP");
            formattedSearchUtil.AssignFormattedSearch("NCM", SQL.NCM_Get, "2000008200", "mt_Item", "cl_NCM");
            formattedSearchUtil.AssignFormattedSearch("Cidade", SQL.City_Get, "2000008200", "mt_Item", "cl_City");
        }
    }
}
