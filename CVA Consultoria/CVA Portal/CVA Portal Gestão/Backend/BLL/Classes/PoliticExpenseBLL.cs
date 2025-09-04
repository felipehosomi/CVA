using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class PoliticExpenseBLL
    {
        #region Atributos
        private PoliticExpenseDAO _politicExpenseDAO { get; set; }
        #endregion

        #region Construtor
        public PoliticExpenseBLL()
        {
            this._politicExpenseDAO = new PoliticExpenseDAO();
        }
        #endregion

        public List<StatusModel> GetSpecificStatus()
        {
            throw new NotImplementedException();
        }

        public List<PoliticExpenseModel> Get()
        {
            throw new NotImplementedException();
        }

        public List<PoliticExpenseModel> GetBy_Project(int projectId, int user)
        {
            try
            {
                var data = _politicExpenseDAO.GetBy_Project(projectId, user);
                if (data.Rows.Count == 0)
                    return null;

                var list = new List<PoliticExpenseModel>();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var model =                 new PoliticExpenseModel();
                    //model.Value =               Convert.ToDecimal(data.Rows[i]["Value"].ToString());
                    model.Value =               data.Rows[i]["Value"].ToString();
                    model.Expense =             new ExpenseTypeModel();
                    model.Expense.Id =          Convert.ToInt32(data.Rows[i]["Expense.Id"].ToString());
                    model.Expense.UnitMeter =   Convert.ToInt32(data.Rows[i]["Expense.UnitMeter"].ToString());
                    model.Expense.Name =        data.Rows[i]["Expense.Name"].ToString();
                    
                    list.Add(model);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MessageModel Save(PoliticExpenseModel model)
        {
            try
            {
                var resultId = _politicExpenseDAO.Save(model);
                if (resultId == 0)
                    return MessageBLL.Generate("Ocorreu um erro ao salvar a política de despesa", 99, true);
                else
                    return MessageBLL.Generate("Política de despesa salva com sucesso", resultId);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }
    }
}
