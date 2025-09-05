using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.ObservacoesFiscais.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ObservacoesFiscais.VIEW
{
    public class DocumentBaseView : BaseForm
    {
        private Form Form;

        #region FormDataEvent
        public override bool FormDataEvent()
        {
            Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);

            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    this.SetObsNF();
                }
            }
            return true;
        }
        #endregion

        #region SetObsNF
        private void SetObsNF()
        {
            DBDataSource dtsDoc = this.Form.DataSources.DBDataSources.Item(0);
            if (!String.IsNullOrEmpty(dtsDoc.GetValue("DocEntry", dtsDoc.Offset)))
            {
                int docEntry = Convert.ToInt32(dtsDoc.GetValue("DocEntry", dtsDoc.Offset));
                int objType = Convert.ToInt32(dtsDoc.GetValue("ObjType", dtsDoc.Offset));
                try
                {
                    DocumentBLL.UpdateObs(docEntry, objType);
                }
                catch (Exception ex)
                {
                    SBOApp.Application.MessageBox(ex.Message);
                }
            }
        }
        #endregion
    }
}
