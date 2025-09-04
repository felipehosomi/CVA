using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.Alessi.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    /// <summary>
    /// Importação folha de pagamento
    /// </summary>
    public class f2000003002 : BaseForm
    {
        #region Constructor
        public f2000003002()
        {
            FormCount++;
        }

        public f2000003002(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003002(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003002(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_File")
                    {
                        DialogUtil dialog = new DialogUtil();
                        string file = dialog.OpenFileDialog("Arquivos csv|*.csv;");
                        EditText et_File = Form.Items.Item("et_File").Specific as EditText;
                        et_File.Value = file;
                    }
                    if (ItemEventInfo.ItemUID == "bt_Imp")
                    {
                        this.Importar();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    IChooseFromListEvent chooseFromListEvent = (IChooseFromListEvent)(ItemEventInfo);
                    
                    if (chooseFromListEvent.SelectedObjects != null)
                    {
                        try
                        {
                            EditText et_Branch = Form.Items.Item("et_Branch").Specific as EditText;
                            et_Branch.Value = chooseFromListEvent.SelectedObjects.GetValue(0, 0).ToString();
                        }
                        catch { }
                    }
                }
            }

            return true;
        }

        private void Importar()
        {
            Form.Freeze(true);

            EditText et_File = Form.Items.Item("et_File").Specific as EditText;
            EditText et_Branch = Form.Items.Item("et_Branch").Specific as EditText;
            EditText et_DocDate = Form.Items.Item("et_DocDate").Specific as EditText;
            EditText et_DueDate = Form.Items.Item("et_DueDate").Specific as EditText;

            int bplId;
            DateTime docDate;
            DateTime dueDate;

            if (!Int32.TryParse(et_Branch.Value, out bplId))
            {
                SBOApp.Application.SetStatusBarMessage("Filial deve ser informada!");
                return;
            }
            if (!DateTime.TryParseExact(et_DocDate.Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out docDate))
            {
                SBOApp.Application.SetStatusBarMessage("Data lançamento deve ser informada!");
                return;
            }
            if (!DateTime.TryParseExact(et_DueDate.Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dueDate))
            {
                SBOApp.Application.SetStatusBarMessage("Data vencimento deve ser informada!");
                return;
            }
            if (String.IsNullOrEmpty(et_File.Value))
            {
                SBOApp.Application.SetStatusBarMessage("Arquivo deve ser informado!");
                return;
            }

            string lcmId = String.Empty;
            string msg = FolhaPagamentoBLL.Gerar(bplId, docDate, dueDate, et_File.Value, ref lcmId);
            if (!String.IsNullOrEmpty(msg))
            {
                SBOApp.Application.SetStatusBarMessage(msg);
            }
            else
            {
                SBOApp.Application.MessageBox("Importação efetuada com sucesso! Nrº do LCM gerado: " + lcmId);
            }

            Form.Freeze(false);
        }
    }
}