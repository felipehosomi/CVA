using Dover.Framework.Attribute;

namespace CVA.View.ControleQualidade
{
    [ResourceBOM("CVA.View.ControleQualidade.Resources.UDO.CVA_Udo.xml", ResourceType.UDO)]
    [ResourceBOM("CVA.View.ControleQualidade.Resources.UserFields.CVA_UserFields.xml", ResourceType.UserField)]
    [ResourceBOM("CVA.View.ControleQualidade.Resources.UserTables.CVA_UserTables.xml", ResourceType.UserTable)]
    [Menu(FatherUID = "4352", UniqueID = "CVAInspecaoQualidade", String = "CVA - Plano de Inspeção", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 30, Image = "")]
    [Menu(FatherUID = "4352", UniqueID = "CVAApontamentoQualidade", String = "CVA - Apontamento de Qualidade", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 31, Image = "")]
    [Menu(FatherUID = "4352", UniqueID = "CVAApontamentoInspetor", String = "CVA - Apontamento de Qualidade - Inspetor", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 32, Image = "")]
    [Menu(FatherUID = "4352", UniqueID = "CVAOperador", String = "CVA - Operador", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 33, Image = "")]
    [AddIn(Name ="CVA.View.ControleQualidade", Namespace = "CVA Consultoria", Description = "Addin de controle de qualidade de itens")]
    public class Addin
    {
    }
}