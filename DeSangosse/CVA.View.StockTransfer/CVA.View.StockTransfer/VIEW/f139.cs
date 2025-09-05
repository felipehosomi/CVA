using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.StockTransfer.BLL;
using CVA.View.StockTransfer.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.View.StockTransfer.VIEW
{
    /// <summary>
    /// Pedido de venda
    /// </summary>
    public class f139 : BaseForm
    {
        private static bool Authorized = false;
        private static int StockTransferId = 0;
        private static string ErrorMessage = "";
        public static bool Canceling = false;
        public static bool Closing = false;

        #region Constructor
        public f139()
        {
            FormCount++;
        }

        public f139(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f139(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f139(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Canceling = false;
                    Closing = false;
                    StockTransferId = 0;
                    SBOApp.Application.StatusBarEvent += Application_StatusBarEvent;
                }
            }
            else
            {

                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    ErrorMessage = String.Empty;
                    SBOApp.Application.StatusBarEvent -= Application_StatusBarEvent;
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        Canceling = false;
                        Closing = false;
                        if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            DBDataSource ds_ORDR = Form.DataSources.DBDataSources.Item("ORDR");
                            string calcularPeso = ds_ORDR.GetValue("U_CVA_Calcular", ds_ORDR.Offset);
                            if (calcularPeso == "S")
                            {
                                MedidaItemBLL meditaItemBLL = new MedidaItemBLL();
                                double pesoBruto = 0;
                                double pesoLiquido = 0;
                                Matrix mtx = Form.Items.Item("38").Specific;
                                for (int i = 1; i <= mtx.RowCount; i++)
                                {
                                    string itemCode = ((EditText)mtx.Columns.Item("1").Cells.Item(i).Specific).Value;
                                    string sPesoLiquido = ((EditText)mtx.Columns.Item("58").Cells.Item(i).Specific).Value.Replace("kg", "").Replace(".", "");
                                    if (!string.IsNullOrEmpty(itemCode) && !string.IsNullOrEmpty(sPesoLiquido))
                                    {
                                        SBOApp.Application.StatusBar.SetSystemMessage($"Verificando peso do  item {itemCode.ToString()}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                        SBOApp.Application.StatusBar.SetSystemMessage($"Recuperando peso liquido {sPesoLiquido.ToString()}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                        pesoLiquido = Convert.ToDouble(sPesoLiquido);
                                        var medidaItem = meditaItemBLL.GetPesoBrutoItem(itemCode);
                                        pesoBruto = pesoBruto + ((pesoLiquido * medidaItem.U_CVA_Densidade) + (pesoLiquido * medidaItem.U_CVA_PesoEmbalagem) + (pesoLiquido * medidaItem.U_CVA_PesoPalete));
                                        SBOApp.Application.StatusBar.SetSystemMessage($"Calculando peso bruto do item {pesoBruto.ToString()}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                    }
                                }

                                SBOApp.Application.StatusBar.SetSystemMessage($"Adicionando peso bruto na aba imposto {pesoBruto.ToString()}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                Form.Freeze(true);

                                ((EditText)Form.Items.Item("2034").Specific).String = pesoBruto.ToString();
                                //Form.PaneLevel = 1;
                                Form.Freeze(false);
                            }
                        }
                    }
                }
            }

            return true;
        }

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    DBDataSource ds_ORDR = Form.DataSources.DBDataSources.Item("ORDR");
                    Authorized = ds_ORDR.GetValue("Confirmed", ds_ORDR.Offset) == "Y";
                    StockTransferId = 0;
                    Canceling = false;
                    Closing = false;
                }
                if (BusinessObjectInfo.ActionSuccess)
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                    {
                        DBDataSource ds_ORDR = Form.DataSources.DBDataSources.Item("ORDR");
                        string docEntry = ds_ORDR.GetValue("DocEntry", ds_ORDR.Offset);
                        StockTransferRequestBLL stockTransferRequestBLL = new StockTransferRequestBLL();

                        //if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                        //{
                        //    string canceled = ds_ORDR.GetValue("CANCELED", ds_ORDR.Offset);
                        //    if (canceled != "C")
                        //    {
                        //        if (stockTransferRequestBLL.Exists(Convert.ToInt32(docEntry), 17))
                        //        {
                        //            SBOApp.Application.MessageBox("Atenção: já existe uma solicitação de transferência para este documento");
                        //            return true;
                        //        }
                        //    }
                        //}

                        if (ds_ORDR.GetValue("Confirmed", ds_ORDR.Offset) == "Y") // Flag autorizado
                        {
                            string msg = stockTransferRequestBLL.SetStockTransferBaseDocument(BoObjectTypes.oOrders, Convert.ToInt32(docEntry), StockTransferId);
                            if (!String.IsNullOrEmpty(msg))
                            {
                                SBOApp.Application.MessageBox("Erro ao gerar solicitação de transferência: " + msg);
                            }
                            else
                            {
                                if (StockTransferId > 0)
                                {
                                    StockTransferId = 0;
                                    SBOApp.Application.MessageBox("Solicitação de transferência gerada com sucesso!");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                //{
                //    DBDataSource ds_RDR1 = Form.DataSources.DBDataSources.Item("RDR1");
                //    bool isTransfer = false;
                //    for (int i = 0; i < ds_RDR1.Size; i++)
                //    {
                //        string usage = ds_RDR1.GetValue("Usage", i);
                //        if (!String.IsNullOrEmpty(usage.Trim()))
                //        {
                //            isTransfer = UsageBLL.ValidateTransfer(usage);
                //            if (isTransfer)
                //            {
                //                break;
                //            }
                //        }
                //    }

                //    if (isTransfer)
                //    {
                //        DBDataSource ds_ORDR = Form.DataSources.DBDataSources.Item("ORDR");
                //        if (Authorized)
                //        {
                //            if (!Canceling && !Closing)
                //            {
                //                ErrorMessage = "Impossível alterar pedido já autorizado";
                //                return false;
                //            }
                //        }
                //        else
                //        {
                //            Authorized = ds_ORDR.GetValue("Confirmed", ds_ORDR.Offset) == "Y";
                //        }
                //    }
                // }
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    if (Canceling || Closing)
                    {
                        return true;
                    }
                    DBDataSource ds_ORDR = Form.DataSources.DBDataSources.Item("ORDR");
                    DBDataSource ds_RDR1 = Form.DataSources.DBDataSources.Item("RDR1");
                    if (ds_ORDR.GetValue("Confirmed", ds_ORDR.Offset) == "Y") // Flag autorizadof
                    {
                        StockTransferModel model = new StockTransferModel();
                        model.CardCode = ds_ORDR.GetValue("CardCode", ds_ORDR.Offset);
                        //model.BaseDocEntry = Convert.ToInt32(ds_ORDR.GetValue("DocEntry", ds_ORDR.Offset));
                        model.DocType = BoObjectTypes.oOrders;
                        model.Items = new List<StockTransferItemModel>();

                        ds_RDR1 = Form.DataSources.DBDataSources.Item("RDR1");
                        for (int i = 0; i < ds_RDR1.Size; i++)
                        {
                            StockTransferItemModel item = new StockTransferItemModel();
                            item.ItemCode = ds_RDR1.GetValue("ItemCode", i);
                            item.Quantity = Convert.ToDouble(ds_RDR1.GetValue("Quantity", i).Replace(".", ","));
                            item.Usage = ds_RDR1.GetValue("Usage", i);
                            item.InvQty = Convert.ToDouble(ds_RDR1.GetValue("InvQty", i).Replace(".", ","));
                            item.PackQty = Convert.ToDouble(ds_RDR1.GetValue("PackQty", i).Replace(".", ","));
                            item.Price = Convert.ToDouble(ds_RDR1.GetValue("Price", i).Replace(".", ","));
                            item.WhsCode = ds_RDR1.GetValue("WhsCode", i);
                            model.Items.Add(item);
                        }
                        StockTransferRequestBLL stockTransferRequestBLL = new StockTransferRequestBLL();
                        if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                        {
                            if (stockTransferRequestBLL.Exists(Convert.ToInt32(ds_ORDR.GetValue("DocEntry", ds_ORDR.Offset)), 17))
                            {
                                var listOrdersLines = stockTransferRequestBLL.GetListOrdersLines(Convert.ToInt32(ds_ORDR.GetValue("DocEntry", ds_ORDR.Offset)));
                                if (listOrdersLines.Count != model.Items.Count)
                                {
                                    ErrorMessage = "Não é permitido alterar os itens após a geração da transferência!";
                                    return false;
                                }
                                foreach (var item in model.Items)
                                {
                                    var itemOrder = listOrdersLines.Find(i => i.ItemCode == item.ItemCode);
                                    if (itemOrder == null)
                                    {
                                        ErrorMessage = "Não é permitido inclusão de itens após a geração da transferência!";
                                        return false;
                                    }
                                }
                                foreach (var item in listOrdersLines)
                                {
                                    var itemOrder = model.Items.Find(i => i.ItemCode == item.ItemCode);
                                    if (itemOrder == null)
                                    {
                                        ErrorMessage = "Não é permitido exclusão de itens após a geraração da transferência!";
                                        return false;
                                    }
                                    if (itemOrder.InvQty != item.InvQty)
                                    {
                                        ErrorMessage = "Não é permitido alterar o campo Qtd.(UM estoque) após a geraração da transferência!!";
                                        return false;
                                    }
                                    if (itemOrder.PackQty != item.PackQty)
                                    {
                                        ErrorMessage = "Não é permitido alterar o campo Qtde. (CXs) após a geraração da transferência!!";
                                        return false;
                                    }
                                    if (itemOrder.Price != item.Price)
                                    {
                                        ErrorMessage = "Não é permitido alterar o campo Preço após a geraração da transferência!!";
                                        return false;
                                    }
                                    if (itemOrder.Usage != item.Usage)
                                    {
                                        ErrorMessage = "Não é permitido alterar o campo Utilização após a geraração da transferência!!";
                                        return false;
                                    }
                                    if (itemOrder.WhsCode != item.WhsCode)
                                    {
                                        ErrorMessage = "Não é permitido alterar o campo Depósito após a geraração da transferência!!";
                                        return false;
                                    }
                                    if (itemOrder.Quantity != item.Quantity)
                                    {
                                        ErrorMessage = "Não é permitido alterar o campo Quantidade após a geraração da transferência!!";
                                        return false;
                                    }
                                }

                                return true;
                            }
                        }
                        string msg = stockTransferRequestBLL.GenerateStockTransferRequest(model, ref StockTransferId);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            ErrorMessage = "Erro ao gerar solicitação de transferência: " + msg;
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        #endregion

        private void Application_StatusBarEvent(string text, BoStatusBarMessageType messageType)
        {
            if (text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                SBOApp.Application.StatusBar.SetText(ErrorMessage, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
