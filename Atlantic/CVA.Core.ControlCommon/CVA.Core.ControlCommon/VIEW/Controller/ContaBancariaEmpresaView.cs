using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.ContaBancariaEmpresa, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class ContaBancariaEmpresaView : DocumentoView
    {
        public ContaBancariaEmpresaView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Contas bancárias da empresa - Configuração";
            ObjectType = CVAObjectEnum.ContaBancariaEmpresa;
            TableName = "DSC1";
            CodeColumn = "BankCode";
        }
    }
}
