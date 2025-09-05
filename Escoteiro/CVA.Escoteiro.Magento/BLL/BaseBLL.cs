using CVA.Escoteiro.Magento.DAO;

namespace CVA.Escoteiro.Magento.BLL
{
    public class BaseBLL
    {
        public IDAO DAO { get; set; }

        public BaseBLL()
        {
            DAO = new HanaDAO();
        }
    }
}
