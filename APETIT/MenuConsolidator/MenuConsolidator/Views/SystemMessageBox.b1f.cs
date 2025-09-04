using MenuConsolidator.Controllers;
using SAPbouiCOM.Framework;

namespace MenuConsolidator.Views
{
    [FormAttribute("0", "Views/SystemMessageBox.b1f")]
    class SystemMessageBox : SystemFormBase
    {
        public SystemMessageBox()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new LoadAfterHandler(this.Form_LoadAfter);

        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!CommonController.InProcess) return;

            try
            {
                if (((SAPbouiCOM.StaticText)UIAPIRawForm.Items.Item("7").Specific).Caption != "Atualizar o preço na linha de acordo com o novo fornecedor?" &&
                    !((SAPbouiCOM.StaticText)UIAPIRawForm.Items.Item("7").Specific).Caption.StartsWith("Deseja modificar o tipo de pedido para"))
                    return;

                UIAPIRawForm.Items.Item("1").Click();
            }
            catch
            {

            }
        }
    }
}
