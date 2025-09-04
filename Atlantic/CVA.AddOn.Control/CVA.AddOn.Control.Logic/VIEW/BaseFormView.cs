using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Control.Logic.BLL;
using CVA.AddOn.Control.Logic.BLL.CVACommon;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.VIEW
{
    public class BaseFormView : BaseForm
    {
        #region Properties
        private Form Form;

        public static bool Adding { get; set; }

        public static CVAObjectEnum ObjectType { get; set; }
        public static string CodeColumn { get; set; }
        public static string CodeField { get; set; }
        public static string FocusField { get; set; }
        public static string TableName { get; set; }

        public static Objeto ObjectModel { get; set; }

        private static Item it_Code { get; set; }
        private static Item it_Focus { get; set; }

        public static string ErrorMessage { get; set; } = String.Empty;
        public bool IsIdentity { get; set; } = false;
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                switch (ItemEventInfo.EventType)
                {
                    case BoEventTypes.et_FORM_LOAD:
                        try
                        {
                            SBOApp.Application.StatusBarEvent += StatusBarEvent;
                            SBOApp.Application.MenuEvent += Application_MenuEvent;
                        }
                        catch { }
                        break;
                    case BoEventTypes.et_FORM_CLOSE:
                        it_Code = null;
                        it_Focus = null;
                        try
                        {
                            SBOApp.Application.StatusBarEvent -= StatusBarEvent;
                            SBOApp.Application.MenuEvent -= Application_MenuEvent;
                            return true;
                        }
                        catch { }
                        break;
                }
            }
            if (ItemEventInfo.EventType == BoEventTypes.et_FORM_UNLOAD)
            {
                return true;
            }

            Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            if (Adding)
            {
                return true;
            }
            if (Form.Mode == BoFormMode.fm_EDIT_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE || Form.Mode == BoFormMode.fm_OK_MODE)
            {
                try
                {
                    if (it_Code == null || it_Code.UniqueID != CodeField)
                    { }
                }
                catch
                {
                    it_Code = null;
                }
                try
                {
                    if (it_Focus == null || it_Focus.UniqueID != FocusField)
                    { }
                }
                catch
                {
                    it_Focus = null;
                }
                if (!String.IsNullOrEmpty(CodeField) && (it_Code == null || it_Code.UniqueID != CodeField))
                {
                    it_Code = Form.Items.Item(CodeField);
                }
                if (!String.IsNullOrEmpty(FocusField) && (it_Focus == null || it_Focus.UniqueID != FocusField))
                {
                    it_Focus = Form.Items.Item(FocusField);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN && ItemEventInfo.ItemUID == CodeField)
                {
                    if (it_Code?.Enabled == true)
                    {
                        it_Focus.Click();
                        try
                        {
                            it_Code.Enabled = false;
                        }
                        catch { }
                    }
                }
                else
                {
                    if (it_Code?.Enabled == true)
                    {
                        try
                        {
                            it_Code.Enabled = false;
                        }
                        catch { }
                    }
                }
            }
            return true;
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
            if (ObjectModel == null || ObjectModel.ID != (int)ObjectType)
            {
                ObjetoBLL objectBLL = new ObjetoBLL();
                ObjectModel = objectBLL.GetObjeto(ObjectType);
            }
            if (ObjectModel.Status == 1) // Se estiver desativado, não faz nada
            {
                return true;
            }

            if (BusinessObjectInfo.BeforeAction)
            {
                switch (BusinessObjectInfo.EventType)
                {
                    case BoEventTypes.et_FORM_DATA_ADD:
                        return this.ValidateDatabaseAction();
                    case BoEventTypes.et_FORM_DATA_UPDATE:
                        return this.OnFormDataUpdateBefore();
                }
            }
            else
            {
                switch (BusinessObjectInfo.EventType)
                {
                    case BoEventTypes.et_FORM_DATA_ADD:
                        if (BusinessObjectInfo.ActionSuccess)
                        {
                            this.ValidateAfterAction(FuncaoEnum.Add);
                        }
                        break;
                    case BoEventTypes.et_FORM_DATA_UPDATE:
                        if (BusinessObjectInfo.ActionSuccess)
                        {
                            this.ValidateAfterAction(FuncaoEnum.Update);
                        }
                        break;
                    case BoEventTypes.et_FORM_DATA_LOAD:
                        this.OnFormDataLoadAfter();
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        protected void OnFormDataLoadAfter()
        {
            Item it_Code = null;
            Item it_Focus = null;
            if (!String.IsNullOrEmpty(CodeField))
            {
                it_Code = Form.Items.Item(CodeField);
            }
            if (!String.IsNullOrEmpty(FocusField))
            {
                it_Focus = Form.Items.Item(FocusField);
            }

            if (it_Code?.Enabled == true)
            {
                try
                {
                    it_Code.Enabled = false;
                }
                catch
                {
                    it_Focus.Click();
                    it_Code.Enabled = false;
                }
            }
        }

        protected bool OnFormDataUpdateBefore()
        {
            bool isOk = ValidateDatabaseAction();

            if (isOk)
            {
                DBDataSource dts = Form.DataSources.DBDataSources.Item(TableName);
                string code = dts.GetValue(CodeColumn, dts.Offset);

                RegistroBLL registroBLL = new RegistroBLL();
                Registro registro = registroBLL.Get(code, ObjectType);
                if (registro != null && registro.Status.HasValue)
                {
                    if (registro.Status != (int)StatusEnum.Replicado)
                    {
                        if (registro.Status == (int)StatusEnum.Erro)
                        {
                            if (registroBLL.IsFirstErrorObject(code, ObjectType))
                            {
                                return true;
                            }
                        }

                        ErrorMessage = "CVA - Registro não foi replicado em todas as bases, por favor aguarde e tente novamente";
                        isOk = false;
                    }
                }
            }
            return isOk;
        }
        #endregion

        #region DeleteEvent
        private void Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.BeforeAction)
            {
                if (pVal.MenuUID == "5909")
                {
                    Adding = true;
                }
                if (pVal.MenuUID == "1283" || pVal.MenuUID == "5910")
                {
                    SBOApp.Application.SetStatusBarMessage("CVA - Ação desabilitada");
                    BubbleEvent = false;
                }
            }
        }
        #endregion

        #region SupportMethods
        public bool ValidateDatabaseAction()
        {
            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.NaoConectado)
            {
                ErrorMessage = "CVA - Ação não permitida - AddOn não conectado na base replicadora";
                return false;
            }
            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum || (StaticKeys.Base?.TipoBase == TipoBaseEnum.Consolidadora && ObjectType != CVAObjectEnum.Usuario))
            {
                ErrorMessage = "CVA - Ação não permitida no banco da dados atual";
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ValidateAfterAction(FuncaoEnum funcao)
        {
            if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Replicadora || (StaticKeys.Base?.TipoBase == TipoBaseEnum.Consolidadora && ObjectType == CVAObjectEnum.Usuario))
            {
                Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                DBDataSource dts = Form.DataSources.DBDataSources.Item(TableName);
                string code = dts.GetValue(CodeColumn, dts.Offset);

                RegistroBLL registroBLL = new RegistroBLL();

                if (!this.IsIdentity || funcao == FuncaoEnum.Update)
                {
                    code = dts.GetValue(CodeColumn, dts.Offset);
                }
                else
                {
                    code = new GenericBLL().GetMaxId(CodeColumn, TableName);
                }

                Registro registro = registroBLL.Get(code, ObjectType);
                if (funcao == FuncaoEnum.Update && registro?.Status == (int)StatusEnum.Erro)
                {
                    registroBLL.RestartErrorReg(ObjectType);
                }
                registroBLL.Insert(code, ObjectType, funcao);
            }
        }
        #endregion

        #region StatusBarEvent
        public void StatusBarEvent(string text, BoStatusBarMessageType messageType)
        {
            if (text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                SBOApp.Application.StatusBar.SetText(ErrorMessage, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
        }
        #endregion
    }
}
