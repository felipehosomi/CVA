using CVA.IntegracaoMagento.StockItems.Controller;
using CVA.IntegracaoMagento.StockItems.Infraestructure;
using CVA.IntegracaoMagento.StockItems.Infrastructure;
using CVA.IntegracaoMagento.StockItems.Models.Magento;
using CVA.IntegracaoMagento.StockItems.Models.SAP;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.StockItems
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        public Integration()
        {
        }

        public async Task SetIntegration()
        {
            DateTime dataAtual = DateTime.Now.AddSeconds(1);
            string sCaminho = String.Format(@"{0}Log", System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (!(System.IO.Directory.Exists(sCaminho)))
                System.IO.Directory.CreateDirectory(sCaminho);

            sCaminho = String.Format(@"{0}\\Log_{1}.txt", sCaminho, String.Format(@"{0}{1}{2}", dataAtual.Year.ToString(), dataAtual.Month.ToString().PadLeft(2, '0'), dataAtual.Day.ToString().PadLeft(2, '0')));

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            Util.GravarLog(sCaminho, "[PROCESSO] - Iniciando o processo.");

            try
            {
                SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);
                var objConfig = await SLConnection.GetAsync<List<Metadata_Config.CVA_CONFIG_MAG>>("CVA_CONFIG_MAG");
                var token = new Token();
                var sourceitem = new SourceItems();

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");

                token.apiAddressUri = objConfig[0].U_ApiUrl; //MagentoURL
                token.username = objConfig[0].U_ApiUsuario; //MagentoUser
                token.password = objConfig[0].U_ApiSenha; //MagentoPassword
                token.MagentoClientId = objConfig[0].U_ApiClientId; //MagentoClientId
                token.MagentoSecretId = objConfig[0].U_ApiClientSecret; //MagentoClientSecret
                Token.create_CN(token);

                if (String.IsNullOrEmpty(token.bearerTolken))
                    throw new Exception("[PROCESSO] Conexão Magento: Bearer Token não gerado.");

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK)");
                var stockSap = await SLConnection.GetAsync<List<Metadata_Stock.Stock>>("sml.svc/CVA_MAGENTO_STOCK");
                if (stockSap.Count > 0)
                {
                    foreach (var item in stockSap)
                    {
                        Util.GravarLog(sCaminho, "[PROCESSO] - Alterando estoque: " + item.ItemCode + " | Saldo: " + item.OnHand + " | Filial: " + item.DepositoMagento);

                        string sMensagemErro = String.Empty;
                        sourceitem.sourceItems = new List<SourceItems.SourceItem>();
                        sourceitem.sourceItems.Add(new SourceItems.SourceItem
                        {
                            quantity = item.OnHand,
                            sku = item.ItemCode,
                            source_code = item.DepositoMagento,
                            status = 1
                        });

                        StockItemsController.update_STOCK(token, sourceitem, ref sMensagemErro);
                        if (String.IsNullOrEmpty(sMensagemErro))
                        {
                            DateTime horaAtual = DateTime.Now.AddSeconds(10);
                            string sData = String.Format(@"{0}{1}{2}", horaAtual.Year.ToString(), horaAtual.Month.ToString().PadLeft(2, '0'), horaAtual.Day.ToString().PadLeft(2, '0'));
                            string sHora =String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0'));

                            var hana = new Hana();
                            string sValor = item.OnHand.ToString();
                            sValor = sValor.Replace(',', '.');
                            string sQuery = (item.Novo.Equals("N") ? HanaCommands.UpdateHana : HanaCommands.InsertHana);
                            sQuery = String.Format(sQuery, Database, item.ItemCode, item.WareHouse, sValor, sData, sHora);
                            //Util.GravarLog(sCaminho, String.Format(@"[QUERY] - {0}", sQuery));
                            await hana.ExecuteNonQueryAsync(sQuery);
                        }
                        else
                            Util.GravarLog(sCaminho, String.Format(@"[MAGENTO] - Mensagem: {0}", sMensagemErro));
                    }
                }
            }
            catch (FlurlHttpException ex)
            {
                var responseString = await ex.Call.Response.GetStringAsync();
                Util.GravarLog(sCaminho, "[PROCESSO] - (Erro): " + responseString);
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, "[PROCESSO] - (Erro): " + ex.Message);
            }

            Util.GravarLog(sCaminho, "[PROCESSO] - Processo finalizado.");
        }
    }
}
