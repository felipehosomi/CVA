using System.Configuration;
using ImcopaWEB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestImcopa
{
    [TestClass]
    public class MetasProdutosTest
    {
        [TestMethod]
        public void RetornaListaDeMetasComSucesso()
        {
            var sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            var tDados = new ZSDTT_0002();
            
            try
            {
                sapProxy.Connection.Open();
                sapProxy.Zsd_Get_Products_Goal_Realized("9000", "803779", ref tDados);
                var dtRetorno = tDados.ToADODataTable();
                Assert.IsTrue(dtRetorno.Rows.Count > 0);
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                {
                    sapProxy.Connection.Close();
                }
            }
        }
    }
}
