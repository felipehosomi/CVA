using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Entrada
{
    [Form(B1Forms.RecebimentoFuturo, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class RecebimentoFuturoView : DocumentoView
    {
        public RecebimentoFuturoView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "NF Recebimento Futuro";
        }
    }
}
