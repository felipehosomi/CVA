using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class ProdutoBLL
    {
        public static int GetQtdEmbalagem(string itemCode)
        {
            int qtdEmb = 0;
            object  qtd = CrudController.ExecuteScalar(String.Format(Query.BuscaQtdEmbalagem, itemCode));
            if (qtd != null)
            {
                
                return qtdEmb = Convert.ToInt32(qtd.ToString());
            }
            else
            {
                return qtdEmb;
            }
        }
    }
}
