using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class PedidoVendaBLL
    {
        public bool ValidaLotes(string docEntry)
        {
            var ret = true;
            var sql = Query.VerificaLote;
            sql = String.Format(sql, docEntry);

            List<string> lista = CrudB1Controller.FillStringList(sql);
         
            if(lista != null)
            {
                if(lista.Count > 0)
                {
                    ret = false;
                    var erro = "";
                   
                    foreach (var item in lista)
                    {
                        var i = int.Parse(item) + 1;
                       erro = erro + ($"Linha {i}: Informar número(s) de lote do item \n");
                    }

                    if (!string.IsNullOrEmpty(erro))
                    {
                        SBOApp.Application.MessageBox(erro);
                    }
                }
                else
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }

            return ret;
        }
    }
}
