using CVA.Cointer.Core.Model;
using SAPbobsCOM;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CVA.Cointer.Core.BLL
{
    public class ReturnInvoiceBLL
    {
        public string Generate(ReturnInvoiceModel returnInvoiceModel, List<InvoiceItemModel> originalList)
        {
            List<InvoiceItemModel> list;
            if (returnInvoiceModel.GroupItensYN == "Y")
            {
                list = new List<InvoiceItemModel>();
                IEnumerable<IGrouping<string, InvoiceItemModel>> groupedByItem = originalList.GroupBy(m => m.ItemCode + ',' + m.DocEntry);
                foreach (var modelByItem in groupedByItem)
                {
                    InvoiceItemModel model = new InvoiceItemModel();
                    //                    modelByItem.Key.Substring(0, modelByItem.Key.IndexOf(","));
                    model.ItemCode = modelByItem.ElementAt(0).ItemCode;
                    model.Quantity = modelByItem.Sum(m => m.Quantity);
                    model.Warehouse = modelByItem.ElementAt(0).Warehouse;
                    model.Usage = modelByItem.ElementAt(0).Usage;
                    model.TaxCode = modelByItem.ElementAt(0).TaxCode;
                    model.Price = modelByItem.ElementAt(0).Price;
                    model.StockPrice = modelByItem.ElementAt(0).StockPrice;

                    model.BatchList = new List<BatchModel>();
                    model.Batch = "";

                    foreach (var item in modelByItem)
                    {
                        BatchModel batchModel = model.BatchList.FirstOrDefault(m => m.Batch == item.Batch);
                        if (batchModel != null)
                        {
                            batchModel.Quantity += item.Quantity;
                        }
                        else
                        {
                            model.Batch += ", " + item.Batch;
                            model.BatchList.Add(new BatchModel() { Batch = item.Batch, Quantity = item.Quantity });
                        }
                    }
                    if (!String.IsNullOrEmpty(model.Batch))
                    {
                        model.Batch = model.Batch.Substring(2);
                    }

                    list.Add(model);
                }
            }
            else
            {
                list = originalList;
                foreach (var item in list)
                {
                    item.BatchList = new List<BatchModel>();
                    item.BatchList.Add(new BatchModel() { Batch = item.Batch, Quantity = item.Quantity });
                }
            }

            Documents doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oDrafts);
            try
            {
                doc.DocObjectCode = BoObjectTypes.oCreditNotes;
                doc.DocDate = returnInvoiceModel.DocDate;
                doc.DocDueDate = returnInvoiceModel.DueDate;
                doc.TaxDate = returnInvoiceModel.TaxDate;
                doc.CardCode = originalList[0].CardCode;
                doc.TaxExtension.MainUsage = originalList[0].Usage;
                doc.BPL_IDAssignedToInvoice = originalList[0].BPLId;
                string comments = string.Join(",", originalList.Select(m => m.Serial.ToString()).Distinct());
                if (comments.Contains(","))
                {
                    comments = "Baseado em NFs " + comments;
                }
                else
                {
                    comments = "Baseado em NF " + comments;
                }
                doc.Comments = comments;

                foreach (var item in list)
                {
                    if (!String.IsNullOrEmpty(doc.Lines.ItemCode))
                    {
                        doc.Lines.Add();
                    }

                    doc.Lines.ItemCode = item.ItemCode;
                    doc.Lines.Quantity = item.Quantity;
                    doc.Lines.UnitPrice = item.Price;
                    doc.Lines.WarehouseCode = item.Warehouse;
                    doc.Lines.Usage = item.Usage.ToString();
                    doc.Lines.TaxCode = item.TaxCode;
                    doc.Lines.EnableReturnCost = BoYesNoEnum.tYES;
                    doc.Lines.ReturnCost = item.StockPrice;

                    if (!string.IsNullOrEmpty(item.Batch))
                    {
                        foreach (var batch in item.BatchList)
                        {
                            if (!string.IsNullOrEmpty(doc.Lines.BatchNumbers.BatchNumber))
                            {
                                doc.Lines.BatchNumbers.Add();
                            }

                            doc.Lines.BatchNumbers.BatchNumber = batch.Batch;
                            doc.Lines.BatchNumbers.Quantity = batch.Quantity;
                        }
                    }

                }

                if (doc.Add() != 0)
                {
                    return SBOApp.Company.GetLastErrorDescription();
                }
                else
                {
                    int docEntryDraft = Convert.ToInt32(SBOApp.Company.GetNewObjectKey());
                    Marshal.ReleaseComObject(doc);
                    doc = null;
                    doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oDrafts);

                    doc.GetByKey(docEntryDraft);

                    doc.ImportFileNum = docEntryDraft;
                    doc.UserFields.Fields.Item("U_CVA_DocEntryDraft").Value = docEntryDraft;

                    if (doc.Update() != 0)
                    {
                        return "Erro ao atualizar esboço: " + SBOApp.Company.GetLastErrorDescription();
                    }

                    //CrudDAO.ExecuteNonQuery(String.Format(Hana.Draft_UpdateDocEntryUserField, docEntryDraft));

                    CrudDAO crudDAO = new CrudDAO("@CVA_CONSIGNMENT");
                    crudDAO.UserTableType = BoUTBTableType.bott_MasterData;
                    foreach (var item in originalList)
                    {
                        item.DocEntryDraft = docEntryDraft;
                        crudDAO.Model = item;
                        string code = crudDAO.CreateModel();
                    }

                    Form frmDraft = (Form)SBOApp.Application.OpenForm((BoFormObjectEnum)112, "", SBOApp.Company.GetNewObjectKey());
                    frmDraft.Select();

                    try
                    {
                        ComboBox cb_Usage = (ComboBox)frmDraft.Items.Item("1720002171").Specific;
                        cb_Usage.Select(originalList[0].Usage.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Erro criar esboço: " + SBOApp.Company.GetLastErrorDescription();
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
                doc = null;
            }
            return "";
        }
    }
}
