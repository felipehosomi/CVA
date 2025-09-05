using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using System;
using System.Collections.Generic;

namespace CVA.View.Hybel.BLL
{
    public class SimuladorVendaBLL
    {
        public static string GetSimulacaoSQL(string code)
        {
            string sql = String.Format(SQL.SimuladorVenda_GetSimulacao, code);
            return sql;
        }

        public static string GetEntradasSQL(string itemCode)
        {
            string sql = String.Format(SQL.SimuladorVenda_GetEntradas, itemCode);
            return sql;
        }

        public static string GetSaidasSQL(string itemCode)
        {
            string sql = String.Format(SQL.SimuladorVenda_GetSaidas, itemCode);
            return sql;
        }

        public static string GetOPsSQL(string itemCode)
        {
            string sql = String.Format(SQL.SimuladorVenda_GetOPs, itemCode);
            return sql;
        }

        public static string GetRoteiroSQL(int op, string itemCode)
        {
            string sql = String.Format(SQL.SimuladorVenda_GetRoteiro, op, itemCode);
            return sql;
        }

        public static List<SimuladorVendaPedidoModel> GetPedidos()
        {
            string sql = String.Format(SQL.SimuladorVenda_GetPedidos);
            return new CrudController().FillModelListAccordingToSql<SimuladorVendaPedidoModel>(sql);
        }

        public static bool Exists()
        {
            string sql = String.Format(SQL.SimuladorVenda_GetCount, SBOApp.Company.UserName);
            return Convert.ToInt32(CrudController.ExecuteScalar(sql).ToString()) > 0;
        }
    }
}
