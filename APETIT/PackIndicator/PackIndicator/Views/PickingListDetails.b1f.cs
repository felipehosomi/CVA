using SAPbouiCOM.Framework;
using PackIndicator.Controllers;

namespace PackIndicator.Views
{
    [FormAttribute("60020", "Views/PickingListDetails.b1f")]
    class PickingListDetails : SystemFormBase
    {
        public PickingListDetails()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            if (!CommonController.InProcess) return;

            ((SAPbouiCOM.EditText)UIAPIRawForm.Items.Item("5").Specific).Value = "Inserido via sugestão de picking.";
            UIAPIRawForm.Items.Item("1").Click(); // Liberado
        }
    }
}
