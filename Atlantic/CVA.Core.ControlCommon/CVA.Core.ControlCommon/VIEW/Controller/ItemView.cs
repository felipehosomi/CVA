using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.Item, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class ItemView : DocumentoView
    {
        public ItemView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            FormTitle = "Dados do cadastro do item";
            ObjectType = CVAObjectEnum.Item;
            TableName = "OITM";
            CodeColumn = "ItemCode";

            CodeField = "5";
            FocusField = "7";
        }
    }
}
