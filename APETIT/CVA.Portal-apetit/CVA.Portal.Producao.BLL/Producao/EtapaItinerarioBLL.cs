using CVA.Portal.Producao.Model.Producao;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class EtapaItinerarioBLL : BaseBLL
    {
        public EtapaItinerarioBLL()
        {
            DAO.TableName = "ORST";
        }

        public List<EtapaItinerarioModel> Get()
        {
            return DAO.RetrieveModelList<EtapaItinerarioModel>();
        }

        public EtapaItinerarioModel GetByCode(string code)
        {
            return DAO.RetrieveModel<EtapaItinerarioModel>($"\"Code\" = '{code}'");
        }
    }
}
