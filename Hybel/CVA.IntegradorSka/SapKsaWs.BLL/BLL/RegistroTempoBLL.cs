using DelimitedDataHelper.Tab;
using log4net;
using SapKsaWs.BLL.HELPER;
using SapKsaWs.BLL.MODEL;
using SapKsaWs.DAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SapKsaWs.BLL
{
    public class RegistroTempoBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RegistroTempoBLL()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public List<SspExportProdModel> BuscaDadosMes()
        {
            Logger.Debug("Buscando dados tabela SSPExportProd");
            var list = new List<SspExportProdModel>();

            SqlHelper helperSka = new SqlHelper("ska");
            SqlHelper helperSap = new SqlHelper("sap");


            using (var dr = helperSka.ExecuteQuery(DAO.MES.OP_GetApontamentoHoras))
            {
                while (dr.Read())
                {
                    var encerr = dr["ENCERR"].ToString().TrimEnd().ToLower();
                    var status = dr["STATUS"].ToString().TrimEnd().ToLower();
                    var reptype = dr["REPTYPE"].ToString().TrimEnd().ToLower();

                    var imp = new SspExportProdModel();
                    try
                    {
                        imp.Id = int.Parse(dr["Id"].ToString().TrimEnd());
                        imp.OP = dr["OP"].ToString().TrimEnd();
                        imp.BELPOS_ID = int.Parse(dr["BELPOS_ID"].ToString().TrimEnd());
                        imp.POS_ID = int.Parse(dr["POS_ID"].ToString().TrimEnd());
                        imp.StatusLog = StatusLogEnum.EmProcessamento;
                        imp.OPERADOR = dr["OPERADOR"].ToString().TrimEnd();

                        object statusPosicao = helperSap.ExecuteScalar(String.Format(BEAS.OP_VerificaStatusPosicao, imp.OP, imp.BELPOS_ID, imp.POS_ID));
                        if (statusPosicao == null || statusPosicao == DBNull.Value)
                        {
                            imp.StatusLog = StatusLogEnum.VerificarObservacao;
                            imp.Erro = "Posição de ordem de produção não encontrada na tabela do BEAS";
                            continue;
                        }
                        else if (statusPosicao.ToString().Trim() == "J")
                        {
                            imp.StatusLog = StatusLogEnum.PosicaoOPFechada;
                            imp.Erro = "Posição de ordem de produção fechada";
                            continue;
                        }
                        object operador = helperSap.ExecuteScalar(String.Format(DAO.BEAS.Colaborador_GetId, Convert.ToInt32(imp.OPERADOR)));
                        if (operador == DBNull.Value || operador == null)
                        {
                            imp.StatusLog = StatusLogEnum.UsuarioNaoCadastrado;
                            imp.Erro = $"Usuário {imp.OPERADOR} não cadastrado no BEAS";
                            continue;
                        }

                        int qtde = Convert.ToInt32(helperSap.ExecuteScalar(String.Format(DAO.BEAS.OP_GetQuantidade, imp.OP)));

                        imp.OPERADOR = operador.ToString();

                        imp.OPER = dr["OPER"].ToString().TrimEnd();
                        imp.CODPECA = dr["CODPECA"].ToString().TrimEnd();
                        imp.MAQ = dr["MAQ"].ToString().TrimEnd();
                        imp.TURNO = (string.IsNullOrEmpty(dr["TURNO"].ToString())) ? 0 : int.Parse(dr["TURNO"].ToString().TrimEnd());
                        imp.DATAINI = DateTime.Parse(dr["DATAINI"].ToString().TrimEnd());
                        imp.DATAFIM = DateTime.Parse(dr["DATAFIM"].ToString().TrimEnd());
                        imp.REJ = int.Parse(dr["REJ"].ToString().TrimEnd());
                        //imp.QUANT = qtde - imp.REJ;
                        imp.QUANT = qtde;
                        imp.UNID = dr["UNID"].ToString().TrimEnd();
                        imp.TMLIQPROD = float.Parse(dr["TMLIQPROD"].ToString().TrimEnd());
                        imp.TMINTPROD = float.Parse(dr["TMINTPROD"].ToString().TrimEnd());
                        imp.CNTREP = (string.IsNullOrEmpty(dr["CNTREP"].ToString())) ? 0 : int.Parse(dr["CNTREP"].ToString().TrimEnd());
                        imp.ENCERR = encerr == "false" ? 0 : 1;
                        imp.STATUS = status == "false" ? 0 : 1;
                        imp.REPTYPE = reptype == "false" ? 0 : 1;
                        imp.REPDATETIME = DateTime.Parse(dr["REPDATETIME"].ToString().TrimEnd());
                    }
                    catch (Exception ex)
                    {
                        imp.StatusLog = StatusLogEnum.VerificarObservacao;
                        imp.Erro = ex.Message;
                    }
                    finally
                    {
                        list.Add(imp);
                    }
                }

            }

            helperSka.Dispose();
            helperSap.Dispose();

            return list.Count > 0 ? list.OrderBy(l => l.Id).ToList() : list;
        }

        public void GeraArquivo(List<SspExportProdModel> toImport)
        {
            toImport = toImport.Where(i => i.StatusLog == StatusLogEnum.EmProcessamento).ToList();
            var filePath = ConfigurationManager.AppSettings["ArquivoTempo"];

            var lst = new List<SspExportIntModel>();

            using (var helper = new SqlHelper("sap"))
            {
                foreach (var item in toImport)
                {
                    var ssp = new SspExportIntModel();

                    var recursoQuery = String.Format(DAO.BEAS.Recurso_GetId, item.MAQ);
                    var recurso = string.Empty;

                    using (var dr = helper.ExecuteQuery(recursoQuery))
                    {
                        while (dr.Read())
                        {
                            if (dr["aplatz_id"] != DBNull.Value && dr["aplatz_id"] != null)
                            {
                                recurso = dr["aplatz_id"].ToString();
                            }
                            else
                            {
                                throw new Exception($"Recurso {item.MAQ} não encontrado no BEAS!");
                            }
                        }
                    }

                    var encerramento = item.ENCERR == 0 ? "N" : "J";
                    TimeSpan diff = item.DATAFIM - item.DATAFIM;
                    var zeit = diff.TotalHours;

                    ssp.Id = item.Id;
                    ssp.BELNR_ID = item.OP;
                    ssp.BELPOS_ID = item.BELPOS_ID;
                    ssp.POS_ID = item.POS_ID;
                    ssp.TYP = "A";
                    ssp.RESOURCENPOS_ID = 0;
                    ssp.PERS_ID = item.OPERADOR;
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

            IEnumerable<IGrouping<string, SspExportIntModel>> groupedByOP = lst.GroupBy(l => l.BELNR_ID);
            foreach (var itemByOP in groupedByOP)
            {
                IEnumerable<IGrouping<int, SspExportIntModel>> groupedByBelPosId = itemByOP.GroupBy(l => l.BELPOS_ID);
                foreach (var itemByBelPosId in groupedByBelPosId)
                {
                    IEnumerable<IGrouping<int, SspExportIntModel>> groupedByPosId = itemByBelPosId.GroupBy(l => l.POS_ID);

                    foreach (var itemByPosId in groupedByPosId)
                    {
                        Logger.Debug($"Gerando arquivo Tempo - OP: {itemByOP.Key.Trim()}, BELPOS_ID: {itemByBelPosId.Key}, POS_ID: {itemByPosId.Key}");
                        string fileName = $"Tempo_OP_{itemByOP.Key.Trim()}_{itemByBelPosId.Key}_{itemByPosId.Key}_{itemByPosId.ElementAt(0).Id}_{DateTime.Now.ToString("ddMMyyyy_HHmmssfff")}.txt";
                        TabDelimitedDataWriter.WriteToTabDelimitedFile(itemByPosId, Path.Combine(filePath, fileName));
                    }
                }
            }
        }

        public void AtualizaProducaoMes(List<SspExportProdModel> toImport)
        {
            Logger.Debug("Atualizando dados tabela SspExportProd");
            using (var helper = new SqlHelper("ska"))
            {
                foreach (var item in toImport)
                {
                    helper.ExecuteNonQuery($"UPDATE SspExportProd SET STATUS = 1 WHERE Id = {item.Id}");
                    if (item.StatusLog == StatusLogEnum.EmProcessamento)
                    {
                        helper.ExecuteNonQuery($"UPDATE SspExportProd_Log SET STATUS = {(int)item.StatusLog},  Description = '{item.Erro}' WHERE [TYPE] = 0 AND ID_MES = {item.Id}");
                    }
                    else
                    {
                        helper.ExecuteNonQuery($"UPDATE SspExportProd_Log SET STATUS = {(int)item.StatusLog},  Description = '{item.Erro}' WHERE ID_MES = {item.Id}");
                    }
                }
            }
        }

        public void VerificaRegistroTempo()
        {
            Logger.Debug("Verificando registro de tempo");
            string update = DAO.MES.Log_Update;
            SqlHelper helperSka = new SqlHelper("ska");
            SqlHelper helperSap = new SqlHelper("sap");

            using (var dr = helperSap.ExecuteQuery(DAO.BEAS.HorasApontadas_Get))
            {
                while (dr.Read())
                {
                    int idMes = (int)dr["Id"];
                    string erro = dr["Erro"].ToString();
                    StatusLogEnum status;
                    if (!String.IsNullOrEmpty(erro))
                    {
                        status = StatusLogEnum.VerificarObservacao;
                    }
                    else
                    {
                        status = StatusLogEnum.ImportadoComSucesso;
                    }

                    string updateSql = String.Format(update, (int)status, erro.Replace("'", ""), idMes, (int)TipoLogEnum.Hora);
                    helperSka.ExecuteNonQuery(updateSql);
                }
            }

            helperSka.Dispose();
            helperSap.Dispose();
        }
    }
}
