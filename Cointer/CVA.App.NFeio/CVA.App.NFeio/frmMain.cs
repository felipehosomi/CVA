using ClosedXML.Excel;
using CVA.App.NFeio.Model;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CVA.App.NFeio
{
    public partial class frmMain : Form
    {
        private IXLWorksheet wsIbgeCodes;
        public frmMain()
        {
            InitializeComponent();

            FlurlHttp.Configure(c =>
            {
                c.JsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            });
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Planilha Excel (*.xlsx)|*.xlsx";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                    btnImport.Enabled = true;
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Carregando planilha...";
            btnImport.Enabled = false;

            Task.Run(() =>
            {
                try
                {
                    wsIbgeCodes = new XLWorkbook("ibge_codes.xlsx").Worksheet("dados");
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    Config config;

                    using (Stream reader = new FileStream(Path.Combine(Application.StartupPath, "config.xml"), FileMode.Open))
                    {
                        // Call the Deserialize method to restore the object's state.
                        config = (Config)serializer.Deserialize(reader);
                    }

                    if (File.Exists(txtFilePath.Text))
                    {
                        var workbook = new XLWorkbook(txtFilePath.Text);
                        foreach (var item in config.ExcelSheet)
                        {
                            var list = ConvertWorksheetToObject(workbook.Worksheet(item.id), item);
                            SendToApi(list, item, config.ApiUrl);
                        }
                    }
                    else
                    {
                        throw new Exception("O arquivo selecionado não existe.");
                    }
                    MessageBox.Show("Transmissão concluída", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    btnImport.Invoke((MethodInvoker)delegate { btnImport.Enabled = true; });
                    MessageBox.Show(ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void SendToApi(Dictionary<int, ApiModel> list, ConfigExcelSheet config, string apiUrl)
        {
            lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = $"{config.id} - Enviando dados para a API..."; });

            int total = list.Count;
            int current = 0;
            progBar.Invoke((MethodInvoker)delegate { progBar.Maximum = total; });
            Directory.CreateDirectory("logs");

            string logFile = $"{config.id}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            foreach (var item in list)
            {
                current++;
                string requestBody = JsonConvert.SerializeObject(item.Value);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.5));
                //var result = String.Format(apiUrl, config.CompanyId)
                //    .WithHeader("Authorization", config.ApiKey)
                //    .WithHeader("Content-Type", "application/json")
                //    .AllowAnyHttpStatus()
                //    .PostStringAsync(requestBody).Result;

                //string responseBody = result.Content.ReadAsStringAsync().Result;
                //WriteLog(logFile, item.Key, requestBody, responseBody, result.IsSuccessStatusCode);
                lblCurrent.Invoke((MethodInvoker)delegate { lblCurrent.Text = $"{current}/{total}"; });
                progBar.Invoke((MethodInvoker)delegate { progBar.PerformStep(); });
            }

            lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = $"{config.id} - Transmissão concluída"; });
        }

        private void SendToApi(Dictionary<int, ApiModel> savassiList, Dictionary<int, ApiModel> santosList)
        {
            lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = "Enviando dados para a API..."; });

            string savassiUrl = string.Format(ConfigurationManager.AppSettings["ApiUrl"], ConfigurationManager.AppSettings["SAVASSI_companyId"]);
            string savassiApiKey = ConfigurationManager.AppSettings["SAVASSI_ApiKey"];
            string santosUrl = string.Format(ConfigurationManager.AppSettings["ApiUrl"], ConfigurationManager.AppSettings["SANTOS_companyId"]);
            string santosApiKey = ConfigurationManager.AppSettings["SANTOS_ApiKey"];

            int total = savassiList.Count + santosList.Count;
            int current = 0;
            progBar.Invoke((MethodInvoker)delegate { progBar.Maximum = total; });
            Directory.CreateDirectory("logs");

            string savassiLogFile = $"SAVASSI_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            foreach (var item in savassiList)
            {
                current++;
                string requestBody = JsonConvert.SerializeObject(item.Value);
                var result = savassiUrl
                    .WithHeader("Authorization", savassiApiKey)
                    .WithHeader("Content-Type", "application/json")
                    .AllowAnyHttpStatus()
                    .PostStringAsync(requestBody).Result;

                string responseBody = result.Content.ReadAsStringAsync().Result;
                WriteLog(savassiLogFile, item.Key, requestBody, responseBody, result.IsSuccessStatusCode);
                lblCurrent.Invoke((MethodInvoker)delegate { lblCurrent.Text = $"{current}/{total}"; });
                progBar.Invoke((MethodInvoker)delegate { progBar.PerformStep(); });
            }

            string santosLogFile = $"SANTOS_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            foreach (var item in santosList)
            {
                current++;
                string requestBody = JsonConvert.SerializeObject(item.Value);
                var result = santosUrl
                    .WithHeader("Authorization", santosApiKey)
                    .WithHeader("Content-Type", "application/json")
                    .AllowAnyHttpStatus()
                    .PostStringAsync(requestBody).Result;

                string responseBody = result.Content.ReadAsStringAsync().Result;
                WriteLog(santosLogFile, item.Key, requestBody, responseBody, result.IsSuccessStatusCode);
                lblCurrent.Invoke((MethodInvoker)delegate { lblCurrent.Text = $"{current}/{total}"; });
                progBar.Invoke((MethodInvoker)delegate { progBar.PerformStep(); });
            }

            lblStatus.Invoke((MethodInvoker)delegate { lblStatus.Text = "Transmissão concluída"; });
            MessageBox.Show("Transmissão concluída", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void WriteLog(string savassiLogFile, int excelRow, string requestBody, string responseBody, bool success)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{DateTime.Now} - Linha da planilha: {excelRow}");
            sb.AppendLine($"{DateTime.Now} - Situação: {(success ? "OK" : "ERRO")}");
            sb.AppendLine($"{DateTime.Now} - Dados enviados: {requestBody}");
            sb.AppendLine($"{DateTime.Now} - Dados recebidos: {JToken.Parse(responseBody).ToString(Formatting.None)}");
            sb.AppendLine(string.Empty);

            File.AppendAllText("logs\\" + savassiLogFile, sb.ToString());
        }

        private Dictionary<int, ApiModel> ConvertWorksheetToObject(IXLWorksheet worksheet, ConfigExcelSheet config)
        {
            var dic = new Dictionary<int, ApiModel>();
            //string cityServiceCode = ConfigurationManager.AppSettings[$"{worksheet.Name.ToUpper()}_cityServiceCode"];
            //string federalServiceCode = ConfigurationManager.AppSettings[$"{worksheet.Name.ToUpper()}_federalServiceCode"];
            //string cnaeCode = ConfigurationManager.AppSettings[$"{worksheet.Name.ToUpper()}_cnaeCode"];

            long? ConverTaxNumberToLong(string value)
            {
                string onlyNumbers = Regex.Replace(value, "[^0-9]", "");
                if (string.IsNullOrWhiteSpace(onlyNumbers))
                    return null;
                else
                    return Convert.ToInt64(onlyNumbers);
            }

            DateTime? CheckIssueDate(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return DateTime.Now;
                else
                    return Convert.ToDateTime(value);
            }

            // Ignora primeira e última linha (cabeçalho e total)
            for (int i = 2; i < worksheet.Rows().Count(); i++)
            {
                dic.Add(i, new ApiModel
                {
                    borrower = new Borrower
                    {
                        type = "NaturalPerson",
                        name = worksheet.Row(i).Cell("A").Value.ToString(),
                        federalTaxNumber = ConverTaxNumberToLong(worksheet.Row(i).Cell("B").Value.ToString()),
                        email = worksheet.Row(i).Cell("C").Value.ToString(),
                        address = new Address
                        {
                            country = "BRA",
                            postalCode = worksheet.Row(i).Cell("D").Value.ToString(),
                            street = worksheet.Row(i).Cell("E").Value.ToString(),
                            number = worksheet.Row(i).Cell("F").Value.ToString(),
                            additionalInformation = worksheet.Row(i).Cell("G").Value.ToString(),
                            district = worksheet.Row(i).Cell("H").Value.ToString(),
                            city = new City
                            {
                                code = GetIbgeCode(worksheet.Row(i).Cell("I").Value.ToString(), worksheet.Row(i).Cell("K").Value.ToString(), worksheet.Row(i).Cell("J").Value.ToString()),
                                name = worksheet.Row(i).Cell("J").Value.ToString()
                            },
                            state = worksheet.Row(i).Cell("K").Value.ToString()
                        }
                    },
                    cityServiceCode = config.CityServiceCode,
                    federalServiceCode = config.FederalServiceCode,
                    cnaeCode = config.CnaeCode,
                    description = worksheet.Row(i).Cell("M").Value.ToString(),
                    servicesAmount = Convert.ToDecimal(worksheet.Row(i).Cell("L").Value),
                    issuedOn = CheckIssueDate(worksheet.Row(i).Cell("N").Value.ToString()),
                });
            }

            return dic;
        }

        private string GetIbgeCode(string ibgeCode, string state, string city)
        {
            // Se código IBGE não estiver presente, procura o código pelo estado e cidade na planilha ibge_codes.xlsx
            if (!string.IsNullOrWhiteSpace(ibgeCode))
                return ibgeCode;

            var result = wsIbgeCodes.Rows().Where(x => string.Equals(x.Cell("A").Value.ToString(), state, StringComparison.InvariantCultureIgnoreCase)
                                                    && string.Equals(x.Cell("B").Value.ToString(), city, StringComparison.InvariantCultureIgnoreCase));

            if (result != null && result.Count() > 0)
            {
                string code = result.First().Cell("C").Value.ToString();
                return code;
            }

            return null;
        }
    }
}
