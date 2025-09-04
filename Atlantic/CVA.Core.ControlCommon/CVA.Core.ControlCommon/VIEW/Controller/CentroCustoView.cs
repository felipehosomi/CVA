using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.CentroCusto, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class CentroCustoView : DocumentoView
    {
        public CentroCustoView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Centros de custo - Configuração";
            ObjectType = CVAObjectEnum.CentroCusto;
            TableName = "OPRC";
            CodeColumn = "PrcCode";
        }
    }
}
