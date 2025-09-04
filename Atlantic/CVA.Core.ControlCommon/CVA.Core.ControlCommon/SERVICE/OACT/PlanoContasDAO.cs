using CVA.Core.ControlCommon.SERVICE.Resource;
using Dover.Framework.DAO;

namespace CVA.Core.ControlCommon.SERVICE.OACT
{
    public class PlanoContasDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }

        public PlanoContasDAO(BusinessOneDAO businessOneDAO)
        {
            this._businessOneDAO = businessOneDAO;
        }

        public int Exists(string code)
        {
            string sql = string.Format(Query.Account_Exists, code);
            return this._businessOneDAO.ExecuteSqlForObject<int>(sql);
        }
    }
}
