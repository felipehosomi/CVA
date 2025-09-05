using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.ApplicationServices.ServiceLayer;
using App.ApplicationServices.Services;

namespace App.ApplicationServices.Test.Hana
{
    [TestClass]
    public class ServiceLayerServiceTest
    {
        [TestMethod]
        public void ServiceLayerService_Connection()
        {
            var session = ConnectionService.Instance.Connection();
            Assert.IsTrue(session != null);
        }

        [TestMethod]
        public void ServiceLayerService_Connection_Cache()
        {
            var session = ConnectionService.Instance.Connection();
            Assert.IsTrue(session != null);
        }
        
    }
}
