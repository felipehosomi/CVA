using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Saida
{
    [Form(B1Forms.DevolucaoNotaFiscalSaida, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class DevolucaoNotaSaidaView : DocumentoView
    {
        public DevolucaoNotaSaidaView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Dev.Nota Fiscal Saída";
        }
    }
}
