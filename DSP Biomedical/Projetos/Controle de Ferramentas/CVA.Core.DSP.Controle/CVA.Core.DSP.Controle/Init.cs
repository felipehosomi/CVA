

using Dover.Framework.Attribute;

namespace CVA.Core.DSP.Controle
{
    [ResourceBOM("CVA.Core.DSP.Controle.Resources.Forms.CadastroMotivos.xml", ResourceType.UDO)]


    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserTable.UDT_CVA_Tool.xml", ResourceType.UserTable)]
    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserTable.UDT_CadastroMotivos.xml", ResourceType.UserTable)]
    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserTable.UDT_CVA_MAQ_PARADAS.xml", ResourceType.UserTable)]


    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserFields.UDF_SubLote.xml", ResourceType.UserField)]
    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserFields.CVAUserFields.xml", ResourceType.UserField)]
    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserFields.ORSC_UDF.xml", ResourceType.UserField)]
    [ResourceBOM("CVA.Core.DSP.Controle.Resources.UserFields.UDF_CVA_MAQ_PARADAS.xml", ResourceType.UserField)]


    [Menu(FatherUID = "13312", UniqueID = "CVAMaquinasParadas", String = "[CVA] Apontamento de Maquinas Paradas", Type = SAPbouiCOM.BoMenuType.mt_STRING, Image = "")]

    [AddIn(Name = "CVA.Core.DSP.Controle", Description = "Add-in de Controle", Namespace = "CVA Consultoria")]



    public class Init
    {

    }
}
