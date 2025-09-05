using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using CVA.AddOn.Hub.Logic.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class FreteBLL
    {
        public string ExecutaRateio(ref Documents doc, List<DocumentItemModel> itemList)
        {
            string msg = String.Empty;

            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            string itemCode = doc.Lines.ItemCode;
            string usage = String.Empty;

            IEnumerable<IGrouping<string, DocumentItemModel>> groupedByOcrCode = itemList.GroupBy(i => i.OcrCode2);
            List<int> lineSet = new List<int>();

            foreach (var itemByOcrCode in groupedByOcrCode)
            {
                double lineTotal = itemByOcrCode.Sum(i => i.LineTotal);
                double vatSum = itemByOcrCode.Sum(i => i.VatSum);

                bool existeCentroCusto = false;
                for (int i = 0; i < doc.Lines.Count; i++)
                {
                    if (lineSet.Contains(i))
                    {
                        continue;
                    }
                    doc.Lines.SetCurrentLine(i);
                    if (i == 0)
                    {
                        usage = doc.Lines.Usage;
                    }
                    if (i == 0)
                    {
                        lineSet.Add(i);
                        existeCentroCusto = true;
                        double price =  Math.Round(doc.DocTotal * (lineTotal + vatSum) / itemList.Sum(m => m.LineTotal + m.VatSum), 2);
                        doc.Lines.UnitPrice = price;
                        doc.Lines.CostingCode2 = itemByOcrCode.Key;
                        break;
                    }
                    if (doc.Lines.CostingCode2 == itemByOcrCode.Key)
                    {
                        lineSet.Add(i);
                        existeCentroCusto = true;
                        double price = Math.Round(doc.Lines.UnitPrice + (doc.DocTotal * (lineTotal + vatSum) / itemList.Sum(m => m.LineTotal + m.VatSum)), 2);
                        doc.Lines.UnitPrice = price;
                        break;
                    }
                }

                if (!existeCentroCusto)
                {
                    doc.Lines.Add();
                    doc.Lines.ItemCode = itemCode;
                    if (!String.IsNullOrEmpty(usage) && usage != "0")
                    {
                        doc.Lines.Usage = usage;
                    }
                    doc.Lines.CostingCode2 = itemByOcrCode.Key;
                    doc.Lines.Quantity = 1;
                    doc.Lines.UnitPrice = Math.Round(doc.DocTotal * (lineTotal + vatSum) / itemList.Sum(m => m.LineTotal + m.VatSum), 2);
                }
            }

            return msg;
        }

        public List<DocumentItemModel> GetItems(int docType, int docEntry)
        {
            string sql = Query.DocumentoItem_Get;

            string tableName = String.Empty;
            switch ((BoObjectTypes)docType)
            {
                case BoObjectTypes.oOrders:
                    tableName = "RDR";
                    break;
                case BoObjectTypes.oDeliveryNotes:
                    tableName = "DLN";
                    break;
                case BoObjectTypes.oInvoices:
                    tableName = "INV";
                    break;
                case BoObjectTypes.oCreditNotes:
                    tableName = "RIN";
                    break;
                case BoObjectTypes.oPurchaseOrders:
                    tableName = "POR";
                    break;
                case BoObjectTypes.oPurchaseDeliveryNotes:
                    tableName = "PDN";
                    break;
                case BoObjectTypes.oPurchaseInvoices:
                    tableName = "PCH";
                    break;
                case BoObjectTypes.oPurchaseCreditNotes:
                    tableName = "RPC";
                    break;
            }

            sql = String.Format(sql, tableName, docEntry);

            CrudController crudController = new CrudController();
            return crudController.FillModelListAccordingToSql<DocumentItemModel>(sql);
        }
    }
}
