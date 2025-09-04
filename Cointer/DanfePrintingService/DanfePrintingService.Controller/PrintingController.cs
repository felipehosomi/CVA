using Sap.Data.Hana;
using Spire.Pdf;
using System;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Threading;

namespace DanfePrintingService.Controller
{
    public static class PrintingController
    {
        private static HanaConnection _HanaConnection = new HanaConnection(ConfigurationManager.ConnectionStrings["Hana"].ConnectionString);

        public static void Print()
        {
            try
            {
                // Obtém o tempo de espera do serviço
                var waitTime = Convert.ToInt32(ConfigurationManager.AppSettings["WaitTime"]);

                while (true)
                {
                    try
                    {
                        // Realiza conexão com o banco de dados
                        _HanaConnection.Open();

                        foreach (DataRow pdfPathFile in GetPDFToPrint().Rows)
                        {
                            // Caso o arquivo PDF do DANFE não tenha ainda sido criado, não realiza nada
                            if (!File.Exists(pdfPathFile["DanfeFilePath"].ToString())) continue;

                            // Obtém o nome da impressora configurado para a bancada indicada no documento
                            var printerName = ConfigurationManager.AppSettings[pdfPathFile["Bancada"].ToString()];

                            // Imprime o PDF do DANFE obtido
                            SendToPrinter(int.Parse(pdfPathFile["DocEntry"].ToString()), pdfPathFile["DanfeFilePath"].ToString(), printerName);                           
                        }

                        // Fecha a conexão com o banco de dados
                        _HanaConnection.Close();

                        // Coloca o serviço no modo sleep
                        Thread.Sleep(waitTime);
                    }
                    catch (Exception ex)
                    {
                        var logger = File.AppendText(string.Format("{0}\\Log.txt", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));
                        logger.WriteLine($"{DateTime.Now} - Exceção: {ex.Message} | {ex.StackTrace}");
                        logger.Close();

                        // Caso a conexão com o banco de dados tenha sido aberta, fecha a conexão
                        if (_HanaConnection.State == ConnectionState.Open) _HanaConnection.Close();

                        // Coloca o serviço no modo sleep
                        Thread.Sleep(waitTime);
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = File.AppendText(string.Format("{0}\\Log.txt", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));
                logger.WriteLine($"{DateTime.Now} - Exceção: {ex.Message} | {ex.StackTrace}");
                logger.Close();
            }
        }

        private static DataTable GetPDFToPrint()
        {
            var command = $@"select OINV.""DocEntry"", OINV.""U_CVA_BancadaConferencia"" as ""Bancada"",
                                    ""SettingCompany"".""PathToDanfe"" || '\' || ""Process"".""KeyNfe""  || '.pdf' as ""DanfeFilePath""
                               from ""DBInvOne"".""Process""
                              inner join ""DBInvOne"".""SettingCompany"" on ""SettingCompany"".""CompanyId"" = ""Process"".""CompanyId""
                              inner join ""{ConfigurationManager.AppSettings["DataBase"]}"".OINV on OINV.""DocEntry"" = ""Process"".""DocEntry""
                                and OINV.""ObjType"" = ""Process"".""DocType""
                              where ""Process"".""StatusId"" = 4 -- Autorizado
                                and ""SettingCompany"".""CompanyDb"" = '{ConfigurationManager.AppSettings["DataBase"]}'
                                and OINV.""U_CVA_StatusImpressao"" = 'W' -- Aguardando";
            var dataAdapter = new HanaDataAdapter(command, _HanaConnection);
            var table = new DataTable();

            dataAdapter.Fill(table);

            return table;
        }
        
        private static void UpdateDocPrintStatus(int docEntry, string status)
        {
            var command = $@"update ""{ConfigurationManager.AppSettings["DataBase"]}"".OINV 
                                set OINV.""U_CVA_StatusImpressao"" = '{status}'
                              where OINV.""DocEntry"" = {docEntry}";
            var updateCommand = new HanaCommand(command, _HanaConnection);

            updateCommand.ExecuteNonQuery();
        }

        private static void SendToPrinter(int docEntry, string filePath, string printerName)
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

                // Altera o status de impressão do documento para Impresso
                UpdateDocPrintStatus(docEntry, "P");

                var logger = File.AppendText(string.Format("{0}\\Log.txt", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));
                logger.WriteLine($"{DateTime.Now} - Arquivo {filePath} impresso com sucesso.");
                logger.Close();
            }
            catch (Exception ex)
            {
                // Altera o status de impressão do documento para Erro
                UpdateDocPrintStatus(docEntry, "E");

                var logger = File.AppendText(string.Format("{0}\\Log.txt", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));
                logger.WriteLine($"{DateTime.Now} - Houve um erro ao tentar imprimir o arquivo {filePath} | {ex.Message}");
                logger.Close();
            }
        }
    }
}
