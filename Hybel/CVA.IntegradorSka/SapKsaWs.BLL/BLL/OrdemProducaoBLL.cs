using DelimitedDataHelper.Tab;
using log4net;
using SapKsaWs.BLL.HELPER;
using SapKsaWs.BLL.MODEL;
using SapKsaWs.DAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SapKsaWs.BLL
{
    public class OrdemProducaoBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OrdemProducaoBLL()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void IntegraOPs()
        {
            using (var helper = new SqlHelper("sap"))
            {
                helper.ExecuteNonQuery(DAO.BEAS.OP_GetPendentesIntegracao);
            }
        }

        public List<SspImport> BuscaOPsPendentes()
        {
            var list = new List<SspImport>();
            Logger.Debug("Iniciando busca de OP's");

            using (var helper = new SqlHelper("sap"))
            {
                using (var dr = helper.ExecuteQuery(DAO.BEAS.OP_GetPendentesIntegracao))
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

            Logger.Debug("OP's encontradas: " + list.Count);

            return list.Count > 0 ? list.Distinct().ToList() : list;
        }

        public void SalvaDadosMes(List<SspImport> toImport)
        {
            if (toImport.Count == 0)
            {
                return;
            }
            var itemErro = new SspImport();
            
            Logger.Debug("Salvando dados tabela SSPImport");
            try
            {
                using (var helper = new SqlHelper("ska"))
                {
                    foreach (var item in toImport)
                    {
                        string plandtini = item.PLANDTINI == null ? string.Empty : ((DateTime)item.PLANDTINI).ToString("yyyy-MM-dd HH:mm:ss");
                        string plandtfim = item.PLANDTFIM == null ? string.Empty : ((DateTime)item.PLANDTFIM).ToString("yyyy-MM-dd HH:mm:ss");
                        string tstamp = item.TSTAMP == null ? string.Empty : ((DateTime)item.TSTAMP).ToString("yyyy-MM-dd HH:mm:ss");

                        string sql = MES.SSPImport_Upsert;
                        sql = String.Format(sql,
                            item.OP.Trim(),
                            item.BELPOS_ID,
                            item.POS_ID,
                            item.OPER.Trim(),
                            item.CODPECA.Trim(),
                            item.MAQ.Trim(),
                            plandtini == "" ? "NULL " : $"'{plandtini}'",
                            plandtfim == "" ? "NULL " : $"'{plandtfim}'",
                            item.PLANQTY,
                            item.CYCQTY,
                            item.PLANTMUNIT,
                            item.PLANTMSETUP,
                            item.ACAO,
                            item.STATUS);

                        helper.ExecuteNonQuery(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool VerificaTabelaImportacaoMes(string OP, int? BELPOS_ID, int? POS_ID, DateTime? TSTAMP)
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
