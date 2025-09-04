using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class InspecaoOPBLL : BaseBLL
    {
        public List<DocumentoModel> GetOPs(DateTime? dataDe, DateTime? dataAte, string status, int? pedido, int? op, string codigoItem)
        {
            if (!dataDe.HasValue)
            {
                dataDe = new DateTime(1900, 01, 01);
            }
            if (!dataAte.HasValue)
            {
                dataAte = new DateTime(2100, 01, 01);
            }

            object objPedido, objOP;

            if (pedido.HasValue)
                objPedido = pedido.Value;
            else
                objPedido = "NULL";

            if (op.HasValue)
                objOP = op.Value;
            else
                objOP = "NULL";

            return DAO.FillListFromCommand<DocumentoModel>(string.Format(
                Commands.Resource.GetString("OP_GetInspecao"), 
                BaseBLL.Database, dataDe.Value.ToString("yyyy-MM-dd"),
                dataAte.Value.ToString("yyyy-MM-dd"), 
                status,
                objPedido,
                objOP, 
                string.IsNullOrWhiteSpace(codigoItem) ? "NULL" : $"'{codigoItem}'"));
        }
    }
}
