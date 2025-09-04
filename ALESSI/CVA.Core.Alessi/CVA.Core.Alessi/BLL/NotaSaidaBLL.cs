using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using System;

namespace CVA.Core.Alessi.BLL
{
    public class NotaSaidaBLL
    {
        public static string GetCarrier(int docEntry)
        {
            object carrier = CrudController.ExecuteScalar(String.Format(Query.NotaSaida_GetCarrier, docEntry));
            if (carrier != null)
            {
                return carrier.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
