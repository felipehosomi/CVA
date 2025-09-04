using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace EnvioNfLote.Helpers
{
    public class DI
    {
        public static bool VerificaConf()
        {
            var ret = false;

            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT Code FROM [@CVA_EMLCONF]");

            ret = oRecordset.RecordCount > 0;

            return ret;
        }

        public static void InsereConf(string empresa, string email, string usuario, string senha, string smtp, string porta, string ssl, string emailCopia, string mensagem, string subject)
        {
            UserTable oUserTable = B1Connection.Instance.Company.UserTables.Item("CVA_EMLCONF");
            oUserTable.Code = "1";
            oUserTable.Name = "1";
            oUserTable.UserFields.Fields.Item("U_Emp").Value = empresa;
            oUserTable.UserFields.Fields.Item("U_Eml").Value = email;
            oUserTable.UserFields.Fields.Item("U_Usr").Value = usuario;
            oUserTable.UserFields.Fields.Item("U_Pas").Value = senha;
            oUserTable.UserFields.Fields.Item("U_Smtp").Value = smtp;
            oUserTable.UserFields.Fields.Item("U_Ssl").Value = ssl;
            oUserTable.UserFields.Fields.Item("U_Por").Value = porta;
            oUserTable.UserFields.Fields.Item("U_DflMes").Value = mensagem;
            oUserTable.UserFields.Fields.Item("U_EmlCop").Value = emailCopia;
            oUserTable.UserFields.Fields.Item("U_Subject").Value = subject;

            if (oUserTable.Add() != 0)
                throw new Exception(B1Connection.Instance.Company.GetLastErrorDescription());

        }

        public static void AtualizaConf(string empresa, string email, string usuario, string senha, string smtp, string porta, string ssl, string emailCopia, string mensagem, string subject)
        {
            UserTable oUserTable = B1Connection.Instance.Company.UserTables.Item("CVA_EMLCONF");

            if (oUserTable.GetByKey("1"))
            {
                oUserTable.UserFields.Fields.Item("U_Emp").Value = empresa;
                oUserTable.UserFields.Fields.Item("U_Eml").Value = email;
                oUserTable.UserFields.Fields.Item("U_Usr").Value = usuario;
                oUserTable.UserFields.Fields.Item("U_Pas").Value = senha;
                oUserTable.UserFields.Fields.Item("U_Smtp").Value = smtp;
                oUserTable.UserFields.Fields.Item("U_Ssl").Value = ssl;
                oUserTable.UserFields.Fields.Item("U_Por").Value = porta;
                oUserTable.UserFields.Fields.Item("U_DflMes").Value = mensagem;
                oUserTable.UserFields.Fields.Item("U_EmlCop").Value = emailCopia;
                oUserTable.UserFields.Fields.Item("U_Subject").Value = subject;

                if (oUserTable.Update() != 0)
                    throw new Exception(B1Connection.Instance.Company.GetLastErrorDescription());
            }
        }

        public static void BuscaConf(ref SAPbouiCOM.Form oForm)
        {
            UserTable oUserTable = B1Connection.Instance.Company.UserTables.Item("CVA_EMLCONF");

            if (oUserTable.GetByKey("1"))
            {
                var empresa = oUserTable.UserFields.Fields.Item("U_Emp").Value;
                var email = oUserTable.UserFields.Fields.Item("U_Eml").Value;
                var usuario = oUserTable.UserFields.Fields.Item("U_Usr").Value;
                var senha = oUserTable.UserFields.Fields.Item("U_Pas").Value;
                var smtp = oUserTable.UserFields.Fields.Item("U_Smtp").Value;
                var ssl = oUserTable.UserFields.Fields.Item("U_Ssl").Value;
                var porta = oUserTable.UserFields.Fields.Item("U_Por").Value;
                var mensagem = oUserTable.UserFields.Fields.Item("U_DflMes").Value;
                var emailCopia = oUserTable.UserFields.Fields.Item("U_EmlCop").Value;
                var subject = oUserTable.UserFields.Fields.Item("U_Subject").Value;

                oForm.DataSources.UserDataSources.Item("dsMail").ValueEx = email;
                oForm.DataSources.UserDataSources.Item("dsUsr").ValueEx = usuario.ToString();
                oForm.DataSources.UserDataSources.Item("dsPwd").ValueEx = senha.ToString();
                oForm.DataSources.UserDataSources.Item("dsSmtp").ValueEx = smtp.ToString();
                oForm.DataSources.UserDataSources.Item("dsPort").ValueEx = porta.ToString();
                oForm.DataSources.UserDataSources.Item("dsMailCp").ValueEx = emailCopia.ToString();
                oForm.DataSources.UserDataSources.Item("dsMsg").ValueEx = mensagem.ToString();
                oForm.DataSources.UserDataSources.Item("dsSub").ValueEx = subject.ToString();
                ((SAPbouiCOM.ComboBox)oForm.Items.Item("cbComp").Specific).Select(empresa.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
                ((SAPbouiCOM.ComboBox)oForm.Items.Item("cbSsl").Specific).Select(ssl.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            }
        }

        public static StructConfig? BuscaConf()
        {
            UserTable oUserTable = B1Connection.Instance.Company.UserTables.Item("CVA_EMLCONF");

            if (oUserTable.GetByKey("1"))
            {
                var empresa = oUserTable.UserFields.Fields.Item("U_Emp").Value;
                var email = oUserTable.UserFields.Fields.Item("U_Eml").Value;
                var usuario = oUserTable.UserFields.Fields.Item("U_Usr").Value;
                var senha = oUserTable.UserFields.Fields.Item("U_Pas").Value;
                var smtp = oUserTable.UserFields.Fields.Item("U_Smtp").Value;
                var ssl = oUserTable.UserFields.Fields.Item("U_Ssl").Value;
                var porta = oUserTable.UserFields.Fields.Item("U_Por").Value;
                var mensagem = oUserTable.UserFields.Fields.Item("U_DflMes").Value;
                var emailCopia = oUserTable.UserFields.Fields.Item("U_EmlCop").Value;
                var subject = oUserTable.UserFields.Fields.Item("U_Subject").Value;

                var ret = new StructConfig();
                ret.Email = email.ToString();
                ret.Empresa = empresa.ToString();
                ret.Usuario = usuario.ToString();
                ret.Senha = senha.ToString();
                ret.Smtp = smtp.ToString();
                ret.Ssl = ssl.ToString();
                ret.Porta = int.Parse(porta.ToString());
                ret.Mensagem = mensagem.ToString();
                ret.EmailCopia = emailCopia.ToString();
                ret.Subject = subject.ToString();

                return ret;
            }

            return null;
        }

        public static void BuscaDocumentos(ref SAPbouiCOM.Form oForm, ref SAPbouiCOM.Matrix oMatrix, string empresa, string dataInicial, string dataFinal, string documentoInicial, string documentoFinal, string status)
        {
            B1Connection.Instance.Application.MessageBox("Buscando documentos. Por favor, aguarde.");

            oForm.Freeze(true);
            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            var query = new StringBuilder();
            query.AppendLine($"EXEC spc_CVA_RetornaDocumentosEnvioNFLote @bplId = {empresa}");

            if (!string.IsNullOrEmpty(dataInicial) && !string.IsNullOrEmpty(dataFinal))
                query.Append($", @dataInicial = '{dataInicial}', @dataFinal = '{dataFinal}'");

            if (!string.IsNullOrEmpty(documentoInicial) && !string.IsNullOrEmpty(documentoFinal))
                query.Append($", @documentoInicial = {documentoInicial}, @documentoFinal = {documentoFinal}");

            if (!string.IsNullOrEmpty(status))
                query.AppendLine($", @status = {status}");

            oRecordset.DoQuery(query.ToString());

            if(oRecordset.RecordCount > 0)
            {
                oMatrix.Clear();
                oMatrix.LoadFromDataSourceEx(true);

                var j = 1;
                var rowCount = oRecordset.RecordCount;
                                
                try
                {
                    for (int i = 0; i < rowCount; i++)
                    {
                        oMatrix.AddRow();
                        oMatrix.ClearRowData(i);
                    }

                    SAPbouiCOM.UserDataSource dsCol1 = oForm.DataSources.UserDataSources.Item("dsCol1");
                    SAPbouiCOM.UserDataSource dsCol2 = oForm.DataSources.UserDataSources.Item("dsCol2");
                    SAPbouiCOM.UserDataSource dsCol3 = oForm.DataSources.UserDataSources.Item("dsCol3");
                    SAPbouiCOM.UserDataSource dsCol4 = oForm.DataSources.UserDataSources.Item("dsCol4");
                    SAPbouiCOM.UserDataSource dsCol5 = oForm.DataSources.UserDataSources.Item("dsCol5");
                    SAPbouiCOM.UserDataSource dsCol6 = oForm.DataSources.UserDataSources.Item("dsCol6");
                    SAPbouiCOM.UserDataSource dsCol7 = oForm.DataSources.UserDataSources.Item("dsCol7");
                    SAPbouiCOM.UserDataSource dsCol8 = oForm.DataSources.UserDataSources.Item("dsCol8");
                    SAPbouiCOM.UserDataSource dsCol9 = oForm.DataSources.UserDataSources.Item("dsCol9");
                    SAPbouiCOM.UserDataSource dsCol10 = oForm.DataSources.UserDataSources.Item("dsCol10");

                    double total = 0;

                    while (!oRecordset.EoF)
                    {
                        oMatrix.ClearRowData(j);

                        var docEntry = oRecordset.Fields.Item("DocEntry").Value.ToString();
                        var cardCode = oRecordset.Fields.Item("CardCode").Value.ToString();
                        var cardName = oRecordset.Fields.Item("CardName").Value.ToString();
                        var docDate = Format_StringToDate(oRecordset.Fields.Item("DocDate").Value.ToString());
                        var serial = oRecordset.Fields.Item("Serial").Value.ToString();
                        var docTotal = oRecordset.Fields.Item("DocTotal").Value.ToString();
                        var emailPadrao = oRecordset.Fields.Item("EmailPadrao").Value.ToString();
                        var emailsList = oRecordset.Fields.Item("EmailsList").Value.ToString();

                        dsCol1.ValueEx = $"{j}";
                        dsCol3.ValueEx = docEntry;
                        dsCol4.ValueEx = cardCode;
                        dsCol5.ValueEx = cardName;
                        dsCol6.ValueEx = docDate.ToString("yyyyMMdd");
                        dsCol7.ValueEx = serial;
                        dsCol8.ValueEx = emailPadrao;
                        dsCol10.ValueEx = emailsList;
                        dsCol9.ValueEx = docTotal.Replace(".", "").Replace(",", ".");

                        total += Convert.ToDouble(docTotal);

                        oMatrix.SetLineData(j);
                        j++;
                        oRecordset.MoveNext();
                    }

                    var st_ValorTotal = ((SAPbouiCOM.StaticText)oForm.Items.Item("st_Total").Specific);
                    st_ValorTotal.Caption = "Valor Total: R$ " + total.ToString("#,##0.00");
                }
                catch (Exception)
                {
                    oForm.Freeze(false);
                    throw;
                }
            }
            oForm.Freeze(false);
        }

        public static Dictionary<int, string> BuscaEmpresas()
        {
            var ret = new Dictionary<int, string>();
            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT T0.BPLId, T0.BPLName + ' - ' + T0.TaxIdNum AS BPLName FROM OBPL T0 WHERE T0.Disabled = 'N'");

            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    ret.Add(Convert.ToInt32(oRecordset.Fields.Item("BPLId").Value.ToString()), oRecordset.Fields.Item("BPLName").Value.ToString());
                    oRecordset.MoveNext();
                }
            }

            return ret;
        }

        public static void EnviaDocumentos(List<string> documentos)
        {
            try
            {
                B1Connection.Instance.Application.MessageBox("Processo em andamento. Aguarde o envio dos documentos.");

                foreach (var item in documentos)
                {
                    var emailPadrao = string.Empty;
                    var emailsCopia = string.Empty;
                    var chaveAcesso = string.Empty;
                    var caminhoDanfe = string.Empty;
                    var caminhoXml = string.Empty;

                    Documents oDocuments = (Documents)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oInvoices);

                    if (oDocuments.GetByKey(int.Parse(item)))
                    {
                        BusinessPartners oBusinessPartners = (BusinessPartners)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                        oBusinessPartners.GetByKey(oDocuments.CardCode);

                        emailPadrao = oBusinessPartners.EmailAddress;

                        for (int i = 0; i < oBusinessPartners.ContactEmployees.Count; i++)
                        {
                            oBusinessPartners.ContactEmployees.SetCurrentLine(i);
                            emailsCopia = emailsCopia + $"{oBusinessPartners.ContactEmployees.E_Mail};";
                        }

                        chaveAcesso = GetChaveAcesso(oDocuments.DocEntry);
                        caminhoXml = GetCaminhoXml(oDocuments.BPL_IDAssignedToInvoice);
                        caminhoDanfe = GetCaminhoDanfe(oDocuments.BPL_IDAssignedToInvoice);

                        if (string.IsNullOrEmpty(emailPadrao))
                            throw new Exception("E-mail padrão não pode ser em branco.");

                        if (string.IsNullOrEmpty(chaveAcesso))
                            throw new Exception("Chave de acesso não encontrada.");

                        if (string.IsNullOrEmpty(caminhoXml))
                            throw new Exception("Caminho do XML não encontrado.");

                        if (string.IsNullOrEmpty(caminhoDanfe))
                            throw new Exception("Caminho do DANFE não encontrado.");
                        
                        Mailer.SendMail(emailPadrao, emailsCopia, chaveAcesso, caminhoXml, caminhoDanfe, oDocuments.CardName, oDocuments.Series.ToString(), oDocuments.BPLName);

                        oDocuments.UserFields.Fields.Item("U_CVA_DocEnviado").Value = 1;

                        if (oDocuments.Update() != 0)
                            throw new Exception(B1Connection.Instance.Company.GetLastErrorDescription());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string Format_MoneyToString(double valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_MoneyToString(valor, BoMoneyPrecisionTypes.mpt_Sum);
            return oRecordset.Fields.Item(0).Value;
        }

        private static string Format_DateToString(DateTime valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_DateToString(valor);
            return oRecordset.Fields.Item(0).Value;
        }

        private static DateTime Format_StringToDate(string valor)
        {
            var oSboBob = (SBObob)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            var sValor = string.Empty;

            if (valor.Contains(" 00:00:00"))
                sValor = valor.Length > 10 ? valor.Substring(0, 10) : valor;
            else
                sValor = valor;

            oRecordset = oSboBob.Format_StringToDate(sValor);
            return Convert.ToDateTime(oRecordset.Fields.Item(0).Value);
        }

        private static double Format_StringToDouble(string s)
        {
            double d = 0;
            // This part is fast, when regional settings equal to sap B1 settings:
            try
            {
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
            }
            // Speed up performance: extend CompaneService variables to global variables and query them at addon startup.
            try
            {
                var nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                var oCompanyService = B1Connection.Instance.Company.GetCompanyService();
                var oAdminInfo = oCompanyService.GetAdminInfo();
                var sbodsep = oAdminInfo.DecimalSeparator;
                var sbotsep = oAdminInfo.ThousandsSeparator;
                // ReSharper disable StringIndexOfIsCultureSpecific.1
                if (s.IndexOf("".PadLeft(1)) > 0)
                // ReSharper restore StringIndexOfIsCultureSpecific.1
                {
                    // ReSharper disable StringIndexOfIsCultureSpecific.1
                    s = oAdminInfo.DisplayCurrencyontheRight == BoYesNoEnum.tYES
                        ? s.Substring(0, s.IndexOf("".PadLeft(1)))
                        : s.Substring(s.IndexOf("".PadLeft(1)), s.Length - s.IndexOf("".PadLeft(1)));
                    // ReSharper restore StringIndexOfIsCultureSpecific.1
                }
                var s1 = s.Replace(sbotsep, nfi.NumberGroupSeparator);

                s1 = s1.Replace(sbodsep, nfi.NumberDecimalSeparator);
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
                return 0;
            }
        }

        private static string GetChaveAcesso(int docEntry)
        {
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT TOP 1 U_ChaveAcesso FROM [@SKL25NFE] WHERE U_DocEntry = {docEntry} AND U_tipoDocumento = 'NS' ORDER BY Code DESC");
            return oRecordset.Fields.Item(0).Value;
        }

        private static string GetCaminhoDanfe(int bplId)
        {
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT U_DanfePathPDF FROM [@SKL25CFG] WHERE U_Ambiente = 1 AND Code = {bplId}");
            return oRecordset.Fields.Item(0).Value;
        }

        private static string GetCaminhoXml(int bplId)
        {
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT U_pathArmXML FROM [@SKL25CFG] WHERE U_Ambiente = 1 AND Code = {bplId}");
            return oRecordset.Fields.Item(0).Value;
        }
    }

    public struct StructConfig
    {
        public string Empresa { get; set; }
        public string Email { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Smtp { get; set; }
        public string Ssl { get; set; }
        public int Porta { get; set; }
        public string Mensagem { get; set; }
        public string EmailCopia { get; set; }
        public string Subject { get; set; }
    }
}
