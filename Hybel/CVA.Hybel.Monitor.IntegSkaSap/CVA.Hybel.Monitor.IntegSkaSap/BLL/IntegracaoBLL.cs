using CVA.Hybel.Monitor.IntegSkaSap.DAO;
using CVA.Hybel.Monitor.IntegSkaSap.HELPER;
using CVA.Hybel.Monitor.IntegSkaSap.MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace CVA.Hybel.Monitor.IntegSkaSap.BLL
{
    public class IntegracaoBLL
    {
        private static List<IntegracaoModel> List = new List<IntegracaoModel>();

        public static List<IntegracaoModel> GetListTest()
        {
            List.Add(new IntegracaoModel() { OP = (Convert.ToInt32(List.Count) + 1).ToString() });
            return List;
        }

        public static List<IntegracaoModel> GetList(FiltroModel filtroModel)
        {
            SqlHelper sqlHelper = new SqlHelper();
            string sql = String.Format(SQL.Integracao_Get, ConfigurationManager.AppSettings["MESDatabase"]);
            string where = String.Empty;
            if (filtroModel.DataDe.HasValue)
            {
                where += $" AND PROD.REPDATETIME >= CAST('{filtroModel.DataDe.Value.ToString("yyyyMMdd HH:mm")}' AS DATETIME) ";
            }
            if (filtroModel.DataAte.HasValue)
            {
                where += $" AND PROD.REPDATETIME <= CAST('{filtroModel.DataAte.Value.ToString("yyyyMMdd HH:mm")}' AS DATETIME) ";
            }
            if (!String.IsNullOrEmpty(filtroModel.OP))
            {
                where += $" AND PROD.OP = '{filtroModel.OP.Trim()}' ";
            }
            if (!String.IsNullOrEmpty(filtroModel.Status))
            {
                where += String.Format(SQL.Integracao_WhereStatus, filtroModel.Status);
            }
            if (!String.IsNullOrEmpty(filtroModel.BELPOS_ID))
            {
                where += $" AND PROD.BELPOS_ID = '{filtroModel.BELPOS_ID.Trim()}' ";
            }
            if (!String.IsNullOrEmpty(filtroModel.OPER))
            {
                where += $" AND PROD.OPER = '{filtroModel.OPER.Trim()}' ";
            }
            if (!String.IsNullOrEmpty(filtroModel.OPERADOR))
            {
                where += $" AND PROD.OPERADOR LIKE '%{filtroModel.OPERADOR.Trim()}%' ";
            }
            if (!String.IsNullOrEmpty(filtroModel.MAQ))
            {
                where += $" AND PROD.MAQ = '{filtroModel.MAQ}' ";
            }
            if (!String.IsNullOrEmpty(filtroModel.CODPECA))
            {
                where += $" AND PROD.CODPECA LIKE '%{filtroModel.CODPECA.Trim()}%' ";
            }

            if (!String.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where.Substring(4);
            }
            sql += " ORDER BY PROD.ID DESC ";

            //StreamWriter sw = new StreamWriter(@"C:\CVA Consultoria\Monitor SKA\consulta.sql");
            //sw.WriteLine(sql);
            //sw.Close();
            return sqlHelper.FillModelListAccordingToSql<IntegracaoModel>(sql);
        }

        public static void SetStatus(List<IntegracaoModel> list)
        {
            SqlHelper sqlHelper = new SqlHelper();
            string sql = SQL.Integracao_UpdateStatus;
            string mesDatabase = ConfigurationManager.AppSettings["MESDatabase"];

            foreach (var item in list)
            {
                sqlHelper.ExecuteNonQuery(String.Format(sql, mesDatabase, item.ID));
            }
        }
    }
}
