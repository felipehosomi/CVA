using System.Collections.Generic;
using App.ApplicationServices.Addon;
using System.Text;
using System;
using SAPbobsCOM;
using System.Configuration;
using SinapseVirtual.Core.System;
using System.IO;
using App.ApplicationServices.Services;
using App.ApplicationServices;
using System.Threading;
using App.Infrastructure.Configuration;

namespace Electra.Currency.Task
{
    public class CurrencyTaskHelper
    {
        public static IConfigurationRepository _configurationRepository = new AppSettingsConfigurationRepository();
        private readonly List<string> _currencies = new List<string>();

        public void Execute()
        {
            string sMoeda = String.Empty;
            string sCurrency = String.Empty;
            string sData = String.Empty;
            int diasTentativas = 7;
            try
            {
                DateTime baseDateInit = DateTime.Today;
                if (ConnectionService.Instance.Connection())
                {
                
                    List<string> list = HanaService.getCurrencies();

                    DateTime baseDate;
                    baseDate = baseDateInit.AddDays(-1);
                    sData = baseDate.ToString("yyyyMMdd");
                    var urlFile = "http://www4.bcb.gov.br/Download/fechamento/{0}.csv".StrFrmt(new object[] { sData });

                    #region Tratativa devido ao fim de semana/feriados, obter cotação retroativa
                    int tentativas = 0;
                    while (!urlFile.RemoteFileExists())
                    {
                        Thread.Sleep(100);
                        tentativas++;
                        baseDate = baseDate.AddDays(-1);
                        sData = baseDate.ToString("yyyyMMdd");
                        urlFile = "http://www4.bcb.gov.br/Download/fechamento/{0}.csv".StrFrmt(new object[] { baseDate.ToString("yyyyMMdd") });
                        if (tentativas >= diasTentativas)
                        {
                            throw new Exception($"Não encontrado arquivo até {diasTentativas} dias para trás no site.");
                        }
                    }

                    #endregion
                    string str = new RequestManager().HttpMethod(SinapseVirtual.Core.System.HttpMethod.Get).Url(urlFile).Response();
                    foreach (string line in str.Split(new char[] { '\n' }))
                    {
                        try
                        {
                            string[] strArray = line.Split(new char[] { ';' });
                            if ((strArray.Length >= 5) && list.Contains(strArray[3]))
                            {
                                
                                sMoeda = strArray[3];
                                sMoeda = HanaService.getCurrencies(sMoeda);
                                sCurrency = strArray[4];

                                Console.WriteLine($"Atualizando Taxa de Câmbio - Data:{baseDateInit.ToString("dd/MM/yyyy")}  Moeda:{sMoeda} Valor:{sCurrency}");
                                CurrencyService.AtualizarCurrency(sMoeda, Convert.ToDouble(sCurrency, System.Globalization.CultureInfo.GetCultureInfo("pt-BR")), baseDateInit);
                            }
                            else
                            {
                                if (strArray[0].Contains("404"))
                                {
                                    Console.WriteLine(String.Format(@"Erro ao atualizar as taxas de câmbio. Detalhes: Arquivo não encontrado no site do Banco Central. ({0})", urlFile));
                                    break;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            LogService.GravarException(String.Format(@"Erro ao atualizar a taxa de câmbio da moeda {0}. Detalhes: {1} - {2}", sMoeda, exception.Message, sData));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogService.GravarException(String.Format(@"Erro ao atualizar as taxas de câmbio. Detalhes: {0}", exception.Message));
            }
        }
    }
}
