using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.Indicador, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class IndicadorView : DocumentoView
    {
        public IndicadorView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Indicadores - Configuração";
            ObjectType = CVAObjectEnum.Indicador;
            TableName = "OIDC";
            CodeColumn = "Code";
        }
    }
}
