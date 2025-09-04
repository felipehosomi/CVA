using Newtonsoft.Json;
using Sap.Data.Hana;
using System.Collections.Generic;
using System.Data;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public class HannaConnector
    {
        private readonly string _connectionString;
        private readonly string _dbCompany;

        public HannaConnector(string connectionString, string dbCompany)
        {
            this._connectionString = connectionString;
            this._dbCompany = dbCompany;
        }

        public void ExecuteNonQueryWithCompany(string unformatedQuery)
        {
            if (string.IsNullOrEmpty(unformatedQuery) || !unformatedQuery.Contains("{0}"))
            {
                //Helper.LogInfo($"ExecuteNonQueryWithCompany não executado por falta de informação");
                return;
            }

            var execQuery = unformatedQuery.Replace("{0}", _dbCompany).Replace("{0}", _dbCompany);

            //Helper.LogInfo($"ExecuteNonQueryWithCompany execQuery = {execQuery} ,connectionString = {_connectionString} ,STR = {unformatedQuery}, dbcompany = {_dbCompany} ");

            using (var _conn = new HanaConnection(_connectionString))
            {
                _conn.Open();
                HanaCommand da = new HanaCommand(execQuery, _conn);
                da.ExecuteNonQuery();
            }
        }

        public List<dynamic> QueryListWithCompany(string unformatedQuery)
        {
            using (var _conn = new HanaConnection(_connectionString))
            {
                _conn.Open();
                var FormularioTable = new DataTable();
                var dataAdapter = new HanaDataAdapter(string.Format(unformatedQuery, _dbCompany), _conn);
                
                dataAdapter.Fill(FormularioTable);
                
                //Helper.LogInfo($"Registros para processamento {FormularioTable.Rows.Count}");

                var lstRet = new List<dynamic>();
                
                foreach (DataRow row in FormularioTable.Rows)
                {
                    IDictionary<string, object> ret = new Dictionary<string, object>();
                    for (int i = 0; i < FormularioTable.Columns.Count; i++)
                    {
                        ret.Add(FormularioTable.Columns[i].ToString(), row[i]);
                    }
                    lstRet.Add(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(ret)));
                }

                //Helper.LogInfo($"DADOS ENCONTRADOS : {JsonConvert.SerializeObject(lstRet)} ");

                return lstRet;
            }
        }
    }
}
