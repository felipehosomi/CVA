using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.GrupoItem, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class GrupoItemView : DocumentoView
    {
        public GrupoItemView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Grupo de itens - Configuração";
            ObjectType = CVAObjectEnum.GrupoItem;
            TableName = "OITB";
            CodeColumn = "ItmsGrpCod";

            CodeField = "6";
            FocusField = "10002024";
        }
    }
}
