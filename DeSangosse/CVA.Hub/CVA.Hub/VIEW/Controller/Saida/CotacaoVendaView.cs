using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Saida
{
    [Form(B1Forms.CotacaoVenda, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class CotacaoVendaView : DocumentoView
    {
        public CotacaoVendaView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Cotação de vendas";
        }
    }
}
