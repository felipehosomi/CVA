using SAPbobsCOM;
using SAPbouiCOM.Framework;
using System;

namespace MenuConsolidator.Controllers
{
    class BranchTransferController
    {
        public static void SetTransfer(int pruchaseDocEntry, bool showSCN)
        {
            try
            {
                // Se está em uma trasação
                if (CommonController.Company.InTransaction)
                {
                    // Então
                    // Finaliza a transação realizando um rollback para uma nova ser aberta
                    CommonController.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                
                var purchaseOrder = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                var order = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oOrders);
                var businessPlace = (BusinessPlaces)CommonController.Company.GetBusinessObject(BoObjectTypes.oBusinessPlaces);
                var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                purchaseOrder.GetByKey(pruchaseDocEntry);
                businessPlace.GetByKey(purchaseOrder.BPL_IDAssignedToInvoice);

                order.CardCode = businessPlace.DefaultCustomerID;
                order.DocDueDate = DateTime.Now;
                order.BPL_IDAssignedToInvoice = GetBranchID(purchaseOrder.CardCode);

                // Abre uma nova transação
                CommonController.Company.StartTransaction();

                for (var i = 0; i < purchaseOrder.Lines.Count; i++)
                {
                    purchaseOrder.Lines.SetCurrentLine(i);

                    #region Definição de utilização e categoria de estocagem no pedido de compra e venda
                    string sql = $@"select top 1 PAM1.""U_BUsage"", PAM1.""U_SUsage"", PAM1.""U_Category"", PAM1.""U_Calendar"", ITM1.""Price""
                                          from OITM
                                          inner join ""@CVA_PAM1"" PAM1 
                                          	on PAM1.""U_ItemCode"" = '{purchaseOrder.Lines.ItemCode}'
                                            or PAM1.""U_SFamilia"" = OITM.""U_CVA_Subfamilia""
                                            or PAM1.""U_Familia"" = OITM.""U_CVA_Familia""
                                            or PAM1.""U_ItmsGrpCod"" = OITM.""ItmsGrpCod""
                                          inner join ITM1 ON ITM1.""ItemCode"" = OITM.""ItemCode"" AND ITM1.""PriceList"" = PAM1.""U_ListNum""
                                          where PAM1.""Code"" = '{purchaseOrder.BPL_IDAssignedToInvoice}'
                                          and OITM.""ItemCode"" = '{purchaseOrder.Lines.ItemCode}'
                                          order by PAM1.""U_ItemCode"" nulls last, 
	                                            PAM1.""U_SFamilia"" nulls last,
	                                            PAM1.""U_Familia"" nulls last,
	                                            PAM1.""U_ItmsGrpCod"" nulls last
	                                            ";

                    recordset.DoQuery(sql);

                    var calendar = String.Empty;
                    var dueDate = purchaseOrder.DocDueDate;
                    var transportDays = 0;

                    if (recordset.RecordCount != 0)
                    {
                        var routeCode = String.Empty;
                        calendar = recordset.Fields.Item("U_Calendar").Value.ToString();

                        if (recordset.Fields.Item("U_BUsage").Value.ToString() != "0") purchaseOrder.Lines.Usage = recordset.Fields.Item("U_BUsage").Value.ToString();
                        purchaseOrder.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value = recordset.Fields.Item("U_Category").Value.ToString();

                        if (recordset.Fields.Item("U_SUsage").Value.ToString() != "0")
                        {
                            order.TaxExtension.MainUsage = int.Parse(recordset.Fields.Item("U_SUsage").Value.ToString());
                            order.Lines.Usage = recordset.Fields.Item("U_SUsage").Value.ToString();
                        }
                        order.Lines.UserFields.Fields.Item("U_CVA_CatEstoque").Value = recordset.Fields.Item("U_Category").Value.ToString();
                        //order.Lines.UnitPrice = (double)recordset.Fields.Item("Price").Value;

                        recordset.DoQuery($@"select ""@CVA_ROTAENTREGA"".""Code"", ""@CVA_LN_ROTAENTREGA"".""U_Calendar"", ""@CVA_LN_ROTAENTREGA"".""U_TranspDays""
                                               from ""@CVA_ROTAENTREGA""
                                              inner join ""@CVA_LN_ROTAENTREGA"" on ""@CVA_LN_ROTAENTREGA"".""Code"" = ""@CVA_ROTAENTREGA"".""Code""
                                              where ""@CVA_ROTAENTREGA"".""U_CVA_FILIAL_PRINCIPAL"" = {order.BPL_IDAssignedToInvoice}
                                                and ""@CVA_LN_ROTAENTREGA"".""U_CVA_FILIAL_DESTINO"" = {purchaseOrder.BPL_IDAssignedToInvoice}
                                                and ""@CVA_LN_ROTAENTREGA"".""U_Calendar"" = '{calendar}'");

                        if (recordset.RecordCount != 0)
                        {
                            routeCode = recordset.Fields.Item("Code").Value.ToString();
                            calendar = recordset.Fields.Item("U_Calendar").Value.ToString();
                            transportDays = String.IsNullOrEmpty(recordset.Fields.Item("U_TranspDays").Value.ToString()) ? 0 : int.Parse(recordset.Fields.Item("U_TranspDays").Value.ToString());

                            order.Lines.UserFields.Fields.Item("U_CVA_RotaEntrega").Value = routeCode;
                        }
                        
                    }
                    order.Lines.UnitPrice = purchaseOrder.Lines.UnitPrice;
                    #endregion

                    #region Definição de data de vencimento no cabeçalho do documento
                    // Realiza o recalculo da data de vencimento conforme os dias de transporte
                    dueDate = dueDate.AddDays(transportDays * (-1));

                    // Analisa a nova data de vencimento com o calendário de entregas
                    recordset.DoQuery($@"select top 1 CLN1.""U_Date""
                                           from ""@CVA_CLN1"" as CLN1
                                          where CLN1.""Code"" = '{calendar}'
                                            and CLN1.""U_Date"" <= '{dueDate.ToString("yyyyMMdd")}'
                                          order by CLN1.""U_Date"" desc");

                    if (recordset.RecordCount != 0)
                    {
                        dueDate = DateTime.Parse(recordset.Fields.Item("U_Date").Value.ToString());
                    }

                    // Define a nota data de vencimento
                    order.DocDueDate = dueDate;
                    #endregion

                    order.Lines.ShipDate = purchaseOrder.Lines.ShipDate.AddDays(transportDays * (-1));
                    order.Lines.ItemCode = showSCN ? purchaseOrder.Lines.SupplierCatNum : purchaseOrder.Lines.ItemCode;
                    order.Lines.Quantity = purchaseOrder.Lines.Quantity;
                    order.Lines.CostingCode = purchaseOrder.Lines.CostingCode;
                    order.Lines.COGSCostingCode = purchaseOrder.Lines.COGSCostingCode;

                    order.Lines.UserFields.Fields.Item("U_nfe_B2B_xPed").Value = purchaseOrder.DocNum.ToString();
                    order.Lines.UserFields.Fields.Item("U_nfe_B2B_nItemPed").Value = purchaseOrder.Lines.LineNum.ToString();
                    order.Lines.UserFields.Fields.Item("U_CVA_PrchDueDate").Value = purchaseOrder.Lines.ShipDate;

                    order.Lines.Add();
                }

                order.DocumentReferences.ReferencedDocEntry = purchaseOrder.DocEntry;
                order.DocumentReferences.ReferencedObjectType = ReferencedObjectTypeEnum.rot_PurchaseOrder;

                order.Add();

                if (CommonController.Company.GetLastErrorCode() == 0)
                {
                    purchaseOrder.Update();

                    if (CommonController.Company.GetLastErrorCode() == 0)
                    {
                        SetCompletedTransfer(purchaseOrder.DocEntry.ToString(), purchaseOrder.DocObjectCodeEx, purchaseOrder.CardCode, purchaseOrder.BPL_IDAssignedToInvoice.ToString());
                        return;
                    }
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), SAPbouiCOM.BoMessageTime.bmt_Short);
                }

                // Finaliza a transação realizando um rollback para uma nova ser aberta, 
                // porém a atualização do pedido de compra faz com que a propriedade Company.InTransaction
                // fique com o valor false. Mesmo assim, se realizar um rollback ocorre uma exceção vazia,
                // mas a invalidação do processo ocorre normalmente.
                CommonController.Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), SAPbouiCOM.BoMessageTime.bmt_Short);
            }
            catch (Exception ex)
            {
                if (String.IsNullOrEmpty(ex.Message)) return;

                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);

