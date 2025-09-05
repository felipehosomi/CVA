
using Dover.Framework.Attribute;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.PreenchimentoDeLote
{
    [AddIn(Name = "CVA.PreenchimentoDeLote", Description = "Preenchimento de Lote", Namespace = "CVA Consultoria")]

    [ResourceBOM("CVA.PreenchimentoDeLote.RESOURCES.UDF.UDF_SubLote.xml", ResourceType.UserField)]
    public class Addin
    {
        private Application _application { get; set; }

        public Addin(Application application)
        {
            this._application = application;
        }
    }
}