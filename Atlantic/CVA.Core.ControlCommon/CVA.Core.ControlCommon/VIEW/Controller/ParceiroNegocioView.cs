using CVA.Core.ControlCommon.BLL;
using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.ParceiroNegocio, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class ParceiroNegocioView : DocumentoView
    {
        BusinessPartnerBLL _businessPartnerBLL { get; }

        public ParceiroNegocioView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL, BusinessPartnerBLL businessPartnerBLL) : base(application, registroBLL, genericBLL)
        {
            _businessPartnerBLL = businessPartnerBLL;

            FormTitle = "Cadastro de parceiros de negócio";
            ObjectType = CVAObjectEnum.ParceiroNegocio;
            TableName = "OCRD";
            CodeColumn = "CardCode";
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            base.OnFormDataAddBefore(ref pVal, out BubbleEvent);
            if (BubbleEvent)
            {
                BubbleEvent = this.ValidateBankAccount();
            }
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            base.OnFormDataUpdateBefore(ref pVal, out BubbleEvent);
            if (BubbleEvent)
            {
                BubbleEvent = this.ValidateBankAccount();
            }
        }

        private bool ValidateBankAccount()
        {
            DBDataSource dts = UIAPIRawForm.DataSources.DBDataSources.Item(TableName);
            string cardCode = dts.GetValue("CardCode", dts.Offset);
            string bankCountry = dts.GetValue("BankCountr", dts.Offset);
            int bankCode;
            int branch;
            string account = dts.GetValue("DflAccount", dts.Offset);

            Int32.TryParse(dts.GetValue("BankCode", dts.Offset), out bankCode);
            Int32.TryParse(dts.GetValue("DflBranch", dts.Offset), out branch);

            if (!String.IsNullOrEmpty(account))
            {
                string existingCardCode = _businessPartnerBLL.BankAccountExists(cardCode, bankCountry, bankCode, branch, account);
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
