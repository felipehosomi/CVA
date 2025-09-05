using System;
using SAPbobsCOM;
using System.Data;
using CVA.View.Comissionamento.Helpers;
using CVA.View.Comissionamento.Models;
using System.IO;

namespace CVA.View.Comissionamento.Controllers
{
    public class ComissoesController
    {
        private readonly SapFactory Factory;
        private readonly DIHelper Helper;

        public ComissoesController(SapFactory factory, DIHelper helper)
        {
            Factory = factory;
            Helper = helper;
        }

        public string SetInvoices(CalculoComissaoFiltroModel filtroModel)
        {
            var ret = string.Empty;

            var rstQuery = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var rstSave = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            var code = Helper.GetNextCode("CVA_CALC_COMISSAO");
            var tableDocEntry = Helper.GetNextDocEntry("CVA_CALC_COMISSAO");
            string sql = $"EXEC spc_CVA_Comissoes 'N', 'T', {filtroModel.Filial}, '{filtroModel.DataComissaoInicial}', '{filtroModel.DataComissaoFinal}', '{filtroModel.DataMetaInicial}', '{filtroModel.DataMetaFinal}', {filtroModel.DiasUteis}, {filtroModel.Feriados}";
            rstQuery.DoQuery(sql);
            int i = 1;

            if (rstQuery.RecordCount > 0)
            {
                sql = String.Empty;

                while (!rstQuery.EoF)
                {
                    try
                    {
                        var prioridade = rstQuery.Fields.Item("U_PRIORIDADE").Value.ToString();
                        var filialRet = rstQuery.Fields.Item("U_BPLID").Value.ToString();
                        var comissionado = rstQuery.Fields.Item("U_COMISSIONADO").Value.ToString();
                        var comissionadoNome = rstQuery.Fields.Item("U_COMNAME").Value.ToString().Replace("'", "");
                        var cardCode = rstQuery.Fields.Item("U_CARDCODE").Value.ToString();
                        var cardName = rstQuery.Fields.Item("U_CARDNAME").Value.ToString().Replace("'", "");
                        var regra = rstQuery.Fields.Item("U_REGRA").Value.ToString();
                        var docDate = (DateTime)rstQuery.Fields.Item("U_DOCDATE").Value;
                        var dueDate = (DateTime)rstQuery.Fields.Item("U_DUEDATE").Value;
                        var objType = rstQuery.Fields.Item("U_OBJTYPE").Value.ToString();
                        var docEntry = rstQuery.Fields.Item("U_DOCENTRY").Value.ToString();
                        var linha = rstQuery.Fields.Item("U_LINENUM").Value.ToString();
                        var itemCode = rstQuery.Fields.Item("U_ITEMCODE").Value.ToString();
                        var itemName = rstQuery.Fields.Item("U_ITEMNAME").Value.ToString().Replace("'", "");
                        var ocrCode = rstQuery.Fields.Item("U_CENTROCUSTO").Value.ToString();
                        var instId = rstQuery.Fields.Item("U_PARCELA").Value.ToString();
                        var total = rstQuery.Fields.Item("U_VALOR").Value.ToString();
                        var taxSum = rstQuery.Fields.Item("U_IMPOSTOS").Value.ToString();
                        var meta = rstQuery.Fields.Item("U_META").Value.ToString();
                        var totalVendas = rstQuery.Fields.Item("U_TOTALVENDAS").Value.ToString();
                        var percentMeta = rstQuery.Fields.Item("U_PORCMETA").Value.ToString();
                        var percentComissao = rstQuery.Fields.Item("U_COMISSAO").Value.ToString();
                        var percentComissaoEquip = rstQuery.Fields.Item("U_COMISSAOEQUIP").Value.ToString();
                        var valorComissao = rstQuery.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                        var valorComissaoEquip = rstQuery.Fields.Item("U_VALORCOMEQUIP").Value.ToString();
                        //var dsr = rstQuery.Fields.Item("U_DSR").Value.ToString();
                        var valorComissaoTotal = rstQuery.Fields.Item("U_VALORCOMTOTAL").Value.ToString();
                        var comissaoPaga = rstQuery.Fields.Item("U_PAGO").Value.ToString();
                        var dataPagamento = (DateTime)rstQuery.Fields.Item("U_DATAPAGAMENTO").Value;
                        var dataRecebimento = (DateTime)rstQuery.Fields.Item("U_TAXDATE").Value;
                        int indEquipe = (int)rstQuery.Fields.Item("U_INDEQUIPE").Value;

                        sql = GetUpsertSql(code.ToString(), tableDocEntry.ToString(), prioridade, filialRet, comissionado, comissionadoNome, cardCode, cardName, regra, docDate, dueDate, objType, docEntry, linha, itemCode, itemName, ocrCode, instId, total, taxSum, meta, totalVendas, percentMeta, percentComissao, percentComissaoEquip, valorComissao, valorComissaoEquip, filtroModel.DiasUteis.ToString(), filtroModel.Feriados.ToString(), valorComissaoTotal, comissaoPaga, dataPagamento, dataRecebimento, indEquipe);
                        rstSave.DoQuery(sql);
                        int rowCount = (int)rstSave.Fields.Item(0).Value;
                        if (rowCount == 0)
                        {
                            code++;
                            tableDocEntry++;
                        }
                        i++;
                        rstQuery.MoveNext();
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = new StreamWriter("c:\\temp\\comissao.sql");
                        sw.WriteLine(sql);
                        sw.Close();
                        Factory.Application.SetStatusBarMessage(ex.Message);
                        break;
                    }
                }

                ret = "Documentos enviados para pagamento.";
            }

            return ret;
        }

