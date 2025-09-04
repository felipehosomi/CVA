using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.GrupoCliente, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class GrupoClienteView : DocumentoView
    {
        public GrupoClienteView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            IsIdentity = true;
            FormTitle = "Grupo de clientes - Configuração";
            ObjectType = CVAObjectEnum.GrupoParceiroNegocio;
            TableName = "OCRG";
            CodeColumn = "GroupCode";
        }
    }
}
