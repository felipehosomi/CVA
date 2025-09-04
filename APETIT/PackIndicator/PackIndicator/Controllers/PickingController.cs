using PackIndicator.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using SAPbouiCOM.Framework;
using PackIndicator.Extensions;
using System.Runtime.InteropServices;

namespace PackIndicator.Controllers
{
    class PickingController
    {
        public static void UpdateDocument(List<PickingData> pickingData)
        {
            var order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
            order.GetByKey(pickingData.FirstOrDefault().DocEntry);
            var docLineNum = 0;

            var purchase = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            int purchaseDocNum = 0;
            if (order.DocumentReferences.ReferencedObjectType == ReferencedObjectTypeEnum.rot_PurchaseOrder)
            {
                purchase.GetByKey(order.DocumentReferences.ReferencedDocEntry);
                purchaseDocNum = purchase.DocNum;
            }

            try
            {
                foreach (var line in pickingData.Where(x => x.Packages.Count > 0 || x.DocLineType == LineType.Balance).ToList())
                {
                    Packages package = line.Packages.FirstOrDefault();

                    if (line.DocLineType == LineType.Balance)
                    {
                        order.Lines.SetCurrentSalesLineByLineNum(line.OriginalDocLineNum);
                        try
                        {
                            docLineNum = int.Parse(order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value.ToString());
                        }
                        catch { }
                        //var originalLine = order.Lines;

                        string catEstoque = order.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value.ToString();
                        DateTime purchaseDueDate = (DateTime)order.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value;
                        string rotaEntrega = order.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value.ToString();
                        string usage = order.Lines.Usage;
                        string purchaseOrderNum = order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value.ToString();
                        string costingCode = order.Lines.CostingCode;
                        string cogsCostingCode = order.Lines.COGSCostingCode;

                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value;

                        order.Lines.Add();
                        line.DocLineNum = order.Lines.Count - 1;

                        order.Lines.ItemCode = line.ItemCode;
                        order.Lines.ShipDate = line.ShipDate;
                        if (usage != "0") order.Lines.Usage = usage;

                        order.Lines.CostingCode = costingCode;
                        order.Lines.COGSCostingCode = cogsCostingCode;

                        order.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value = catEstoque;
                        order.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value = purchaseDueDate;
                        order.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value = rotaEntrega;
                        order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value = purchaseOrderNum;

                        order.Lines.UnitPrice = line.Price;
                        order.Lines.UoMEntry = PackageController.GetUomEntry(line.OriginalUom);
                        order.Lines.Quantity = line.OriginalQty;

                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = String.Empty;
                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = String.Empty;
                        order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = String.Empty;
                        order.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = String.Empty;
                        if (order.DocumentReferences.ReferencedObjectType == ReferencedObjectTypeEnum.rot_PurchaseOrder)
                        {
                            purchase.Lines.SetCurrentPurchaseLineByLineNum(docLineNum, line, purchase.DocNum);

                            catEstoque = purchase.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value.ToString();
                            purchaseDueDate = (DateTime)purchase.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value;
                            rotaEntrega = purchase.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value.ToString();
                            usage = purchase.Lines.Usage;

                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value;

                            purchase.Lines.Add();

                            purchase.Lines.ItemCode = line.ItemCode;
                            purchase.Lines.ShipDate = line.ShipDate;

                            if (usage != "0") purchase.Lines.Usage = usage;
                            purchase.Lines.CostingCode = costingCode;
                            purchase.Lines.COGSCostingCode = cogsCostingCode;

                            purchase.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value = catEstoque;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value = purchaseDueDate;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value = rotaEntrega;

                            purchase.Lines.UnitPrice = line.Price;
                            purchase.Lines.UoMEntry = PackageController.GetUomEntry(line.OriginalUom);
                            purchase.Lines.Quantity = line.OriginalQty;

                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = String.Empty;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = String.Empty;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = String.Empty;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = String.Empty;
                            order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = purchase.Lines.LineNum.ToString();
                        }

                        continue;
                    }

                    if (line.DocLineType == LineType.New)
                    {
                        order.Lines.SetCurrentSalesLineByLineNum(line.OriginalDocLineNum);
                        try
                        {
                            docLineNum = int.Parse(order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value.ToString());
                        }
                        catch { }
                        //var originalLine = order.Lines;
                        string usage = order.Lines.Usage;

                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value;

                        string catEstoque = order.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value.ToString();
                        DateTime purchaseDueDate = (DateTime)order.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value;
                        string rotaEntrega = order.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value.ToString();
                        string purchaseOrderNum = order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value.ToString();
                        string costingCode = order.Lines.CostingCode;
                        string cogsCostingCode = order.Lines.COGSCostingCode;

                        order.Lines.Add();
                        line.DocLineNum = order.Lines.Count - 1;

                        order.Lines.ItemCode = line.ItemCode;
                        order.Lines.ShipDate = line.ShipDate;
                        if (usage != "0") order.Lines.Usage = usage;
                        order.Lines.CostingCode = costingCode;
                        order.Lines.COGSCostingCode = cogsCostingCode;

                        order.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value = catEstoque;
                        order.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value = purchaseDueDate;
                        order.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value = rotaEntrega;

                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = line.OriginalUom;
                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = line.OriginalQty;
                        order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value = purchaseOrderNum;
                        order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = package.Volume;

                        if (package.Validade.Year != 1900)
                            order.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = package.Validade;

                        if (order.DocumentReferences.ReferencedObjectType == ReferencedObjectTypeEnum.rot_PurchaseOrder)
                        {
                            purchase.Lines.SetCurrentPurchaseLineByLineNum(docLineNum, line, purchase.DocNum);

                            usage = purchase.Lines.Usage;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value;

                            catEstoque = purchase.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value.ToString();
                            purchaseDueDate = (DateTime)purchase.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value;
                            rotaEntrega = purchase.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value.ToString();

                            purchase.Lines.Add();
                            purchase.Lines.ItemCode = line.ItemCode;
                            purchase.Lines.ShipDate = line.ShipDate;
                            if (usage != "0") purchase.Lines.Usage = usage;
                            purchase.Lines.CostingCode = costingCode;
                            purchase.Lines.COGSCostingCode = cogsCostingCode;

                            purchase.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value = catEstoque;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value = purchaseDueDate;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value = rotaEntrega;

                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = line.OriginalUom;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = line.OriginalQty;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = package.Volume;
                            if (package.Validade.Year != 1900)
                                purchase.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = package.Validade;
                            order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = purchase.Lines.LineNum.ToString();
                        }
                    }
                    else
                    {
                        order.Lines.SetCurrentSalesLineByLineNum(line.DocLineNum);
                        try
                        {
                            docLineNum = int.Parse(order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value.ToString());
                        }
                        catch { }
                        if (!String.IsNullOrEmpty(order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value.ToString())) continue;

                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = order.Lines.UoMCode;
                        order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = line.OriginalQty;
                        order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = package.Volume;
                        if (package.Validade.Year != 1900)
                            order.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = package.Validade;

                        if (order.DocumentReferences.ReferencedObjectType == ReferencedObjectTypeEnum.rot_PurchaseOrder)
                        {
                            purchase.Lines.SetCurrentPurchaseLineByLineNum(docLineNum, line, purchase.DocNum);

                            if (!String.IsNullOrEmpty(purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value.ToString())) continue;

                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = purchase.Lines.UoMCode;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = line.OriginalQty;
                            purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = package.Volume;
                            if (package.Validade.Year != 1900) purchase.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = package.Validade;
                        }
                    }

                    order.Lines.UoMEntry = package.UoMEntry;
                    order.Lines.UnitsOfMeasurment = package.Fatorconversao;
                    order.Lines.Quantity = package.Quantidade;

                    if (line.Price * package.Fatorconversao != order.Lines.UnitPrice)
                    {
                        order.Lines.UnitPrice = line.Price * package.Fatorconversao;
                    }
                    else
                    {
                        // Bug da DI-API. Se definir o mesmo preço unitário (UnitPrice) contido na linha, perde-se o valor.
                        // Para que isso não aconteça, é preciso definir o Price com um valor completamente diferente.
                        order.Lines.Price = order.Lines.Price + 1.0;
                    }

                    if (order.DocumentReferences.ReferencedObjectType == ReferencedObjectTypeEnum.rot_PurchaseOrder)
                    {
                        purchase.Lines.Quantity = package.Quantidade;
                        purchase.Lines.UoMEntry = package.UoMEntry;

                        if (line.Price * package.Fatorconversao != purchase.Lines.UnitPrice)
                        {
                            purchase.Lines.UnitPrice = line.Price * package.Fatorconversao;
                        }
                        else
                        {
                            // Bug da DI-API. Se definir o mesmo preço unitário (UnitPrice) contido na linha, perde-se o valor.
                            // Para que isso não aconteça, é preciso definir o Price com um valor completamente diferente.
                            purchase.Lines.Price = purchase.Lines.Price + 1.0;
                        }
                    }
                }

                if (order.Update() != 0)
                {
                    throw new Exception($"Pedido de venda {pickingData.FirstOrDefault().DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                }

                // Caso o pedido de venda tenha referência com um pedido de compra, trata-se de um documento espelhado.
                // Então, deve-se realizar a alteração do pedido de compra.
                if (order.DocumentReferences.ReferencedObjectType == ReferencedObjectTypeEnum.rot_PurchaseOrder)
                {
                    if (purchase.Update() != 0)
                    {
                        throw new Exception($"Pedido de compra espelhado {purchase.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.ReleaseComObject(order);
                Marshal.ReleaseComObject(purchase);

                order = null;
                purchase = null;
            }
        }

        private static void UpdateReferencedPurchase(int docEntry, List<PickingData> pickingData, Documents order)
        {
            var purchase = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            purchase.GetByKey(docEntry);
            var docLineNum = int.Parse(order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value.ToString());
            var docNum = purchase.DocNum;

            foreach (var line in pickingData.Where(x => x.Packages.Count > 0 || x.DocLineType == LineType.Balance).ToList())
            {
                Packages package = line.Packages.FirstOrDefault(); ;

                if (line.DocLineType == LineType.Balance)
                {

                    purchase.Lines.SetCurrentPurchaseLineByLineNum(docLineNum, line, purchase.DocNum);

                    var originalLine = purchase.Lines;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value;

                    purchase.Lines.Add();
                    purchase.Lines.ItemCode = line.ItemCode;
                    purchase.Lines.ShipDate = line.ShipDate;
                    if (originalLine.Usage != "0") purchase.Lines.Usage = originalLine.Usage;

                    for (var i = 0; i < originalLine.UserFields.Fields.Count; i++)
                    {
                        try
                        {
                            purchase.Lines.UserFields.Fields.Item(i).Value = originalLine.UserFields.Fields.Item(i).Value;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    purchase.Lines.UnitPrice = line.Price;
                    purchase.Lines.UoMEntry = PackageController.GetUomEntry(line.OriginalUom);
                    purchase.Lines.Quantity = line.OriginalQty;

                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = String.Empty;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = String.Empty;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = String.Empty;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = String.Empty;
                    order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = purchase.Lines.LineNum.ToString();
                    order.Update();
                    continue;
                }

                if (line.DocLineType == LineType.New)
                {
                    purchase.Lines.SetCurrentPurchaseLineByLineNum(docLineNum, line, purchase.DocNum);

                    var originalLine = purchase.Lines;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value;

                    purchase.Lines.Add();
                    purchase.Lines.ItemCode = line.ItemCode;
                    purchase.Lines.ShipDate = line.ShipDate;
                    if (originalLine.Usage != "0") purchase.Lines.Usage = originalLine.Usage;

                    for (var i = 0; i < originalLine.UserFields.Fields.Count; i++)
                    {
                        try
                        {
                            purchase.Lines.UserFields.Fields.Item(i).Value = originalLine.UserFields.Fields.Item(i).Value;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = purchase.Lines.LineNum.ToString();
                    order.Update();
                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = line.OriginalUom;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = line.OriginalQty;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = package.Volume;
                    if (package.Validade.Year != 1900) purchase.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = package.Validade;
                }
                else
                {
                    purchase.Lines.SetCurrentPurchaseLineByLineNum(docLineNum, line, purchase.DocNum);

                    if (!String.IsNullOrEmpty(purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value.ToString())) continue;

                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = purchase.Lines.UoMCode;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = line.OriginalQty;
                    purchase.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = package.Volume;
                    if (package.Validade.Year != 1900) purchase.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = package.Validade;
                }

                purchase.Lines.UoMEntry = package.UoMEntry;
                purchase.Lines.Quantity = package.Quantidade;

                if (line.Price * package.Fatorconversao != purchase.Lines.UnitPrice)
                {
                    purchase.Lines.UnitPrice = line.Price * package.Fatorconversao;
                }
                else
                {
                    // Bug da DI-API. Se definir o mesmo preço unitário (UnitPrice) contido na linha, perde-se o valor.
                    // Para que isso não aconteça, é preciso definir o Price com um valor completamente diferente.
                    purchase.Lines.Price = purchase.Lines.Price + 1.0;
                }
            }

            purchase.Update();

            if (CommonController.Company.GetLastErrorCode() != 0)
            {
                throw new Exception($"Pedido de compra espelhado {docNum}: {CommonController.Company.GetLastErrorDescription()}");
            }
        }

        private static void BreakDocumentLine(Documents order, Documents purchaseOrder, int lineNum, double newQty, double balanceQty, int newPurchaseLineNum)
        {

            try
            {
                var docNum = order.DocNum;
                order.Lines.SetCurrentSalesLineByLineNum(lineNum);

                double originalQty = double.Parse(order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value.ToString());
                double originalItemPerPack = order.Lines.UnitsOfMeasurment;
                string itemCode = order.Lines.ItemCode;
                string usage = order.Lines.Usage;
                double price = order.Lines.Price;
                DateTime shipDate = order.Lines.ShipDate;
                string uom = order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value.ToString();
                string purchaseOrderNum = order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value.ToString();
                string costingCode = order.Lines.CostingCode;
                string cogsCostingCode = order.Lines.COGSCostingCode;

                Dictionary<int, object> userFieldsOrder = new Dictionary<int, object>();
                Dictionary<int, object> userFieldsPurchaseOrder = new Dictionary<int, object>();
                for (var i = 0; i < order.Lines.UserFields.Fields.Count; i++)
                {
                    object userField = order.Lines.UserFields.Fields.Item(i).Value;
                    if (userField != null && userField.ToString() != "")
                    {
                        userFieldsOrder.Add(i, userField);
                    }
                }
                for (var i = 0; i < purchaseOrder.Lines.UserFields.Fields.Count; i++)
                {
                    object userField = purchaseOrder.Lines.UserFields.Fields.Item(i).Value;
                    if (userField != null && userField.ToString() != "")
                    {
                        userFieldsPurchaseOrder.Add(i, userField);
                    }
                }

                // Busca a linha correspondente do pedido de compra
                purchaseOrder.Lines.SetCurrentLine(Convert.ToInt32(order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value));

                DateTime purchaseShipDate = purchaseOrder.Lines.ShipDate;
                string purchaseUsage = purchaseOrder.Lines.Usage;
                double purchasePrice = purchaseOrder.Lines.Price;
                purchaseOrder.Lines.Quantity = newQty;

                purchaseOrder.Lines.Add();

                purchaseOrder.Lines.ItemCode = itemCode;
                purchaseOrder.Lines.ShipDate = purchaseShipDate;
                if (purchaseUsage != "0")
                {
                    purchaseOrder.Lines.Usage = purchaseUsage;
                }
                if (!String.IsNullOrEmpty(uom))
                {
                    purchaseOrder.Lines.UoMEntry = PackageController.GetUomEntry(uom);
                }

                purchaseOrder.Lines.UnitPrice = purchasePrice;
                purchaseOrder.Lines.Quantity = balanceQty * originalItemPerPack;
                purchaseOrder.Lines.CostingCode = costingCode;
                purchaseOrder.Lines.COGSCostingCode = cogsCostingCode;

                foreach (var userField in userFieldsPurchaseOrder)
                {
                    try
                    {
                        purchaseOrder.Lines.UserFields.Fields.Item(userField.Key).Value = userField.Value;
                    }
                    catch { }
                }

                purchaseOrder.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = "";
                purchaseOrder.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = "";
                purchaseOrder.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = "";
                purchaseOrder.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = "";

                order.Lines.Quantity = newQty;

                order.Lines.Add();
                order.Lines.ItemCode = itemCode;
                order.Lines.ShipDate = shipDate;
                if (usage != "0")
                {
                    order.Lines.Usage = usage;
                }
                if (!String.IsNullOrEmpty(uom))
                {
                    order.Lines.UoMEntry = PackageController.GetUomEntry(uom);
                }
                order.Lines.UnitPrice = price;
                order.Lines.Quantity = balanceQty * originalItemPerPack;
                order.Lines.CostingCode = costingCode;
                order.Lines.COGSCostingCode = cogsCostingCode;

                foreach (var userField in userFieldsOrder)
                {
                    try
                    {
                        order.Lines.UserFields.Fields.Item(userField.Key).Value = userField.Value;
                    }
                    catch { }
                }

                order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = "";
                order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = "";
                order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = "";
                order.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = "";
                order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = newPurchaseLineNum.ToString();
                order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value = purchaseOrderNum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        //private static void SetNewLineQuantity(int docEntry, int lineNum, double newQty)
        //{
        //    var order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
        //    order.GetByKey(docEntry);
        //    var docNum = order.DocNum;
        //    order.Lines.SetCurrentSalesLineByLineNum(lineNum);
        //    order.Lines.Quantity = newQty;
        //    order.Update();

        //    if (CommonController.Company.GetLastErrorCode() != 0)
        //    {
        //        throw new Exception($"Pedido de venda {docNum}: {CommonController.Company.GetLastErrorDescription()}");
        //    }
        //}

        private static void ResetDocumentLine(int docEntry, List<int> lineNumbers)
        {
            var order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
            order.GetByKey(docEntry);
            var docNum = order.DocNum;

            foreach (var lineNum in lineNumbers)
            {
                order.Lines.SetCurrentSalesLineByLineNum(lineNum);

                order.Lines.UoMEntry = PackageController.GetUomEntry(order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value.ToString());
                order.Lines.Quantity = double.Parse(order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value.ToString());
                order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = "";
                order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = "";
                order.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = "";
                order.Lines.UserFields.Fields.Item("U_CVA_SuggestedTotal").Value = "";
                //order.Lines.UserFields.Fields.Item("U_CVA_Break").Value = "Y";
            }

            order.Update();

            if (CommonController.Company.GetLastErrorCode() != 0)
            {
                throw new Exception($"Pedido de venda {docNum}: {CommonController.Company.GetLastErrorDescription()}");
            }
        }

        public static void SetPickList(int absEntry, List<WMSIntegrationFile.Items> wmsItems)
        {
            // Se está em uma trasação
            if (CommonController.Company.InTransaction)
            {
                // Então
                // Finaliza a transação realizando um rollback para uma nova ser aberta
                CommonController.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
            }
            // Abre uma nova transação
            CommonController.Company.StartTransaction();

            var order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
            var purchaseOrder = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            var pickList = (PickLists)CommonController.Company.GetBusinessObject(BoObjectTypes.oPickLists);
            int removedPickLines = 0;
            List<int> orderLinesToClose = new List<int>();
            //List<int> purchaseLinesToClose = new List<int>();

            try
            {
                pickList.GetByKey(absEntry);

                bool orderHasChanges = false;
                int newPurchaseLineNum = 0;

                foreach (var item in wmsItems)
                {
                    if (pickList.Lines.Count > item.PickEntry - removedPickLines)
                    {
                        pickList.Lines.SetCurrentLine(item.PickEntry - removedPickLines);
                    }

                    if (pickList.Lines.LineNumber != item.PickEntry)
                    {
                        for (var i = 0; i < pickList.Lines.Count; i++)
                        {
                            pickList.Lines.SetCurrentLine(i);

                            if (pickList.Lines.LineNumber == item.PickEntry) break;
                        }
                    }

                    if (order.DocEntry != pickList.Lines.OrderEntry)
                    {
                        if (orderHasChanges)
                        {
                            if (purchaseOrder.Update() != 0)
                            {
                                throw new Exception($"Erro ao atualizar pedido de compras {purchaseOrder.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                            }
                            if (order.Update() != 0)
                            {
                                throw new Exception($"Erro ao atualizar pedido de vendas {order.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                            }

                            if (pickList.Update() != 0)
                            {
                                throw new Exception("Erro ao atualizar picking: " + CommonController.Company.GetLastErrorDescription());
                            }

                            Marshal.ReleaseComObject(purchaseOrder);
                            Marshal.ReleaseComObject(order);

                            purchaseOrder = null;
                            order = null;

                            purchaseOrder = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                            order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
                        }

                        orderHasChanges = false;
                        order.GetByKey(pickList.Lines.OrderEntry);
                        order.Lines.SetCurrentLine(pickList.Lines.OrderRowID);

                        if (order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value != null)
                        {
                            int docEntryPurchaseOrder = PurchaseOrderController.GetDocEntry(Convert.ToInt32(order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value));
                            purchaseOrder.GetByKey(docEntryPurchaseOrder);

                            // Busca o novo LineNum do pedido de compra para setar no pedido de venda
                            purchaseOrder.Lines.SetCurrentLine(purchaseOrder.Lines.Count - 1);
                            newPurchaseLineNum = purchaseOrder.Lines.LineNum + 1;
                        }
                    }

                    order.Lines.SetCurrentLine(pickList.Lines.OrderRowID);

                    if (pickList.Lines.LineNumber != item.PickEntry)
                    {
                        order.Lines.SetCurrentSalesLineByLineNum(pickList.Lines.OrderRowID);
                        throw new Exception($@"Linha {item.PickEntry} do Pedido de Compra {purchaseOrder.DocNum}, espelhada do pedido de venda {order.DocNum} linha {order.Lines.VisualOrder}, não encontrada.");
                    }

                    if (pickList.Status == BoPickStatus.ps_Closed) continue;

                    if (order.Lines.LineStatus == BoStatus.bost_Close) continue;

                    if (item.Quantity == 0)
                    {
                        pickList.Lines.ReleasedQuantity = 0;
                        string uom = order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value.ToString();

                        if (uom != order.Lines.UoMCode)
                        {
                            double originalQty = double.Parse(order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value.ToString());
                            double originalItemPerPack = order.Lines.UnitsOfMeasurment;
                            string itemCode = order.Lines.ItemCode;
                            string usage = order.Lines.Usage;
                            double price = order.Lines.Price;
                            DateTime shipDate = order.Lines.ShipDate;
                            string purchaseOrderNum = order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value.ToString();
                            string purchaseOrderLine = order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value.ToString();
                            string costingCode = order.Lines.CostingCode;
                            string cogsCostingCode = order.Lines.COGSCostingCode;

                            order.Lines.Delete();

                            if (pickList.Update() != 0)
                            {
                                throw new Exception("Erro ao remover linha do picking: " + CommonController.Company.GetLastErrorDescription());
                            }

                            if (purchaseOrder.DocEntry != 0)
                            {
                                if (purchaseOrder.Update() != 0)
                                {
                                    throw new Exception($"Erro ao atualizar pedido de compras {purchaseOrder.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                                }
                            }
                            if (order.Update() != 0)
                            {
                                throw new Exception($"Erro ao atualizar pedido de vendas {order.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                            }


                            pickList.GetByKey(absEntry);
                            // Foi removida uma linha, portanto subtrai o ID para buscar as próximas
                            removedPickLines++;

                            order.GetByKey(pickList.Lines.OrderEntry);
                            order.Lines.SetCurrentLine(pickList.Lines.OrderRowID);

                            if (order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value != null)
                            {
                                int docEntryPurchaseOrder = PurchaseOrderController.GetDocEntry(Convert.ToInt32(order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value));
                                purchaseOrder.GetByKey(docEntryPurchaseOrder);

                                // Busca o novo LineNum do pedido de compra para setar no pedido de venda
                                purchaseOrder.Lines.SetCurrentLine(purchaseOrder.Lines.Count - 1);
                            }

                            //orderLinesToClose.Add(order.Lines.LineNum);

                            order.Lines.Add();
                            order.Lines.ItemCode = itemCode;
                            order.Lines.Usage = usage;
                            order.Lines.Price = price;
                            order.Lines.ShipDate = shipDate;
                            order.Lines.UoMEntry = PackageController.GetUomEntry(uom);
                            order.Lines.CostingCode = costingCode;
                            order.Lines.COGSCostingCode = cogsCostingCode;

                            order.Lines.Quantity = originalQty;

                            order.Lines.UserFields.Fields.Item("U_CVA_OriginalQty").Value = "";
                            order.Lines.UserFields.Fields.Item("U_CVA_OriginalUom").Value = "";
                            order.Lines.UserFields.Fields.Item("U_CVA_DueDate").Value = "";
                            order.Lines.UserFields.Fields.Item("U_CVA_Break").Value = "Y";

                            order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value = purchaseOrderNum;
                            order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = purchaseOrderLine;

                            orderHasChanges = true;
                        }
                        continue;
                    }
                    else if (Math.Round(pickList.Lines.ReleasedQuantity / order.Lines.UnitsOfMeasurment, 5) > item.Quantity)
                    {
                        orderHasChanges = true;
                        BreakDocumentLine(order, purchaseOrder, pickList.Lines.OrderRowID, item.Quantity, Math.Round(pickList.Lines.ReleasedQuantity / order.Lines.UnitsOfMeasurment, 5) - item.Quantity, newPurchaseLineNum);
                        // Setar quantidade liberada para zero, se não fica quantidade em aberto, mesmo diminuindo do pedido
                        pickList.Lines.ReleasedQuantity = 0;
                        newPurchaseLineNum++;

                    }
                    else if (Math.Round(pickList.Lines.ReleasedQuantity / order.Lines.UnitsOfMeasurment, 5) < item.Quantity)
                    {
                        orderHasChanges = true;
                        order.Lines.Quantity = item.Quantity;
                    }

                    //pickList.Lines.ReleasedQuantity = item.Quantity;
                    pickList.Lines.PickedQuantity = item.Quantity;
                }

                if (orderHasChanges)
                {
                    if (purchaseOrder.DocEntry != 0)
                    {
                        if (purchaseOrder.Update() != 0)
                        {
                            throw new Exception($"Erro ao atualizar pedido de compras {purchaseOrder.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                        }
                    }
                    if (order.Update() != 0)
                    {
                        throw new Exception($"Erro ao atualizar pedido de vendas {order.DocNum}: {CommonController.Company.GetLastErrorDescription()}");
                    }
                }

                if (pickList.Update() != 0)
                {
                    throw new Exception("Erro ao atualizar picking: " + CommonController.Company.GetLastErrorDescription());
                }

                if (CommonController.Company.InTransaction)
                {
                    // Finaliza a transação realizando um commit
                    CommonController.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                }

                removedPickLines = 0;
                //SAPbouiCOM.Form orderForm = null;
                //if (wmsItems.Any(m => m.Quantity == 0))
                //{
                //    orderForm = Application.SBO_Application.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Order, "", pickList.Lines.OrderEntry.ToString());
                //}

                //foreach (var item in wmsItems.Where(m => m.Quantity == 0))
                //{
                //    SAPbouiCOM.Matrix mtLines = (SAPbouiCOM.Matrix)orderForm.Items.Item(11).Specific;
                //    SAPbouiCOM.Column oColumn = mtLines.Columns.Item("#");
                //    oColumn.Cells.Item(item.PickEntry - removedPickLines).Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                //    orderForm.Menu.Item("1293").Activate();
                //    removedPickLines++;
                //}

                //orderForm.Close();


                //SAPbouiCOM.Form pickingForm = null;
                //if (wmsItems.Any(m => m.Quantity == 0))
                //{
                //    pickingForm = Application.SBO_Application.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_PickList, "", absEntry.ToString());
                //}

                //foreach (var item in wmsItems.Where(m => m.Quantity == 0))
                //{
                //    SAPbouiCOM.Matrix mtLines = (SAPbouiCOM.Matrix)pickingForm.Items.Item(11).Specific;
                //    SAPbouiCOM.Column oColumn = mtLines.Columns.Item("#");
                //    oColumn.Cells.Item(item.PickEntry - removedPickLines).Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                //    pickingForm.Menu.Item("1293").Activate();
                //    removedPickLines++;
                //}

                //pickingForm.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.ReleaseComObject(pickList);
                Marshal.ReleaseComObject(purchaseOrder);
                Marshal.ReleaseComObject(order);

                pickList = null;
                purchaseOrder = null;
                order = null;
            }
        }

        public static void SetZeroPickList(int absEntry, List<WMSIntegrationFile.Items> wmsItems)
        {
            // Se está em uma trasação
            if (CommonController.Company.InTransaction)
            {
                // Então
                // Finaliza a transação realizando um rollback para uma nova ser aberta
                CommonController.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
            }
            // Abre uma nova transação
            CommonController.Company.StartTransaction();

            var ordersList = new Dictionary<int, List<int>>();

            var pickList = (PickLists)CommonController.Company.GetBusinessObject(BoObjectTypes.oPickLists);
            pickList.GetByKey(absEntry);

            foreach (var item in wmsItems)
            {
                if (pickList.Lines.Count > item.PickEntry)
                {
                    pickList.Lines.SetCurrentLine(item.PickEntry);
                }

                if (pickList.Lines.LineNumber != item.PickEntry)
                {
                    for (var i = 0; i < pickList.Lines.Count; i++)
                    {
                        pickList.Lines.SetCurrentLine(i);

                        if (pickList.Lines.LineNumber == item.PickEntry) break;
                    }
                }

                if (pickList.Lines.LineNumber != item.PickEntry)
                {
                    throw new Exception($"A lista de picking não contém a linha {item.PickEntry}");
                }

                if (pickList.Status == BoPickStatus.ps_Closed) continue;

                var order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
                order.GetByKey(pickList.Lines.OrderEntry);
                order.Lines.SetCurrentSalesLineByLineNum(pickList.Lines.OrderRowID);

                if (order.Lines.LineStatus == BoStatus.bost_Close) continue;

                if (ordersList.ContainsKey(pickList.Lines.OrderEntry))
                {
                    ordersList[pickList.Lines.OrderEntry].Add(pickList.Lines.OrderRowID);
                }
                else
                {
                    ordersList.Add(pickList.Lines.OrderEntry, new List<int> { pickList.Lines.OrderRowID });
                }

                pickList.Lines.ReleasedQuantity = item.Quantity;
                pickList.Lines.PickedQuantity = item.Quantity;
            }

            pickList.Update();

            if (CommonController.Company.GetLastErrorCode() != 0)
            {
                throw new Exception(CommonController.Company.GetLastErrorDescription());
            }

            foreach (var order in ordersList)
            {
                ResetDocumentLine(order.Key, order.Value);
            }

            if (CommonController.Company.InTransaction)
            {
                // Finaliza a transação realizando um commit
                CommonController.Company.EndTransaction(BoWfTransOpt.wf_Commit);
            }
        }

        public static int GetDocLineNum(int absEntry, int pickEntry)
        {
            var pickList = (PickLists)CommonController.Company.GetBusinessObject(BoObjectTypes.oPickLists);
            pickList.GetByKey(absEntry);
            pickList.Lines.SetCurrentLine(pickEntry);

            if (pickList.Lines.LineNumber != pickEntry)
            {
                for (var i = 0; i < pickList.Lines.Count; i++)
                {
                    pickList.Lines.SetCurrentLine(i);

                    if (pickList.Lines.LineNumber == pickEntry) break;
                }
            }

            return pickList.Lines.OrderRowID;
        }

        public static void SetPickList(List<PickingData> pickingData)
        {
            var pickList = (PickLists)CommonController.Company.GetBusinessObject(BoObjectTypes.oPickLists);

            pickList.PickDate = DateTime.Now;
            pickList.Remarks = "Inserido via sugestão de picking.";

            foreach (var line in pickingData.Where(x => x.Packages.Count > 0 && x.ReleasePicking).ToList())
            {
                if (line.Packages.FirstOrDefault().Quantidade == 0)
                {
                    continue;
                }

                pickList.Lines.BaseObjectType = "17";
                pickList.Lines.OrderEntry = line.DocEntry;
                pickList.Lines.OrderRowID = line.DocLineNum;
                pickList.Lines.ReleasedQuantity = line.Packages.FirstOrDefault().Quantidade;
                pickList.Lines.Add();
            }

            if (pickList.Add() != 0)
            {
                Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), SAPbouiCOM.BoMessageTime.bmt_Short);
            }

            Marshal.ReleaseComObject(pickList);
            pickList = null;
        }
    }
}
