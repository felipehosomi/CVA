using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Entrada
{
    [Form(B1Forms.RecebimentoMercadoria, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class RecebimentoMercadoriaView : DocumentoView
    {
        public RecebimentoMercadoriaView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Recebimento de mercadorias";
        }
    }
}
