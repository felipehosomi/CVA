using CVA.AddOn.Hub.Logic.MODEL;
using CVA.Hub.DAO.OITM;
using CVA.Hub.MODEL;

namespace CVA.Hub.BLL
{
    public class ItemBLL
    {
        private ItemDAO _ItemDAO { get; }
        
        public ItemBLL()
        {
            _ItemDAO = new ItemDAO();
        }

        public DocumentItemModel GetCentroCusto(string itemCode)
        {
            return _ItemDAO.GetCentroCusto(itemCode);
        }

        public double GetQtdePorEmbalagem(string itemCode, TipoDoc tipoDoc)
        {
            if (tipoDoc == TipoDoc.Saida)
            {
                return _ItemDAO.GetColumnValue<double>("SalPackUn", itemCode);
            }
            else
            {
                return _ItemDAO.GetColumnValue<double>("PurPackUn", itemCode);
            }
        }

        public bool ItemExiste(string itemCode)
        {
            return _ItemDAO.ItemExiste(itemCode);
        }

        public string ItemGetObs(string itemCode)
        {
            return _ItemDAO.GetItemObs(itemCode);
        }
    }
}
