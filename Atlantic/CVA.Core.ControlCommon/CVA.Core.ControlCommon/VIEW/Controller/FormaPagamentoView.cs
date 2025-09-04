using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.FormaPagamento, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class FormaPagamentoView : DocumentoView
    {
        public FormaPagamentoView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Formas de pagamento - Configuração";
            ObjectType = CVAObjectEnum.FormaPagamento;
            TableName = "OPYM";
            CodeColumn = "PayMethCod";

            CodeField = "5";
            FocusField = "6";
        }
    }
}
