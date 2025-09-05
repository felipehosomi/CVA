using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Dover.Framework.Attribute;
using System;
using System.Collections.Generic;
using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.BLL;
using log4net;
using System.Linq;
using System.Globalization;

namespace CVA.View.ControleQualidade.VIEW
{
    [FormAttribute("CVAOperador", "CVA.View.ControleQualidade.Resources.Form.CadastroOperador.srf")]
    [MenuEvent(UniqueUID = "CVAOperador")]
    public partial class CadastroOperadorView : DoverUserFormBase
    {
        private SAPbouiCOM.Application _application { get; set; }
        public ILog _Log { get; set; }
        public EditText et_Code { get; set; }

        private OperadorBLL _OperadorBLL { get; set; }

        public CadastroOperadorView(SAPbouiCOM.Application application, OperadorBLL operadorBLL)
        {
            _application = application;
            _OperadorBLL = operadorBLL;
            _Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            OnInitializeCustomComponents();
        }

        private void OnInitializeCustomComponents()
        {
            try
            {
                et_Code = this.GetItem("et_Code").Specific as EditText;

                UIAPIRawForm.AutoManaged = true;
                GetItem("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                GetItem("et_Code").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_Find, BoModeVisualBehavior.mvb_True);
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage("CVA Exception: " + ex.Message);
            }
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            et_Code.Value = _OperadorBLL.GetNextCode();
        }
    }
}
