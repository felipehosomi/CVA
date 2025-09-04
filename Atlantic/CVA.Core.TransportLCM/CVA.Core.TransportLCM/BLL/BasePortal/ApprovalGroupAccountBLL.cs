using CVA.Core.TransportLCM.SERVICE.BasePortal;

namespace CVA.Core.TransportLCM.BLL.BasePortal
{
    public class ApprovalGroupAccountBLL
    {
        private ApprovalGroupAccountDAO _ApprovalGroupAccountDAO { get; }

        public ApprovalGroupAccountBLL()
        {
            _ApprovalGroupAccountDAO = new ApprovalGroupAccountDAO();
        }

        public int GetDistinctGroups(string accounts)
        {
            return _ApprovalGroupAccountDAO.GetDistinctGroups(accounts);
        }

        public string GetGroupByRange(double docTotal, string account)
        {
            string group = _ApprovalGroupAccountDAO.GetGroupByRange(docTotal, account);
            return group;
        }
    }
}
