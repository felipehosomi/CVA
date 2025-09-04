using MODEL.Classes;

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IExpense
    {
        [OperationContract]
        MessageModel Expense_Save(ExpenseModel model);

        [OperationContract]
        List<ExpenseModel> GetExpense_ByUserID(int userId, int mes, int ano);

        [OperationContract]
        ExpenseModel Expense_Get(int id);

        [OperationContract]
        List<ExpenseModel> Expense_Search(int col, int cli, int prj, DateTime? de, DateTime? ate);

        [OperationContract]
        MessageModel Expense_Remove(int id);
    }
}