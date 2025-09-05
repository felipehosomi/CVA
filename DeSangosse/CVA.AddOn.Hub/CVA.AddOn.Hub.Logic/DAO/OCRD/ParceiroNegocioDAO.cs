using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.DAO.OCRD
{
    public class ParceiroNegocioDAO
    {
        public string GetNomeGrupo(string cardCode)
        {
            return CrudController.ExecuteScalar(String.Format(Query.ParceiroNegocio_GetNomeGrupo, cardCode)).ToString();
        }
    }
}
