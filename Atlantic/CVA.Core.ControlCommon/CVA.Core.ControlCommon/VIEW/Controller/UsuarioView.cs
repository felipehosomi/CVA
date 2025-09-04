using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.Usuario, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class UsuarioView : DocumentoView
    {
        public UsuarioView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Usuários - Configuração";
            ObjectType = CVAObjectEnum.Usuario;
            TableName = "OUSR";
            CodeColumn = "USERID";
        }
    }
}
