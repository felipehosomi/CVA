using ClosedXML.Excel;
using CVA.Fibra.ConciliacaoCartaCredito.Core.DAO;
using CVA.Fibra.ConciliacaoCartaCredito.Core.Model;
using SAPbobsCOM;
using SBO.Hub;
using SBO.Hub.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Fibra.ConciliacaoCartaCredito.Core.BLL
{
    public class DepositBLL
    {
        public List<ImportLogModel> Import(string file, string account)
        {
            List<ImportLogModel> list = new List<ImportLogModel>();
            SAPbouiCOM.ProgressBar progressBar = null;

            try
            {
                SBOApp.Application.SetStatusBarMessage("Iniciando leitura da planilha, por favor aguarde", SAPbouiCOM.BoMessageTime.bmt_Medium, false);

                var wb = new XLWorkbook(file);
                var ws = wb.Worksheet(1);

                int lastRow = ws.LastRowUsed().RowNumber() + 1;

                progressBar = SBOApp.Application.StatusBar.CreateProgressBar("", lastRow, false);

                CrudDAO crudDAO = new CrudDAO();

                for (int i = 2; i < lastRow; i++)
                {
                    progressBar.Value++;

                    IXLRow row = ws.Row(i);

                    ImportLogModel importLogModel = crudDAO.FillModelFromSql<ImportLogModel>(String.Format(Hana.CreditCard_Get, row.Cell("E").Value));

                    importLogModel.Line = i;
                    importLogModel.NSU = row.Cell("E").Value.ToString();
                    try
                    {
                        importLogModel.Date = Convert.ToDateTime(row.Cell("A").Value);
                    }
                    catch (Exception ex)
                    {
                        importLogModel.Comments = "Erro ao converter data: " + ex.Message;
                    }
                    if (importLogModel.CreditCardId == 0)
                    {
                        importLogModel.Comments = "NSU não encontrado";
                    }
                    if (importLogModel.Deposited == "Y")
                    {
                        importLogModel.Comments = "NSU já depositado";
                    }

                    list.Add(importLogModel);
                }

                progressBar.Maximum = list.Count(m => String.IsNullOrEmpty(m.Comments));
                progressBar.Value = 0;

                IEnumerable <IGrouping<int, ImportLogModel>> groupByBPL = list.Where(m => String.IsNullOrEmpty(m.Comments)).GroupBy(m => m.BPLId);

                foreach (var itemByBPL in groupByBPL)
                {
                    IEnumerable<IGrouping<DateTime, ImportLogModel>> groupByDate = itemByBPL.GroupBy(m => m.Date);
                    foreach (var itemByDate in groupByDate)
                    {

                        CompanyService companyService = SBOApp.Company.GetCompanyService();
                        DepositsService dpService = (DepositsService)companyService.GetBusinessService(ServiceTypes.DepositsService);
                        Deposit dpsAddCreditCard = (Deposit)dpService.GetDataInterface(DepositsServiceDataInterfaces.dsDeposit);
                        DepositParams depositParams = null;
                        CreditLine credit = null;
                        try
                        {
                            dpsAddCreditCard.DepositType = BoDepositTypeEnum.dtCredit;
                            dpsAddCreditCard.ReconcileAfterDeposit = BoYesNoEnum.tNO;
                            dpsAddCreditCard.BankAccountNum = account;
                            dpsAddCreditCard.DepositAccount = account;
                            dpsAddCreditCard.VoucherAccount = account;
                            dpsAddCreditCard.BPLID = itemByBPL.Key;
                            dpsAddCreditCard.DepositDate = itemByDate.Key;

                            foreach (var item in itemByDate)
                            {
                                progressBar.Value++;
                                credit = dpsAddCreditCard.Credits.Add();
                                credit.AbsId = Convert.ToInt32(item.CreditCardId);
                            }


                            depositParams = dpService.AddDeposit(dpsAddCreditCard);
                            foreach (var item in itemByDate)
                            {
                                item.DepositId = depositParams.DepositNumber;
                            }
                        }
                        catch (Exception ex)
                        {
                            foreach (var item in itemByDate)
                            {
                                item.Comments = ex.Message;
                            }
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(companyService);
                            companyService = null;

                            Marshal.ReleaseComObject(dpService);
                            dpService = null;

                            Marshal.ReleaseComObject(dpsAddCreditCard);
                            dpsAddCreditCard = null;

                            if (credit != null)
                            {
                                Marshal.ReleaseComObject(credit);
                                credit = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (progressBar != null)
                {
                    progressBar.Stop();

                    Marshal.ReleaseComObject(progressBar);
                    progressBar = null;
                }
            }

            return list;
        }
    }
}
