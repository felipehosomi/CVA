using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace CVA.Monitor.WBC.Excel.Files
{
    class Monitoramento
    {
        public Company oCompany;
        public ParametrosSap ps;
        public ParametrosConexao pc = new ParametrosConexao();

        private static string sErrMsg = null;
        private static int lErrCode = 0;
        private static int lRetCode;


        private SAPbobsCOM.Recordset oRecordSet;

        public Monitoramento()
        {
            string sFolderMonitor = CVA.Monitor.WBC.Excel.Files.Properties.Settings.Default.FolderMonitor;
            string sFolderMonitorprocessed = string.Format(@"{0}\processed", sFolderMonitor);
            string sFolderMonitorErrorLog = string.Format(@"{0}\ErrorLog", sFolderMonitor);

            VerificarDiretorios(sFolderMonitor, sFolderMonitorprocessed, sFolderMonitorErrorLog);
            DirectoryInfo d = new DirectoryInfo(sFolderMonitor);
            if (d.GetFiles("*.xlsx").Count() == 0) return;

            var settings = Properties.Settings.Default;
            int retVal;
            string retStr;

            ps.Company = settings.Base;
            ps.Usuario = settings.UsuarioSAP;
            ps.Senha = settings.SenhaSAP;
            ps.Tempo = 5;

            pc.database = settings.Base;
            pc.usuario = settings.UsuarioDB;
            pc.senha = settings.SenhaDB;
            pc.servidor = settings.Servidor;
            pc.tipo = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            pc.licenca = settings.ServidorLicenca;

            if (ConectarSap(ps, pc, out retVal, out retStr, out oCompany))
            {
                LogInfo("Conexão SAP realizada com sucesso em " + oCompany.CompanyDB + " como " + oCompany.UserName);

                #region Criação dos Campos SAP B1
                if (settings.CriarUDF.Equals("S"))
                {
                    //	U_CVA_Chave
                    AdcionarCampo("ORDR", "CVA_Chave", "Chave", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                    //Data Importação e Hora da importação
                    AdcionarCampo("ORDR", "CVA_DataImport", "Data Importação", SAPbobsCOM.BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, null, null);
                    //Hora da importação
                    AdcionarCampo("ORDR", "CVA_HoraImport", "Hora Importação", SAPbobsCOM.BoFieldTypes.db_Date, 10, BoFldSubTypes.st_Time, null, null);
                    //project
                    AdcionarCampo("OPRJ", "CVA_WBC", "Valor WBC", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                    //centro de custo
                    AdcionarCampo("OPRC", "CVA_WBC", "Valor WBC", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                    string[,] vv;
                    vv = new string[2, 2];
                    vv[0, 0] = "LP";
                    vv[0, 1] = "LP";
                    vv[1, 0] = "CP";
                    vv[1, 1] = "CP";
                    //centro de custo
                    AdcionarCampo("OPRC", "CVA_WBC2", "Valor WBC 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, vv, null);
                    //vendedor
                    AdcionarCampo("OSLP", "CVA_WBC", "Valor WBC", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                    //Código do Item
                    AdcionarCampo("OITM", "CVA_WBC", "Valor WBC", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                    //Código do Item
                    AdcionarCampo("OITM", "CVA_WBC2", "Valor WBC 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, vv, null);
                    //Documento de Marketing
                    //•	U_EE_Mes(int)– Campo para importar o mês de competência do contrato;
                    AdcionarCampo("ORDR", "CVA_EE_Mes", "Mês do Contrato WBC", SAPbobsCOM.BoFieldTypes.db_Numeric, 10, BoFldSubTypes.st_None, null, null);
                    //•	U_EE_Ano(int) – Campo para importar o ano de competência do contrato;
                    AdcionarCampo("ORDR", "CVA_EE_Ano", "Ano do Contrato WBC", SAPbobsCOM.BoFieldTypes.db_Numeric, 10, BoFldSubTypes.st_None, null, null);
                    //•	U_EE_SPREAD(monetário) – Campo para importar o valor da coluna “FORM_AGIO”;
                    AdcionarCampoMonetario("ORDR", "CVA_EE_SPREAD", "FORM_AGIO WBC");
                    //•	U_EE_TRU(monetário) – Campo para importar o valor da coluna “TRU”;
                    AdcionarCampoMonetario("ORDR", "CVA_EE_TRU", "TRU WBC");
                    //•	U_EE_CodWBC – Código interno do WBC, importante armazenar para poder realizar pesquisar e cruzar informações entre os sistemas;
                    AdcionarCampo("ORDR", "CVA_EE_CodWBC", "Cod WBC", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                    //•	U_EE_Status – Importado / Reprocessar.Por padrão todo pedido será registrado como “Importado”, em caso de precisar 
                    //atualizar um pedido no SAP o operador poderá acessar o Pedido de Venda no SAP e mudar o status para “Reprocessar”, com isso o
                    //importador permitirá ler o mesmo registro na nova planilha e atualizar o pedido existente ao invés de inserir um novo.
                    AdcionarCampoComboBox("ORDR", "CVA_EE_Status", "Status WBC");
                    //Suprimento_inicio	Suprimento_termino
                    AdcionarCampo("ORDR", "CVA_Suprimento_Inicio", "Suprimento Ínicio", SAPbobsCOM.BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, null, null);
                    //Suprimento_termino
                    AdcionarCampo("ORDR", "CVA_Suprimento_Termino", "Suprimento Término", SAPbobsCOM.BoFieldTypes.db_Date, 10, BoFldSubTypes.st_None, null, null);
                    AdcionarCampo("ORDR", "CVA_EE_CodCCEE", "Cod CCEE", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, BoFldSubTypes.st_None, null, null);
                }
                #endregion

                foreach (var file in d.GetFiles("*.xlsx"))
                {
                    Thread.Sleep(2000);

                    using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                        {
                            WorkbookPart workbookPart = doc.WorkbookPart;
                            SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                            SharedStringTable sst = sstpart.SharedStringTable;

                            WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                            Worksheet sheet = worksheetPart.Worksheet;

                            var cells = sheet.Descendants<Cell>();
                            var rows = sheet.Descendants<Row>();

                            Int64 iRowIndexCabecalho = 0;
                            //encontra a linha do cabeçalho
                            foreach (Row row in rows)
                            {
                                foreach (Cell c in row.Elements<Cell>())
                                {
                                    if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                    {
                                        int ssid = int.Parse(c.CellValue.Text);
                                        string str = sst.ChildElements[ssid].InnerText;
                                        //LogInfo(string.Format("Shared string {0}: {1}", ssid, str));
                                        if (str.Equals("Situacao"))
                                        {
                                            iRowIndexCabecalho = Convert.ToInt64(row.RowIndex.Value);
                                            break;
                                        }
                                    }
                                    else if (c.CellValue != null)
                                    {
                                        if (c.CellValue.Text.Equals("Situacao"))
                                        {
                                            iRowIndexCabecalho = Convert.ToInt64(row.RowIndex.Value);
                                        }
                                    }
                                }
                                //encontrou a linha do Cabeçalho
                                if (iRowIndexCabecalho > 0)
                                {
                                    break;
                                }
                            }

                            //mapeia a ordenação do campos de acordo com o cabeçalho
                            List<MapaColunaPosicao> lstMapaColunaPosicao = new List<MapaColunaPosicao>();

                            foreach (Row row in rows)
                            {
                                if (Convert.ToInt64(row.RowIndex.Value) == iRowIndexCabecalho)
                                {
                                    foreach (Cell c in row.Elements<Cell>())
                                    {
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            string str = sst.ChildElements[ssid].InnerText;
                                            lstMapaColunaPosicao.Add(new MapaColunaPosicao(str, RemoveNumbers(c.CellReference.InnerText)));
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            lstMapaColunaPosicao.Add(new MapaColunaPosicao(c.CellValue.Text, RemoveNumbers(c.CellReference.InnerText)));
                                        }
                                    }
                                }
                                else if (Convert.ToInt64(row.RowIndex.Value) > iRowIndexCabecalho)
                                {
                                    break;
                                }
                            }

                            List<Documento> lstDocumento = new List<Documento>();

                            //varre as linhas após a linha do cabeçalho, montando o pedido
                            foreach (Row row in rows)
                            {
                                Documento oDocumento = new Documento();
                                if (Convert.ToInt64(row.RowIndex.Value) > iRowIndexCabecalho)
                                {

                                    //para cada linha, verifica as colunas, montando o documento de marketing
                                    foreach (Cell c in row.Elements<Cell>())
                                    {
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            string str = sst.ChildElements[ssid].InnerText;
                                            string sColuna = RemoveNumbers(c.CellReference.InnerText);
                                            MontaPreDocumento(lstMapaColunaPosicao, oDocumento, str, sColuna, Convert.ToInt64(row.RowIndex.Value));
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            string sColuna = RemoveNumbers(c.CellReference.InnerText);
                                            MontaPreDocumento(lstMapaColunaPosicao, oDocumento, c.CellValue.Text, sColuna, Convert.ToInt64(row.RowIndex.Value));
                                        }
                                    }
                                }
                                else if (Convert.ToInt64(row.RowIndex.Value) <= iRowIndexCabecalho)
                                {
                                    continue;
                                }
                                oDocumento.Chave = oDocumento.Movimentacao + oDocumento.Contraparte_CNPJ + oDocumento.Codigo_WBC + oDocumento.Mes + oDocumento.Ano;

                                lstDocumento.Add(oDocumento);
                            }

                            oRecordSet = oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                            foreach (Documento documento in lstDocumento.Where(a => a.Situacao_Contrato == "Liberado").OrderBy(a => a.LinhaPlanilha))
                            {
                                try
                                {
                                    LogInfo(string.Format("Processando linha: {0} - Chave {1}", documento.LinhaPlanilha, documento.Chave));


                                    if ((documento.Movimentacao == "Compra" || documento.Movimentacao == "Venda") && (documento.Valor_parcela_1 == null || Convert.ToDouble(documento.Valor_parcela_1.Replace(".", ",")) == 0))
                                        continue;

                                    if (documento.Movimentacao == "Serviço" && (documento.ValorReajustado == null || Convert.ToDouble(documento.ValorReajustado.Replace(".", ",")) == 0))
                                        continue;

                                    if (string.IsNullOrEmpty(Convert.ToString(documento.Contraparte_CNPJ).Trim()))
                                        continue;

                                    string TipoContrato = (documento.Suprimento_termino - documento.Suprimento_inicio).Days <= 31 ? "CP" : "LP";

                                    #region Contrato

                                    if (TipoContrato.Equals("LP"))
                                    {
                                        if (string.IsNullOrEmpty(documento.Numero_referencia_contrato))
                                        {
                                            documento.MsgErro = string.Format($"Número do contrato não pode ser vazio ou nulo, o documento não foi integrado, Linha:{documento.LinhaPlanilha}");
                                            continue;
                                        }
                                    }
                                    #endregion

                                    #region Procura pelo Cliente/Fornecedor

                                    if ((documento.Movimentacao.Equals("Serviço")) || (documento.Movimentacao.Equals("Venda")))
                                    {
                                        string sql = string.Format("SELECT distinct T1.\"CardCode\" as CardCode, T0.\"CardName\" as CardName  FROM \"CRD7\"  T1 inner join \"OCRD\" T0 on T0.\"CardCode\" = T1.\"CardCode\" and T0.\"CardType\" = 'C' WHERE replace(replace(replace(T1.\"TaxId0\", '.', ''), '/', ''), '-', '') = '{0}' and T1.\"AddrType\" = 'S'", documento.Contraparte_CNPJ);
                                        oRecordSet.DoQuery(sql);
                                    }
                                    else if (documento.Movimentacao.Equals("Compra"))
                                        oRecordSet.DoQuery(string.Format("SELECT distinct T1.\"CardCode\" as CardCode, T0.\"CardName\" as CardName  FROM \"CRD7\"  T1 inner join \"OCRD\" T0 on T0.\"CardCode\" = T1.\"CardCode\" and T0.\"CardType\" = 'S' WHERE replace(replace(replace(T1.\"TaxId0\", '.', ''), '/', ''), '-', '') = '{0}' and T1.\"AddrType\" = 'S'", documento.Contraparte_CNPJ));

                                    if (oRecordSet.RecordCount == 0)
                                    {
                                        documento.MsgErro = string.Format("Cliente não encontrato Através do CNPJ: {0}", documento.Contraparte_CNPJ);
                                        continue;
                                    }
                                    else
                                    {
                                        documento.CardCode = oRecordSet.Fields.Item("CardCode").Value;
                                        documento.CardName = oRecordSet.Fields.Item("CardName").Value;
                                    }
                                    #endregion

                                    #region Procura pelo Uso Principal Cliente
                                    oRecordSet.DoQuery(string.Format("select coalesce(T0.\"MainUsage\",0) as MainUsage from \"OCRD\" T0 where T0.\"CardCode\"='{0}'", documento.CardCode));
                                    if (oRecordSet.RecordCount > 0)
                                        documento.UsoPrincipal = oRecordSet.Fields.Item("MainUsage").Value;
                                    #endregion

                                    #region Procura pelo Centro de Custo
                                    if (documento.Movimentacao == "Serviço" && string.IsNullOrEmpty(documento.Perfil_CCEE_vendedor))
                                        documento.CentroDeCusto = CVA.Monitor.WBC.Excel.Files.Properties.Settings.Default.Perfil_CCEE_Default;
                                    else
                                    {
                                        oRecordSet.DoQuery(string.Format("select top 1 T0.\"PrcCode\" from \"OPRC\" T0 where Ucase(T0.\"U_CVA_WBC\")=Ucase('{0}') and Ucase(T0.\"U_CVA_WBC2\")=Ucase('{1}')", documento.Perfil_CCEE_vendedor, TipoContrato));
                                        if (oRecordSet.RecordCount == 0)
                                        {
                                            documento.MsgErro = string.Format("Centro de Custo não encontrato Através do Perfil_CCEE_vendedor: {0} e Tipo de Contrato: {1}", documento.Perfil_CCEE_vendedor, TipoContrato);
                                            continue;
                                        }
                                        else
                                            documento.CentroDeCusto = oRecordSet.Fields.Item("PrcCode").Value;
                                    }
                                    #endregion

                                    #region Procura pelo Project
                                    try
                                    {
                                        oRecordSet.DoQuery(string.Format("select top 1 T0.\"PrjCode\" from \"OPRJ\" T0 where Ucase(T0.\"U_CVA_WBC\")=Ucase('{0}')", documento.Portfolio_Vendedor));
                                        if (oRecordSet.RecordCount > 0)
                                            documento.Portfolio_Vendedor = oRecordSet.Fields.Item("PrjCode").Value;
                                    }
                                    catch { }
                                    #endregion

                                    #region Procura pelo Código do Vendedor
                                    if (!string.IsNullOrEmpty(documento.Interveniente_Comissionado))
                                    {
                                        oRecordSet.DoQuery(string.Format("select coalesce(T0.\"SlpCode\",0) as SlpCode from \"OSLP\" T0 where T0.\"SlpCode\"={0}", Convert.ToInt32(documento.Interveniente_Comissionado)));
                                        if (oRecordSet.RecordCount > 0)
                                            documento.SlpCode = Convert.ToString(oRecordSet.Fields.Item("SlpCode").Value);
                                    }
                                    #endregion

                                    #region Procura Pela Filial
                                    oRecordSet.DoQuery(string.Format("select T0.\"BPLId\" from \"OBPL\" T0 where  replace(replace(replace(T0.\"TaxIdNum\",'.',''),'/',''),'-','')='{0}'", documento.Parte_CNPJ));
                                    if (oRecordSet.RecordCount == 0)
                                    {
                                        documento.MsgErro = string.Format("Filial não encontrata Através do CNPJ: {0}", documento.Parte_CNPJ);
                                        continue;
                                    }
                                    else
                                        documento.BPLId = Convert.ToString(oRecordSet.Fields.Item("BPLId").Value);
                                    #endregion

                                    #region Procura Pelo Indicator
                                    if (!string.IsNullOrEmpty(documento.Submercado))
                                    {
                                        oRecordSet.DoQuery(string.Format("select T0.\"Code\" from \"OIDC\" T0 where T0.\"Name\"='{0}'", documento.Submercado));
                                        if (oRecordSet.RecordCount == 0)
                                        {
                                            documento.MsgErro = string.Format("Submercado não encontrado Através da descrição: {0}", documento.Submercado);
                                            continue;
                                        }
                                        else
                                            documento.Indicator = Convert.ToString(oRecordSet.Fields.Item("Code").Value);
                                    }
                                    #endregion

                                    #region Procura pela condição de pgto se for em 3 parcelas

                                    string valor1 = string.Empty;
                                    string valor2 = string.Empty;
                                    string valor3 = string.Empty;

                                    if (string.IsNullOrEmpty(documento.Valor_parcela_1))
                                    {
                                        documento.MsgErro = $"Valor da primeira parcela não pode ser vazia ou nula, Linha: {documento.LinhaPlanilha}";
                                        continue;
                                    }

                                    if (!string.IsNullOrEmpty(documento.Valor_parcela_2))
                                        valor2 = documento.Valor_parcela_2.Replace(".", ",");
                                    else
                                        valor2 = "0";

                                    if (!string.IsNullOrEmpty(documento.Valor_parcela_3))
                                        valor3 = documento.Valor_parcela_2.Replace(".", ",");
                                    else
                                        valor3 = "0";

                                    double parcela2 = Convert.ToDouble(valor2);
                                    double parcela3 = Convert.ToDouble(valor3);

                                    if (parcela2 > 0 || parcela3 > 0)
                                    {
                                        documento.EhTresPagamentos = true;

                                        oRecordSet.DoQuery(string.Format("SELECT T0.\"GroupNum\" FROM \"OCTG\" T0 WHERE T0.\"PymntGroup\"='{0}'", documento.Condicao_pagto));

                                        if (oRecordSet.RecordCount == 0)
                                        {
                                            documento.MsgErro = string.Format($"Condição de pagamento: {documento.Condicao_pagto}, da Linha:{documento.LinhaPlanilha} não encontrada.");
                                            continue;
                                        }
                                        else
                                            documento.Condicao_pagto = oRecordSet.Fields.Item("GroupNum").Value.ToString();

                                    }

                                    #endregion

                                    //gera os documentos de marketing
                                    #region Documento de Markting

                                    SAPbobsCOM.Documents _document;

                                    if ((documento.Movimentacao.Equals("Serviço")) || (documento.Movimentacao.Equals("Venda")))
                                    {
                                        _document = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                        //verifica se o pedido ja existe, atraves do campo chave
                                        int iDocEntry = 0;

                                        try
                                        {
                                            oRecordSet.DoQuery(string.Format("select coalesce(T0.\"DocEntry\",0) DocEntry from \"ORDR\" T0 where T0.\"U_CVA_Chave\"='{0}'", documento.Chave));

                                            if (oRecordSet.RecordCount > 0)
                                                iDocEntry = oRecordSet.Fields.Item("DocEntry").Value;

                                            if (iDocEntry > 0)
                                            {
                                                _document.GetByKey(iDocEntry);
                                                if (_document.UserFields.Fields.Item("U_CVA_EE_Status").Value != "R")
                                                    continue;
                                            }
                                        }
                                        catch { }

                                        if (documento.Movimentacao.Equals("Serviço"))
                                        {
                                            switch (documento.Nome_Contrato.Substring(0, documento.Nome_Contrato.IndexOf(' ')).Trim())
                                            {
                                                case "CSO":
                                                    documento.ItemCode = ConfigurationManager.AppSettings["CSO"].ToString();
                                                    break;
                                                case "CRO":
                                                    documento.ItemCode = ConfigurationManager.AppSettings["CRO"].ToString();
                                                    break;
                                                case "CC":
                                                    documento.ItemCode = ConfigurationManager.AppSettings["CC"].ToString();
                                                    break;
                                                case "CAS":
                                                    documento.ItemCode = ConfigurationManager.AppSettings["CAS"].ToString();
                                                    break;
                                                default:
                                                    documento.MsgErro = string.Format("Item Serviço não encontrado na configuração: {0}", documento.Nome_Contrato.Substring(0, documento.Nome_Contrato.IndexOf(' ')).Trim());
                                                    continue;
                                            }
                                        }
                                        else if (documento.Movimentacao.Equals("Venda"))
                                        {
                                            oRecordSet.DoQuery(string.Format("select top 1 T0.\"ItemCode\" from \"OITM\" T0 where Ucase(T0.\"U_CVA_WBC\")=Ucase('{0}')", documento.Perfil_CCEE_vendedor));

                                            if (oRecordSet.RecordCount == 0)
                                            {
                                                documento.MsgErro = string.Format("Item não encontrado para para o Perfil_CCEE_vendedor: {0} ", documento.Perfil_CCEE_vendedor);
                                                continue;
                                            }
                                            else
                                                documento.ItemCode = oRecordSet.Fields.Item("ItemCode").Value;
                                        }

                                        MontaDocumento(documento, _document, !(iDocEntry > 0));

                                        if (iDocEntry > 0)
                                        {
                                            if (_document.Update() != 0)
                                            {
                                                documento.MsgErro = string.Format("Erro ao Atualizar pedido Nr. {0}: {1} - {2}", iDocEntry, oCompany.GetLastErrorDescription(), oCompany.GetLastErrorCode());
                                            }
                                            else
                                                LogInfoGreen(string.Format("Documento de Venda Modificado com sucesso: {0}, referente a linha {1}", iDocEntry, documento.LinhaPlanilha));
                                        }
                                        else
                                        {
                                            if (_document.Add() != 0)
                                            {
                                                documento.MsgErro = string.Format("Erro ao Inserir novo pedido: {0} - {1}", oCompany.GetLastErrorDescription(), oCompany.GetLastErrorCode());
                                            }
                                            else
                                                LogInfoGreen(string.Format("Documento de Venda Adicionado com sucesso: {0}, referente a linha {1}", oCompany.GetNewObjectKey(), documento.LinhaPlanilha));

                                        }
                                        SetCOMObjectFree(_document);
                                    }
                                    else if (documento.Movimentacao.Equals("Compra"))
                                    {
                                        _document = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

                                        //verifica se o pedido ja existe, atraves do campo chave
                                        int iDocEntry = 0;
                                        oRecordSet.DoQuery(string.Format("select coalesce(T0.\"DocEntry\",0) DocEntry from \"OPOR\" T0 where T0.\"U_CVA_Chave\"='{0}'", documento.Chave));

                                        if (oRecordSet.RecordCount > 0)
                                            iDocEntry = oRecordSet.Fields.Item("DocEntry").Value;

                                        if (iDocEntry > 0)
                                        {
                                            _document.GetByKey(iDocEntry);
                                            if (_document.UserFields.Fields.Item("U_CVA_EE_Status").Value != "R")
                                                continue;
                                        }

                                        oRecordSet.DoQuery(string.Format("select top 1 T0.\"ItemCode\" from \"OITM\" T0 where Ucase(T0.\"U_CVA_WBC\")=Ucase('{0}')", documento.Perfil_CCEE_vendedor));

                                        if (oRecordSet.RecordCount == 0)
                                        {
                                            documento.MsgErro = string.Format("Item não encontrado para o Perfil_CCEE_vendedor: {0}", documento.Perfil_CCEE_vendedor);
                                            continue;
                                        }
                                        else
                                            documento.ItemCode = oRecordSet.Fields.Item("ItemCode").Value;

                                        MontaDocumento(documento, _document, !(iDocEntry > 0));

                                        if (iDocEntry > 0)
                                        {
                                            if (_document.Update() != 0)
                                            {
                                                documento.MsgErro = string.Format("Erro ao Atualizar pedido Nr. {0}: {1} - {2}", iDocEntry, oCompany.GetLastErrorDescription(), oCompany.GetLastErrorCode());
                                            }
                                            else
                                            {
                                                LogInfoGreen(string.Format("Documento de Compra Atualizado com sucesso: {0}, referente a linha {1}", oCompany.GetNewObjectKey(), documento.LinhaPlanilha));
                                            }
                                        }
                                        else
                                        {
                                            if (_document.Add() != 0)
                                            {
                                                documento.MsgErro = string.Format("Erro ao Inserir novo pedido: {0} - {1}", oCompany.GetLastErrorDescription(), oCompany.GetLastErrorCode());
                                            }
                                            else
                                            {
                                                LogInfoGreen(string.Format("Documento de Compra Adcionado com sucesso: {0}, referente a linha {1}", oCompany.GetNewObjectKey(), documento.LinhaPlanilha));
                                            }
                                        }

                                    }

                                    #endregion Documento de Markting
                                }
                                catch (Exception ex)
                                {
                                    documento.MsgErro = string.Format("Erro ao Atualizar pedido: {0}", ex.Message);
                                }

                            }

                            //gera o arquivo de erros do arquivo
                            string path = string.Format(@"{0}\{1}", sFolderMonitorErrorLog, string.Format("Logging_{0}.txt", DateTime.Today.ToString("ddMMyyyy")));
                            using (var tw = new StreamWriter(path, true))
                            {
                                foreach (Documento item in lstDocumento.Where(a => a.Situacao_Contrato == "Liberado").Where(a => a.MsgErro != null).OrderBy(a => a.LinhaPlanilha))
                                {
                                    tw.WriteLine(string.Format("Data Hora: {0} - File: {1} - Linha: {2} - Erro: {3}", DateTime.Now, file.Name, item.LinhaPlanilha.ToString(), item.MsgErro));
                                }
                            }
                        }
                    }

                    //move o arquivo para a pasta de processados
                    File.Move(file.FullName, string.Format(@"{0}\{1}", sFolderMonitorprocessed, string.Format("{0}_{1}.xlsx", file.Name.Replace(".xlsx", ""), DateTime.Now.ToString("ddMMyyyyHHmm"))));
                }
                //}   
                //loop
                oCompany.Disconnect();
                oCompany = null;
            }
            else
            {
                LogError(retStr);
            }
            return;
        }

        private static void MontaDocumento(Documento documento, Documents _document, bool bNovo = true)
        {
            _document.CardCode = documento.CardCode;

            if (documento.Data_emissao_prevista != null) _document.TaxDate = Convert.ToDateTime(documento.Data_emissao_prevista);

            _document.BPL_IDAssignedToInvoice = Convert.ToInt16(documento.BPLId);

            if (documento.UsoPrincipal > 0 & bNovo) _document.TaxExtension.MainUsage = documento.UsoPrincipal;

            if (!string.IsNullOrEmpty(documento.Indicator)) _document.Indicator = documento.Indicator;

            _document.DocDueDate = documento.Data_parcela_1;

            _document.TaxExtension.Incoterms = "9";

            _document.TaxExtension.Brand = "ELECTRA";

            int.TryParse(documento.Condicao_pagto, out int condicaoPagamento);

            if (documento.EhTresPagamentos)
            {
                _document.GroupNumber = condicaoPagamento;
                _document.PaymentGroupCode = condicaoPagamento;
            }

            try
            {
                if (!string.IsNullOrEmpty(documento.Numero_referencia_contrato))
                    _document.UserFields.Fields.Item("U_SKILL_xCont").Value = documento.Numero_referencia_contrato.ToString();

                _document.UserFields.Fields.Item("U_CVA_EE_Mes").Value = documento.Mes;
                _document.UserFields.Fields.Item("U_CVA_EE_Ano").Value = documento.Ano;
                _document.UserFields.Fields.Item("U_CVA_DataImport").Value = DateTime.Today;
                _document.UserFields.Fields.Item("U_CVA_HoraImport").Value = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString().PadLeft(2, '0');
            }
            catch { }

            documento.CardName = (documento.CardName.Length > 30) ? documento.CardName.Substring(0, 30) : documento.CardName;

            if (documento.Movimentacao == "Compra")
                _document.JournalMemo = $"PC {documento.CardName}";
            else
                _document.JournalMemo = $"PV {documento.CardName}";

            _document.OpeningRemarks = (documento.Movimentacao.Equals("Compra")) ? documento.Observacao_Pagamentos : string.IsNullOrEmpty(documento.Observacao_Pagamentos) ? $"FATURAMENTO REF {documento.Mes}/{documento.Ano}" : documento.Observacao_Pagamentos + " " + $"FATURAMENTO REF {documento.Mes}/{documento.Ano}";

            if (!string.IsNullOrEmpty(documento.Portfolio_Vendedor)) _document.Project = documento.Portfolio_Vendedor;

            try
            {
                if (!string.IsNullOrEmpty(documento.Form_AGIO)) _document.UserFields.Fields.Item("U_CVA_EE_SPREAD").Value = documento.Form_AGIO;
                if (!string.IsNullOrEmpty(documento.Valor_TRU)) _document.UserFields.Fields.Item("U_CVA_EE_TRU").Value = documento.Valor_TRU;
                _document.UserFields.Fields.Item("U_CVA_EE_CodWBC").Value = documento.Codigo_WBC;
                _document.UserFields.Fields.Item("U_CVA_Chave").Value = documento.Chave;
                _document.UserFields.Fields.Item("U_CVA_EE_Status").Value = "I";
                if (!string.IsNullOrEmpty(documento.Codigo_CCEE)) _document.UserFields.Fields.Item("U_CVA_EE_CodCCEE").Value = documento.Codigo_CCEE;
                _document.UserFields.Fields.Item("U_CVA_Suprimento_Inicio").Value = documento.Suprimento_inicio;
                _document.UserFields.Fields.Item("U_CVA_Suprimento_Termino").Value = documento.Suprimento_termino;
            }
            catch { }

            if (!string.IsNullOrEmpty(documento.SlpCode))
                _document.SalesPersonCode = Convert.ToInt16(documento.SlpCode);

            if (!string.IsNullOrEmpty(documento.Portfolio_Vendedor)) _document.Lines.ProjectCode = documento.Portfolio_Vendedor;

            _document.Lines.ItemCode = documento.ItemCode;

            if (documento.UsoPrincipal > 0) _document.Lines.Usage = documento.UsoPrincipal.ToString();

            _document.Lines.COGSCostingCode = documento.CentroDeCusto;

            if (documento.Movimentacao.Equals("Serviço"))
            {
                _document.Lines.Quantity = 1;
                _document.Lines.UnitPrice = Convert.ToDouble(documento.ValorReajustado.Replace(".", ","));
            }
            else
            {
                _document.Lines.Quantity = Convert.ToDouble(documento.QuantAtualizada.Replace(".", ","));

                var parcela1 = Convert.ToDouble(documento.Valor_parcela_1.Replace(".", ","));

                double parcela2 = 0;
                double parcela3 = 0;
                string valor2 = string.Empty;
                string valor3 = string.Empty;

                if (documento.EhTresPagamentos)
                {
                    if (!string.IsNullOrEmpty(documento.Valor_parcela_2))
                        valor2 = documento.Valor_parcela_2.Replace(".", ",");
                    else
                        valor2 = "0";

                    if (!string.IsNullOrEmpty(documento.Valor_parcela_3))
                        valor3 = documento.Valor_parcela_3.Replace(".", ",");
                    else
                        valor3 = "0";

                    parcela2 = Convert.ToDouble(valor2.Replace(".", ","));
                    parcela3 = Convert.ToDouble(valor3.Replace(".", ","));
                }

                double precolinha = (parcela1 + parcela2 + parcela3) / _document.Lines.Quantity;
                _document.Lines.UnitPrice = precolinha;
            }

            // Condição CSTCode terminar em 41 
            //_document.Lines.UserFields.Fields.Item("U_SKILL_Benefiscal").Value = "PR800003";
        }

        private static void MontaPreDocumento(List<MapaColunaPosicao> lstMapaColunaPosicao, Documento oDocumento, string str, string sColuna, Int64 iInha)
        {
            if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Movimentacao"))
            {
                oDocumento.Movimentacao = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Contraparte_CNPJ"))
            {
                oDocumento.Contraparte_CNPJ = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Parte_CNPJ"))
            {
                oDocumento.Parte_CNPJ = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Submercado"))
            {
                oDocumento.Submercado = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Codigo_CCEE"))
            {
                oDocumento.Codigo_CCEE = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Codigo_WBC"))
            {
                oDocumento.Codigo_WBC = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Observacao"))
            {
                oDocumento.Observacao = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Ano"))
            {
                oDocumento.Ano = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Mes"))
            {
                oDocumento.Mes = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Valor_parcela_1"))
            {
                oDocumento.Valor_parcela_1 = str.ToString(CultureInfo.InvariantCulture);
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Data_parcela_1"))
            {
                double d = double.Parse(str);
                DateTime conv = DateTime.FromOADate(d);
                oDocumento.Data_parcela_1 = conv;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Data_emissao_fatura"))
            {
                double d = double.Parse(str);
                DateTime conv = DateTime.FromOADate(d);
                oDocumento.Data_emissao_fatura = conv;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.ToUpper().Equals("FORM_AGIO"))
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    oDocumento.Form_AGIO = str;
                }
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Valor_TRU"))
            {
                oDocumento.Valor_TRU = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Perfil_CCEE_vendedor"))
            {
                oDocumento.Perfil_CCEE_vendedor = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Situacao_faturamento_backoffice"))
            {
                oDocumento.Situacao_faturamento_backoffice = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("QuantAtualizada"))
            {
                oDocumento.QuantAtualizada = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals(CVA.Monitor.WBC.Excel.Files.Properties.Settings.Default.ColunaCodigoDoVendedor))
            {
                oDocumento.CodigoDoVededor = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Observacao_Pagamentos"))
            {
                oDocumento.Observacao_Pagamentos = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Situacao_Contrato"))
            {
                oDocumento.Situacao_Contrato = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Nome_contrato"))
            {
                oDocumento.Nome_Contrato = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("ValorReajustado"))
            {
                oDocumento.ValorReajustado = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Portfolio_Vendedor"))
            {
                oDocumento.Portfolio_Vendedor = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Interveniente_Comissionado"))
            {
                oDocumento.Interveniente_Comissionado = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Suprimento_inicio"))
            {
                double d = double.Parse(str);
                DateTime conv = DateTime.FromOADate(d);
                oDocumento.Suprimento_inicio = conv;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Suprimento_termino"))
            {
                double d = double.Parse(str);
                DateTime conv = DateTime.FromOADate(d);
                oDocumento.Suprimento_termino = conv;
            }

            //Novos Campos PCH 62
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Condicao_pagto"))
            {
                oDocumento.Condicao_pagto = str;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Valor_parcela_2"))
            {
                oDocumento.Valor_parcela_2 = str.ToString(CultureInfo.InvariantCulture);
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Valor_parcela_3"))
            {
                oDocumento.Valor_parcela_3 = str.ToString(CultureInfo.InvariantCulture);
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Data_emissao_prevista"))
            {
                double d = double.Parse(str);
                DateTime conv = DateTime.FromOADate(d);
                oDocumento.Data_emissao_prevista = conv;
            }
            else if (lstMapaColunaPosicao.Where(a => a.Posicao == sColuna).FirstOrDefault().NomeColuna.Equals("Numero_referencia_contrato"))
            {
                oDocumento.Numero_referencia_contrato = str;
            }

            oDocumento.LinhaPlanilha = iInha;
        }

        private string RemoveNumbers(string sParam)
        {
            return sParam.Replace("0", "").Replace("1", "").Replace("2", "")
                .Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "")
                .Replace("7", "").Replace("8", "").Replace("9", "");
        }

        private static void VerificarDiretorios(string sFolderMonitor, string sFolderMonitorprocessed, string sFolderMonitorErrorLog)
        {
            try
            {
                if (!Directory.Exists(sFolderMonitor))
                {
                    Directory.CreateDirectory(sFolderMonitor);
                }

                if (!Directory.Exists(sFolderMonitorprocessed))
                {
                    Directory.CreateDirectory(sFolderMonitorprocessed);
                }

                if (!Directory.Exists(sFolderMonitorErrorLog))
                {
                    Directory.CreateDirectory(sFolderMonitorErrorLog);
                }
            }
            catch (Exception ex)
            {

                LogError(ex.Message);
            }
        }

        private void AdcionarCampo(string sTableName, string sFieldName, string sFieldDescription, SAPbobsCOM.BoFieldTypes FieldType, int EditSize, BoFldSubTypes subType, string[,] valoresValidos, string valorDefault)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

            oUserFieldsMD.TableName = sTableName;
            oUserFieldsMD.Name = sFieldName;
            oUserFieldsMD.Description = sFieldDescription;
            oUserFieldsMD.Type = FieldType;
            oUserFieldsMD.EditSize = EditSize;
            oUserFieldsMD.SubType = subType;

            //adicionar valores válidos
            if (valoresValidos != null)
            {
                Int32 qtd = valoresValidos.GetLength(0);
                if (qtd > 0)
                {
                    for (int i = 0; i < qtd; i++)
                    {
                        oUserFieldsMD.ValidValues.Value = valoresValidos[i, 0];
                        oUserFieldsMD.ValidValues.Description = valoresValidos[i, 1];
                        oUserFieldsMD.ValidValues.Add();
                    }
                }
            }

            lRetCode = oUserFieldsMD.Add();

            if (lRetCode != 0)
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                LogError(string.Format("Erro ao Adcionar campo em {0}: {1} - {2}", oUserFieldsMD.TableName, lErrCode, sErrMsg));
            }
            else
            {
                LogInfo("Field: \'" + oUserFieldsMD.Name + "\' was added successfuly to " + oUserFieldsMD.TableName + " Table");

            }
            SetCOMObjectFree(oUserFieldsMD);
        }

        private void AdcionarCampoMonetario(string sTableName, string sFieldName, string sFieldDescription)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

            oUserFieldsMD.TableName = sTableName;
            oUserFieldsMD.Name = sFieldName;
            oUserFieldsMD.Description = sFieldDescription;
            oUserFieldsMD.Type = BoFieldTypes.db_Float;
            oUserFieldsMD.SubType = BoFldSubTypes.st_Price;
            lRetCode = oUserFieldsMD.Add();

            if (lRetCode != 0)
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                LogError(string.Format("Erro ao Adcionar campo em {0}: {1} - {2}", oUserFieldsMD.TableName, lErrCode, sErrMsg));
            }
            else
            {
                LogInfo("Field: \'" + oUserFieldsMD.Name + "\' was added successfuly to " + oUserFieldsMD.TableName + " Table");

            }
            SetCOMObjectFree(oUserFieldsMD);
        }

        private void AdcionarCampoComboBox(string sTableName, string sFieldName, string sFieldDescription)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

            oUserFieldsMD.TableName = sTableName;
            oUserFieldsMD.Name = sFieldName;
            oUserFieldsMD.Description = sFieldDescription;
            oUserFieldsMD.Type = BoFieldTypes.db_Alpha;
            oUserFieldsMD.EditSize = 1;

            oUserFieldsMD.ValidValues.Value = "I";
            oUserFieldsMD.ValidValues.Description = "Importado";
            oUserFieldsMD.ValidValues.Add();
            oUserFieldsMD.ValidValues.Value = "R";
            oUserFieldsMD.ValidValues.Description = "Reprocessar";
            oUserFieldsMD.ValidValues.Add();
            oUserFieldsMD.DefaultValue = "I";
            lRetCode = oUserFieldsMD.Add();

            if (lRetCode != 0)
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                LogError(string.Format("Erro ao Adcionar campo em {0}: {1} - {2}", oUserFieldsMD.TableName, lErrCode, sErrMsg));
            }
            else
            {
                LogInfo("Field: \'" + oUserFieldsMD.Name + "\' was added successfuly to " + oUserFieldsMD.TableName + " Table");

            }
            SetCOMObjectFree(oUserFieldsMD);
        }

        public static void LogError(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " :=> " + texto);
            Console.WriteLine("");
        }

        public static void LogInfoGreen(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " :=> " + texto);
            Console.WriteLine("");
        }

        public static void LogInfo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " :=> " + texto);
            Console.WriteLine("");
        }

        public void SetCOMObjectFree(object objeto)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objeto);
                objeto = null;
                GC.Collect();
            }
            catch (Exception)
            {
            }
        }

        public bool ConectarSap(ParametrosSap ps, ParametrosConexao pc, out int retVal, out string retStr, out SAPbobsCOM.Company oCompany)
        {

            bool conectado = false;
            oCompany = new SAPbobsCOM.Company();
            retVal = 0;
            retStr = "";

            oCompany.Server = pc.servidor;
            oCompany.DbServerType = pc.tipo;

            oCompany.DbUserName = pc.usuario;
            oCompany.DbPassword = pc.senha;
            oCompany.LicenseServer = pc.licenca;

            oCompany.CompanyDB = ps.Company;
            oCompany.UserName = ps.Usuario;
            oCompany.Password = ps.Senha;

            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br;

            retVal = oCompany.Connect();

            if (retVal != 0)
            {
                oCompany.GetLastError(out retVal, out retStr);
                return conectado;
            }
            else
            {
                conectado = true;
                return conectado;
            }

        }
    }
}
