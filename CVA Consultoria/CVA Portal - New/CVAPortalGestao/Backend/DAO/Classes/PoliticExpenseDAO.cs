using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class PoliticExpenseDAO
    {
        public int Save(PoliticExpenseModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PoliticExpense_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("ID_TIP_DESP", model.Expense.Id);
                    conn.InsertParameter("VLR", Convert.ToDecimal(model.Value));

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PoliticExpenseModel> Get()
        {
            throw new NotImplementedException();
        }

        public DataTable GetBy_Project(int projectId, int user)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PoliticExpense_GetProject);
                    conn.InsertParameter("ID", projectId);
                    conn.InsertParameter("USR", user);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(PoliticExpenseModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(PoliticExpenseModel model)
        {
            throw new NotImplementedException();
        }

        public PoliticExpenseModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
