using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller.Saida
{
    [Form(B1Forms.NotaFiscalSaida, "CVA.Hub.VIEW.Form.EmptyFormPartial.srf")]
    public class NotaFiscalSaidaView : DocumentoView
    {
        public NotaFiscalSaidaView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL) : base(application, itemBLL, utilizacaoBLL, documentoBLL)
        {
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Title = "Nota Fiscal de Saída";
        }
    }
}
