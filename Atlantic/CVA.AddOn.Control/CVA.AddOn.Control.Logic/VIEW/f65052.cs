using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Control.Logic.BLL;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using SAPbouiCOM;
using System;

namespace CVA.AddOn.Control.Logic.VIEW
{
    // Contas de Banco do Parceiro de Negócios
    public class f65052 : BaseForm
    {
        public static string CardCode { get; set; } = String.Empty;

        #region Constructor
        public f65052()
        {
            FormCount++;
        }

        public f65052(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f65052(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f65052(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "4")
                {
                    if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum)
                    {
                        Matrix mt_Bank = Form.Items.Item("3").Specific as Matrix;
                        int row = mt_Bank.GetNextSelectedRow();
                        if (row > 0)
                        {
                            string bankKey = ((EditText)mt_Bank.GetCellSpecific("BankCode", row)).Value;
                            string account = ((EditText)mt_Bank.GetCellSpecific("Account", row)).Value;
                            string branch = ((EditText)mt_Bank.GetCellSpecific("Branch", row)).Value;

                            BusinessPartnerBLL bll = new BusinessPartnerBLL();
                            string error = bll.SetDefaultBankAccount(CardCode, bankKey, account, branch);
                            if (!String.IsNullOrEmpty(error))
                            {
                                SBOApp.Application.SetStatusBarMessage(error);
                            }
                            else
                            {
                                SBOApp.Application.SetStatusBarMessage("Conta do Banco padrão atualizada com sucesso!", BoMessageTime.bmt_Medium, false);
                            }
                        }
                        
                    }
                }
            }
            return true;
        }
    }
}
