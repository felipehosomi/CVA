using CVA.Core.DSP.Controle.BLL;
using CVA.Core.DSP.Controle.HELPER;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace CVA.Core.DSP.Controle.VIEW
{
    public partial class ItemView
    {
        private FerramentasBLL _ferramentasBLL { get; set; }
        private SAPbouiCOM.Application _application { get; set; }
        public EditText et_ItemCode { get; set; }
        public EditText et_Tool { get; set; }
        public EditText et_Counter { get; set; }
    }

    [FormAttribute(B1Forms.DadosDoCadastroDoItem)]
    public partial class ItemView : DoverSystemFormBase
    {
        public ItemView(FerramentasBLL ferramentasBLL, SAPbouiCOM.Application application)
        {
            _application = application;
            _ferramentasBLL = ferramentasBLL;
        }

        public override void OnInitializeComponent()
        {
            et_ItemCode = this.GetItem("5").Specific as EditText;
        }

        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }
    }
}