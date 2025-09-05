using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Envio de e-mail de boletos
    /// </summary>
    public class f2000003045 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f2000003045()
        {
            FormCount++;
        }

        public f2000003045(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003045(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003045(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();

            ChooseFromList cf_PNDe = Form.ChooseFromLists.Item("cf_PNDe");
            Conditions pConditions = cf_PNDe.GetConditions();
            cf_PNDe.SetConditions(pConditions);
            pConditions = cf_PNDe.GetConditions();
            Condition condition = pConditions.Add();
            condition.Alias = "CardType";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "C";
            cf_PNDe.SetConditions(pConditions);

            ChooseFromList cf_PNAte = Form.ChooseFromLists.Item("cf_PNAte");
            pConditions = cf_PNAte.GetConditions();
            cf_PNAte.SetConditions(pConditions);
            pConditions = cf_PNAte.GetConditions();
            condition = pConditions.Add();
            condition.Alias = "CardType";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "C";
            cf_PNAte.SetConditions(pConditions);

            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Find")
                    {
                        Form.Freeze(true);
                        string cardCodeFrom = Form.DataSources.UserDataSources.Item("ud_PNDe").Value.Trim();
                        string cardCodeTo = Form.DataSources.UserDataSources.Item("ud_PNAte").Value.Trim();
                        string dateFrom = ((EditText)Form.Items.Item("et_DtDe").Specific).Value;
                        string dateTo = ((EditText)Form.Items.Item("et_DtAte").Specific).Value;
                        string status = Form.DataSources.UserDataSources.Item("ud_Status").Value.Trim();

                        DataTable dt_Boleto = Form.DataSources.DataTables.Item("dt_Boleto");
                        dt_Boleto.ExecuteQuery(BoletoBLL.GetBoletoSQL(cardCodeFrom, cardCodeTo, dateFrom, dateTo, status));

                        Grid gr_Boleto = Form.Items.Item("gr_Boleto").Specific as Grid;
                        gr_Boleto.Columns.Item("Enviar").Type = BoGridColumnType.gct_CheckBox;
                        gr_Boleto.Columns.Item("Enviar").TitleObject.Caption = "";
                        for (int i = 1; i < gr_Boleto.Columns.Count; i++)
                        {
                            gr_Boleto.Columns.Item(i).Editable = false;
                        }
                        Form.Freeze(false);
                    }
                    if (ItemEventInfo.ItemUID == "bt_Send")
                    {
                        BoletoBLL.SendEmails(Form.DataSources.DataTables.Item("dt_Boleto"));
                        DataTable dt_Boleto = Form.DataSources.DataTables.Item("dt_Boleto");
                        dt_Boleto.Rows.Clear();
                        SBOApp.Application.MessageBox("E-mails sendo enviados. Verifique o log para mais informações");
                    }
                    if (ItemEventInfo.ItemUID == "bt_Select")
                    {
                        Form.Freeze(true);
                        Button bt_Select = Form.Items.Item("bt_Select").Specific as Button;
                        string select;
                        if (bt_Select.Caption == "Selecionar Todos")
                        {
                            select = "Y";
                            bt_Select.Caption = "Selecionar Nenhum";
                        }
                        else
                        {
                            select = "N";
                            bt_Select.Caption = "Selecionar Todos";
                        }

                        DataTable dt_Boleto = Form.DataSources.DataTables.Item("dt_Boleto");
                        for (int i = 0; i < dt_Boleto.Rows.Count; i++)
                        {
                            dt_Boleto.SetValue("Enviar", i, select);
                        }
                        Form.Freeze(false);
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    this.ChooseFromList();
                }
            }

            return true;
        }

        #region ChooseFromList
        private void ChooseFromList()
        {
            IChooseFromListEvent oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvento.ChooseFromListUID);
            DataTable oDataTable = oCFLEvento.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    string value = oDataTable.GetValue(0, 0).ToString();
                    Form.DataSources.UserDataSources.Item(oCFL.UniqueID.Replace("cf_", "ud_")).Value = value;
                }
            }
        }
        #endregion
    }
}
