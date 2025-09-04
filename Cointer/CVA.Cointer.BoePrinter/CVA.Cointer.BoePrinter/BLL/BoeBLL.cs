using CVA.Cointer.BoePrinter.DAO;
using CVA.Cointer.BoePrinter.Model;
using log4net;
using Sap.Data.Hana;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CVA.Cointer.BoePrinter.BLL
{
    public class BoeBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9";

        public void PrintPending()
        {
            log4net.Config.XmlConfigurator.Configure();

            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    Logger.Info("Processo já em execução, não é possível executar mais de uma instância");
                    return;
                }

                HanaDAO hanaDAO = new HanaDAO();

                string database = ConfigurationManager.AppSettings["Database"];
                string companyID = ConfigurationManager.AppSettings["CompanyID"];
                string integrationDatabase = ConfigurationManager.AppSettings["IntegrationDatabase"];
                string dir = ConfigurationManager.AppSettings["Dir"];

                if (!Directory.Exists(dir))
                {
                    Logger.Info("Diretório raíz não encontrado: " + dir);
                    return;
                }

                while (true)
                {
                    List<BoeModel> list = hanaDAO.FillModelList<BoeModel>(String.Format(Hana.Boe_GetPending, database, integrationDatabase, companyID));
                    Logger.Info("Pendentes: " + list.Count);
                    string printerName = "";

                    foreach (var model in list)
                    {
                        string status = "";
                        try
                        {
                            if (model.Bancada == 0)
                            {
                                throw new Exception("Bancada não informada para o documento " + model.DocNum);
                            }

                            printerName = ConfigurationManager.AppSettings[model.Bancada.ToString()];
                            if (String.IsNullOrEmpty(printerName))
                            {
                                throw new Exception("Impressora não encontrada no arquivo de configuração para a bancada " + model.Bancada);
                            }

                            string pdfDir = Path.Combine(dir, model.ContractBank.ToString(), model.DateCreate.Year.ToString(), model.DateCreate.Month.ToString().PadLeft(2, '0'), model.DateCreate.Day.ToString().PadLeft(2, '0'));
                            if (!Directory.Exists(pdfDir))
                            {
                                throw new Exception("Diretório não encontrado: " + pdfDir);
                            }

                            string[] files = Directory.GetFiles(pdfDir, $"{model.Serial}-{model.OurNumber}*");
                            if (files.Length == 0)
                            {
                                throw new Exception($"PDF não encontrado: {model.Serial}-{model.OurNumber}*");
                            }

                            string error = SendToPrinter(files[0], printerName);
                            if (String.IsNullOrEmpty(error))
                            {
                                status = "P";
                                Logger.Info($"Nr Doc {model.DocNum} - Arquivo impresso com sucesso - {printerName}");
                                hanaDAO.ExecuteNonQuery(String.Format(Hana.Boe_UpdatePrintedStatus, integrationDatabase, companyID, model.IntegrationId));
                            }
                            else
                            {
                                throw new Exception("Erro ao imprimir " + error);
                            }
                        }
                        catch (Exception ex)
                        {
                            status = "W";
                            Logger.Error($"Nr Doc {model.DocNum} - {ex.Message} - {printerName}");
                            if (ex.InnerException != null)
                            {
                                Logger.Error($"Nr Doc {model.DocNum} - {ex.InnerException.Message}");
                            }
                        }
                        finally
                        {

                        }
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private string SendToPrinter(string filePath, string printerName)
        {
            try
            {
                var doc = new PdfDocument();

                doc.LoadFromFile(filePath);
                // Impressão de forma silenciosa
                doc.PrintSettings.PrintController = new StandardPrintController();
                doc.PrintSettings.PrinterName = printerName;
                doc.PrintSettings.Copies = 1;

                doc.Print();
                doc.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
