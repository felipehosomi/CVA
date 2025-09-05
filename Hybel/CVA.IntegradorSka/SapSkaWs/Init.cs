using SapSkaWs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SapSkaWs
{
    public class Init
    {
        public Init()
        {
            //Database.VerifyData();
        }

        public List<SspImport> ExportData()
        {
            var list = new List<SspImport>();

            using (var helper = new SqlHelper("sap"))
            {
                using (var dr = helper.ExecuteQuery(GetQuery()))
                {
                    while (dr.Read())
                    {
                        var imp = new SspImport();
                        imp.OP = dr["OP"].ToString();
                        imp.OPER = dr["OPER"].ToString();
                        imp.CODPECA = dr["CODPECA"].ToString();
                        imp.MAQ = dr["MAQ"].ToString();
                        imp.PLANDTINI = (dr["PLANDTINI"] == DBNull.Value) ? (DateTime?)null : ((DateTime)dr["PLANDTINI"]);
                        imp.PLANDTFIM = (dr["PLANDTFIM"] == DBNull.Value) ? (DateTime?)null : ((DateTime)dr["PLANDTFIM"]);
                        imp.PLANQTY = int.Parse(dr["PLANQTY"].ToString());
                        imp.CYCQTY = int.Parse(dr["CYCQTY"].ToString());
                        imp.PLANTMUNIT = float.Parse(dr["PLANTMUNIT"].ToString());
                        imp.PLANTMSETUP = float.Parse(dr["PLANTMSETUP"].ToString());
                        imp.ACAO = int.Parse(dr["ACAO"].ToString());
                        imp.STATUS = int.Parse(dr["STATUS"].ToString());
                        imp.BELPOS_ID = int.Parse(dr["belpos_id"].ToString());
                        imp.POS_ID = int.Parse(dr["Pos_id"].ToString());
                        imp.TSTAMP = (dr["ErfTStamp"] == DBNull.Value) ? (DateTime?)null : ((DateTime)dr["ErfTStamp"]);
                        list.Add(imp);
                    }
                }
            }

            return list.Count > 0 ? list.Distinct().ToList() : list;
        }

        public void ImportData(List<SspImport> toImport)
        {
            var itemErro = new SspImport();
            var acaoErro = "A";

            try
            {
                using (var helper = new SqlHelper("ska"))
                {
                    foreach (var item in toImport)
                    {
                        itemErro = item;
                        if (!Verifica(item.OP, item.BELPOS_ID, item.POS_ID, item.TSTAMP))
                        {
                            string plandtini = item.PLANDTINI == null ? string.Empty : ((DateTime)item.PLANDTINI).ToString("yyyy-MM-dd HH:mm:ss");
                            string plandtfim = item.PLANDTFIM == null ? string.Empty : ((DateTime)item.PLANDTFIM).ToString("yyyy-MM-dd HH:mm:ss");
                            string tstamp = item.TSTAMP == null ? string.Empty : ((DateTime)item.TSTAMP).ToString("yyyy-MM-dd HH:mm:ss");

                            acaoErro = "I";
                            var query = new StringBuilder();
                            query.AppendLine("SET DATEFORMAT 'ymd'; INSERT INTO SSPImport (OP, OPER, CODPECA, MAQ, PLANDTINI, PLANDTFIM, PLANQTY, CYCQTY, PLANTMUNIT, PLANTMSETUP, ACAO, STATUS, BELPOS_ID, POS_ID, TSTAMP) ");
                            query.AppendLine("VALUES (");
                            query.Append($"'{item.OP.TrimEnd()}', ");
                            query.Append($"'{item.OPER.TrimEnd()}', ");
                            query.Append($"'{item.CODPECA.TrimEnd()}', ");
                            query.Append($"'{item.MAQ.TrimEnd()}', ");
                            query.Append(plandtini == "" ? "NULL, " : $"'{plandtini}', ");
                            query.Append(plandtfim == "" ? "NULL, " : $"'{plandtfim}', ");
                            query.Append($"{item.PLANQTY}, ");
                            query.Append($"{item.CYCQTY}, ");
                            query.Append($"{item.PLANTMUNIT}, ");
                            query.Append($"{item.PLANTMSETUP}, ");
                            query.Append($"{item.ACAO}, ");
                            query.Append($"{item.STATUS}, ");
                            query.Append($"{item.BELPOS_ID}, ");
                            query.Append($"{item.POS_ID}, ");
                            query.Append(tstamp == "" ? "NULL " : $"'{tstamp}'");
                            query.Append(");");
                            helper.ExecuteNonQuery(query.ToString());
                            Database.InsertLog(item, "Importado com sucesso.");
                        }
                        else
                        {
                            acaoErro = "U";
                            string plandtini = item.PLANDTINI == null ? string.Empty : ((DateTime)item.PLANDTINI).ToString("yyyy-MM-dd HH:mm:ss");
                            string plandtfim = item.PLANDTFIM == null ? string.Empty : ((DateTime)item.PLANDTFIM).ToString("yyyy-MM-dd HH:mm:ss");
                            string tstamp = item.TSTAMP == null ? string.Empty : ((DateTime)item.TSTAMP).ToString("yyyy-MM-dd HH:mm:ss");
                            var query = new StringBuilder();
                            query.AppendLine($"SET DATEFORMAT 'ymd'; UPDATE SSPImport SET OPER = '{item.OPER}', ");
                            //query.Append($", ");
                            query.Append($"CODPECA = '{item.CODPECA}', ");
                            query.Append($"MAQ = '{item.MAQ}', ");
                            query.Append(plandtini == "" ? "PLANDTINI = NULL, " : $"PLANDTINI = '{plandtini}', ");
                            query.Append(plandtfim == "" ? "PLANDTFIM = NULL, " : $"PLANDTFIM = '{plandtfim}', ");
                            query.Append(tstamp == "" ? "TSTAMP = NULL, " : $"TSTAMP = '{tstamp}', ");
                            query.Append($"PLANQTY = {item.PLANQTY}, ");
                            query.Append($"CYCQTY = {item.CYCQTY}, ");
                            query.Append($"PLANTMUNIT = {item.PLANTMUNIT}, ");
                            query.Append($"PLANTMSETUP = {item.PLANTMSETUP}, ");
                            query.Append($"ACAO = {item.ACAO}, ");
                            query.Append($"STATUS = {item.STATUS} ");
                            //query.Append($"BELPOS_ID = {item.BELPOS_ID}, ");
                            //query.Append($"POS_ID = {item.POS_ID} ");
                            query.Append($"WHERE OP = '{item.OP}' AND BELPOS_ID = {item.BELPOS_ID} AND POS_ID = {item.POS_ID} AND (TSTAMP = '{tstamp}' OR TSTAMP = NULL);");

                            helper.ExecuteNonQuery(query.ToString());
                            Database.InsertLog(item, "Importado com sucesso.", "U");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Database.InsertLog(itemErro, ex.Message, acaoErro, "E");
                throw;
            }
        }

        private string GetQuery()
        {
            return ConfigurationManager.AppSettings["SQL1"];
        }

        private bool Verifica(string OP, int? BELPOS_ID, int? POS_ID, DateTime? TSTAMP)
        {
            try
            {
                using (var helper = new SqlHelper("ska"))
                {
                    var ret = false;
                    string tstamp = TSTAMP == null ? string.Empty : ((DateTime)TSTAMP).ToString("yyyy-MM-dd HH:mm:ss");
                    var query = $"SET DATEFORMAT 'ymd'; SELECT 1 FROM SSPImport WHERE OP = '{OP}' AND BELPOS_ID = {BELPOS_ID} AND POS_ID = {POS_ID} AND (TSTAMP = '{tstamp}' OR TSTAMP IS NULL)";
                    var rdr = helper.ExecuteQuery(query);
                    
                    if (rdr.HasRows)
                        ret = true;

                    return ret; 
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
