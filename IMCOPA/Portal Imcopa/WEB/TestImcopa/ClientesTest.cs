using System.Configuration;
using ImcopaWEB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestImcopa
{
    [TestClass]
    public class ClientesTest
    {
        [TestMethod]
        public void BuscarClientes()
        {
            var sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            var tDados = new ZSDTT_0005();

            try
            {
                sapProxy.Connection.Open();
                sapProxy.Zsd_Get_Clients_Linked("9000", "0000101128", ref tDados);
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
