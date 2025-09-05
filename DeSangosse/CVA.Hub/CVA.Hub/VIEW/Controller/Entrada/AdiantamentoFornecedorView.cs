using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Entrada
{
    [Form(B1Forms.AdiantamentoFornecedor, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class AdiantamentoFornecedorView : DocumentoView
    {
        public AdiantamentoFornecedorView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Adiantamento para fornecedor";
        }
    }
}
