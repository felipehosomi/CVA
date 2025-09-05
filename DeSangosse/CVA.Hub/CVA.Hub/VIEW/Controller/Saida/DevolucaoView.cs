using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Saida
{
    [Form(B1Forms.DevolucaoSaida, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class DevolucaoView : DocumentoView
    {
        public DevolucaoView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Devolução";
        }
    }
}
