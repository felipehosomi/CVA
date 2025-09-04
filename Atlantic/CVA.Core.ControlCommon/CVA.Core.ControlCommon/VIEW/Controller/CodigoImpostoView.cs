using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.CodigoImposto, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class CodigoImpostoView : DocumentoView
    {
        public CodigoImpostoView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Códigos de imposto - Configuração";
            ObjectType = CVAObjectEnum.CodigoImposto;
            TableName = "OSTC";
            CodeColumn = "Code";
        }
    }
}
