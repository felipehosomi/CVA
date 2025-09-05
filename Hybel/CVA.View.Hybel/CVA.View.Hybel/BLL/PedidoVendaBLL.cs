using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CVA.View.Hybel.BLL
{
    public class PedidoVendaBLL
    {
        public static string GetTransferenciaFiliaisSQL(int filial)
        {
            string sql = String.Format(SQL.PedidoVenda_GetTransferenciaFilial, filial);
            return sql;
        }

        public static string GerarTransferenciaFiliais(int filial, DateTime dataEntrega, string tipoQtde, SAPbouiCOM.DataTable dt_Item)
        {
            string msg = String.Empty;

            Documents doc = SBOApp.Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
            doc.BPL_IDAssignedToInvoice = 1;
            doc.DocDate = DateTime.Today;
            doc.DocDueDate = dataEntrega;
            object cardCode = CrudController.ExecuteScalar(String.Format(SQL.Cliente_GetByFilial, filial)).ToString();
            if (cardCode == null)
            {
                return "Cliente não encontrado para filial informada!";
            }
            object usage = CrudController.ExecuteScalar(String.Format(SQL.Utilizacao_GetByPN, cardCode)).ToString();
            if (usage == null)
            {
                return "Utilização principal não informada no cadastro do PN!";
            }

            doc.CardCode = cardCode.ToString();
            doc.TaxExtension.MainUsage = Convert.ToInt32(usage);

            string colunaQtde = tipoQtde == "P" ? "Pedido" : "Quantidade";

            for (int i = 0; i < dt_Item.Rows.Count; i++)
            {
                if (dt_Item.GetValue("Gerar", i).ToString() != "Y" || Convert.ToDouble(dt_Item.GetValue(colunaQtde, i)) == 0)
                {
                    continue;
                }

                if (!String.IsNullOrEmpty(doc.Lines.ItemCode))
                {
                    doc.Lines.Add();
                }
                doc.Lines.ItemCode = dt_Item.GetValue("Cód. Item", i).ToString();
                doc.Lines.Quantity = Convert.ToDouble(dt_Item.GetValue(colunaQtde, i));
                doc.Lines.ShipDate = dataEntrega;
                doc.Lines.Usage = usage.ToString();
            }

            if (doc.Add() != 0)
            {
                msg = SBOApp.Company.GetLastErrorDescription();
            }

            Marshal.ReleaseComObject(doc);
            doc = null;

            return msg;
        }

        public static List<ItemModel> GetItems(int docNum)
        {
            string sql = String.Format(SQL.PedidoVenda_GetItems, docNum);
            return new CrudController().FillModelListAccordingToSql<ItemModel>(sql);
        }
    }
}
