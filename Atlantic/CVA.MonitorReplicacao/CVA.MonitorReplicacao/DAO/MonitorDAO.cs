using CVA.MonitorReplicacao.DAO.Resources;
using CVA.MonitorReplicacao.MODEL;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.MonitorReplicacao.DAO
{
    public class MonitorDAO
    {
        private BusinessOneDAO _dao { get; set; }

        public MonitorDAO(BusinessOneDAO dao)
        {
            _dao = dao;
        }

        public string GetRegQuery(FilterModel filterModel)
        {
            if (String.IsNullOrEmpty(filterModel.IdFrom))
            {
                filterModel.IdFrom = "0";
            }
            if (String.IsNullOrEmpty(filterModel.IdTo))
            {
                filterModel.IdTo = Int32.MaxValue.ToString();
            }
            if (String.IsNullOrEmpty(filterModel.DateFrom))
            {
                filterModel.DateFrom = "19000101";
            }
            if (String.IsNullOrEmpty(filterModel.DateTo))
            {
                filterModel.DateTo = "22000101";
            }

            string query = Query.Register_Get;
            query = String.Format(query,
                                filterModel.IdFrom,
                                filterModel.IdTo,
                                filterModel.DateFrom,
                                filterModel.DateTo,
                                filterModel.Code,
                                filterModel.Status,
                                filterModel.Source,
                                filterModel.Object,
                                filterModel.Function,
                                filterModel.ErrorBase);

            return query;
        }
    }
}
