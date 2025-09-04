using CVA.AddOn.Control.Logic.BLL;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using SAPbouiCOM;
using System;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Parceiro de Negócio
    /// </summary>
    public class f134 : BaseFormView
    {
        #region Constructor
        public f134()
        {
            FormCount++;
        }

        public f134(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f134(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f134(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.ParceiroNegocio;
            TableName = "OCRD";
            CodeColumn = "CardCode";

            CodeField = String.Empty;
            FocusField = String.Empty;

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "96")
                {
                    if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum)
                    {
                        f65052.CardCode = ((EditText)Form.Items.Item("5").Specific).Value;
                    }
                }
            }

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            IsIdentity = false;
            ObjectType = MODEL.CVAObjectEnum.ParceiroNegocio;
            TableName = "OCRD";
            CodeColumn = "CardCode";

            CodeField = String.Empty;
            FocusField = String.Empty;

            bool isOk = base.FormDataEvent();
            //if (isOk)
            //{
            //    if (BusinessObjectInfo.BeforeAction && (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE))
            //    {
            //        return ValidateBankAccount();
            //    }
            //}
            return isOk;
        }
        #endregion

        private bool ValidateBankAccount()
        {
            DBDataSource dts = Form.DataSources.DBDataSources.Item(TableName);
            string cardCode = dts.GetValue("CardCode", dts.Offset);
            string bankCountry = dts.GetValue("BankCountr", dts.Offset);
            int bankCode;
            int branch;
            string account = dts.GetValue("DflAccount", dts.Offset);

            Int32.TryParse(dts.GetValue("BankCode", dts.Offset), out bankCode);
            Int32.TryParse(dts.GetValue("DflBranch", dts.Offset), out branch);

            if (!String.IsNullOrEmpty(account))
            {
                string existingCardCode = new BusinessPartnerBLL().BankAccountExists(cardCode, bankCountry, bankCode, branch, account);
                if (!String.IsNullOrEmpty(existingCardCode))
                {
                    ErrorMessage = "Conta Bancária já existente no Parceiro de Negócio: " + existingCardCode;
                    return false;
                }
            }
            return true;
        }
    }
}
