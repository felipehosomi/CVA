using CVA.AddOn.Common;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using SAPbouiCOM;
using System;

namespace CVA.AddOn.Control.Logic.VIEW
{
    /// <summary>
    /// Editar Plano de Contas
    /// </summary>
    public class f750 : BaseFormView
    {
        public static string OriginalAcctCode { get; set; }

        #region Constructor
        public f750()
        {
            FormCount++;
        }

        public f750(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f750(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f750(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
            {
                IsIdentity = false;
                ObjectType = MODEL.CVAObjectEnum.PlanoContas;
                TableName = "OACT";
                CodeColumn = "AcctCode";
                CodeField = "12";
                FocusField = "11";

                if (!Adding)
                {
                    ErrorMessage = "CVA - Ação não permitida no banco da dados atual";
                    return false;
                }
                Adding = false;
                bool isOk = base.ValidateDatabaseAction();
                if (isOk)
                {
                    if (String.IsNullOrEmpty(OriginalAcctCode))
                    {
                        base.ValidateAfterAction(FuncaoEnum.Add);
                    }
                }
                else
                {
                    ErrorMessage = "CVA - Ação não permitida no banco da dados atual";
                    return false;
                }
            }
            return true;
            //return base.FormDataEvent();
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            IsIdentity = true;
            ObjectType = MODEL.CVAObjectEnum.PlanoContas;
            TableName = "OACT";
            CodeColumn = "AcctCode";
            CodeField = "12";
            FocusField = "11";

            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        
                    }
                    if (ItemEventInfo.ItemUID == "3")
                    {
                        OriginalAcctCode = ((EditText)Form.Items.Item("12").Specific).Value;
                        Adding = false;
                    }
                    if (ItemEventInfo.ItemUID == "6")
                    {
                        OriginalAcctCode = "";
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "3")
                    {
                        if (Adding)
                        {
                            SBOApp.Application.SetStatusBarMessage("CVA - Por favor, clique em atualizar antes de continuar");
                            return false;
                        }
                    }
                    if (ItemEventInfo.ItemUID == "6")
                    {
                        if (StaticKeys.Base?.TipoBase == MODEL.CVACommon.TipoBaseEnum.Replicadora)
                        {
                            Adding = true;
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("CVA - Ação não permitida no banco da dados atual");
                            return false;
                        }
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    Adding = false;
                }
            }
            return true;
        }
        #endregion
    }
}
