using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace Picking.Producao.Addon.View
{
    class f60100 : BaseForm
    {
        #region Constructor
        public f60100()
        {
            FormCount++;
        }

        public f60100(ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f60100(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f60100(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            //DBDataSource ds_OHEM = Form.DataSources.DBDataSources.Item("OHEM");
            //if (!BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
            //{
            //    string nome = ds_OHEM.GetValue("firstName", 0) + ds_OHEM.GetValue("lastName", 0);
            //    string senha = ds_OHEM.GetValue("U_CVA_Senha", 0);
            //    if (!String.IsNullOrEmpty(senha.Trim()))
            //    {
            //        ((EditText)Form.Items.Item("et_Senha").Specific).Value = EncryptController.Decrypt(senha, nome);
            //    }
            //}
            //else
            //{
            //    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
            //    {
            //        string nome = ds_OHEM.GetValue("firstName", 0) + ds_OHEM.GetValue("lastName", 0);
            //        string senha = ds_OHEM.GetValue("U_CVA_Senha", 0);
            //        if (!String.IsNullOrEmpty(senha.Trim()))
            //        {
            //            ((EditText)Form.Items.Item("et_Senha").Specific).Value = EncryptController.Encrypt(senha, nome);
            //        }
            //    }
            //}
            return true;
        }
    }
}
