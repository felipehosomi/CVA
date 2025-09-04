using CVA.Fibra.ConciliacaoCartaCredito.Core.BLL;
using CVA.Fibra.ConciliacaoCartaCredito.Core.Model;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Forms;
using SBO.Hub.UI;
using SBO.Hub.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Fibra.ConciliacaoCartaCredito.Core.Forms
{
    public class FrmImport : BaseForm
    {
        public FrmImport()
        {
            FormCount++;
        }

        public FrmImport(ItemEvent itemEvent)
        {
            ItemEventInfo = itemEvent;
        }

        public override object Show()
        {
            Form form = (Form)base.Show();

            var cf_Account = form.ChooseFromLists.Item("cf_Account");
            var oCons = cf_Account.GetConditions();
            var oCon = oCons.Add();
            oCon.Alias = "Levels";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "5";
            cf_Account.SetConditions(oCons);

            return form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                try
                {
                    Form.Freeze(true);
                    if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                    {
                        if (ItemEventInfo.ItemUID == "bt_File")
                        {
                            DialogUtil dialogUtil = new DialogUtil();
                            Form.DataSources.UserDataSources.Item("ud_File").Value = dialogUtil.OpenFileDialog("Excel|*.xlsx");

                        }
                        if (ItemEventInfo.ItemUID == "bt_Import")
                        {
                            if (String.IsNullOrEmpty(Form.DataSources.UserDataSources.Item("ud_File").Value))
                            {
                                SBOApp.Application.SetStatusBarMessage("Arquivo deve ser informado");
                                return false;
                            }
                            if (String.IsNullOrEmpty(Form.DataSources.UserDataSources.Item("ud_Account").Value))
                            {
                                SBOApp.Application.SetStatusBarMessage("Conta Contábil deve ser informada");
                                return false;
                            }

                            DepositBLL depositBLL = new DepositBLL();
                            List<ImportLogModel> list = depositBLL.Import(Form.DataSources.UserDataSources.Item("ud_File").Value, Form.DataSources.UserDataSources.Item("ud_Account").Value);

                            DataTable dt_Log = Form.DataSources.DataTables.Item("dt_Log");
                            dt_Log.Clear();
                            dt_Log.CreateColumns(typeof(ImportLogModel));
                            dt_Log.FillTable<ImportLogModel>(list);

                            ((Grid)Form.Items.Item("gr_Log").Specific).AutoResizeColumns();
                            SBOApp.Application.SetStatusBarMessage("Importação finalizada!", SAPbouiCOM.BoMessageTime.bmt_Medium, false);
                        }
                    }
                    if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                    {
                        var cflEvent = (IChooseFromListEvent)ItemEventInfo;
                        var cflId = cflEvent.ChooseFromListUID;

                        var dataTable = cflEvent.SelectedObjects;
                        if (dataTable != null)
                        {
                            Form.DataSources.UserDataSources.Item("ud_Account").Value = dataTable.GetValue(0, 0).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SBOApp.Application.SetStatusBarMessage(ex.Message);
                }
                finally
                {
                    Form.Freeze(false);
                }
            }
            return true;
        }
    }
}
