using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Log envio de e-mail de boletos
    /// </summary>
    public class f2000003046 : BaseForm
    {
        Form Form;
        public static string Path;

        #region Constructor
        public f2000003046()
        {
            FormCount++;
        }

        public f2000003046(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003046(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003046(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
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
                        string cardCodeFrom = Form.DataSources.UserDataSources.Item("ud_PNDe").Value.Trim();
                        string cardCodeTo = Form.DataSources.UserDataSources.Item("ud_PNAte").Value.Trim();
                        string dateFrom = ((EditText)Form.Items.Item("et_DtDe").Specific).Value;
                        string dateTo = ((EditText)Form.Items.Item("et_DtAte").Specific).Value;
                        string status = Form.DataSources.UserDataSources.Item("ud_Status").Value.Trim();

                        DataTable dt_Boleto = Form.DataSources.DataTables.Item("dt_Boleto");
                        dt_Boleto.ExecuteQuery(BoletoBLL.GetLogBoletoSQL(cardCodeFrom, cardCodeTo, dateFrom, dateTo, status));
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
