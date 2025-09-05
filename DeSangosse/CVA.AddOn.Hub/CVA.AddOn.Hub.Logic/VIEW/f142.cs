using SAPbouiCOM;
using CVA.AddOn.Common.Util;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;
using CVA.AddOn.Hub.Logic.BLL;
using CVA.Hub.BLL;
using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.MODEL;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    /// <summary>
    /// Pedido de Compra
    /// </summary>
    public class f142 : DocumentBaseView
    {
        #region Constructor
        private static string CardCode = String.Empty;

        public f142()
        {
            FormCount++;
        }

        public f142(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f142(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f142(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "4" || ItemEventInfo.ItemUID == "54")
                    {
                        if (Form.Mode == BoFormMode.fm_ADD_MODE)
                        {
                            EditText et_CardCode = (EditText)Form.Items.Item("4").Specific;

                            if (et_CardCode.Value != CardCode)
                            {
                                CardCode = et_CardCode.Value;
                                DataTable dt_Docs = Form.DataSources.DataTables.Item("dt_Docs");
                                dt_Docs.Rows.Clear();
                            }
                        }
                        this.SetFreightFieldsVisibility();
                    }
                    if (ItemEventInfo.ItemUID == "gr_Docs" && ItemEventInfo.ColUID == "Chave Origem")
                    {
                        this.SetDocEntryAndSerial();
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    CardCode = String.Empty;
                    ErrorMessage = String.Empty;
                    CreateFreightFields();
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.CharPressed == 13)
                    {
                        if (ItemEventInfo.ItemUID == "et_Doc")
                        {
                            this.AddFreightRow();
                            return false;
                        }
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "fl_DocFret")
                    {
                        Form.PaneLevel = 99;
                        DBDataSource dt_OPOR = Form.DataSources.DBDataSources.Item("OPOR");
                        string cancelado = dt_OPOR.GetValue("CANCELED", dt_OPOR.Offset);

                        Form.Items.Item("bt_Doc").Visible = cancelado != "Y";
                    }
                    if (ItemEventInfo.ItemUID == "bt_Doc")
                    {
                        this.AddFreightRow();
                    }
                    if (ItemEventInfo.ItemUID == "1" && Form.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        DBDataSource dt_OPOR = Form.DataSources.DBDataSources.Item("OPOR");
                        string rateioEfetuado = dt_OPOR.GetValue("U_CVA_Rateio_Frete", dt_OPOR.Offset);
                        string cancelado = dt_OPOR.GetValue("CANCELED", dt_OPOR.Offset);

                        if (rateioEfetuado == "Y" && cancelado != "Y")
                        {
                            SBOApp.Application.SetStatusBarMessage("Impossível alterar, frete rateado. Necessário cancelamento do pedido");
                            return false;
                        }
                    }
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        DataTable dt_Docs = Form.DataSources.DataTables.Item("dt_Docs");
                        for (int i = 0; i < dt_Docs.Rows.Count; i++)
                        {
                            int docEntry = (int)dt_Docs.GetValue("Chave Origem", i);
                            int docNum = (int)dt_Docs.GetValue("Nº Origem", i);
                            if (docEntry != 0 && docNum == 0)
                            {
                                SBOApp.Application.SetStatusBarMessage($"Chave Origem {docEntry} não encontrada ou já referenciada em outro pedido");
                                return false;
                            }
                        }
                    }
                }
            }

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (base.FormDataEvent())
            {
                if (!BusinessObjectInfo.BeforeAction)
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                    {
                        BlockCancel = false;
                        this.SetFreightFieldsVisibility();
                        DBDataSource dt_OPOR = Form.DataSources.DBDataSources.Item("OPOR");
                        DataTable dt_Docs = Form.DataSources.DataTables.Item("dt_Docs");
                        dt_Docs.Rows.Clear();
                        dt_Docs.ExecuteQuery(String.Format(Query.PedidoCompra_GetDocFrete, dt_OPOR.GetValue("DocEntry", dt_OPOR.Offset)));
                        Grid gr_Docs = (Grid)Form.Items.Item("gr_Docs").Specific;
                        gr_Docs.DataTable = dt_Docs;
                        for (int i = 0; i < gr_Docs.Columns.Count; i++)
                        {
                            gr_Docs.Columns.Item(i).Editable = false;
                        }
                        CardCode = dt_OPOR.GetValue("CardCode", dt_OPOR.Offset).Trim();
                        this.SetGridLayout();
                    }
                    if (BusinessObjectInfo.ActionSuccess && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                    {
                        //form = SBOApp.Application.Forms.ActiveForm;
                        PedidoCompraBLL pedidoCompraBLL = new PedidoCompraBLL();
                        int docEntry = Convert.ToInt32(DocumentXmlController.GetXmlField(BusinessObjectInfo));

                        string msg = pedidoCompraBLL.InsereDocsFrete(docEntry, Form.DataSources.DataTables.Item("dt_Docs"));
                        if (!String.IsNullOrEmpty(msg))
                        {
                            SBOApp.Application.MessageBox(msg);
                        }
                        else
                        {
                            DataTable dt_Docs = Form.DataSources.DataTables.Item("dt_Docs");
                            dt_Docs.Rows.Clear();
                        }
                    }
                }
                else
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                    {
                        if (Form.Items.Item("fl_DocFret").Visible)
                        {
                            DBDataSource dt_POR1 = Form.DataSources.DBDataSources.Item("POR1");
                            for (int i = 0; i < dt_POR1.Size; i++)
                            {
                                double valorUnitario;
                                double.TryParse(dt_POR1.GetValue("Price", i), out valorUnitario);
                                if (valorUnitario == 0)
                                {
                                    ErrorMessage = "Valor unitário deve ser maior do que 0";
                                    return false;
                                }
                            }
                        }
                    }
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE && BlockCancel)
                    {
                        ParceiroNegocioBLL parceiroNegocioBLL = new ParceiroNegocioBLL();
                        if (parceiroNegocioBLL.GetNomeGrupo(CardCode).ToUpper().Trim() == "TRANSPORTADORA")
                        {
                            ErrorMessage = "Informe o motivo do cancelamento";
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        #endregion

        public static bool OnClickMenuCancel(Form form)
        {
            DBDataSource dt_OPOR = form.DataSources.DBDataSources.Item("OPOR");
            string motivoCancelamento = dt_OPOR.GetValue("U_CVA_MotivoCancel", dt_OPOR.Offset).Trim();
            if (String.IsNullOrEmpty(motivoCancelamento))
            {
                ParceiroNegocioBLL parceiroNegocioBLL = new ParceiroNegocioBLL();
                if (parceiroNegocioBLL.GetNomeGrupo(dt_OPOR.GetValue("CardCode", dt_OPOR.Offset)).ToUpper().Trim() == "TRANSPORTADORA")
                {
                    BlockCancel = true;
                    SBOApp.Application.SetStatusBarMessage("Informe o motivo do cancelamento");
                    return false;
                }
                return true;
            }
            else
            {
                PedidoCompraBLL.SetMotivoCancelamento(Convert.ToInt32(dt_OPOR.GetValue("DocEntry", dt_OPOR.Offset)), motivoCancelamento);

                BlockCancel = false;
                return true;
            }
        }


        public static void OnClickMenuAdd(Form form)
        {
            DataTable dt_Docs = form.DataSources.DataTables.Item("dt_Docs");
            dt_Docs.Rows.Clear();
        }

        #region PrivateMethods
        private void SetFreightFieldsVisibility()
        {
            EditText et_CardCode = (EditText)Form.Items.Item("4").Specific;
            bool visible = false;
            if (!String.IsNullOrEmpty(et_CardCode.Value))
            {
                ParceiroNegocioBLL parceiroNegocioBLL = new ParceiroNegocioBLL();
                if (parceiroNegocioBLL.GetNomeGrupo(et_CardCode.Value).ToUpper().Trim() == "TRANSPORTADORA")
                {
                    visible = true;
                }
            }

            Form.Items.Item("st_TpFrete").Visible = visible;
            Form.Items.Item("cb_TpFrete").Visible = visible;
            Form.Items.Item("fl_DocFret").Visible = visible;
            Form.Items.Item("st_Marca").Visible = visible;
            Form.Items.Item("cb_Marca").Visible = visible;
        }

        #region RightClickEvent
        public override bool RightClickEvent()
        {
            if (Form.Mode == BoFormMode.fm_ADD_MODE)
            {
                if (ContextMenuInfo.BeforeAction && ContextMenuInfo.EventType == BoEventTypes.et_RIGHT_CLICK)
                {
                    if (ContextMenuInfo.ItemUID == "gr_Docs")
                    {
                        if (ContextMenuInfo.Row >= 0)
                        {
                            Grid gr_Docs = (Grid)Form.Items.Item("gr_Docs").Specific;
                            gr_Docs.SetCellFocus(ContextMenuInfo.Row, 1);
                            this.CreateRightClickMenuItem();
                        }
                    }
                    else
                    {
                        if (Form.Menu.Exists("M9142"))
                        {
                            Form.Menu.RemoveEx("M9142");
                        }
                    }
                }
            }
            return true;
        }

        private void CreateRightClickMenuItem()
        {
            try
            {
                if (!Form.Menu.Exists("M9142"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "M9142";
                    oCreationPackage.String = "Remover Linha Frete";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
        }
        #endregion

        #region SetDocEntryAndSerial
        private void SetDocEntryAndSerial()
        {
            DocumentoBLL documentoBLL = new DocumentoBLL();
            Grid gr_Docs = (Grid)Form.Items.Item("gr_Docs").Specific;

            int docType;
            Int32.TryParse(gr_Docs.DataTable.GetValue("Tipo Origem", ItemEventInfo.Row).ToString(), out docType);
            int docNum;
            Int32.TryParse(gr_Docs.DataTable.GetValue("Chave Origem", ItemEventInfo.Row).ToString(), out docNum);
            if (docType == 0 || docNum == 0)
            {
                return;
            }

            string tipoFrete = ((ComboBox)Form.Items.Item("cb_TpFrete").Specific).Value;
            if (String.IsNullOrEmpty(tipoFrete.Trim()))
            {
                SBOApp.Application.SetStatusBarMessage("Favor informar o tipo do frete!");
                return;
            }

            string marca = ((ComboBox)Form.Items.Item("cb_Marca").Specific).Value;
            if(tipoFrete.Trim() == "3" && String.IsNullOrEmpty(marca.Trim()))
            {
                SBOApp.Application.SetStatusBarMessage("Favor informar a marca do frete dedicado!");
                return;
            }

            DocumentoModel docModel = documentoBLL.GetDocEntryAndSerial(docType, docNum);
            if (docModel.DocEntry != 0)
            {
                // Se for frete normal, verifica se o documento já não está em outro pedido
                //"1", "Frete Fracionado
                //"3", "Frete Dedicado"
                //"4", "Frete Emergencial"
                if (tipoFrete.Trim() == "1" || tipoFrete.Trim() == "3" || tipoFrete.Trim() == "4")
                {
                    PedidoCompraBLL pedidoCompraBLL = new PedidoCompraBLL();
                    string msg = pedidoCompraBLL.VerificaExistente(docType, docModel.DocEntry);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        SBOApp.Application.SetStatusBarMessage(msg);
                        return;
                    }
                }

                if (docModel.Serial != 0)
                {
                    gr_Docs.DataTable.SetValue("Nº Origem", ItemEventInfo.Row, docModel.Serial);
                }
                else
                {
                    gr_Docs.DataTable.SetValue("Nº Origem", ItemEventInfo.Row, docNum);
                }

                gr_Docs.DataTable.SetValue("DocEntry", ItemEventInfo.Row, docModel.DocEntry);
                gr_Docs.DataTable.SetValue("Serial", ItemEventInfo.Row, docModel.Serial);
            }
            else
            {
                SBOApp.Application.SetStatusBarMessage("Nenhum documento em aberto encontrado!");
            }
        }
        #endregion

        #region CreateFreightFields
        private void CreateFreightFields()
        {
            Item it_Ref = null;
            Item it_Text = default(Item);
            Item it_Combo = default(Item);
            Item it_Edit = default(Item);
            Item it_Button = default(Item);

            it_Ref = Form.Items.Item("70");

            it_Text = Form.Items.Add("st_TpFrete", BoFormItemTypes.it_STATIC);
            it_Text.Top = it_Ref.Top + 15;
            it_Text.Height = it_Ref.Height;
            it_Text.Left = it_Ref.Left;
            it_Text.Width = it_Ref.Width;
            it_Text.Visible = false;
            ((StaticText)it_Text.Specific).Caption = "Tipo Frete";

            it_Combo = Form.Items.Add("cb_TpFrete", BoFormItemTypes.it_COMBO_BOX);
            it_Combo.Top = it_Ref.Top + 15;
            it_Combo.Height = it_Ref.Height;
            it_Combo.Left = it_Ref.Left + it_Ref.Width;
            it_Combo.Width = it_Ref.Width;
            it_Combo.Visible = false;
            it_Combo.DisplayDesc = true;
            ((ComboBox)it_Combo.Specific).DataBind.SetBound(true, "OPOR", "U_CVA_Tipo_Frete");

            it_Ref = Form.Items.Item("st_TpFrete");

            it_Text = Form.Items.Add("st_Marca", BoFormItemTypes.it_STATIC);
            it_Text.Top = it_Ref.Top + 15;
            it_Text.Height = it_Ref.Height;
            it_Text.Left = it_Ref.Left;
            it_Text.Width = it_Ref.Width;
            it_Text.Visible = false;
            ((StaticText)it_Text.Specific).Caption = "Marca";

            it_Ref = Form.Items.Item("cb_TpFrete");

            it_Combo = Form.Items.Add("cb_Marca", BoFormItemTypes.it_COMBO_BOX);
            it_Combo.Top = it_Ref.Top + 15;
            it_Combo.Height = it_Ref.Height;
            it_Combo.Left = it_Ref.Left;
            it_Combo.Width = it_Ref.Width;
            it_Combo.Visible = false;
            it_Combo.DisplayDesc = true;
            ((ComboBox)it_Combo.Specific).DataBind.SetBound(true, "OPOR", "U_CVA_Frete_Marca");

            Folder fl_DocFret = null;
            Item it_Folder = default(Item);

            it_Ref = Form.Items.Item("1980000501");
            it_Folder = Form.Items.Add("fl_DocFret", BoFormItemTypes.it_FOLDER);
            it_Folder.Top = it_Ref.Top;
            it_Folder.Height = it_Ref.Height;
            it_Folder.Left = it_Ref.Left + it_Ref.Width;
            it_Folder.Width = it_Ref.Width;
            it_Folder.Visible = true;
            fl_DocFret = (Folder)it_Folder.Specific;
            fl_DocFret.Caption = "Docs. Relacionados";

            it_Folder.Visible = false;

            Form.DataSources.UserDataSources.Add("ds_CvaDoc", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            fl_DocFret.DataBind.SetBound(true, "", "ds_CvaDoc");

            fl_DocFret.GroupWith(it_Ref.UniqueID);

            fl_DocFret.Pane = 99;
            it_Ref = Form.Items.Item("38");

            Item it_GridDocs = Form.Items.Add("gr_Docs", BoFormItemTypes.it_GRID);
            it_GridDocs.FromPane = fl_DocFret.Pane;
            it_GridDocs.ToPane = fl_DocFret.Pane;
            it_GridDocs.Top = it_Ref.Top;
            it_GridDocs.Left = it_Ref.Left;
            it_GridDocs.Width = it_Ref.Width;
            it_GridDocs.Height = it_Ref.Height;
            //it_GridDocs.Enabled = false;

            it_GridDocs.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

            it_Text = Form.Items.Add("st_TpDoc", BoFormItemTypes.it_STATIC);
            it_Text.FromPane = fl_DocFret.Pane;
            it_Text.ToPane = fl_DocFret.Pane;
            it_Text.Top = it_GridDocs.Top - 15;
            it_Text.Left = it_GridDocs.Left;
            it_Text.Visible = true;
            it_Text.Width = 100;

            ((StaticText)it_Text.Specific).Caption = "Tipo Documento";

            it_Combo = Form.Items.Add("cb_TpDoc", BoFormItemTypes.it_COMBO_BOX);
            it_Combo.FromPane = fl_DocFret.Pane;
            it_Combo.ToPane = fl_DocFret.Pane;
            it_Combo.Top = it_GridDocs.Top - 15;
            it_Combo.Left = it_GridDocs.Left + it_Text.Width;
            it_Combo.Width = 120;

            it_Combo.DisplayDesc = true;
            ((ComboBox)it_Combo.Specific).AddValidValues("@CVA_DOC_FRETE", "DocType");
            //((ComboBox)it_Combo.Specific).DataBind.SetBound(true, "OPOR", "U_CVA_Tipo_Frete");

            it_Text = Form.Items.Add("st_Doc", BoFormItemTypes.it_STATIC);
            it_Text.FromPane = fl_DocFret.Pane;
            it_Text.ToPane = fl_DocFret.Pane;
            it_Text.Top = it_GridDocs.Top - 15;
            it_Text.Left = it_Combo.Left + it_Combo.Width + 10;
            it_Text.Width = 100;
            it_Text.LinkTo = "cb_TpDoc";

            ((StaticText)it_Text.Specific).Caption = "Chave Documento";

            it_Edit = Form.Items.Add("et_Doc", BoFormItemTypes.it_EDIT);
            it_Edit.FromPane = fl_DocFret.Pane;
            it_Edit.ToPane = fl_DocFret.Pane;
            it_Edit.Top = it_GridDocs.Top - 15;
            it_Edit.Left = it_Text.Left + it_Text.Width + 15;
            it_Edit.Width = 80;
            it_Edit.LinkTo = "st_Doc";
            //((ComboBox)it_Combo.Specific).DataBind.SetBound(true, "OPOR", "U_CVA_Tipo_Frete");

            it_Button = Form.Items.Add("bt_Doc", BoFormItemTypes.it_BUTTON);
            it_Button.FromPane = fl_DocFret.Pane;
            it_Button.ToPane = fl_DocFret.Pane;
            it_Button.Top = it_GridDocs.Top - 20;
            it_Button.Left = it_Edit.Left + it_Edit.Width + 10;
            it_Button.LinkTo = "et_Doc";

            ((Button)it_Button.Specific).Caption = "Adicionar";

            DataTable dt_Docs = Form.DataSources.DataTables.Add("dt_Docs");
            dt_Docs.ExecuteQuery(String.Format(Query.PedidoCompra_GetDocFrete, 0));

            Grid gr_Docs = (Grid)it_GridDocs.Specific;
            gr_Docs.DataTable = dt_Docs;
            for (int i = 0; i < gr_Docs.Columns.Count; i++)
            {
                gr_Docs.Columns.Item(i).Editable = false;
            }

            this.SetGridLayout();
        }
        #endregion

        #region AddFreightRow
        private void AddFreightRow()
        {
            try
            {
                Form.Freeze(true);
                string tipoFrete = ((ComboBox)Form.Items.Item("cb_TpFrete").Specific).Value;
                if (String.IsNullOrEmpty(tipoFrete.Trim()))
                {
                    SBOApp.Application.SetStatusBarMessage("Favor informar o tipo do frete!");
                    return;
                }

                string marca = ((ComboBox)Form.Items.Item("cb_Marca").Specific).Value;
                if (tipoFrete.Trim() == "3" && String.IsNullOrEmpty(marca.Trim()))
                {
                    SBOApp.Application.SetStatusBarMessage("Favor informar a marca do frete dedicado!");
                    return;
                }

                ComboBox cb_TpDoc = (ComboBox)Form.Items.Item("cb_TpDoc").Specific;
                EditText et_Doc = (EditText)Form.Items.Item("et_Doc").Specific;

                if (String.IsNullOrEmpty(cb_TpDoc.Value))
                {
                    SBOApp.Application.SetStatusBarMessage("Tipo documento deve ser informado!");
                    return;
                }

                int docNum;
                if (!Int32.TryParse(et_Doc.Value, out docNum))
                {
                    SBOApp.Application.SetStatusBarMessage("Chave do documento deve ser informada!");
                    return;
                }

                DataTable dt_Docs = Form.DataSources.DataTables.Item("dt_Docs");
                for (int i = 0; i < dt_Docs.Rows.Count; i++)
                {
                    string objType = dt_Docs.GetValue("Tipo Origem", i).ToString();
                    int docKey;
                    Int32.TryParse(dt_Docs.GetValue("Chave Origem", i).ToString(), out docKey);
                    if (objType == cb_TpDoc.Value && docNum == docKey)
                    {
                        SBOApp.Application.SetStatusBarMessage("Documento já adicionado!");
                        return;
                    }
                }

                DocumentoBLL documentoBLL = new DocumentoBLL();
                DocumentoModel docModel = documentoBLL.GetDocEntryAndSerial(Convert.ToInt32(cb_TpDoc.Value), docNum);
                if (docModel.DocEntry != 0)
                {
                    // Se for frete normal, verifica se o documento já não está em outro pedido
                    //"1", "Frete Fracionado
                    //"3", "Frete Dedicado"
                    //"4", "Frete Emergencial"
                    if (tipoFrete.Trim() == "1" || tipoFrete.Trim() == "3" || tipoFrete.Trim() == "4")
                    {
                        PedidoCompraBLL pedidoCompraBLL = new PedidoCompraBLL();
                        string msg = pedidoCompraBLL.VerificaExistente(Convert.ToInt32(cb_TpDoc.Value), docModel.DocEntry);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            SBOApp.Application.SetStatusBarMessage(msg);
                            return;
                        }
                    }

                    dt_Docs.Rows.Add();

                    dt_Docs.SetValue("Tipo Origem", dt_Docs.Rows.Count - 1, Convert.ToInt32(cb_TpDoc.Value));
                    dt_Docs.SetValue("Chave Origem", dt_Docs.Rows.Count - 1, docNum);
                    
                    if (docModel.Serial != 0)
                    {
                        dt_Docs.SetValue("Nº Origem", dt_Docs.Rows.Count - 1, docModel.Serial);
                    }
                    else
                    {
                        dt_Docs.SetValue("Nº Origem", dt_Docs.Rows.Count - 1, docNum);
                    }
                    dt_Docs.SetValue("DocEntry", dt_Docs.Rows.Count - 1, docModel.DocEntry);
                    dt_Docs.SetValue("Serial", dt_Docs.Rows.Count - 1, docModel.Serial);

                    et_Doc.Value = String.Empty;
                }
                else
                {
                    SBOApp.Application.SetStatusBarMessage("Nenhum documento em aberto encontrado!");
                    return;
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Erro geral: " + ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
        }
        #endregion

        private void SetGridLayout()
        {
            Grid gr_Docs = (Grid)Form.Items.Item("gr_Docs").Specific;
            gr_Docs.Columns.Item("Tipo Origem").Type = BoGridColumnType.gct_ComboBox;
            ((ComboBoxColumn)gr_Docs.Columns.Item("Tipo Origem")).AddValidValues("@CVA_DOC_FRETE", "DocType");
            ((ComboBoxColumn)gr_Docs.Columns.Item("Tipo Origem")).DisplayType = BoComboDisplayType.cdt_Description;

            gr_Docs.Columns.Item("Code").Visible = false;
            gr_Docs.Columns.Item("DocEntry").Visible = false;
            gr_Docs.Columns.Item("Serial").Visible = false;
        }
        #endregion


    }
}