                // Se está em uma trasação
                if (CommonController.Company.InTransaction)
                {
                    // Então
                    // Finaliza a transação realizando um rollback para uma nova ser aberta
                    CommonController.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
            finally
            {
                // Se está em uma trasação
                if (CommonController.Company.InTransaction)
                {
                    // Finaliza a transação realizando um commit
                    CommonController.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                }
            }
        }

        private static int GetBranchID(string cardCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = $@"select ""BPLId"" from OBPL where ""DflVendor"" = '{cardCode}'";
            recordset.DoQuery(query);

            if (recordset.RecordCount == 0)
            {
                throw new Exception($"Não existe nenhuma filial cujo PN {cardCode} esteja vinculado.");
            }

            return int.Parse(recordset.Fields.Item("BPLId").Value.ToString());
        }

        private static void SetCompletedTransfer(string docEntry, string docType, string cardCode, string bplID)
        {
            var optr = CommonController.Company.UserTables.Item("CVA_OPTR");
            optr.Code = docEntry;
            optr.Name = docEntry;
            optr.UserFields.Fields.Item("U_DocType").Value = docType;
            optr.UserFields.Fields.Item("U_CardCode").Value = cardCode;
            optr.UserFields.Fields.Item("U_BPLId").Value = bplID;

            optr.Add();

            if (CommonController.Company.GetLastErrorCode() != 0)
            {
                Application.SBO_Application.StatusBar.SetText(CommonController.Company.GetLastErrorDescription(), SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }
    }
}
