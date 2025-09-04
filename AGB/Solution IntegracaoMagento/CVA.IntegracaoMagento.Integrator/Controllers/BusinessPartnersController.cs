using CVA.IntegracaoMagento.Integrator.Models.SAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.Integrator.Controllers
{
    internal class BusinessPartnersController
    {
        internal async Task GetClient_SAP(string sCardCode)
        {
            var businessPartner = await Integration.SLConnection.GetAsync<BusinessPartners>($"BusinessPartners('{sCardCode}')");

            string steste = businessPartner.CardCode;
        }
    }
}
