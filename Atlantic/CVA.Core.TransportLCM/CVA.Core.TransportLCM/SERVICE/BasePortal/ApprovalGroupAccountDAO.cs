using CVA.Core.TransportLCM.HELPER;
using System;

namespace CVA.Core.TransportLCM.SERVICE.BasePortal
{
    public class ApprovalGroupAccountDAO
    {
        private SqlHelper SqlHelper { get; }

        public ApprovalGroupAccountDAO()
        {
            SqlHelper = new SqlHelper(StaticKeys.ConfigPortalModel.Servidor, StaticKeys.ConfigPortalModel.Banco, StaticKeys.ConfigPortalModel.Usuario, StaticKeys.ConfigPortalModel.Senha);
        }

        public int GetDistinctGroups(string accounts)
        {
            string query = String.Format(Resources.Query.ApprovalGroupAccount_GetDistinctGroups, accounts);
            return SqlHelper.GetRowCount(query);
        }

        public string GetGroupByRange(double docTotal, string account)
        {
            string query = String.Format(Resources.Query.ApprovalGroupAccount_GetByRange, docTotal.ToString().Replace(",", "."), account);
            var group = SqlHelper.ExecuteScalar(query);
            if (group != null)
            {
                return group.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
