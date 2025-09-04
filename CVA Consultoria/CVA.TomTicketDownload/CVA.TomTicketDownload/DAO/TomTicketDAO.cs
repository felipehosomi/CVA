using CVA.TomTicketDownload.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CVA.TomTicketDownload.DAO
{
    public class TomTicketDAO
    {
        SqlConnection conn { get; set; }
        public TomTicketDAO _TomTicketDAO { get; set; }

        private static readonly string token = ConfigurationManager.AppSettings["Token"];
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public TomTicketDAO()
        {
            conn = new SqlConnection();
            OpenConnection();
        }
        public void OpenConnection()
        {
            conn.Close();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MeuContext"].ConnectionString;
            conn.Open();
        }

        public void InsertTicket(Chamado chamado)
        {
            var chamados = new List<Chamado>();
            var historicoStatus = new List<HistoricoStatus>();

            IRestResponse<List<ResultRoot>> resposta;

            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    chamado.Titulo = (!String.IsNullOrEmpty(chamado.Titulo) ? chamado.Titulo.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Titulo);
                    chamado.Titulo = (chamado.Titulo.Length > 500 ? chamado.Titulo.Substring(0, 498) : chamado.Titulo);
                    chamado.Mimetype = (!String.IsNullOrEmpty(chamado.Mimetype) ? chamado.Mimetype.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Mimetype);
                    chamado.EmailCliente = (!String.IsNullOrEmpty(chamado.EmailCliente) ? chamado.EmailCliente.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.EmailCliente);
                    chamado.Deadline = (!String.IsNullOrEmpty(chamado.Deadline) ? chamado.Deadline.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Deadline);
                    chamado.NomeCliente = (!String.IsNullOrEmpty(chamado.NomeCliente) ? chamado.NomeCliente.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.NomeCliente);
                    chamado.TipoChamado = (!String.IsNullOrEmpty(chamado.TipoChamado) ? chamado.TipoChamado.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.TipoChamado);
                    chamado.AvaliadoProblemaResolvido = (!String.IsNullOrEmpty(chamado.AvaliadoProblemaResolvido) ? chamado.AvaliadoProblemaResolvido.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.AvaliadoProblemaResolvido);
                    chamado.AvaliadoAtendimento = (!String.IsNullOrEmpty(chamado.AvaliadoAtendimento) ? chamado.AvaliadoAtendimento.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.AvaliadoAtendimento);
                    chamado.AvaliacaoComentario = (!String.IsNullOrEmpty(chamado.AvaliacaoComentario) ? chamado.AvaliacaoComentario.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.AvaliacaoComentario);
                    chamado.AvaliacaoComentario = (chamado.AvaliacaoComentario?.Length > 500 ? chamado.AvaliacaoComentario?.Substring(0, 498) : chamado?.AvaliacaoComentario);
                    chamado.DataPrimeiraResposta = (!String.IsNullOrEmpty(chamado.DataPrimeiraResposta) ? chamado.DataPrimeiraResposta.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.DataPrimeiraResposta);
                    chamado.DescSituacao = (!String.IsNullOrEmpty(chamado.DescSituacao) ? chamado.DescSituacao.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.DescSituacao);
                    chamado.Categoria = (!String.IsNullOrEmpty(chamado.Categoria) ? chamado.Categoria.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Categoria);
                    chamado.Departamento = (!String.IsNullOrEmpty(chamado.Departamento) ? chamado.Departamento.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Departamento);
                    chamado.Atendente = (!String.IsNullOrEmpty(chamado.Atendente) ? chamado.Atendente.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Atendente);
                    chamado.IdCliente = (!String.IsNullOrEmpty(chamado.IdCliente) ? chamado.IdCliente.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.IdCliente);
                    chamado.Status = (!String.IsNullOrEmpty(chamado.Status) ? chamado.Status.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.Status);
                    chamado.NomeOrganizacao = (!String.IsNullOrEmpty(chamado.NomeOrganizacao) ? chamado.NomeOrganizacao.ToString().Replace("'", String.Empty).Replace(System.Environment.NewLine, String.Empty) : chamado.NomeOrganizacao);

                    cmd.CommandText = string.Format(@$"INSERT INTO chamados(IdChamado,Protocolo,Titulo,Mimetype,EmailCliente,Prioridade,TempoTrabalho,TempoAbertura,DataCriacao,Deadline,ValorItemHora,ValorItemHoraExtra,ValorFinal,ValorFinalExtra,NomeCliente,TipoChamado,AvaliadoProblemaResolvido,AvaliadoAtendimento,AvaliacaoComentario,DataPrimeiraResposta,DataEncerramento,UltimaSituacao,DataUltimaSituacao,SlaInicio,SlaDeadline,SlaInicializacaoCumprido,SlaDeadlineCumprido,DescSituacao,Categoria,Departamento,Atendente,IdCliente,Status,DataUltimoStatus,NomeOrganizacao,HistoricoStatus)VALUES('{chamado.IdChamado}',{chamado.Protocolo},'{chamado.Titulo}','{chamado.Mimetype}','{chamado.EmailCliente}',{chamado.Prioridade},{chamado.TempoTrabalho},'{chamado.TempoAbertura}','{chamado.DataCriacao}','{chamado.Deadline}',{chamado.ValorItemHora},{chamado.ValorItemHoraExtra},{chamado.ValorFinal},{chamado.ValorFinalExtra},'{chamado.NomeCliente}','{chamado.TipoChamado}','{chamado.AvaliadoProblemaResolvido}','{chamado.AvaliadoAtendimento}','{chamado.AvaliacaoComentario}','{chamado.DataPrimeiraResposta}','{chamado.DataEncerramento}',{chamado.UltimaSituacao},'{chamado.DataUltimaSituacao}','{chamado.SlaInicio}','{chamado.SlaDeadline}','{chamado.SlaInicializacaoCumprido}','{chamado.SlaDeadlineCumprido}','{chamado.DescSituacao}','{chamado.Categoria}','{chamado.Departamento}','{chamado.Atendente}','{chamado.IdCliente}','{chamado.Status}','{chamado.DataUltimoStatus}','{chamado.NomeOrganizacao}','')"); //{chamado.HistoricoStatus}
                    cmd.ExecuteReader();
                    conn.Close();

                    RestClient client = new RestClient("https://api.tomticket.com/chamado/" + token + "/" + chamado.IdChamado + "");
                    RestRequest requisicao = new RestRequest("", Method.GET);
                    resposta = client.Execute<List<ResultRoot>>(requisicao);

                    var res = resposta?.Data;
                    foreach (var item in res)
                    {
                        chamados.AddRange(item.Data);
                    }

                    foreach (var item in chamados)
                    {
                        historicoStatus = item.HistoricoStatus;
                    }
                    foreach (var item in historicoStatus)
                    {
                        item.ProtolocoChamado = chamados[0]?.Protocolo.ToString();

                        try
                        {
                            if (!Check_HistoryExists(item.Id, item.ProtolocoChamado, item.Inicio))
                            {
                                Console.WriteLine($"Inserindo historico do chamado: {item.ProtolocoChamado}");
                                InsertHistory(item);
                            }
                            else if (Check_HistoryUpdate(item.Id, item.ProtolocoChamado, item.Inicio, item.Fim))
                            {
                                Console.WriteLine($"Atualizando historico do chamado: {item.ProtolocoChamado}");
                                UpdateHistory(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Erro ao Cadastrar / Atualizar historico item: " + item.ProtolocoChamado + "--> " + ex.Message);
                            var LoggerError = new Log();
                            LoggerError.ProtolocoChamado = item.ProtolocoChamado;
                            LoggerError.Data = DateTime.Now.ToString();
                            LoggerError.Motivo = ex.Message;
                            InserLog(LoggerError);

                        }
                        finally
                        {
                            if (item.Id == "dd0710db5b26f955bb8a61ce07e5ea78" && !String.IsNullOrEmpty(item.Inicio) && !String.IsNullOrEmpty(item.Fim))
                            {
                                chamado.TempoAtendimento += tempoAtendimento(item.Inicio, item.Fim);
                                UpdateAtendimento(chamado);
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao inserir ticket --> " + ex.Message);
                var LoggerError = new Log();
                LoggerError.ProtolocoChamado = chamado.Protocolo.ToString();
                LoggerError.Data = DateTime.Now.ToString();
                LoggerError.Motivo = "Erro ao inserir ticket --> " + ex.Message;
                InserLog(LoggerError);
                throw ex;
            }
        }
        public void InsertHistory(HistoricoStatus hs)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    if (String.IsNullOrEmpty(hs.TempoAtendimento))
                    {
                        hs.TempoAtendimento = "0";
                    }
                    if (!String.IsNullOrEmpty(hs.Inicio) && !String.IsNullOrEmpty(hs.Fim))
                    {
                        hs.TempoAtendimento = tempoAtendimento(hs.Inicio, hs.Fim).ToString();
                    }
                    cmd.CommandText = string.Format(@$"INSERT INTO historico_status(IdStatus,Status,Atendente_inicio,Atendente_fim,Inicio,Fim,ProtolocoChamado,TempoAtendimento)VALUES('{hs.Id}','{hs.Status}','{hs.Atendente_inicio }','{hs.Atendente_fim}','{hs.Inicio}','{hs.Fim}','{hs.ProtolocoChamado}','{hs.TempoAtendimento}')");
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao inserir historico_status --> " + ex.Message);
                var LoggerError = new Log();
                LoggerError.ProtolocoChamado = hs.ProtolocoChamado;
                LoggerError.Data = DateTime.Now.ToString();
                LoggerError.Motivo = "Erro ao inserir historico_status --> " + ex.Message;
                InserLog(LoggerError);
                throw ex;
            }
        }
        public void UpdateHistory(HistoricoStatus hs)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    if (String.IsNullOrEmpty(hs.TempoAtendimento))
                    {
                        hs.TempoAtendimento = "0";
                    }
                    if (!String.IsNullOrEmpty(hs.Inicio) && !String.IsNullOrEmpty(hs.Fim))
                    {
                        hs.TempoAtendimento = tempoAtendimento(hs.Inicio, hs.Fim).ToString();
                    }

                    string query = "UPDATE historico_status SET Atendente_fim=@Atendente_fim,Fim=@Fim,TempoAtendimento=@TempoAtendimento WHERE IdStatus =@IdStatus and Inicio=@Inicio and ProtolocoChamado=@ProtolocoChamado;";
                    cmd.CommandText = query;
                    #region CMD Param
                    cmd.Parameters.AddWithValue("@IdStatus", hs.Id);
                    if (String.IsNullOrEmpty(hs.Atendente_fim))
                    {
                        hs.Atendente_fim = "Sem atendente";
                    }
                    cmd.Parameters.AddWithValue("@Atendente_fim", hs.Atendente_fim);
                    if (String.IsNullOrEmpty(hs.Fim))
                    {
                        hs.Fim = "Sem fim";
                    }
                    cmd.Parameters.AddWithValue("@Fim", hs.Fim);
                    if (String.IsNullOrEmpty(hs.Inicio))
                    {
                        hs.Inicio = "Sem inicio";
                    }
                    cmd.Parameters.AddWithValue("@Inicio", hs.Inicio);
                    if (String.IsNullOrEmpty(hs.TempoAtendimento))
                    {
                        hs.TempoAtendimento = "0";
                    }
                    cmd.Parameters.AddWithValue("@TempoAtendimento", hs.TempoAtendimento);
                    if (String.IsNullOrEmpty(hs.ProtolocoChamado))
                    {
                        hs.ProtolocoChamado = "0";
                    }
                    cmd.Parameters.AddWithValue("@ProtolocoChamado", hs.ProtolocoChamado);

                    #endregion
                    cmd.ExecuteNonQuery();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao atualizar historico do ticket --> " + ex.Message);
                var LoggerError = new Log();
                LoggerError.ProtolocoChamado = hs.ProtolocoChamado;
                LoggerError.Data = DateTime.Now.ToString();
                LoggerError.Motivo = "Erro ao atualizar historico do ticket --> " + ex.Message;
                InserLog(LoggerError);
                throw ex;
            }
        }
        public void UpdateAtendimento(Chamado chamado)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    string query = "UPDATE chamados SET TempoAtendimento=@TempoAtendimento WHERE IdChamado=@IdChamado;";
                    cmd.CommandText = query;
                    #region CMD Param
                    cmd.Parameters.AddWithValue("@IdChamado", chamado.IdChamado);
                    cmd.Parameters.AddWithValue("@TempoAtendimento", chamado.TempoAtendimento);
                    #endregion
                    cmd.ExecuteNonQuery();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao atualizar tempo de atendimento do ticket --> " + ex.Message);
                var LoggerError = new Log();
                LoggerError.ProtolocoChamado = chamado.Protocolo.ToString();
                LoggerError.Data = DateTime.Now.ToString();
                LoggerError.Motivo = "Erro ao atualizar tempo de atendimento do ticket --> " + ex.Message;
                InserLog(LoggerError);
                throw ex;
            }
        }
        public void UpdateTicket(Chamado chamado)
        {
            var chamados = new List<Chamado>();
            var historicoStatus = new List<HistoricoStatus>();
            var historicos = new List<Historico>();

            IRestResponse<List<ResultRoot>> resposta;

            try
            {
                using (conn)
                {
                    #region Adiciona Historico
                    RestClient client = new RestClient("https://api.tomticket.com/chamado/" + token + "/" + chamado.IdChamado + "");
                    RestRequest requisicao = new RestRequest("", Method.GET);
                    resposta = client.Execute<List<ResultRoot>>(requisicao);

                    var res = resposta?.Data;
                    foreach (var item in res)
                    {
                        chamados.AddRange(item.Data);
                    }

                    foreach (var item in chamados)
                    {
                        historicoStatus = item.HistoricoStatus;
                        historicos = item.historico;
                    }
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["alteraStatus"]))
                    {
                        if (historicos.Count > 1)
                        {
                            client = new RestClient("https://api.tomticket.com/ticket/status/close/" + token + "");
                            requisicao = new RestRequest("", Method.POST);
                            requisicao.AddParameter("ticket_id", chamados[0].IdChamado);

                            if (historicos[0].Origem == "C" && chamados[0].DescSituacao != "Finalizado" && historicoStatus[0].Id != "dd0710db5b26f955bb8a61ce07e5ea78")
                            {
                                try
                                {
                                    Logger.Info($"Fechando status no chamado: {chamados[0].Protocolo}");
                                    resposta = client.Execute<List<ResultRoot>>(requisicao);
                                    res = resposta?.Data;
                                    if (!res[0].Erro)
                                    {
                                        Logger.Info($"Iniciando status no chamado: {chamados[0].Protocolo}");
                                        client = new RestClient("https://api.tomticket.com/ticket/status/open/" + token + "");
                                        requisicao = new RestRequest("", Method.POST);
                                        requisicao.AddParameter("ticket_id", chamados[0].IdChamado);
                                        requisicao.AddParameter("status_id", "b1cf1dcd5f78d7a83db2387f81857375");
                                        resposta = client.Execute<List<ResultRoot>>(requisicao);
                                        res = resposta?.Data;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error("Erro ao Fechar ou Iniciar status no chamado: " + chamados[0].Protocolo + "-->" + ex.Message);
                                    var LoggerError = new Log();
                                    LoggerError.ProtolocoChamado = chamados[0].Protocolo.ToString();
                                    LoggerError.Data = DateTime.Now.ToString();
                                    LoggerError.Motivo = "Erro ao Fechar ou Iniciar status no chamado: " + chamados[0].Protocolo + "-->" + ex.Message;
                                    InserLog(LoggerError);
                                }
                            }

                        }
                    }

                    foreach (var item in historicoStatus)
                    {
                        item.ProtolocoChamado = chamados[0]?.Protocolo.ToString();

                        try
                        {
                            if (!Check_HistoryExists(item.Id, item.ProtolocoChamado, item.Inicio))
                            {
                                Console.WriteLine($"Inserindo historico do chamado: {item.ProtolocoChamado}");
                                InsertHistory(item);
                            }
                            else if (Check_HistoryUpdate(item.Id, item.ProtolocoChamado, item.Inicio, item.Fim))
                            {
                                Console.WriteLine($"Atualizando historico do chamado: {item.ProtolocoChamado}");
                                UpdateHistory(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Erro ao Cadastrar / Atualizar historico item: " + item.ProtolocoChamado + "--> " + ex.Message);
                            var LoggerError = new Log();
                            LoggerError.ProtolocoChamado = item.ProtolocoChamado;
                            LoggerError.Data = DateTime.Now.ToString();
                            LoggerError.Motivo = ex.Message;
                            InserLog(LoggerError);
                        }
                        finally
                        {
                            if (item.Id == "dd0710db5b26f955bb8a61ce07e5ea78" && !String.IsNullOrEmpty(item.Inicio) && !String.IsNullOrEmpty(item.Fim))
                            {
                                chamado.TempoAtendimento += tempoAtendimento(item.Inicio, item.Fim);
                                UpdateAtendimento(chamado);
                            }
                        }
                    }
                    #endregion

                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    string query = "UPDATE chamados SET Protocolo=@Protocolo,Titulo=@Titulo,Mimetype=@Mimetype,EmailCliente=@EmailCliente,Prioridade=@Prioridade,TempoTrabalho=@TempoTrabalho,TempoAbertura=@TempoAbertura,DataCriacao=@DataCriacao,Deadline=@Deadline,ValorItemHora=@ValorItemHora,ValorItemHoraExtra=@ValorItemHoraExtra,ValorFinal=@ValorFinal,ValorFinalExtra=@ValorFinalExtra,NomeCliente=@NomeCliente,TipoChamado=@TipoChamado,AvaliadoProblemaResolvido=@AvaliadoProblemaResolvido,AvaliadoAtendimento=@AvaliadoAtendimento,AvaliacaoComentario=@AvaliacaoComentario,DataPrimeiraResposta=@DataPrimeiraResposta,DataEncerramento=@DataEncerramento,UltimaSituacao=@UltimaSituacao,DataUltimaSituacao=@DataUltimaSituacao,SlaInicio=@SlaInicio,SlaDeadline=@SlaDeadline,SlaInicializacaoCumprido=@SlaInicializacaoCumprido,SlaDeadlineCumprido=@SlaDeadlineCumprido,DescSituacao=@DescSituacao,Categoria=@Categoria,Departamento=@Departamento,Atendente=@Atendente,IdCliente=@IdCliente,Status=@Status,DataUltimoStatus=@DataUltimoStatus,NomeOrganizacao=@NomeOrganizacao WHERE IdChamado =@IdChamado;";
                    cmd.CommandText = query;
                    #region CMD Param
                    cmd.Parameters.AddWithValue("@IdChamado", chamado.IdChamado);
                    cmd.Parameters.AddWithValue("@Protocolo", chamado.Protocolo);
                    cmd.Parameters.AddWithValue("@Titulo", chamado.Titulo);
                    cmd.Parameters.AddWithValue("@Mimetype", chamado.Mimetype);
                    cmd.Parameters.AddWithValue("@EmailCliente", chamado.EmailCliente);
                    cmd.Parameters.AddWithValue("@Prioridade", chamado.Prioridade);
                    cmd.Parameters.AddWithValue("@TempoTrabalho", chamado.TempoTrabalho);
                    if (String.IsNullOrEmpty(chamado.TempoAbertura))
                    {
                        chamado.TempoAbertura = "0";
                    }
                    cmd.Parameters.AddWithValue("@TempoAbertura", chamado.TempoAbertura);
                    cmd.Parameters.AddWithValue("@DataCriacao", chamado.DataCriacao);
                    cmd.Parameters.AddWithValue("@ValorItemHora", chamado.ValorItemHora);
                    if (String.IsNullOrEmpty(chamado.Deadline))
                    {
                        chamado.Deadline = "0";
                    }
                    cmd.Parameters.AddWithValue("@Deadline", chamado.Deadline);
                    cmd.Parameters.AddWithValue("@ValorItemHoraExtra", chamado.ValorItemHoraExtra);
                    cmd.Parameters.AddWithValue("@ValorFinal", chamado.ValorFinal);
                    cmd.Parameters.AddWithValue("@ValorFinalExtra", chamado.ValorFinalExtra);
                    cmd.Parameters.AddWithValue("@NomeCliente", chamado.NomeCliente);
                    cmd.Parameters.AddWithValue("@TipoChamado", chamado.TipoChamado);
                    if (String.IsNullOrEmpty(chamado.AvaliadoProblemaResolvido))
                    {
                        chamado.AvaliadoProblemaResolvido = "0";
                    }
                    cmd.Parameters.AddWithValue("@AvaliadoProblemaResolvido", chamado.AvaliadoProblemaResolvido);
                    if (String.IsNullOrEmpty(chamado.AvaliadoAtendimento))
                    {
                        chamado.AvaliadoAtendimento = "0";
                    }
                    cmd.Parameters.AddWithValue("@AvaliadoAtendimento", chamado.AvaliadoAtendimento);
                    if (String.IsNullOrEmpty(chamado.AvaliacaoComentario))
                    {
                        chamado.AvaliacaoComentario = "N/A";
                    }
                    cmd.Parameters.AddWithValue("@AvaliacaoComentario", chamado.AvaliacaoComentario);
                    if (String.IsNullOrEmpty(chamado.DataPrimeiraResposta))
                    {
                        chamado.DataPrimeiraResposta = "0";
                    }
                    cmd.Parameters.AddWithValue("@DataPrimeiraResposta", chamado.DataPrimeiraResposta);
                    if (String.IsNullOrEmpty(chamado.DataEncerramento))
                    {
                        chamado.DataEncerramento = "0";
                    }
                    cmd.Parameters.AddWithValue("@DataEncerramento", chamado.DataEncerramento);
                    cmd.Parameters.AddWithValue("@UltimaSituacao", chamado.UltimaSituacao);
                    cmd.Parameters.AddWithValue("@DataUltimaSituacao", chamado.DataUltimaSituacao);
                    if (String.IsNullOrEmpty(chamado.SlaInicio))
                    {
                        chamado.SlaInicio = "N/A";
                    }
                    cmd.Parameters.AddWithValue("@SlaInicio", chamado.SlaInicio);
                    if (String.IsNullOrEmpty(chamado.SlaDeadline))
                    {
                        chamado.SlaDeadline = "0";
                    }
                    cmd.Parameters.AddWithValue("@SlaDeadline", chamado.SlaDeadline);
                    if (String.IsNullOrEmpty(chamado.SlaInicializacaoCumprido))
                    {
                        chamado.SlaInicializacaoCumprido = "0";
                    }
                    cmd.Parameters.AddWithValue("@SlaInicializacaoCumprido", chamado.SlaInicializacaoCumprido);
                    if (chamado.SlaDeadlineCumprido.ToString() == "")
                    {
                        chamado.SlaDeadlineCumprido = false;
                    }
                    cmd.Parameters.AddWithValue("@SlaDeadlineCumprido", chamado.SlaDeadlineCumprido);
                    cmd.Parameters.AddWithValue("@DescSituacao", chamado.DescSituacao);
                    cmd.Parameters.AddWithValue("@Categoria", chamado.Categoria);
                    cmd.Parameters.AddWithValue("@Departamento", chamado.Departamento);
                    if (String.IsNullOrEmpty(chamado.Atendente))
                    {
                        chamado.Atendente = "Sem atendente";
                    }
                    cmd.Parameters.AddWithValue("@Atendente", chamado.Atendente);
                    if (String.IsNullOrEmpty(chamado.IdCliente))
                    {
                        chamado.IdCliente = "0";
                    }
                    cmd.Parameters.AddWithValue("@IdCliente", chamado.IdCliente);
                    if (String.IsNullOrEmpty(chamado.Status))
                    {
                        chamado.Status = "N/D";
                    }
                    cmd.Parameters.AddWithValue("@Status", chamado.Status);
                    if (String.IsNullOrEmpty(chamado.DataUltimoStatus))
                    {
                        chamado.DataUltimoStatus = "N/D";
                    }
                    cmd.Parameters.AddWithValue("@DataUltimoStatus", chamado.DataUltimoStatus);
                    cmd.Parameters.AddWithValue("@NomeOrganizacao", chamado.NomeOrganizacao);


                    #endregion
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao atualizar ticket --> " + ex.Message);
                var LoggerError = new Log();
                LoggerError.ProtolocoChamado = chamado.Protocolo.ToString();
                LoggerError.Data = DateTime.Now.ToString();
                LoggerError.Motivo = "Erro ao atualizar ticket --> " + ex.Message;
                InserLog(LoggerError);
                throw ex;
            }
        }
        public string ultimaSituacao()
        {

            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    DataSet ds = new DataSet();

                    cmd.Connection = conn;

                    string query = "SELECT top 1 DataUltimaSituacao FROM chamados order by SUBSTRING(DataUltimaSituacao, 7, 4) desc ,SUBSTRING(DataUltimaSituacao, 4, 2) desc,SUBSTRING(DataUltimaSituacao, 1, 2) desc, SUBSTRING( DataUltimaSituacao, 12, 2) desc, SUBSTRING( DataUltimaSituacao, 15, 2) desc";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    var result = cmd.ExecuteScalar();

                    conn.Close();

                    return result.ToString();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao verificar ultimas situação dos chamados --> " + ex.Message);
                return "erro" + ex;
            }

        }
        public bool Check_ItemExists(string ItemCode)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format(@"select count(*) as 'item' from chamados where IdChamado = '{0}'", ItemCode);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();

                        var t = tb.Rows[0][0].ToString();
                        if (tb.Rows[0][0].ToString() == "0")
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao verificar chamado existe" + ItemCode + "--> " + ex.Message);
                throw ex;
            }
        }
        public void Delete_ItemExists(string sChamado)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format(@"SELECT Protocolo FROM chamados WHERE IdChamado = '{0}'", sChamado);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);

                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            string sProtocolo = tb.Rows[i][0].ToString();

                            SqlCommand command = new SqlCommand();
                            command.Connection = conn;
                            command.CommandText = String.Format(@"DELETE historico_status WHERE ProtolocoChamado = {0}; 
                                                                  DELETE chamados WHERE Protocolo = {0};", sProtocolo);
                            command.ExecuteNonQuery();
                        }

                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao verificar chamado existe" + sChamado + "--> " + ex.Message);
                throw ex;
            }
        }
        public bool Check_HistoryExists(string ItemCode, string Protocolo, string Inicio)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format(@"select count(*) as 'item' from historico_status where IdStatus = '{0}' and ProtolocoChamado = '{1}' and Inicio = '{2}'", ItemCode, Protocolo, Inicio);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();

                        var t = tb.Rows[0][0].ToString();
                        if (tb.Rows[0][0].ToString() == "0")
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao verificar Historico existe." + Protocolo + "--> " + ex.Message);
                throw ex;
            }
        }
        public bool Check_HistoryUpdate(string ItemCode, string Protocolo, string Inicio, string Fim)
        {
            try
            {
                Logger.Info("Atualização do historico do chamado " + Protocolo + "os dados: Procolo:" + Protocolo + " ItemCode: " + ItemCode + " Inicio:" + Inicio + " Fim:" + Fim);
                if (!String.IsNullOrEmpty(ItemCode) && !String.IsNullOrEmpty(Protocolo) && !String.IsNullOrEmpty(Inicio) && !String.IsNullOrEmpty(Fim))
                {
                    using (conn)
                    {
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = string.Format(@"select count(*) as 'item' from historico_status where IdStatus = '{0}' and ProtolocoChamado = '{1}' and Inicio = '{2}' and  Fim = '{3}'", ItemCode, Protocolo, Inicio, Fim);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            var tb = new DataTable();
                            tb.Load(dr);
                            conn.Close();

                            var t = tb.Rows[0][0].ToString();
                            if (tb.Rows[0][0].ToString() == "0")
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                    return false;
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao verificar Historico Update" + Protocolo + "--> " + ex.Message);
                throw ex;
            }
        }
        public double tempoAtendimento(string inicioAtendimento, string fimAtendimento)
        {
            List<DateTime> lst = (List<DateTime>)holidayListBrazil.GetHolidaysByCurrentYear();
            List<DateTime> lstDias = new List<DateTime>();
            TimeSpan tempoAtendimento = new TimeSpan();
            DateTime dtInicio = DateTime.Parse(inicioAtendimento);
            DateTime dtFinal = DateTime.Parse(fimAtendimento);
            double antesAlmoco = 12;
            DateTime inicio = dtInicio;
            DateTime fim = dtFinal;
            double tmpAtendimento = 0, minutos;

            TimeSpan horaTotal = new TimeSpan(dtFinal.Ticks - dtInicio.Ticks);

            //Menos de 1 dia
            if (horaTotal.Days < 1)
            {
                //se for no mesmo dia
                if (dtFinal.Date == dtInicio.Date)
                {
                    //apenas parte da manhã
                    if (dtInicio.Hour < antesAlmoco && dtFinal.Hour < antesAlmoco)
                    {
                        tempoAtendimento = new TimeSpan(dtFinal.Ticks - dtInicio.Ticks);
                    }//apenas parte da tarde
                    else if (dtInicio.Hour > antesAlmoco && dtFinal.Hour > antesAlmoco)
                    {
                        tempoAtendimento = new TimeSpan(dtFinal.Ticks - dtInicio.Ticks);
                    }// inicio manhã termino a tarde
                    else
                    {
                        if (dtInicio.Hour < antesAlmoco)
                        {
                            tempoAtendimento = new TimeSpan(dtInicio.Date.AddHours(12).AddMinutes(00).Ticks - dtInicio.Ticks);
                        }
                        if (dtFinal.Hour > antesAlmoco)
                        {
                            tempoAtendimento = new TimeSpan(tempoAtendimento.Ticks + (dtFinal.Ticks - dtFinal.Date.AddHours(13).AddMinutes(00).Ticks));
                        }
                    }
                }// menos de 24 horas com final no proximo dia 
                else if (dtFinal.Date > dtInicio.Date)
                {
                    if (dtInicio.Hour < 12)
                    {
                        tempoAtendimento = new TimeSpan(dtInicio.Date.AddHours(16).AddMinutes(30).Ticks - dtInicio.Ticks);
                    }
                    else if (dtInicio.Hour > 12 && (dtInicio.Hour <= 17 && dtInicio.Minute <= 31))
                    {
                        tempoAtendimento = new TimeSpan(dtInicio.Date.AddHours(17).AddMinutes(30).Ticks - dtInicio.Ticks);
                    }

                    if (dtFinal.Hour > 12)
                    {
                        tempoAtendimento = new TimeSpan(tempoAtendimento.Ticks + (dtFinal.Ticks - dtFinal.Date.AddHours(13).AddMinutes(00).Ticks));
                    }
                    else if (dtFinal.Hour < 12)
                    {
                        tempoAtendimento = new TimeSpan(tempoAtendimento.Ticks + (dtFinal.Ticks - dtFinal.Date.AddHours(08).AddMinutes(30).Ticks));
                    }

                }
            }
            else if (horaTotal.Days >= 1)//Mais de 1 dia
            {
                for (int i = 0; i <= horaTotal.Days; i++)
                {
                    if (!lst.Exists(x => x.Date == dtInicio.Date))
                    {
                        if (dtInicio.DayOfWeek == DayOfWeek.Sunday || dtInicio.DayOfWeek == DayOfWeek.Saturday) { }
                        else
                        {
                            if (dtInicio.Day == inicio.Day)
                            {
                                if (inicio.Hour < 12)
                                {
                                    minutos = inicio.Minute;
                                    minutos = minutos / 60;
                                    tmpAtendimento = (12 - dtInicio.Hour - minutos) + 4.5;
                                }
                                else if (inicio.Hour == 17 && inicio.Minute < 30)
                                {
                                    minutos = inicio.Minute;
                                    minutos = minutos / 60;
                                    tmpAtendimento = 17.5 - (dtInicio.Hour + minutos);
                                }
                            }
                            else if (dtInicio.Day == fim.Day)
                            {
                                if (fim.Hour < 12)
                                {
                                    minutos = fim.Minute;
                                    minutos = minutos / 60;
                                    tmpAtendimento += (fim.Hour + minutos) - 8.5;
                                }
                                else
                                {
                                    minutos = fim.Minute;
                                    minutos = minutos / 60;
                                    tmpAtendimento += (fim.Hour + minutos) - 9.5;
                                }
                            }
                            else
                            {
                                tmpAtendimento += 8;
                            }
                        }
                    }
                    dtInicio = dtInicio.AddDays(1);
                }
            }

            DateTime dt = new DateTime();
            dt = dt.AddHours(tmpAtendimento);
            int dia = dt.Day - 1;
            string resultado = ("Atendimento:" + dia + "d " + dt.Hour + "h" + dt.Minute + "min");


            if (tmpAtendimento > 0)
            {
                return tmpAtendimento;
            }
            else
                return Convert.ToDouble(tempoAtendimento.TotalHours);
        }
        public void recalcularHoras()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader = null;
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format(@"select * from historico_status");
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string inicio = reader["Inicio"].ToString();
                        string fim = reader["Fim"].ToString();
                        if (!String.IsNullOrEmpty(inicio) && !String.IsNullOrEmpty(fim))
                        {
                            var resultado = tempoAtendimento(inicio, fim);
                            SqlConnection conn2 = new SqlConnection();
                            conn2.ConnectionString = ConfigurationManager.ConnectionStrings["MeuContext"].ConnectionString;
                            conn2.Open();
                            cmd = new SqlCommand();
                            cmd.Connection = conn2;
                            string query = "UPDATE historico_status SET TempoAtendimento=@TempoAtendimento WHERE ProtolocoChamado=@ProtolocoChamado and Inicio=@Inicio;";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@ProtolocoChamado", reader["ProtolocoChamado"]);
                            cmd.Parameters.AddWithValue("@Inicio", reader["Inicio"]);
                            cmd.Parameters.AddWithValue("@TempoAtendimento", resultado);
                            cmd.ExecuteNonQuery();
                            cmd.ExecuteReader();
                            conn2.Close();
                        }

                    }

                    conn.Close();
                    SqlConnection conn3 = new SqlConnection();
                    conn3.ConnectionString = ConfigurationManager.ConnectionStrings["MeuContext"].ConnectionString;
                    conn3.Open();
                    SqlCommand cmd3 = new SqlCommand();
                    SqlDataReader reader3 = null;
                    cmd3.Connection = conn3;
                    cmd3.CommandText = string.Format(@"select * from chamados");
                    reader3 = cmd3.ExecuteReader();

                    while (reader3.Read())
                    {
                        string qtdHoras = "";
                        string protocolo = reader3["Protocolo"].ToString();
                        SqlConnection conn5 = new SqlConnection();
                        conn5.ConnectionString = ConfigurationManager.ConnectionStrings["MeuContext"].ConnectionString;
                        conn5.Open();
                        cmd = new SqlCommand();
                        cmd.Connection = conn5;
                        string query = "select SUM(CAST(TempoAtendimento as float)) from historico_status where ProtolocoChamado = @Protocolo and IdStatus = 'dd0710db5b26f955bb8a61ce07e5ea78'";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@Protocolo", protocolo);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            var tb = new DataTable();
                            tb.Load(dr);
                            var t = tb.Rows[0][0].ToString();
                            if (tb.Rows[0][0].ToString() == "")
                            {
                                qtdHoras = "0";
                            }
                            else
                            {
                                qtdHoras = tb.Rows[0][0].ToString();
                                SqlConnection conn2 = new SqlConnection();
                                conn2.ConnectionString = ConfigurationManager.ConnectionStrings["MeuContext"].ConnectionString;
                                conn2.Open();
                                cmd = new SqlCommand();
                                cmd.Connection = conn2;
                                string queryHora = "UPDATE chamados SET TempoAtendimento=@TempoAtendimento WHERE Protocolo=@Protocolo;";
                                cmd.CommandText = queryHora;
                                cmd.Parameters.AddWithValue("@Protocolo", protocolo);
                                cmd.Parameters.AddWithValue("@TempoAtendimento", qtdHoras);
                                cmd.ExecuteNonQuery();
                                cmd.ExecuteReader();
                                conn2.Close();
                            }

                        }
                        conn5.Close();
                    }

                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao processar calculo de horas novamente-> " + ex.Message);
                throw ex;
            }
        }

        public void InserLog(Log logger)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = string.Format(@$"INSERT INTO log_tomticket(ProtolocoChamado,Data,Motivo)VALUES('{logger.ProtolocoChamado}','{logger.Data}','{logger.Motivo}')");
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Erro ao inserir log --> " + ex.Message);
                throw ex;
            }
        }
    }
}
