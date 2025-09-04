using CVA.Core.ControlCommon.BLL.CVACommon;
using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.ControlCommon.VIEW.Controller
{
    [Form(B1Forms.VendedoresCompradores, "CVA.Core.ControlCommon.VIEW.Form.EmptyFormPartial.srf")]
    public class VendedoresCompradoresView : DocumentoView
    {
        public VendedoresCompradoresView(SAPbouiCOM.Application application, RegistroBLL registroBLL, BLL.BaseReplicadora.GenericBLL genericBLL) : base(application, registroBLL, genericBLL)
        {
            IsIdentity = true;
            FormTitle = "Vendedores/Compradores - Configuração";
            ObjectType = CVAObjectEnum.VendedoresCompradores;
            TableName = "OSLP";
            CodeColumn = "SlpCOde";
        }
    }
}
