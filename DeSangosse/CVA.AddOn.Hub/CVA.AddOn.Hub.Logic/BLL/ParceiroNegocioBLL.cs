using CVA.AddOn.Hub.Logic.DAO.OCRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class ParceiroNegocioBLL
    {
        private ParceiroNegocioDAO ParceiroNegocioDAO { get; set; }

        public ParceiroNegocioBLL()
        {
            ParceiroNegocioDAO = new ParceiroNegocioDAO();
        }

        public string GetNomeGrupo(string cardCode)
        {
            return ParceiroNegocioDAO.GetNomeGrupo(cardCode);
        }
    }
}
