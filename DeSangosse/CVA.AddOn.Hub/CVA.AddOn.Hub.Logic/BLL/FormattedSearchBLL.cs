using CVA.AddOn.Common;
using CVA.AddOn.Common.Util;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class FormattedSearchBLL
    {
        public static void CreateFormattedSeaches()
        {
            FormattedSearchUtil formattedSearchUtil = new FormattedSearchUtil();
            try
            {
                formattedSearchUtil.CreateQuery("Pedidos de Fretes em Aberto", Query.PedidoCompra_GetFreteEmAberto);
                formattedSearchUtil.CreateQuery("Fretes Registrados", Query.NotaFiscalEntrada_GetFrete);
                formattedSearchUtil.CreateQuery("Pedido Venda - Cliente Inativo", Query.ParceiroNegocio_GetInativo, "CONSULTA AUTOMAÇAO DE CAMPOS");

                //formattedSearchUtil.AssignFormattedSearch("Documentos Rateio Frete", Query.DocumentoRateio_Get, "142", "gr_docs", "Chave Origem");
                formattedSearchUtil.AssignFormattedSearch("Documentos Frete", Query.DocumentoRateio_Get, "142", "et_Doc", "-1", "CONSULTA AUTOMAÇAO DE CAMPOS");
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Erro ao criar consulta formatada: " + ex.Message);
            }
        }
    }
}
