using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class PoliticExpenseContract
    {
        private PoliticExpenseBLL _politicExpenseBLL { get; set; }

        public PoliticExpenseContract()
        {
            this._politicExpenseBLL = new PoliticExpenseBLL();
        }

        public List<PoliticExpenseModel> GetBy_Project(int projectId, int user)
        {
            return _politicExpenseBLL.GetBy_Project(projectId, user);
        }
    }
}