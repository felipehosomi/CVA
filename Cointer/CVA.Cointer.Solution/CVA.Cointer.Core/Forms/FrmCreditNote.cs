using CVA.Cointer.Core.BLL;
using CVA.Cointer.Core.DAO;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.DAO;
using SBO.Hub.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Cointer.Core.Forms
{
    [Form("179")]
    public class FrmCreditNote : SystemForm
    {
        public FrmCreditNote(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public FrmCreditNote(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Item bt_CopyFrom = Form.Items.Item("10000330");
                    Item it_Inv = Form.Items.Add("bt_Inv", BoFormItemTypes.it_BUTTON);

                    it_Inv.Visible = false;
                    it_Inv.Width = 100;
                    it_Inv.Height = bt_CopyFrom.Height;
                    it_Inv.Top = bt_CopyFrom.Top;
                    it_Inv.Left = bt_CopyFrom.Left - it_Inv.Width - 5;
                    it_Inv.LinkTo = "10000330";

                    ((Button)it_Inv.Specific).Caption = "Gerar NF";
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Inv")
                    {
                        if (Form.Mode == BoFormMode.fm_OK_MODE)
                        {
                            int docEntry = Convert.ToInt32(Form.DataSources.DBDataSources.Item("ORIN").GetValue("DocEntry", 0));

                            InvoiceBLL invoiceBLL = new InvoiceBLL();
                            string error = invoiceBLL.GenerateByCreditNote(docEntry);
                            if (!String.IsNullOrEmpty(error))
                            {
                                SBOApp.Application.SetStatusBarMessage(error);
                            }
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("Por favor, salve os dados antes de continuar");
                        }
                    }
                }
            }

            return true;
        }

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    int docEntry = Convert.ToInt32(Form.DataSources.DBDataSources.Item("ORIN").GetValue("DocEntry", 0));
                    Form.Items.Item("bt_Inv").Visible = CrudDAO.ExecuteScalar(String.Format(Hana.Invoice_GetByCreditNote, docEntry)) == null;
                }
            }

            return true;
        }
    }
}
