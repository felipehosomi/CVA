using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IExpenseType
    {
        [OperationContract]
        MessageModel SaveExpenseType(ExpenseTypeModel expenseType);

        [OperationContract]
        List<StatusModel> ExpenseTypeStatus();

        [OperationContract]
        List<ExpenseTypeModel> GetExpenseTypes();

        [OperationContract]
        List<ExpenseTypeModel> GetAllExpenseTypes();

        [OperationContract]
        ExpenseTypeModel GetExpenseType(int id);
    }
}