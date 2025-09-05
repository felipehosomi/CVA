using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using B1.WFN.API.Infrastructure;
using B1.WFN.API.Infrastructure.BasicAuth;
using B1.WFN.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sap.Data.Hana;

namespace B1.WFN.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CashFlowController : ControllerBase
    {
        private static HanaConnection _HanaConnection;
        public static SqlConnection _SqlConnection;


        public CashFlowController(IOptions<ConnectionStrings> connectionStrings)
        {
            if (!String.IsNullOrEmpty(connectionStrings.Value.Hana))
            {

                _HanaConnection = new HanaConnection(Crypto.Decrypt(connectionStrings.Value.Hana));
            }
            else if (!String.IsNullOrEmpty(connectionStrings.Value.SQLServer))
            {
                _SqlConnection = new SqlConnection(Crypto.Decrypt(connectionStrings.Value.SQLServer));
            }
        }

        // POST: cashflow/header
        [HttpPost("header")]
        [BasicAuth]
        public IActionResult Header([FromBody]CashFlowInput[] input)
        {
            try
            {
                var cashFlowInfo = new CashFlowInfoHeader
                {
                    Headers = new List<Header>()
                };

                foreach (DataRow info in GetCashFlowInfo(input[0].datainicial, input[0].datafinal, ((char)PartType.Header).ToString()).Rows)
                {
                    cashFlowInfo.Headers.Add(new Header
                    {
                        //IntegrationType = info["DocCancel"].ToString() == "Y" ? "e" : null,
                        TransType = info["TransType"].ToString(),
                        DocEntry = info["DocEntry"].ToString(),
                        DocDate = DateTime.Parse(info["DocDate"].ToString()).ToString("yyyy/MM/dd"), 
                        JournalRemarks = info["JrnlMemo"].ToString(),
                        DocType = info["ObjType"].ToString(),
                        Serial = info["Serial"].ToString(),
                        DocCurrency = info["DocCur"].ToString(),
                        InstallmentNum = info["Installmnt"].ToString(),
                        DocTotal = info["DocTotal"].ToString(),
                        BranchId = String.IsNullOrEmpty(info["BPLId"].ToString()) ? "" : info["BPLId"].ToString(),
                        Comments = info["Comments"].ToString(),
                        U_GrupoDespesa = info["U_GrupoDespesa"].ToString(),
                        U_SubGrupoDespesa = info["U_SubGrupoDespesa"].ToString()
                    });
                }

                return Ok(cashFlowInfo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
        }

        // POST: cashflow/financial
        [HttpPost("financial")]
        [BasicAuth]
        public IActionResult FinancialOpening([FromBody]CashFlowInput[] input)
        {
            try
            {
                var cashFlowInfo = new CashFlowInfoFinancialOpening
                {
                    FinancialOpenings = new List<FinancialOpening>()
                };

                foreach (DataRow info in GetCashFlowInfo(input[0].datainicial, input[0].datafinal, ((char)PartType.Financial).ToString()).Rows)
                {
                    cashFlowInfo.FinancialOpenings.Add(new FinancialOpening
                    {
                        IntegrationType = info["PayCancel"].ToString() == "Y" ? "e" : null,
                        InstallmentId = $"{info["DocEntry"]}.{info["InstlmntID"]}",
                        DocEntry = info["DocEntry"].ToString(),
                        InstDueDate = (DateTime.Parse(info["DueDate"].ToString())).ToString("yyyy/MM/dd"),
                        PaymentDate = String.IsNullOrEmpty(info["PayDate"].ToString()) ? "" : DateTime.Parse(info["PayDate"].ToString()).ToString("yyyy/MM/dd"),
                        EventType = info["EventType"].ToString(),
                        Account = String.IsNullOrEmpty(info["PmntAcct"].ToString()) ? "PREV" : info["PmntAcct"].ToString(),
                        CardCode = info["CardCode"].ToString(),
                        CardType = info["CardType"].ToString(),
                        PaymentType = info["PaymentType"].ToString(),
                        Value = decimal.Parse(info["InsTotal"].ToString()),
                        BoeNum = info["BoeNum"].ToString(),
                        PaidSum = info["SumApplied"].ToString()
                    });
                }

                return Ok(cashFlowInfo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
        }

        // POST: cashflow/accounting
        [HttpPost("accounting")]
        [BasicAuth]
        public IActionResult AccountingOpening([FromBody]CashFlowInput[] input)
        {
            try
            {
                var cashFlowInfo = new CashFlowInfoAccountingOpening
                {
                    AccountingOpenings = new List<AccountingOpening>()
                };

                foreach (DataRow info in GetCashFlowInfo(input[0].datainicial, input[0].datafinal, ((char)PartType.Accounting).ToString()).Rows)
                {
                    cashFlowInfo.AccountingOpenings.Add(new AccountingOpening
                    {
                        //IntegrationType = info["JrnlCancel"].ToString() == "Y" ? "e" : null,
                        Line_ID = $"{info["DocEntry"]}.{info["Line_ID"]}",
                        DocEntry = info["DocEntry"].ToString(),
                        EventType = info["EventType"].ToString(),
                        Account = info["Account"].ToString(),
                        CostCenter = info["ProfitCode"].ToString(),
                        Value = decimal.Parse(info["JrnlLineValue"].ToString()),
                        RefDate = DateTime.Parse(info["RefDate"].ToString()).ToString("yyyy/MM/dd"),
                        DueDate = DateTime.Parse(info["DueDate"].ToString()).ToString("yyyy/MM/dd")
                    });
                }

                return Ok(cashFlowInfo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
        }

        private DataTable GetCashFlowInfo(DateTime fromDate, DateTime toDate, string partType)
        {
            try
            {
                if (_HanaConnection != null)
                {
                    // Realiza conexão com o banco de dados
                    _HanaConnection.Open();

                    var command = $@"call SP_GetCashFlowInfo('{fromDate:yyyy-MM-dd HH:mm:ss}', '{toDate:yyyy-MM-dd HH:mm:ss}', '{partType}')";
                    var dataAdapter = new HanaDataAdapter(command, _HanaConnection);
                    var dataTable = new DataTable();

                    dataAdapter.Fill(dataTable);

                    // Fecha a conexão com o banco de dados
                    _HanaConnection.Close();

                    return dataTable;
                }
                else if (_SqlConnection != null)
                {
                    _SqlConnection.Open();

                    var command = $@"exec SP_GetCashFlowInfo '{fromDate:yyyy-MM-dd HH:mm:ss}', '{toDate:yyyy-MM-dd HH:mm:ss}', '{partType}'";
                    var sqlCommand = new SqlCommand(command, _SqlConnection);
                    sqlCommand.CommandTimeout = int.MaxValue;
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                        
                    _SqlConnection.Close();

                    return dataTable;
                }
                else
                {
                    return null;
                }
            }
            catch (HanaException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
