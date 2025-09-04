using Dover.Framework.Attribute;
using SAPbouiCOM;

namespace CVA.View.FolhaPagamento
{
    [Menu(FatherUID = "1536", UniqueID = "CVAFolhaPagamento", String = "CVA - Folha de Pagamento", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 18, Image = "")]
    [AddIn(Name = "CVA.View.FolhaPagamento", Description = "CVA – Geração de Folha de Pagamento", Namespace = "CVA Consultoria")]
    public class Init
    {
        private Application _application { get; set; }

        public Init(Application application)
        {
            this._application = application;
        }
    }
}
