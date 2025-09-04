using BLL.Classes;
using MODEL.Classes;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ExpenseContract
    {
        #region Atributos
        private ExpenseBLL _expenseBLL { get; set; }
        #endregion

        #region Construtor
        public ExpenseContract()
        {
            this._expenseBLL = new ExpenseBLL();
        }
        #endregion

        public MessageModel Save(ExpenseModel model)
        {
            return _expenseBLL.Save(model);
        }

        public MessageModel Remove(int id)
        {
            return _expenseBLL.Remove(id);
        }

        public List<ExpenseModel> GetExpense_ByUserID(int id, int mes, int ano)
        {
            return _expenseBLL.Get_ByUser(id, mes, ano);
        }

        public ExpenseModel Get(int id)
        {
            return _expenseBLL.Get(id);
        }

        public List<ExpenseModel> Search(int col, int cli, int prj, DateTime? de, DateTime? ate)
        {
            return _expenseBLL.Search(col, cli, prj, de, ate);
        }
    }
}