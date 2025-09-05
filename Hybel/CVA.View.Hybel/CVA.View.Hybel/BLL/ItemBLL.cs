using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.BLL
{
    public class ItemBLL
    {
        public static string GetItemName(string itemCode)
        {
            string itemName = new CrudController("OITM").Exists("ItemName", $"ItemCode = '{itemCode}'");
            return itemName;
        }

        public static string GetItemCodeByEngenharia(string engenharia)
        {
            object itemCode = CrudController.ExecuteScalar(String.Format(SQL.Item_GetByEngenharia, engenharia));
            return itemCode != null ? itemCode.ToString() : "";
        }

        public static string GetItemSQL(ItemFiltroModel model)
        {
            string sql = String.Format(SQL.Item_Get, model.Filial);
            string where = String.Empty;

            if (!String.IsNullOrEmpty(model.Aplicacao))
            {
                where += $" AND MONT.U_Aplicacao LIKE '{model.Aplicacao}' ";
            }
            if (!String.IsNullOrEmpty(model.CodConcorrente))
            {
                where += $" AND CONC.U_Concorrente LIKE '{model.CodConcorrente}' ";
            }
            if (!String.IsNullOrEmpty(model.CodEngenharia))
            {
                where += $" AND OITM.U_H_Codigo_Engenharia LIKE '{model.CodEngenharia}' ";
            }
            if (!String.IsNullOrEmpty(model.CodMontadora))
            {
                where += $" AND MONT.U_Montadora LIKE '{model.CodMontadora}' ";
            }
            if (!String.IsNullOrEmpty(model.ItemCode))
            {
                where += $" AND OITM.ItemCode LIKE '{model.ItemCode}' ";
            }
            if (!String.IsNullOrEmpty(model.ProdutoMontadora))
            {
                where += $" AND MONT.U_Mont_Prod LIKE '{model.ProdutoMontadora}' ";
            }
            if (!String.IsNullOrEmpty(model.TipoMaquina))
            {
                where += $" AND MONT.U_Tipo_Maquina LIKE '{model.TipoMaquina}' ";
            }
            if (!String.IsNullOrEmpty(model.Funcao))
            {
                where += $" AND MONT.U_Funcao LIKE '{model.Funcao}' ";
            }

            sql += where;

            return sql;
        }
    }
}
