using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Saida
{
    [Form(B1Forms.AdiantamentoCliente, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class AdiantamentoClienteView : DocumentoView
    {
        public AdiantamentoClienteView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Adiantamento de cliente";
        }        
    }
}
