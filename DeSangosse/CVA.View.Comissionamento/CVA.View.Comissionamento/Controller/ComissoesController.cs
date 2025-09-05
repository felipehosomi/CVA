using System;
using CVA.View.Comissionamento.Helpers;
using System.Data;
using SAPbobsCOM;

namespace CVA.View.Comissionamento.Controller
{
    public class ComissoesController
    {
        public static DataTable GetInvoices(string dataInicial, string dataFinal, bool todas, bool pagas, bool naoPagas)
        {
            DataTable dt = null;

            if (todas)
                dt = GetAllInvoices(dataInicial, dataFinal);
            if (pagas)
                dt = GetClosedInvoices(dataInicial, dataFinal);
            if (naoPagas)
                dt = GetOpenInvoices(dataInicial, dataFinal);

            return dt;
        }

        public static string SetInvoices(string dataInicial, string dataFinal, bool todas, bool pagas, bool naoPagas)
        {
            var ret = string.Empty;

            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"EXEC spc_CVA_Comissoes 'N', 'T', '{dataInicial}', '{dataFinal}'");

            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var objType = oRecordset.Fields.Item("U_OBJTYPE").Value.ToString();
                    var docEntry = oRecordset.Fields.Item("U_DOCENTRY").Value.ToString();
                    var linha = oRecordset.Fields.Item("U_LINENUM").Value.ToString();
                    var instId = oRecordset.Fields.Item("U_PARCELA").Value.ToString();
                    var docEntryNF = oRecordset.Fields.Item("U_DOCENTRY_NF").Value.ToString();
                    var docEntryCR = oRecordset.Fields.Item("U_DOCENTRY_CR").Value.ToString();
                    VerifyUdo(docEntry, objType, linha, instId, "", "",docEntryNF,docEntryCR);
                    oRecordset.MoveNext();
                }
                oRecordset.MoveFirst();
                while (!oRecordset.EoF)
                {
                    var prioridade = oRecordset.Fields.Item("U_PRIORIDADE").Value.ToString();
                    var comissionado = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                    var cardCode = oRecordset.Fields.Item("U_CARDCODE").Value.ToString();
                    var cardName = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                    var regra = oRecordset.Fields.Item("U_REGRA").Value.ToString();
                    var docDate = oRecordset.Fields.Item("U_DOCDATE").Value.ToString();
                    var dueDate = oRecordset.Fields.Item("U_DUEDATE").Value.ToString();
                    var objType = oRecordset.Fields.Item("U_OBJTYPE").Value.ToString();
                    var docEntry = oRecordset.Fields.Item("U_DOCENTRY").Value.ToString();
                    var linha = oRecordset.Fields.Item("U_LINENUM").Value.ToString();
                    var itemCode = oRecordset.Fields.Item("U_ITEMCODE").Value.ToString();
                    var itemName = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                    var ocrCode = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                    var instId = oRecordset.Fields.Item("U_PARCELA").Value.ToString();
                    var total = oRecordset.Fields.Item("U_VALOR").Value.ToString();
                    var taxSum = oRecordset.Fields.Item("U_IMPOSTOS").Value.ToString();
                    var percentComissao = oRecordset.Fields.Item("U_COMISSAO").Value.ToString();
                    var valorComissao = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                    var comissaoPaga = oRecordset.Fields.Item("U_PAGO").Value.ToString();
                    var dataPagamento = oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString();
                    var dataRecebimento = oRecordset.Fields.Item("U_TAXDATE").Value.ToString();
                    var docEntryNF = oRecordset.Fields.Item("U_DOCENTRY_NF").Value.ToString();
                    var docEntryCR = oRecordset.Fields.Item("U_DOCENTRY_CR").Value.ToString();
                    AddUdo(prioridade, comissionado, cardCode, cardName, regra, docDate, dueDate, objType, docEntry, linha, itemCode, itemName, ocrCode, instId, total, taxSum, percentComissao, valorComissao, comissaoPaga, dataPagamento, dataRecebimento, docEntryNF, docEntryCR);
                    oRecordset.MoveNext();
                }
                ret = "Documentos enviados para pagamento.";
            }

            return ret;
        }

        public static DataTable GetResumido(string dataInicial, string dataFinal, string vendedor)
        {
            var dt = new DataTable("RESUMIDO");
            var slpCode = string.IsNullOrEmpty(vendedor) ? "0" : vendedor;

            dt.Columns.Add("U_COMISSIONADO", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("U_SLPNAME", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("U_VALORCOMISSAO", typeof(double)).AllowDBNull = true;

            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"exec spc_CVA_PagarComissao 'N', {slpCode}, '{dataInicial}', '{dataFinal}'");

            if(oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var dr = dt.Rows.Add();
                    dr["U_COMISSIONADO"] = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                    dr["U_SLPNAME"] = oRecordset.Fields.Item("SlpName").Value.ToString();
                    dr["U_VALORCOMISSAO"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                    oRecordset.MoveNext();
                }
            }

            return dt;
        }

        public static DataTable GetDetalhado(string dataInicial, string dataFinal, string vendedor)
        {
            var dt = GetInvoicesDataTable();
            var slpCode = string.IsNullOrEmpty(vendedor) ? "0" : vendedor;

            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"exec spc_CVA_PagarComissao 'Y', {slpCode}, '{dataInicial}', '{dataFinal}'");
            var i = 1;

            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var dr = dt.Rows.Add();
                    dr["Row"] = i.ToString();
                    dr["Comissionado"] = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                    dr["CardCode"] = oRecordset.Fields.Item("U_CARDCODE").Value.ToString();
                    dr["CardName"] = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                    dr["Regra"] = oRecordset.Fields.Item("U_REGRA").Value.ToString();
                    dr["DocDate"] = oRecordset.Fields.Item("U_DOCDATE").Value.ToString();
                    dr["DueDate"] = oRecordset.Fields.Item("U_DUEDATE").Value.ToString();
                    dr["ObjType"] = oRecordset.Fields.Item("U_OBJTYPE").Value.ToString();
                    dr["DocEntry"] = oRecordset.Fields.Item("U_DOCENTRY").Value.ToString();
                    dr["Linha"] = oRecordset.Fields.Item("U_LINENUM").Value.ToString();
                    dr["ItemCode"] = oRecordset.Fields.Item("U_ITEMCODE").Value.ToString();
                    dr["ItemName"] = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                    dr["OcrCode"] = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                    dr["InstId"] = oRecordset.Fields.Item("U_PARCELA").Value.ToString();
                    dr["Total"] = oRecordset.Fields.Item("U_VALOR").Value.ToString();
                    dr["TaxSum"] = oRecordset.Fields.Item("U_IMPOSTOS").Value.ToString();
                    dr["PercentComissao"] = oRecordset.Fields.Item("U_COMISSAO").Value.ToString();
                    dr["ValorComissao"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                    dr["Momento"] = oRecordset.Fields.Item("U_MOMENTO").Value.ToString();
                    i++;
                    oRecordset.MoveNext();
                }
            }

            return dt;
        }

        public static void SetInvoices(string vendedor, string dataPagamento)
        {
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"select Code from [@CVA_CALC_COMISSAO] where U_COMISSIONADO = {vendedor} and U_PAGO = 'N'");
            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var oCompanyService = B1Connection.Instance.Company.GetCompanyService();
                    var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");

                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());
                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_PAGO", "Y");
                    oGeneralData.SetProperty("U_DATAPAGAMENTO", DIHelper.Format_StringToDate(dataPagamento));
                    oGeneralService.Update(oGeneralData);
                    oRecordset.MoveNext();
                } 
            }
        }

        public static void SetInvoices(string docEntry, string objType, string lineNum, string dataPagamento)
        {
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"select Code from [@CVA_CALC_COMISSAO] where U_DOCENTRY = {docEntry} and U_OBJTYPE = {objType} and U_LINENUM = {lineNum} and U_PAGO = 'N'");
            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var oCompanyService = B1Connection.Instance.Company.GetCompanyService();
                    var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");

                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());
                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_PAGO", "Y");
                    oGeneralData.SetProperty("U_DATAPAGAMENTO", DIHelper.Format_StringToDate(dataPagamento));
                    oGeneralService.Update(oGeneralData);
                    oRecordset.MoveNext();
                }
            }
        }

        private static void AddUdo(string prioridade, string comissionado, string cardCode, string cardName, string regra, string docDate, string dueDate, string objType, string docEntry, string linha, string itemCode, string itemName, string ocrCode, string instId, string total, string taxSum, string percentComissao, string valorComissao, string comissaoPaga, string dataPagamento, string dataRecebimento, string docEntryNF, string docEntryCR)
        {
            var oCompanyService = B1Connection.Instance.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");
            var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
            var code = DIHelper.GetNextCode("CVA_CALC_COMISSAO");
            oGeneralData.SetProperty("Code", code.ToString());
            oGeneralData.SetProperty("Name", code.ToString());
            oGeneralData.SetProperty("U_COMISSIONADO", comissionado);
            oGeneralData.SetProperty("U_CARDCODE", cardCode);
            oGeneralData.SetProperty("U_CARDNAME", cardName);
            oGeneralData.SetProperty("U_REGRA", regra);
            oGeneralData.SetProperty("U_DOCDATE", docDate);
            oGeneralData.SetProperty("U_DUEDATE", dueDate);
            oGeneralData.SetProperty("U_DOCENTRY", docEntry);
            oGeneralData.SetProperty("U_OBJTYPE", objType);
            oGeneralData.SetProperty("U_ITEMCODE", itemCode);
            oGeneralData.SetProperty("U_ITEMNAME", itemName);
            oGeneralData.SetProperty("U_LINENUM", linha);
            oGeneralData.SetProperty("U_VALOR", total.Replace(".", "").Replace(",", "."));
            oGeneralData.SetProperty("U_PARCELA", instId);
            oGeneralData.SetProperty("U_IMPOSTOS", taxSum.Replace(".", "").Replace(",", "."));
            oGeneralData.SetProperty("U_COMISSAO", percentComissao.Replace(".", "").Replace(",", "."));
            oGeneralData.SetProperty("U_VALORCOMISSAO", valorComissao.Replace(".", "").Replace(",", "."));
            oGeneralData.SetProperty("U_CENTROCUSTO", ocrCode);
            oGeneralData.SetProperty("U_PAGO", comissaoPaga);
            oGeneralData.SetProperty("U_DATAPAGAMENTO", dataPagamento);
            oGeneralData.SetProperty("U_TAXDATE", dataRecebimento);
            oGeneralData.SetProperty("U_DOCENTRY_NF", docEntryNF);
            oGeneralData.SetProperty("U_DOCENTRY_CR", docEntryCR);
            //oGeneralData.SetProperty("U_PRIORIDADE", prioridade);

            var oGeneralParams = oGeneralService.Add(oGeneralData);
        }

        private static bool VerifyUdo(string docEntry, string objType, string linha, string parcela, string regra, string comissionado, string docEntryNF, string docEntryCR)
        {
            var ret = false;
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"DELETE FROM [@CVA_CALC_COMISSAO] WHERE U_DOCENTRY = {docEntry} AND U_OBJTYPE = {objType} AND U_LINENUM = {linha} AND U_PARCELA={parcela} and U_DOCENTRY_NF={docEntryNF} and U_DOCENTRY_CR={docEntryCR}");
           //if (oRecordset.RecordCount > 0)
           ret = true;

            return ret;
        }

        private static DataTable GetAllInvoices(string dataInicial, string dataFinal)
        {
            var dt = GetInvoicesDataTable();

            var dtIni = DIHelper.Format_StringToDate(dataInicial);
            var dtFim = DIHelper.Format_StringToDate(dataFinal);

            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"EXEC spc_CVA_Comissoes 'T', 'N', '{dataInicial}', '{dataFinal}'");
            var i = 1;

            while (!oRecordset.EoF)
            {
                var dr = dt.Rows.Add();
                dr["Row"] = i.ToString();
                dr["Prioridade"] = oRecordset.Fields.Item("U_PRIORIDADE").Value.ToString();
                dr["Comissionado"] = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                dr["CardCode"] = oRecordset.Fields.Item("U_CARDCODE").Value.ToString();
                dr["CardName"] = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                dr["Regra"] = oRecordset.Fields.Item("U_REGRA").Value.ToString();
                dr["DocDate"] = oRecordset.Fields.Item("U_DOCDATE").Value.ToString();
                dr["DueDate"] = oRecordset.Fields.Item("U_DUEDATE").Value.ToString();
                dr["ObjType"] = oRecordset.Fields.Item("U_OBJTYPE").Value.ToString();
                dr["DocEntry"] = oRecordset.Fields.Item("U_DOCENTRY").Value.ToString();
                dr["Linha"] = oRecordset.Fields.Item("U_LINENUM").Value.ToString();
                dr["ItemCode"] = oRecordset.Fields.Item("U_ITEMCODE").Value.ToString();
                dr["ItemName"] = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                dr["OcrCode"] = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                dr["InstId"] = oRecordset.Fields.Item("U_PARCELA").Value.ToString();
                dr["Total"] = oRecordset.Fields.Item("U_VALOR").Value.ToString();
                dr["TaxSum"] = oRecordset.Fields.Item("U_IMPOSTOS").Value.ToString();
                dr["PercentComissao"] = oRecordset.Fields.Item("U_COMISSAO").Value.ToString();
                dr["ValorComissao"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                dr["ComissaoPaga"] = oRecordset.Fields.Item("U_PAGO").Value.ToString();
                dr["Momento"] = oRecordset.Fields.Item("U_MOMENTO").Value.ToString();
                dr["Serial"] = oRecordset.Fields.Item("U_SERIAL").Value.ToString();
                if (oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString() != "1900-01-01")
                    dr["DataPagamento"] = oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString();
                if (oRecordset.Fields.Item("U_TAXDATE").Value.ToString() != "1900-01-01")
                    dr["DataRecebimento"] = oRecordset.Fields.Item("U_TAXDATE").Value.ToString();
                i++;
                oRecordset.MoveNext();
            }

            return dt;
        }

        private static DataTable GetClosedInvoices(string dataInicial, string dataFinal)
        {
            var dt = GetInvoicesDataTable();

            var dtIni = DIHelper.Format_StringToDate(dataInicial);
            var dtFim = DIHelper.Format_StringToDate(dataFinal);

            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"EXEC spc_CVA_Comissoes 'P', 'N', '{dataInicial}', '{dataFinal}'");
            var i = 1;

            while (!oRecordset.EoF)
            {
                var dr = dt.Rows.Add();
                dr["Row"] = i.ToString();
                dr["Prioridade"] = oRecordset.Fields.Item("U_PRIORIDADE").Value.ToString();
                dr["Comissionado"] = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                dr["CardCode"] = oRecordset.Fields.Item("U_CARDCODE").Value.ToString();
                dr["CardName"] = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                dr["Regra"] = oRecordset.Fields.Item("U_REGRA").Value.ToString();
                dr["DocDate"] = oRecordset.Fields.Item("U_DOCDATE").Value.ToString();
                dr["DueDate"] = oRecordset.Fields.Item("U_DUEDATE").Value.ToString();
                dr["ObjType"] = oRecordset.Fields.Item("U_OBJTYPE").Value.ToString();
                dr["DocEntry"] = oRecordset.Fields.Item("U_DOCENTRY").Value.ToString();
                dr["Linha"] = oRecordset.Fields.Item("U_LINENUM").Value.ToString();
                dr["ItemCode"] = oRecordset.Fields.Item("U_ITEMCODE").Value.ToString();
                dr["ItemName"] = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                dr["OcrCode"] = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                dr["InstId"] = oRecordset.Fields.Item("U_PARCELA").Value.ToString();
                dr["Total"] = oRecordset.Fields.Item("U_VALOR").Value.ToString();
                dr["TaxSum"] = oRecordset.Fields.Item("U_IMPOSTOS").Value.ToString();
                dr["PercentComissao"] = oRecordset.Fields.Item("U_COMISSAO").Value.ToString();
                dr["ValorComissao"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                dr["ComissaoPaga"] = oRecordset.Fields.Item("U_PAGO").Value.ToString();
                dr["Momento"] = oRecordset.Fields.Item("U_MOMENTO").Value.ToString();
                dr["Serial"] = oRecordset.Fields.Item("U_SERIAL").Value.ToString();
                if (oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString() != "1900-01-01")
                    dr["DataPagamento"] = oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString();
                if (oRecordset.Fields.Item("U_TAXDATE").Value.ToString() != "1900-01-01")
                    dr["DataRecebimento"] = oRecordset.Fields.Item("U_TAXDATE").Value.ToString();
                i++;
                oRecordset.MoveNext();
            }

            return dt;
        }

        private static DataTable GetOpenInvoices(string dataInicial, string dataFinal)
        {
            var dt = GetInvoicesDataTable();

            var dtIni = DIHelper.Format_StringToDate(dataInicial);
            var dtFim = DIHelper.Format_StringToDate(dataFinal);

            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"EXEC spc_CVA_Comissoes 'N', 'N', '{dataInicial}', '{dataFinal}'");
            var i = 1;

            while (!oRecordset.EoF)
            {
                var dr = dt.Rows.Add();
                dr["Row"] = i.ToString();
                dr["Prioridade"] = oRecordset.Fields.Item("U_PRIORIDADE").Value.ToString();
                dr["Comissionado"] = oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString();
                dr["CardCode"] = oRecordset.Fields.Item("U_CARDCODE").Value.ToString();
                dr["CardName"] = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                dr["Regra"] = oRecordset.Fields.Item("U_REGRA").Value.ToString();
                dr["DocDate"] = oRecordset.Fields.Item("U_DOCDATE").Value.ToString();
                dr["DueDate"] = oRecordset.Fields.Item("U_DUEDATE").Value.ToString();
                dr["ObjType"] = oRecordset.Fields.Item("U_OBJTYPE").Value.ToString();
                dr["DocEntry"] = oRecordset.Fields.Item("U_DOCENTRY").Value.ToString();
                dr["Linha"] = oRecordset.Fields.Item("U_LINENUM").Value.ToString();
                dr["ItemCode"] = oRecordset.Fields.Item("U_ITEMCODE").Value.ToString();
                dr["ItemName"] = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                dr["OcrCode"] = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                dr["InstId"] = oRecordset.Fields.Item("U_PARCELA").Value.ToString();
                dr["Total"] = oRecordset.Fields.Item("U_VALOR").Value.ToString();
                dr["TaxSum"] = oRecordset.Fields.Item("U_IMPOSTOS").Value.ToString();
                dr["PercentComissao"] = oRecordset.Fields.Item("U_COMISSAO").Value.ToString();
                dr["ValorComissao"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                dr["ComissaoPaga"] = oRecordset.Fields.Item("U_PAGO").Value.ToString();
                dr["Momento"] = oRecordset.Fields.Item("U_MOMENTO").Value.ToString();
                dr["Serial"] = oRecordset.Fields.Item("U_SERIAL").Value.ToString();
                if (oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString() != "1900-01-01")
                    dr["DataPagamento"] = oRecordset.Fields.Item("U_DATAPAGAMENTO").Value.ToString();
                if (oRecordset.Fields.Item("U_TAXDATE").Value.ToString() != "1900-01-01")
                    dr["DataRecebimento"] = oRecordset.Fields.Item("U_TAXDATE").Value.ToString();
                i++;
                oRecordset.MoveNext();
            }

            return dt;
        }

        private static DataTable GetInvoicesDataTable()
        {
            var dt = new DataTable("INVOICES");
            dt.Columns.Add("Row", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("Prioridade", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("Comissionado", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("CardCode", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("CardName", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("Regra", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("DocDate", typeof(DateTime)).AllowDBNull = true;
            dt.Columns.Add("DueDate", typeof(DateTime)).AllowDBNull = true;
            dt.Columns.Add("ObjType", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("DocEntry", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("Linha", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("ItemCode", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("ItemName", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("OcrCode", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("InstId", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("Total", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("TaxSum", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("PercentComissao", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("ValorComissao", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("ComissaoPaga", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("DataPagamento", typeof(DateTime)).AllowDBNull = true;
            dt.Columns.Add("Momento", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("Serial", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("DataRecebimento", typeof(DateTime)).AllowDBNull = true;
            return dt;
        }      
    }
}
