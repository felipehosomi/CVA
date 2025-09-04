using CVA.Core.Cianet.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Cianet.BLL
{
    public class QuotationBLL
    {
        TaxDAO TaxDAO;

        public QuotationBLL()
        {
            TaxDAO = new TaxDAO();
        }

        public string CalculateUnitPrice(double priceIPI, string taxCode)
        {
            //double itemPrice = priceIPI;
            //double ipiRate = TaxDAO.GetIPIRate(taxCode);
            //if (ipiRate > 0)
            //{
            //    itemPrice = priceIPI * 100 / (100 + ipiRate);
            //}
            ////return itemPrice;

            double ipiRate = TaxDAO.GetIPIRate(taxCode);
            if (ipiRate > 0)
            {
                priceIPI = ((priceIPI * 100) / (100 + ipiRate));
            }

            return priceIPI.ToString();




        }
    }
}
