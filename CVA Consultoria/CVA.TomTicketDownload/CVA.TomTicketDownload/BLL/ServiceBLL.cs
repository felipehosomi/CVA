using CVA.TomTicketDownload.DAO;
using CVA.TomTicketDownload.Model;
using Flurl;
using Flurl.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CVA.TomTicketDownload.BLL
{
    public class ServiceBLL
    {
        public TomTicketDAO _TomTicketDAO { get; set; }

        private static readonly string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        private static readonly string token = ConfigurationManager.AppSettings["Token"];
        private static readonly string caminhoDestino = ConfigurationManager.AppSettings["CaminhoDestino"];
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ServiceBLL()
        {
            _TomTicketDAO = new TomTicketDAO();
        }

        public async Task SincronizaChamadosAsync()
        {
            int? pagina = 1, totalChamados, chamadosBaixados = 0;
            var chamados = new List<Chamado>();
            ResultRoot resultado;

            try
            {
                Logger.Info("Iniciando o download dos chamados da API do TomTicket...");
                do
                {
                    resultado = await apiUrl
                        .AppendPathSegment("chamados")
                        .AppendPathSegment(token)
                        .AppendPathSegment(pagina)
                        .SetQueryParam("ordem", 0)
                        .GetJsonAsync<ResultRoot>();

                    totalChamados = resultado?.TotalItens;
                    chamadosBaixados += resultado?.Data?.Count;

                    if (resultado?.Data?.Count > 0)
                    {
                        chamados.AddRange(resultado.Data);
                        Logger.Info($"Baixando chamados: {chamadosBaixados} de {totalChamados}");
                    }

                    pagina++;

                } while (resultado?.Data?.Count > 0);
                pagina = 0;
                foreach (var item in chamados)
                {
                    if (pagina == 1000)
                    {
                        Logger.Info($"Muitas chamadas a API aguarde por 10 minutos.");
                        Thread.Sleep(600000);
                        pagina = 0;
                    }
                    pagina++;
                    try
                    {
                        if (!_TomTicketDAO.Check_ItemExists(item.IdChamado))
                        {
                            Logger.Info($"Inserindo chamado: {item.Protocolo}");
                            _TomTicketDAO.InsertTicket(item);
                        }
                        else
                        {
                            Logger.Info($"Atualizando chamado: {item.IdChamado}");
                            _TomTicketDAO.UpdateTicket(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Erro ao Cadastrar / Atualizar! item: " + item.IdChamado + "--> " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao processar:" + ex.Message);
            }
        }

        public void SincronizaUltimosChamados()
        {
            var chamados = new List<Chamado>();
            IRestResponse<List<ResultRoot>> resposta;
            int? pagina = 1, quantidade;

            try
            {
                Logger.Info("Iniciando o download dos ultimos chamados da API do TomTicket...");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var dataAtual = Convert.ToDateTime(_TomTicketDAO.ultimaSituacao());
                long unixTime = ((DateTimeOffset)dataAtual).ToUnixTimeSeconds();

                do
                {
                    RestClient client = null;
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["sincronizarPorData"]))
                    {
                        string dataSincroniza = ConfigurationManager.AppSettings["dataSincroniza"];
                        client = new RestClient("https://api.tomticket.com/chamados/" + token + "/" + pagina + "? lastsituation_upper=" + dataSincroniza + "");
                    }
                    else
                    {
                        client = new RestClient("https://api.tomticket.com/chamados/" + token + "/" + pagina + "? lastsituation_upper=" + unixTime + "");
                    }
                    RestRequest requisicao = new RestRequest("", Method.GET);

                    resposta = client.Execute<List<ResultRoot>>(requisicao);

                    var res = resposta?.Data;
                    quantidade = (int)(res[0]?.Data?.Count);
                    foreach (var item in res)
                    {
                        chamados.AddRange(item.Data);

                    }
                    pagina++;

                } while (quantidade > 0);

                foreach (var item in chamados)
                {

                    try
                    {
                        if (!_TomTicketDAO.Check_ItemExists(item.IdChamado))
                        {
                            Logger.Info($"Inserindo chamado: {item.Protocolo}");
                            _TomTicketDAO.InsertTicket(item);
                        }
                        else
                        {
                            Logger.Info($"Atualizando chamado: {item.Protocolo}");
                            _TomTicketDAO.UpdateTicket(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Info("Erro ao Cadastrar / Atualizar! item: " + item.IdChamado + "--> " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao processar:" + ex.Message);
            }
        }

        public void SincronizaChamadoUnitario(string sChamados)
        {
            var chamados = new List<Chamado>();
            IRestResponse<List<ResultRoot>> resposta;

            try
            {
                Logger.Info("Iniciando o download dos chamados unitários da API do TomTicket...");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var objChamados = sChamados.Split(';');
                RestClient client = null;

                foreach (var itemChamado in objChamados)
                {
                    client = new RestClient("https://api.tomticket.com/chamado/" + token + "/" + itemChamado);
                    RestRequest requisicao = new RestRequest("", Method.GET);
                    resposta = client.Execute<List<ResultRoot>>(requisicao);
                    var res = resposta?.Data;
                    foreach (var item in res)
                    {
                        try
                        {
                            if (item.Erro)
                                throw new Exception(String.Format(@"{0}", item.mensagem));

                            chamados.AddRange(item.Data);
                        }
                        catch (Exception ex)
                        {
                            Logger.Info("Erro ao buscar o item: " + itemChamado + " na API --> " + ex.Message);
                        }
                    }
                }

                foreach (var item in chamados)
                {
                    try
                    {
                        _TomTicketDAO.Delete_ItemExists(item.IdChamado);
                        Logger.Info($"Inserindo chamado: {item.Protocolo}");
                        _TomTicketDAO.InsertTicket(item);
                    }
                    catch (Exception ex)
                    {
                        Logger.Info("Erro ao Cadastrar item: " + item.IdChamado + "--> " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao processar:" + ex.Message);
            }
        }

        public void recalcularHoras()
        {
            try
            {
                _TomTicketDAO.recalcularHoras();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao recalcular:" + ex.Message);
            }
        }
    }
}
