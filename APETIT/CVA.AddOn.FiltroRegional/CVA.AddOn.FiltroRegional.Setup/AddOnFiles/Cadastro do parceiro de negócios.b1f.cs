using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace CVA.AddOn.FiltroRegional
{
    [FormAttribute("134", "Cadastro do parceiro de negócios.b1f")]
    class Cadastro_do_parceiro_de_negócios : SystemFormBase
    {
        private SAPbouiCOM.Button Button0;

        public Cadastro_do_parceiro_de_negócios()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
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

        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var cardCode = ((SAPbouiCOM.EditText)(this.GetItem("5").Specific)).Value.ToString();
            if (!string.IsNullOrEmpty(cardCode))
            {
                bool lExists = false;
                for (int i = 0; i < Application.SBO_Application.Forms.Count; i++)
                {
                    if (Application.SBO_Application.Forms.Item(i).UniqueID == "regionais")
                    {
                        lExists = true;
                    }
                }

                if (!lExists)
                {
                    var form = new Regionais(false, cardCode);
                    form.Show();
                }
            }
        }
    }
}
