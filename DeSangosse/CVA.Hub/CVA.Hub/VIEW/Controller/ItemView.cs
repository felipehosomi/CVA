using CVA.Hub.BLL;
using CVA.Hub.HELPER;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.VIEW.Controller
{
    [Form(B1Forms.Item, "CVA.Hub.VIEW.Form.ItemPartial.srf")]
    public class ItemView : DoverSystemFormBase
    {
        private EditText et_CodCentroCusto { get; set; }
        private EditText et_DescCentroCusto { get; set; }
        private ChooseFromList cf_CentroCusto { get; set; }

        protected SAPbouiCOM.Application _Application { get; set; }

        public ItemView(SAPbouiCOM.Application application)
        {
            _Application = application;
        }

        public override void OnInitializeComponent()
        {
            et_CodCentroCusto = this.GetItem("et_CtCod").Specific as EditText;
            et_DescCentroCusto = this.GetItem("et_CtDesc").Specific as EditText;

            et_DescCentroCusto.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
            et_DescCentroCusto.Item.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);

            cf_CentroCusto = this.UIAPIRawForm.ChooseFromLists.Item("cf_Centro");

            cf_CentroCusto.SetConditions(null);

            SAPbouiCOM.Conditions oCons = cf_CentroCusto.GetConditions();

            SAPbouiCOM.Condition oCon = oCons.Add();
            oCon.Alias = "DimCode";
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            oCon.CondVal = "2";

            cf_CentroCusto.SetConditions(oCons);

            this.OnCustomInitializeFormEvents();
        }

        public void OnCustomInitializeFormEvents()
        {
            _Application.ItemEvent += _Application_ItemEvent;
        }

        protected override void OnFormCloseBefore(SBOItemEventArg pVal, out bool BubbleEvent)
        {
            _Application.ItemEvent -= _Application_ItemEvent;
            BubbleEvent = true;
        }

        private void _Application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            if (!pVal.BeforeAction && pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                SAPbouiCOM.IChooseFromListEvent oCFLEvento = ((SAPbouiCOM.IChooseFromListEvent)(pVal));
                SAPbouiCOM.DataTable oDataTable = oCFLEvento.SelectedObjects;

                if (pVal.ItemUID == "et_CtCod")
                {
                    if (oDataTable != null)
                    {
                        if (oDataTable.Rows.Count > 0)
                        {
                            if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_FIND_MODE)
                            {
                                string code = oDataTable.GetValue("PrcCode", 0).ToString();
                                string desc = oDataTable.GetValue("PrcName", 0).ToString();

                                try
                                {
                                    et_CodCentroCusto.Value = code;
                                }
                                catch { }
                                try
                                {
                                    et_DescCentroCusto.Value = desc;
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            BubbleEvent = true;
        }
    }
}
