using CVA.AddOn.Control.Logic.DAO.OACT;
using CVA.AddOn.Control.Logic.MODEL;

namespace CVA.AddOn.Control.Logic.BLL
{
    public class AccountBLL
    {
        private AccountDAO AccountDAO { get; set; }

        public AccountBLL()
        {
            AccountDAO = new AccountDAO();
        }

        public string UpdateStatus(AccountModel model)
        {
            return AccountDAO.UpdateStatus(model);
        }
    }
}
