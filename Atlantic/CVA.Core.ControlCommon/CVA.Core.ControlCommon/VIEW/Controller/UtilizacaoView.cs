using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.Utilizacao, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class UtilizacaoView : DocumentoView
    {
        public UtilizacaoView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            IsIdentity = true;
            FormTitle = "Utilização - Configuração";
            ObjectType = CVAObjectEnum.Utilizacao;
            TableName = "OUSG";
            CodeColumn = "ID";
        }
    }
}
