using CVA.Portal.Producao.Model.Estoque;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Estoque
{
    public class GrupoItemBLL : BaseBLL
    {
        public GrupoItemBLL()
        {
            DAO.TableName = "OITB";
        }

        public List<GrupoItemModel> Get()
        {
            return DAO.RetrieveModelList<GrupoItemModel>();
        }
    }
}