        public DataTable GetResumido(string dataInicial, string dataFinal, string vendedor)
        {
            var dt = new DataTable("RESUMIDO");
            var slpCode = string.IsNullOrEmpty(vendedor) ? "0" : vendedor;

            dt.Columns.Add("U_COMISSIONADO", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("U_SLPNAME", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("U_VALORCOMISSAO", typeof(double)).AllowDBNull = true;

            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"exec spc_CVA_PagarComissao 'N', {slpCode}, '{dataInicial}', '{dataFinal}'");

            if (oRecordset.RecordCount > 0)
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

        public DataTable GetDetalhado(string dataInicial, string dataFinal, string vendedor)
        {
            var dt = GetInvoicesDataTable();
            var slpCode = string.IsNullOrEmpty(vendedor) ? "0" : vendedor;

            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
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
                    dr["PercentComissaoEquip"] = oRecordset.Fields.Item("U_COMISSAOEQUIP").Value.ToString();
                    dr["ValorComissao"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                    dr["ValorComissaoEquip"] = oRecordset.Fields.Item("U_VALORCOMEQUIP").Value.ToString();
                    //dr["DSR"] = oRecordset.Fields.Item("U_DSR").Value.ToString();
                    dr["ValorComissaoTotal"] = oRecordset.Fields.Item("U_VALORCOMTOTAL").Value.ToString();
                    dr["Momento"] = oRecordset.Fields.Item("U_MOMENTO").Value.ToString();
                    i++;
                    oRecordset.MoveNext();
                }
            }

            return dt;
        }

        public void SetInvoices(string vendedor, string dataPagamento)
        {
            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"select Code from [@CVA_CALC_COMISSAO] where U_COMISSIONADO = {vendedor} and U_PAGO = 'N'");
            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var oCompanyService = Factory.Company.GetCompanyService();
                    var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");

                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());
                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_PAGO", "Y");
                    oGeneralData.SetProperty("U_DATAPAGAMENTO", Helper.Format_StringToDate(dataPagamento));
                    oGeneralService.Update(oGeneralData);
                    oRecordset.MoveNext();
                }
            }
        }

        public void SetInvoices(string docEntry, string objType, string lineNum, string dataPagamento)
        {
            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"select Code from [@CVA_CALC_COMISSAO] where U_DOCENTRY = {docEntry} and U_OBJTYPE = {objType} and U_LINENUM = {lineNum} and U_PAGO = 'N'");
            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var oCompanyService = Factory.Company.GetCompanyService();
                    var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");

                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());
                    var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_PAGO", "Y");
                    oGeneralData.SetProperty("U_DATAPAGAMENTO", Helper.Format_StringToDate(dataPagamento));
                    oGeneralService.Update(oGeneralData);
                    oRecordset.MoveNext();
                }
            }
        }

        public void RemoveCalculo(string docEntry, string objType, string lineNum, string parcela)
        {
            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"DELETE from [@CVA_CALC_COMISSAO] where U_DOCENTRY = {docEntry} and U_OBJTYPE = {objType} and U_LINENUM = {lineNum} and U_PARCELA = {parcela}");
        }

        public void RemovePagamentoDetalhado(string docEntry, string objType, string lineNum, string parcela)
        {
            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"select Code from [@CVA_CALC_COMISSAO] where U_DOCENTRY = {docEntry} and U_OBJTYPE = {objType} and U_LINENUM = {lineNum} and U_PARCELA = {parcela}");
            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var oCompanyService = Factory.Company.GetCompanyService();
                    var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");

                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());
                    oGeneralService.Delete(oGeneralParams);
                    oRecordset.MoveNext();
                }
            }
        }

        public void RemovePagamentoResumido(string comissionado)
        {
            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"select Code from [@CVA_CALC_COMISSAO] where U_COMISSIONADO = {comissionado}");
            if (oRecordset.RecordCount > 0)
            {
                while (!oRecordset.EoF)
                {
                    var oCompanyService = Factory.Company.GetCompanyService();
                    var oGeneralService = oCompanyService.GetGeneralService("UDOCALC");

                    var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                    oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());
                    oGeneralService.Delete(oGeneralParams);
                    oRecordset.MoveNext();
                }
            }
        }

        private string GetUpsertSql(string code, string tableDocEntry, string prioridade, string filial, string comissionado, string comName, string cardCode, string cardName, string regra, DateTime docDate, DateTime dueDate, string objType, string docEntry, string linha, string itemCode, string itemName, string ocrCode, string instId, string total, string taxSum, string meta, string totalVendas, string percentMeta, string percentComissao, string percentComissaoEquip, string valorComissao, string valorComissaoEquip, string diasUteis, string feriados, string valorComissaoTotal, string comissaoPaga, DateTime dataPagamento, DateTime dataRecebimento, int indEquipe)
        {
            string sql =
            $@" DECLARE @ROWCOUNT INT

            UPDATE [@CVA_CALC_COMISSAO]
            SET		U_BPLID			= {filial},		
		            U_CARDCODE		= '{cardCode}',	
		            U_CARDNAME		= '{cardName}',	
		            U_REGRA			= '{regra}',		
		            U_DOCDATE		= '{docDate.ToString("yyyy-MM-dd")}',
		            U_DUEDATE		= '{dueDate.ToString("yyyy-MM-dd")}',
		            U_DOCENTRY		= {docEntry},		
		            U_OBJTYPE		= {objType},
		            U_ITEMCODE		= '{itemCode}',
		            U_ITEMNAME		= '{itemName}',		
		            U_LINENUM		= {linha},
		            U_VALOR			= {total.Replace(".", "").Replace(",", ".")},		
		            U_PARCELA		= {instId},		
		            U_IMPOSTOS		= {taxSum.Replace(".", "").Replace(",", ".")},		
		            U_COMISSAO		= {percentComissao.Replace(".", "").Replace(",", ".")},
		            U_VALORCOMISSAO	= {valorComissao.Replace(".", "").Replace(",", ".")},
		            U_CENTROCUSTO	= '{ocrCode}',	
		            U_TAXDATE		= '{dataRecebimento.ToString("yyyy-MM-dd")}',		
		            U_META			= {meta.Replace(".", "").Replace(",", ".")},			
		            U_PORCMETA		= {percentMeta.Replace(".", "").Replace(",", ".")},		
		            U_COMNAME		= '{comName}',	
		            U_COMISSAOEQUIP	= {percentComissaoEquip.Replace(".", "").Replace(",", ".")},
		            U_VALORCOMEQUIP	= {valorComissaoEquip.Replace(".", "").Replace(",", ".")},
		            U_DIASUTEIS		= {diasUteis},
                    U_FERIADOS		= {feriados},
		            U_VALORCOMTOTAL = {valorComissaoTotal.Replace(".", "").Replace(",", ".")},
		            U_TOTALVENDAS	= {totalVendas.Replace(".", "").Replace(",", ".")},
                    U_INDEQUIPE	    = {indEquipe}
            WHERE U_DOCENTRY = {docEntry} AND U_OBJTYPE = {objType} AND U_LINENUM = {linha} AND U_PARCELA = {instId} AND U_COMISSIONADO	= {comissionado}

			SET @ROWCOUNT = @@ROWCOUNT

            IF @ROWCOUNT = 0
            BEGIN
	            INSERT INTO [@CVA_CALC_COMISSAO]
	            (
		            Code,				-- 0
		            Name,				-- 1
		            DocEntry,			-- 2
		            Canceled,			-- 3
		            Object,				-- 4
		            LogInst,			-- 5
		            UserSign,			-- 6
		            Transfered,			-- 7
		            CreateDate,			-- 8
		            CreateTime,			-- 9
		            UpdateDate,			-- 10
		            UpdateTime,			-- 11
		            DataSource,			-- 12
		            U_BPLID,			-- 13
		            U_COMISSIONADO,		-- 14
		            U_CARDCODE,			-- 15
		            U_CARDNAME,			-- 16
		            U_REGRA,			-- 17
		            U_DOCDATE,			-- 18
		            U_DUEDATE,			-- 19
		            U_DOCENTRY,			-- 20
		            U_OBJTYPE,			-- 21
		            U_ITEMCODE,			-- 22
		            U_ITEMNAME,			-- 23
		            U_LINENUM,			-- 24
		            U_VALOR,			-- 25
		            U_PARCELA,			-- 26
		            U_IMPOSTOS,			-- 27
		            U_COMISSAO,			-- 28
		            U_VALORCOMISSAO,	-- 29
		            U_CENTROCUSTO,		-- 30
		            U_PAGO,				-- 31
		            U_DATAPAGAMENTO,	-- 32
		            U_TAXDATE,			-- 33
		            U_META,				-- 34
		            U_PORCMETA,			-- 35
		            U_COMNAME,			-- 36
		            U_COMISSAOEQUIP,	-- 37
		            U_VALORCOMEQUIP,	-- 38	
		            U_DIASUTEIS,		-- 39
                    U_FERIADOS,		    -- 40
		            U_VALORCOMTOTAL,	-- 41
		            U_TOTALVENDAS,		-- 42
                    U_INDEQUIPE         -- 43
	            )
	            VALUES
	            (
		            '{code}',												-- 0
		            '{code}',												-- 1
		            {tableDocEntry},										-- 2
		            'N',													-- 3
		            'UDOCALC',												-- 4
		            NULL,													-- 5
		            {Factory.Company.UserSignature},						-- 6
		            'N',													-- 7
		            '{DateTime.Today.ToString("yyyy-MM-dd")}',				-- 8
		            '{DateTime.Now.ToString("HHmm")}',						-- 9
		            NULL,													-- 10
		            NULL,													-- 11
		            'O',													-- 12
		            {filial},												-- 13
		            {comissionado},											-- 14
		            '{cardCode}',											-- 15
		            '{cardName}',											-- 16
		            '{regra}',												-- 17
		            '{docDate.ToString("yyyy-MM-dd")}',						-- 18
		            '{dueDate.ToString("yyyy-MM-dd")}',						-- 19
		            {docEntry},												-- 20
		            {objType},												-- 21
		            '{itemCode}',											-- 22
		            '{itemName}',											-- 23
		            {linha},												-- 24
		            {total.Replace(".", "").Replace(",", ".")},				-- 25
		            {instId},												-- 26
		            {taxSum.Replace(".", "").Replace(",", ".")},			-- 27
		            {percentComissao.Replace(".", "").Replace(",", ".")},	-- 28
		            {valorComissao.Replace(".", "").Replace(",", ".")},		-- 29
		            '{ocrCode}',											-- 30
		            '{comissaoPaga}',										-- 31
		            NULL,													-- 32
		            '{dataRecebimento.ToString("yyyy-MM-dd")}',				-- 33
		            {meta.Replace(".", "").Replace(",", ".")},				-- 34
		            {percentMeta.Replace(".", "").Replace(",", ".")},		-- 35
		            '{comName}',											-- 36
		            {percentComissaoEquip.Replace(".", "").Replace(",", ".")},	-- 37
		            {valorComissaoEquip.Replace(".", "").Replace(",", ".")},	-- 38
		            {diasUteis},				                                -- 39
		            {feriados},	                                                -- 40
                    {valorComissaoTotal.Replace(".", "").Replace(",", ".")},	-- 41
		            {totalVendas.Replace(".", "").Replace(",", ".")},			-- 42
                    {indEquipe}                                                 -- 43
	            )
            END

			SELECT @ROWCOUNT";

            return sql;
        }

        public string GetComissoesSQL(CalculoComissaoFiltroModel filtroModel)
        {
            string status = "T";
            string view = "N";

            if (filtroModel.Todas)
            {
                status = "T";
                view = "N";
            }
            else if (filtroModel.Pagas)
            {
                status = "P";
                view = "N";
            }
            else
            {
                status = "N";
                view = "N";
            }

            string sql = $"EXEC spc_CVA_Comissoes '{status}', '{view}', {filtroModel.Filial}, '{filtroModel.DataMetaInicial}', '{filtroModel.DataMetaFinal}', '{filtroModel.DataComissaoInicial}', '{filtroModel.DataComissaoFinal}', {filtroModel.DiasUteis}, {filtroModel.Feriados}";
            return sql;
        }

        public DataTable GetInvoices(string sql)
        {
            var dt = GetInvoicesDataTable();

            var oRecordset = (Recordset)Factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery(sql);
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
                dr["PercentComissaoEquip"] = oRecordset.Fields.Item("U_COMISSAOEQUIP").Value.ToString();
                dr["ValorComissao"] = oRecordset.Fields.Item("U_VALORCOMISSAO").Value.ToString();
                dr["ValorComissaoEquip"] = oRecordset.Fields.Item("U_VALORCOMEQUIP").Value.ToString();
                //dr["DSR"] = oRecordset.Fields.Item("U_DSR").Value.ToString();
                dr["ValorComissaoTotal"] = oRecordset.Fields.Item("U_VALORCOMTOTAL").Value.ToString();
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
            dt.Columns.Add("PercentComissaoEquip", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("ValorComissao", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("ValorComissaoEquip", typeof(double)).AllowDBNull = true;
            //dt.Columns.Add("DSR", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("ValorComissaoTotal", typeof(double)).AllowDBNull = true;
            dt.Columns.Add("ComissaoPaga", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("DataPagamento", typeof(DateTime)).AllowDBNull = true;
            dt.Columns.Add("Momento", typeof(string)).AllowDBNull = true;
            dt.Columns.Add("Serial", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("DataRecebimento", typeof(DateTime)).AllowDBNull = true;

            return dt;
        }
    }
}
