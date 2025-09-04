using CVA.Core.TransportLCM.BLL.BasePortal;
using CVA.Core.TransportLCM.HELPER;
using Dover.Framework.Form;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace CVA.Core.TransportLCM.VIEW.Controller
{
    [Form(B1Forms.PedidoCompra, "CVA.Core.TransportLCM.VIEW.Form.PedidoCompraFormPartial.srf")]
    public class PedidoCompraView : DoverSystemFormBase
    {
        private SAPbouiCOM.Application _application { get; set; }
        private ApprovalGroupAccountBLL _ApprovalGroupAccountBLL { get; set; }
        private Matrix mt_Items { get; set; }
        private EditText et_Appr { get; set; }
        private EditText et_CreateDate { get; set; }
        private Button bt_Save { get; set; }
        private StaticText st_CreateDate { get; set; }
        private string ErrorMessage { get; set; }

        public PedidoCompraView(SAPbouiCOM.Application application, ApprovalGroupAccountBLL approvalGroupAccountBLL)
        {
            _application = application;
            _ApprovalGroupAccountBLL = approvalGroupAccountBLL;
        }

        public override void OnInitializeComponent()
        {
            mt_Items = this.GetItem("38").Specific as Matrix;
            et_Appr = this.GetItem("et_Appr").Specific as EditText;
            bt_Save = this.GetItem("1").Specific as Button;

            _application.StatusBarEvent += _application_StatusBarEvent;
        }

        protected override void OnFormLoadAfter(SBOItemEventArg pVal)
        {
            try
            {
                var mode = UIAPIRawForm.Mode;

                Item oItem = UIAPIRawForm.Items.Add("st_CD", BoFormItemTypes.it_STATIC);
                Item oItemAux = UIAPIRawForm.Items.Item("86");

                oItem.Left = oItemAux.Left;
                oItem.Top = oItemAux.Top + oItemAux.Height + 1;
                oItem.Width = oItemAux.Width;

                st_CreateDate = oItem.Specific as StaticText;
                st_CreateDate.Caption = "Data de criação";

                oItem = UIAPIRawForm.Items.Add("et_CD", BoFormItemTypes.it_EDIT);
                oItemAux = UIAPIRawForm.Items.Item("46");

                oItem.Left = oItemAux.Left;
                oItem.Top = oItemAux.Top + oItemAux.Height + 1;
                oItem.Width = oItemAux.Width;
                oItem.Enabled = false;

                et_CreateDate = oItem.Specific as EditText;
                et_CreateDate.DataBind.SetBound(true, "OPOR", "U_CVA_CreateDate");
                UIAPIRawForm.Mode = mode;
            }
            catch
            {
                UIAPIRawForm.Items.Item("et_CD").Enabled = false;
                et_CreateDate = (EditText)UIAPIRawForm.Items.Item("et_CD").Specific;
                et_CreateDate.DataBind.SetBound(true, "OPOR", "U_CVA_CreateDate");
            }
        }

        protected override void OnFormCloseBefore(SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            _application.StatusBarEvent -= _application_StatusBarEvent;
        }

        private void _application_StatusBarEvent(string text, BoStatusBarMessageType messageType)
        {
            if (text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                _application.SetStatusBarMessage(ErrorMessage);
            }
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = this.ValidateGroupAccount();
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = this.ValidateGroupAccount();
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            try
            {
                et_CreateDate.Value = UIAPIRawForm.DataSources.DBDataSources.Item("OPOR").GetValue("CreateDate", 0);
                UIAPIRawForm.Items.Item("46").Click();
                UIAPIRawForm.Items.Item("et_CD").Enabled = false;
            }
            catch { }
        }

        private bool ValidateGroupAccount()
        {
            try
            {
                string account = String.Empty;
                string accountList = String.Empty;
                for (int i = 1; i < mt_Items.RowCount; i++)
                {
                    EditText et_Conta = mt_Items.GetCellSpecific("29", i) as EditText;
                    if (!String.IsNullOrEmpty(et_Conta.Value.Trim()))
                    {
                        accountList += $", '{et_Conta.Value}'";
                        account = et_Conta.Value;
                    }
                }

                if (!String.IsNullOrEmpty(accountList))
                {
                    accountList = accountList.Substring(2);

                    int count = _ApprovalGroupAccountBLL.GetDistinctGroups(accountList);
                    if (count > 1)
                    {
                        ErrorMessage = "Aprovadores distintos para estas contas, favor inserir um documento para cada responsável";
                        return false;
                    }
                }

                DBDataSource dtsOPOR = this.UIAPIRawForm.DataSources.DBDataSources.Item("OPOR");
                string docTotal = dtsOPOR.GetValue("DocTotal", dtsOPOR.Offset);
                if (!String.IsNullOrEmpty(account))
                {
                    string group = _ApprovalGroupAccountBLL.GetGroupByRange(double.Parse(docTotal.Replace(".", ",")), account);
                    et_Appr.Value = group;
                }
                return true;
            }
            catch (Exception ex)
            {
                _application.MessageBox("Erro ao setar grupo de aprovação: " + ex.Message);
                ErrorMessage = "Erro geral: " + ex.Message;
                return false;
            }
        }
    }
}
