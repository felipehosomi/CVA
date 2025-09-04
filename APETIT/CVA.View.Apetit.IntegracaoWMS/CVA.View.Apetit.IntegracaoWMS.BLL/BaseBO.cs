using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public class BaseBO
    {
        private readonly HannaConnector _conn;

        public BaseBO()
        {
            this._conn = new HannaConnector(ParametrosConexao.param.connectionString, ParametrosConexao.param.database);
        }

        public int GetNextValue(string table, string field)
        {
            if (!string.IsNullOrEmpty(ParametrosConexao.param.connectionString))
            {
                var query = $@"
                        SELECT 
                             MAX(CAST(IFNULL(r.{field.Aspas()}, 0) AS INT)) as Max
                        FROM {{0}}.{table} as r 
                    ";
                var line = _conn.QueryListWithCompany(query).FirstOrDefault();

                if (line?.MAX == null)
                    return 1;
                else
                    return (int)(line.MAX) + 1;
            }

            return 1;
        }

        public dynamic GetLineValues(string table, string field, string where)
        {
            if (!string.IsNullOrEmpty(ParametrosConexao.param.connectionString))
            {
                var query = $@"
                        SELECT  {field}
                        FROM {{0}}.{table} 
                        WHERE {where}
                    ";
                return _conn.QueryListWithCompany(query).FirstOrDefault();
            }

            return null;
        }

        public List<dynamic> GetListValues(string table, string field, string where)
        {
            if (!string.IsNullOrEmpty(ParametrosConexao.param.connectionString))
            {
                var query = $@"
                        SELECT  {field}
                        FROM {{0}}.{table} 
                        {where}
                    ";
                return _conn.QueryListWithCompany(query);
            }

            return null;
        }
    }
}
