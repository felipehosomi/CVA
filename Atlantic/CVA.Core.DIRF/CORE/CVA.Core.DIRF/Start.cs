using Dover.Framework.Attribute;

namespace CVA.Core.DIRF
{
    [AddIn(Name = "CVA.Core.DIRF", Description = "CVA - DIRF", Namespace = "CVA Consultoria")]
    [Menu(FatherUID = "1536", UniqueID = "ViewDirf", String = "CVA - DIRF", Type = SAPbouiCOM.BoMenuType.mt_STRING)]
    public class Start
    {
        public Start()
        {
        }
    }
}
