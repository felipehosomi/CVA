using DelimitedDataHelper.Tab;
using SkaSapWs.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SkaSapWs
{
    public class Init
    {
        public Init()
        {
        }

        public List<SspExportProd> ExportData()
        {
            var list = new List<SspExportProd>();

            using (var helper = new SqlHelper("ska"))
            {
                using (var dr = helper.ExecuteQuery(GetQuery("SQL1")))
                {
                    while (dr.Read())
                    {
                        var encerr = dr["ENCERR"].ToString().TrimEnd().ToLower();
                        var status = dr["STATUS"].ToString().TrimEnd().ToLower();
                        var reptype = dr["REPTYPE"].ToString().TrimEnd().ToLower();

                        var imp = new SspExportProd();
                        imp.Id = int.Parse(dr["Id"].ToString().TrimEnd());
                        imp.OP = dr["OP"].ToString().TrimEnd();
                        imp.OPER = dr["OPER"].ToString().TrimEnd();
                        imp.CODPECA = dr["CODPECA"].ToString().TrimEnd();
                        imp.MAQ = dr["MAQ"].ToString().TrimEnd();
                        imp.TURNO = (string.IsNullOrEmpty(dr["TURNO"].ToString())) ? 0 : int.Parse(dr["TURNO"].ToString().TrimEnd());
                        imp.OPERADOR = dr["OPERADOR"].ToString().TrimEnd();
                        imp.DATAINI = DateTime.Parse(dr["DATAINI"].ToString().TrimEnd());
                        imp.DATAFIM = DateTime.Parse(dr["DATAFIM"].ToString().TrimEnd());
                        imp.QUANT = int.Parse(dr["QUANT"].ToString().TrimEnd());
                        imp.REJ = int.Parse(dr["REJ"].ToString().TrimEnd());
                        imp.UNID = dr["UNID"].ToString().TrimEnd();
                        imp.TMLIQPROD = float.Parse(dr["TMLIQPROD"].ToString().TrimEnd());
                        imp.TMINTPROD = float.Parse(dr["TMINTPROD"].ToString().TrimEnd());
                        imp.CNTREP = (string.IsNullOrEmpty(dr["CNTREP"].ToString())) ? 0 : int.Parse(dr["CNTREP"].ToString().TrimEnd());
                        imp.ENCERR = encerr == "false" ? 0 : 1;
                        imp.STATUS = status == "false" ? 0 : 1;
                        imp.REPTYPE = reptype == "false" ? 0 : 1;
                        imp.REPDATETIME = DateTime.Parse(dr["REPDATETIME"].ToString().TrimEnd());
                        imp.BELPOS_ID = int.Parse(dr["BELPOS_ID"].ToString().TrimEnd());
                        imp.POS_ID = int.Parse(dr["POS_ID"].ToString().TrimEnd());
                        list.Add(imp);
                    }
                }
            }

            return list.Count > 0 ? list.OrderBy(l => l.Id).ToList() : list;
        }

        public void ImportData(List<SspExportProd> toImport)
        {
            var dt = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var filePath = $"{ConfigurationManager.AppSettings["FilePathToImport"]}{dt}.txt";
            var lst = new List<SspExportInt>();

            using(var helper = new SqlHelper("sap"))
            {
                foreach (var item in toImport)
                {
                    var ssp = new SspExportInt();
                    var colaboradorQuery = GetQuery("SQL2").Replace("@colaborador", item.OPERADOR);
                    var recursoQuery = GetQuery("SQL3").Replace("@recurso", item.MAQ);
                    var colaborador = string.Empty;
                    var recurso = string.Empty;

                    using (var dr = helper.ExecuteQuery(colaboradorQuery))
                    {
                        while (dr.Read())
                        {
                            colaborador = dr["pers_id"].ToString();
                        }
                    }

                    using (var dr = helper.ExecuteQuery(recursoQuery))
                    {
                        while (dr.Read())
                        {
                            recurso = dr["aplatz_id"].ToString();
                        }
                    }

                    var encerramento = item.ENCERR == 0 ? "N" : "J";
                    TimeSpan diff = item.DATAFIM - item.DATAFIM;
                    var zeit = diff.TotalHours;

                    ssp.BELNR_ID = item.OP;
                    ssp.BELPOS_ID = item.BELPOS_ID;
                    ssp.POS_ID = item.POS_ID;
                    ssp.TYP = "A";
                    ssp.RESOURCENPOS_ID = 0;
                    ssp.PERS_ID = colaborador;
                    ssp.ANFZEIT = item.DATAINI.ToString("yyyy-MM-dd HH:mm:ss");
                    ssp.ENDZEIT = item.DATAFIM.ToString("yyyy-MM-dd HH:mm:ss");
                    ssp.ZEIT = zeit;
                    ssp.MENGE_GUT_RM = item.QUANT;
                    ssp.MENGE_SCHLECHT_RM = item.REJ;
                    ssp.ABGKZ = encerramento;
                    ssp.manualbooking = string.Empty;
                    ssp.APLATZ_ID = recurso;
                    ssp.KSTST_ID = string.Empty;
                    ssp.GRUND = string.Empty;
                    ssp.DocDate = item.DATAFIM;
                    ssp.Project = string.Empty;
                    ssp.TIMETYPE_ID = string.Empty;
                    ssp.EXTERNAL_COST = string.Empty;
                    ssp.BatchNum = string.Empty;
                    ssp.UDF1 = item.Id;
                    ssp.UDF2 = string.Empty;
                    ssp.UDF3 = string.Empty;
                    ssp.UDF4 = string.Empty;
                    ssp.WKZ_ID = string.Empty;

                    lst.Add(ssp);
                }
            }

            if (lst.Count > 0)
                TabDelimitedDataWriter.WriteToTabDelimitedFile(lst, filePath);

        }

        public void ReturnData(List<SspExportProd> toImport)
        {
            using (var helper = new SqlHelper("ska"))
            {
                foreach (var item in toImport)
                {
                    helper.ExecuteNonQuery($"UPDATE SSPExportProd SET STATUS = 1 WHERE Id = {item.Id}");
                }
            }
        }

        private string GetQuery(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
