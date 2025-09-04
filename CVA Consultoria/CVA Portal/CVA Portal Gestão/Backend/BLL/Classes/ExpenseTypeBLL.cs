using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ExpenseTypeBLL
    {
        #region Atributos
        private ExpenseTypeDAO _expenseTypeDAO { get; set; }
        private StatusBLL _statusBLL { get; set; }
        #endregion

        #region Construtor
        public ExpenseTypeBLL()
        {
            this._expenseTypeDAO = new ExpenseTypeDAO();
            this._statusBLL = new StatusBLL();
        }
        #endregion

        public List<StatusModel> GetSpecificStatus()
        {
            try
            {
                return _statusBLL.Get(ExpenseTypeModel.oObjectType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MessageModel Save(ExpenseTypeModel model)
        {
            try
            {
                var isValid = ValidateFields(ref model);
                if (isValid.Error != null) return isValid;

                if (model.Id != 0)
                    return Update(model);

                var resultId = _expenseTypeDAO.Save(model);
                if (resultId == 0)
                    return MessageBLL.Generate("Ocorreu um erro ao salvar o tipo de despesa", 99, true);

                return MessageBLL.Generate("Tipo de despesa cadastrado com sucesso!", resultId);
            }
            catch (Exception)
            {   
                throw;
            }
        }

        private MessageModel Update(ExpenseTypeModel model)
        {
            try
            {
                var updateExpense = _expenseTypeDAO.Update(model);
                if (updateExpense > 0)
                    return MessageBLL.Generate("Tipo de despesa atualizado com sucesso", 0);
                return MessageBLL.Generate("Erro ao atualizar tipo de despesa", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseTypeModel> Get()
        {
            try
            {
                return _expenseTypeDAO.Get();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseTypeModel Get_ByExpenseID(int expenseID)
        {
            try
            {
                return _expenseTypeDAO.Get_ByExpenseID(expenseID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseTypeModel> GetAll()
        {
            try
            {
                return _expenseTypeDAO.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseTypeModel Get(int id)
        {
            try
            {
                if (id == 0)
                    return null;
                return _expenseTypeDAO.Get(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel ValidateFields(ref ExpenseTypeModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    return MessageBLL.Generate("Informe o nome do tipo de despesa", 99, true);
                return MessageBLL.Generate("Formulário validado com sucesso", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
