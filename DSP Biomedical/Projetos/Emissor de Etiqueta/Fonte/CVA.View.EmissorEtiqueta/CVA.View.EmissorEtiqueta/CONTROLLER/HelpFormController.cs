using Dover.Framework.Attribute;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmissorEtiqueta.CONTROLLER
{
    public partial class HelpFormController
    {
        private Button bt_SiteCva { get; set; }
        private Button bt_Chamado { get; set; }
    }

    [FormAttribute("CVAHelpForm", "CVA.View.EmissorEtiqueta.FORM.CVAHelp.srf")]
    public partial class HelpFormController : DoverUserFormBase
    {
        /// <summary>
        /// Contructor Method
        /// </summary>
        public HelpFormController()
        {

        }

        #region Initiliaze Events
        public override void OnInitializeComponent()
        {
            bt_SiteCva = this.GetItem("btSite").Specific as Button;
            bt_Chamado = this.GetItem("btCham").Specific as Button;

            OnCustomInitializeEvents();
        }
        
        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }

        private void OnCustomInitializeEvents()
        {
            bt_SiteCva.ClickAfter += Bt_SiteCva_ClickAfter;
            bt_Chamado.ClickAfter += Bt_Chamado_ClickAfter;
        }
        #endregion
        
        #region Form Events
        protected internal virtual void Bt_Chamado_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            System.Diagnostics.Process.Start("http://amscvab1.chamados.com.br/");
        }

        protected internal virtual void Bt_SiteCva_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            System.Diagnostics.Process.Start("http://www.cvaconsultoria.com.br");
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            base.OnFormCloseAfter(pVal);
        }

        protected override void OnFormCloseBefore(SBOItemEventArg pVal, out bool BubbleEvent)
        {
            base.OnFormCloseBefore(pVal, out BubbleEvent);
        }
        #endregion

        public void CloseForm()
        {
            this.UIAPIRawForm.Close();
        }
    }
}
