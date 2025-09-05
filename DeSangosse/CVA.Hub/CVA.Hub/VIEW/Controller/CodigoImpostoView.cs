using CVA.Core.TransportLCM.HELPER;
using CVA.Hub.HELPER;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.VIEW.Controller
{
    [Form(B1Forms.CodigoImposto, "CVA.Hub.VIEW.Form.CodigoImpostoPartial.srf")]
    public class CodigoImpostoView : DoverSystemFormBase
    {
        private EditText et_ObsNF { get; set; }

        private FormattedSearch _formattedSearch { get; set; }

        public CodigoImpostoView(FormattedSearch formattedSearch)
        {
            _formattedSearch = formattedSearch;
        }

        public override void OnInitializeComponent()
        {
            _formattedSearch.AssignFormattedSearch("CVA - Texto Imposto", SERVICE.Resource.Query.TextoPredefinido_Get, B1Forms.CodigoImposto, "et_ObsNF");

            et_ObsNF = this.GetItem("et_ObsNF").Specific as EditText;
            this.OnCustomInitializeFormEvents();
        }

        private void OnCustomInitializeFormEvents()
        {
            et_ObsNF.KeyDownBefore += Et_ObsNF_KeyDownBefore;
        }

        internal virtual void Et_ObsNF_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = false;
        }
    }
}
