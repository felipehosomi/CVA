using CVA.NF.Import.MODEL;
using CVA.NF.Import.DAO.Resources;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace CVA.NF.Import.DAO
{
    public class ImpInvoiceDAO
    {
        public void Import(string folder)
        {
            string[] fileList = Directory.GetFiles(folder, "*.xlsx");
            string dirSuccess = Path.Combine(folder, "Sucesso");
            string dirGeneralError = Path.Combine(folder, "Erro Geral");
            string dirImportedWithError = Path.Combine(folder, "Processado com Erros");

            if (!Directory.Exists(dirSuccess))
            {
                Directory.CreateDirectory(dirSuccess);
            }
            if (!Directory.Exists(dirGeneralError))
            {
                Directory.CreateDirectory(dirGeneralError);
            }
            if (!Directory.Exists(dirImportedWithError))
            {
                Directory.CreateDirectory(dirImportedWithError);
            }

            foreach (var file in fileList)
            {
                StreamWriter sw = new StreamWriter("c:\\CVA Consultoria\\Importação\\Log\\Arquivo.txt", true);
                sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Iniciando importação");
                sw.Close();
                sw = new StreamWriter("c:\\CVA Consultoria\\Importação\\Log\\Arquivo.txt", true);

                string fileName = Path.GetFileName(file);
                string pathTo = String.Empty;

                ImportResultEnum result = ImportFile(file);
                switch (result)
                {
                    case ImportResultEnum.Success:
                        pathTo = Path.Combine(dirSuccess, fileName);
                        break;
                    case ImportResultEnum.GeneralError:
                        pathTo = Path.Combine(dirGeneralError, fileName);
                        break;
                    case ImportResultEnum.ImportedWithError:
                        pathTo = Path.Combine(dirImportedWithError, fileName);
                        break;
                }
                sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Importação finalizada");
                sw.Close();
                sw = new StreamWriter("c:\\CVA Consultoria\\Importação\\Log\\Arquivo.txt", true);

                if (File.Exists(pathTo))
                {
                    try
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Deletando: {pathTo}");
                        File.Delete(pathTo);
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Erro ao deletar {pathTo}: {ex.Message}");
                    }
                }
                try
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Movendo: {file} para {pathTo}");
                    Directory.Move(file, pathTo);
                }
                catch (Exception ex)
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Erro ao mover: {ex.Message}");
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception exe)
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Erro ao deletar {file}: {exe.Message}");
                    }
                }

                sw.WriteLine();
                sw.Close();
            }
        }

        public ImportResultEnum ImportFile(string filePath)
        {
            PortalEDM edm = new PortalEDM();

            ExcelFileDAO excelFileDao = new ExcelFileDAO();
            List<ImportParamModel> paramList = edm.CVA_PAR_IMP.ToList();

            var msg = string.Empty;

            Recordset rst = null;
            Documents invoice = null;

            var errorsCount = 0;
            ImportLogModel logModel;
            var fileName = Path.GetFileName(filePath);
            var importedLineCount = 0;
            // Começa no 1 para desconsiderar o header
            int line = 1;

            logModel = new ImportLogModel();
            logModel.INS = DateTime.Now;
            logModel.LINE = line;
            logModel.FILE = fileName;
            logModel.DSCR = "Importação iniciada!";
            logModel.TYPE = "Início";
            edm.CVA_LOG_IMP.Add(logModel);
            edm.SaveChanges();

            try
            {
                XLWorkbook wb = new XLWorkbook(filePath);
                IXLWorksheet ws = wb.Worksheet(1);

                //var firstCell = ws.FirstCellUsed();
                //var lastCell = ws.LastCellUsed();
                //var range = ws.Range(firstCell.Address, lastCell.Address);
                //Create a new DataTable.
                DataTable dtbExcel = new DataTable();

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in ws.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dtbExcel.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dtbExcel.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dtbExcel.Rows[dtbExcel.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                //var dtbExcel = excelFileDao.ReadFile(filePath, ConfigurationManager.AppSettings["InvoiceSheet"]);

                foreach (DataRow dataRow in dtbExcel.Rows)
                {
                    line++;
                    logModel = new ImportLogModel();
                    logModel.INS = DateTime.Now;
                    logModel.LINE = line;
                    logModel.FILE = fileName;

                    logModel.PER = dataRow["Competência"].ToString();
                    logModel.CNPJ = dataRow["CNPJ COMPRADOR"].ToString();
                    logModel.CNTR = dataRow["CONTRATO"].ToString();
                    logModel.REV = 0;
                    if (!string.IsNullOrEmpty(dataRow["Receita Fixa Unitária (R$/MWh)"].ToString()))
                        logModel.REV = Convert.ToDouble(dataRow["Receita Fixa Unitária (R$/MWh)"].ToString());

                    var existingModel = edm.CVA_LOG_IMP.FirstOrDefault(l =>
                                 l.TYPE == "Sucesso" &&
                                 l.CNPJ == logModel.CNPJ &&
                                 l.PER == logModel.PER &&
                                 l.CNTR == logModel.CNTR &&
                                 l.REV == logModel.REV);

                    if (existingModel != null)
                    {
                        continue;
                    }

                    var paramModel = paramList.FirstOrDefault(p => p.CNPJ == dataRow["CNPJ VENDEDOR"].ToString());
                    if (paramModel != null)
                    {
                        if (!SBOConnectionDao.Companies.ContainsKey(paramModel.BASE_ID))
                        {
                            var connMsg = SBOConnectionDao.ConnectToCompany(paramModel.BASE_ID, paramModel.BASE_NAME);
                            if (!string.IsNullOrEmpty(connMsg))
                            {
                                logModel.DSCR = "Erro ao conectar no banco de dados: " + connMsg;
                                logModel.TYPE = "Erro";
                                edm.CVA_LOG_IMP.Add(logModel);
                                edm.SaveChanges();
                                errorsCount++;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        logModel.DSCR = "CNPJ vendedor não parametrizado: " + dataRow["CNPJ VENDEDOR"];
                        logModel.TYPE = "Erro";
                        edm.CVA_LOG_IMP.Add(logModel);
                        edm.SaveChanges();
                        errorsCount++;
                        continue;
                    }

                    importedLineCount++;
                    try
                    {
                        if (SBOConnectionDao.Companies[paramModel.BASE_ID] != null)
                        {
                            if (SBOConnectionDao.Companies[paramModel.BASE_ID].Connected)
                            {
                                invoice = (Documents)SBOConnectionDao.Companies[paramModel.BASE_ID].GetBusinessObject(BoObjectTypes.oInvoices);
                                rst = (Recordset)SBOConnectionDao.Companies[paramModel.BASE_ID].GetBusinessObject(BoObjectTypes.BoRecordset);
                                rst.DoQuery(string.Format(Query.BusinessPartner_GetByCnpj, dataRow["CNPJ COMPRADOR"]));

                                invoice.CardCode = rst.Fields.Item("CardCode").Value.ToString();
                                invoice.BPL_IDAssignedToInvoice = paramModel.BPLID;
                                invoice.UserFields.Fields.Item("U_CVA_Integracao").Value = "Y";
                                invoice.TaxExtension.Incoterms = "9";
                                rst.DoQuery(String.Format(Query.BusinessPartnerContact_GetEmail, invoice.CardCode));
                                string email = String.Empty;
                                while (!rst.EoF)
                                {
                                    email += $";{rst.Fields.Item(0).Value}";
                                    rst.MoveNext();
                                }
                                if (!String.IsNullOrEmpty(email))
                                {
                                    email = email.Substring(1);
                                    invoice.UserFields.Fields.Item("U_EmailEnvDanfe").Value = email;
                                }

                                invoice.Project = paramModel.PROJECT;
                                invoice.SequenceCode = paramModel.NFSEQ;
                                invoice.OpeningRemarks = paramModel.REMARKS;
                                invoice.OpeningRemarks += Environment.NewLine;
                                invoice.OpeningRemarks += $"Referente Contrato: Competência: {dataRow["Competência"].ToString()} - {dataRow["Contrato"].ToString()}";

                                invoice.Lines.ItemCode = paramModel.ITEMCODE;
                                invoice.Lines.Usage = paramModel.USAGE.ToString();
                                invoice.Lines.CostingCode = paramModel.COSTCENTER;
                                if (paramModel.ALIQIPI.HasValue)
                                {
                                    invoice.Lines.UserFields.Fields.Item("U_SX_AliqIPI").Value = Convert.ToInt32(paramModel.ALIQIPI);
                                }
                                invoice.Lines.UserFields.Fields.Item("U_SX_Aplicacao").Value = paramModel.APLIC;

                                double? doubleValue = dataRow["Quantidade de Energia Contratada (MWh)"] as double?;
                                if (!doubleValue.HasValue)
                                {
                                    try
                                    {
                                        doubleValue = Convert.ToDouble(dataRow["Quantidade de Energia Contratada (MWh)"]);
                                    }
                                    catch { }
                                }

                                if (doubleValue.HasValue)
                                    invoice.Lines.Quantity = doubleValue.Value;

                                doubleValue = dataRow["Receita Fixa Unitária (R$/MWh)"] as double?;
                                if (!doubleValue.HasValue)
                                {
                                    try
                                    {
                                        doubleValue = Convert.ToDouble(dataRow["Receita Fixa Unitária (R$/MWh)"]);
                                    }
                                    catch { }
                                }

                                if (doubleValue.HasValue)
                                    invoice.Lines.Price = doubleValue.Value;

                                doubleValue = dataRow["Receita Fixa (R$)"] as double?;
                                if (!doubleValue.HasValue)
                                {
                                    try
                                    {
                                        doubleValue = Convert.ToDouble(dataRow["Receita Fixa (R$)"]);
                                    }
                                    catch { }
                                }

                                if (doubleValue.HasValue)
                                    invoice.Lines.LineTotal = doubleValue.Value;

                                DateTime validDate;

                                DateTime? date1 = null;
                                DateTime? date2 = null;
                                DateTime? date3 = null;

                                if (DateTime.TryParse(dataRow["Vencimento1"].ToString(), out validDate))
                                {
                                    date1 = validDate;
                                }
                                if (DateTime.TryParse(dataRow["Vencimento2"].ToString(), out validDate))
                                {
                                    date2 = validDate;
                                }
                                if (DateTime.TryParse(dataRow["Vencimento3"].ToString(), out validDate))
                                {
                                    date3 = validDate;
                                }

                                double percentage = 100;
                                if (date1.HasValue && date2.HasValue && date3.HasValue)
                                    percentage = 33.33;
                                else if (date1.HasValue && date2.HasValue)
                                    percentage = 50;

                                if (date1.HasValue)
                                {
                                    invoice.Installments.DueDate = date1.Value;
                                    invoice.Installments.Percentage = percentage == 33.33 ? percentage + 0.01 : percentage;
                                    //invoice.Installments.Total = doubleValue.Value * (percentage / 100);
                                }

                                if (date2.HasValue)
                                {
                                    invoice.Installments.Add();
                                    invoice.Installments.DueDate = date2.Value;
                                    invoice.Installments.Percentage = percentage;
                                    //invoice.Installments.Total = doubleValue.Value * (percentage / 100);
                                }

                                if (date3.HasValue)
                                {
                                    invoice.Installments.Add();
                                    invoice.Installments.DueDate = date3.Value;
                                    invoice.Installments.Percentage = percentage;
                                    //invoice.Installments.Total = doubleValue.Value * (percentage / 100);
                                }

                                if (invoice.Add() != 0)
                                {
                                    logModel.DSCR = SBOConnectionDao.Companies[paramModel.BASE_ID].GetLastErrorDescription();
                                    logModel.TYPE = "Erro";
                                    edm.CVA_LOG_IMP.Add(logModel);
                                    edm.SaveChanges();
                                    errorsCount++;

                                    //logModel = new ImportLogModel();
                                    //logModel.INS = DateTime.Now;
                                    //logModel.DSCR = "Erro de negócio detectado, importação finalizada";
                                    //logModel.TYPE = "Erro";
                                    //logModel.LINE = line;
                                    //logModel.FILE = fileName;
                                    //PortalEDM.CVA_LOG_IMP.Add(logModel);

                                    //return "Erro de negócio detectado, importação finalizada";
                                }
                                else
                                {
                                    logModel.DSCR = "Importado com sucesso!";
                                    logModel.TYPE = "Sucesso";
                                    edm.CVA_LOG_IMP.Add(logModel);
                                    edm.SaveChanges();
                                }
                                Marshal.ReleaseComObject(invoice);
                                invoice = null;

                                Marshal.ReleaseComObject(rst);
                                rst = null;
                            }
                            else
                            {
                                logModel.DSCR = "DI Company não conectada";
                                logModel.TYPE = "Erro";
                                edm.CVA_LOG_IMP.Add(logModel);
                                edm.SaveChanges();
                                errorsCount++;
                                continue;
                            }
                        }
                        else
                        {
                            logModel.DSCR = "DI Company não instanciada";
                            logModel.TYPE = "Erro";
                            edm.CVA_LOG_IMP.Add(logModel);
                            edm.SaveChanges();
                            errorsCount++;
                            continue;
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var rs = "";
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            rs =
                                string.Format(
                                    "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            Console.WriteLine(rs);

                            foreach (var ve in eve.ValidationErrors)
                                logModel.DSCR = "Erro geral: " + ve.PropertyName + " - " + ve.ErrorMessage;
                        }

                        logModel.TYPE = "Erro";
                        edm.CVA_LOG_IMP.Add(logModel);
                        edm.SaveChanges();
                        errorsCount++;
                    }
                    catch (Exception ex)
                    {
                        logModel.DSCR = "Erro geral: " + ex.Message;
                        logModel.TYPE = "Erro";
                        edm.CVA_LOG_IMP.Add(logModel);
                        edm.SaveChanges();
                        errorsCount++;
                    }
                }

                logModel = new ImportLogModel();
                logModel.INS = DateTime.Now;
                logModel.LINE = 0;
                logModel.FILE = fileName;
                if (errorsCount == 0)
                {
                    logModel.DSCR = $"Importação finalizada: {importedLineCount} linhas importadas com sucesso!";
                    logModel.TYPE = "Sucesso";
                }
                else
                {
                    logModel.DSCR =
                        $"Importação finalizada: {importedLineCount - errorsCount} linhas importadas com sucesso. {errorsCount} linhas com erro.";
                    logModel.TYPE = "Alerta";
                }
                edm.CVA_LOG_IMP.Add(logModel);
                edm.SaveChanges();
                if (errorsCount == 0)
                {
                    return ImportResultEnum.Success;
                }
                else
                {
                    return ImportResultEnum.ImportedWithError;
                }
            }
            catch (Exception ex)
            {
                line = -1;
                logModel = new ImportLogModel();
                logModel.INS = DateTime.Now;
                logModel.DSCR = "Erro geral: " + ex.Message;
                logModel.TYPE = "Erro";
                logModel.LINE = line;
                logModel.FILE = fileName;
                edm.CVA_LOG_IMP.Add(logModel);
                edm.SaveChanges();

                msg = "Erro geral: " + ex.Message;
                return ImportResultEnum.GeneralError;
            }
            finally
            {
                edm.Dispose();
                line = 0;
                //foreach (var item in SBOConnectionDao.Companies)
                //{
                //    try
                //    {
                //        Marshal.ReleaseComObject(item.Value);
                //        SBOConnectionDao.Companies.Remove(item.Key);
                //        GC.Collect();
                //    }
                //    catch { }
                //}
            }
        }
    }
}
