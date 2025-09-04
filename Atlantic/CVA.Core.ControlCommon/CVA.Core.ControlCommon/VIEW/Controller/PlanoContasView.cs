using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.PlanoContas, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class PlanoContasView : DocumentoView
    {
        public PlanoContasView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Plano de contas";
            ObjectType = CVAObjectEnum.PlanoContas;
            TableName = "OACT";
            CodeColumn = "AcctCode";
        }
    }
}
