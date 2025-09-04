using BLL.Classes;
using MODEL.Classes;
using MODEL.SystemMessage;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ExpenseTypeContract
    {
        private ExpenseTypeBLL _expenseTypeBLL { get; set; }

        public ExpenseTypeContract()
        {
            this._expenseTypeBLL = new ExpenseTypeBLL();
        }

        public Message Save(ExpenseTypeModel expenseType)
        {
            return _expenseTypeBLL.Save(expenseType);
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return _expenseTypeBLL.GetSpecificStatus();
        }

        public List<ExpenseTypeModel> Get()
        {
            return _expenseTypeBLL.Get();
        }

        public List<ExpenseTypeModel> GetAll()
        {
            return _expenseTypeBLL.GetAll();
        }

        public ExpenseTypeModel Get(int id)
        {
            return _expenseTypeBLL.Get(id);
        }
    }
}