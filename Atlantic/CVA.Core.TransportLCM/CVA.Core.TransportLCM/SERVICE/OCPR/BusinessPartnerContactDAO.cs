using CVA.Core.TransportLCM.MODEL;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.SERVICE.OCPR
{
    public class BusinessPartnerContactDAO
    {
        BusinessOneDAO _businessOneDAO { get; set; }

        public BusinessPartnerContactDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public List<string> GetEmails(string cardCode)
        {
            var query = String.Format(SERVICE.Resource.Query.BusinessPartnerContact_GetEmail, cardCode);
            return _businessOneDAO.ExecuteSqlForList<string>(query);
        }
    }
}
