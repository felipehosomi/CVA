using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using AUXILIAR;

namespace DAO.Classes
{
    public class ExpenseTypeDAO
    {
        public int Save(ExpenseTypeModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpenseType_Insert);
                    LoadParameters(model, conn);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {   
                throw;
            }
        }

        private static void LoadParameters(ExpenseTypeModel model, Connection conn)
        {
            conn.InsertParameter("USR", model.User.Id);
            conn.InsertParameter("STU", model.Status.Id);
            conn.InsertParameter("NOM", model.Name);
            conn.InsertParameter("DSCR", model.Description);
            conn.InsertParameter("UN_MED", model.UnitMeter);
        }

        public List<ExpenseTypeModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpenseType_Get);
                    return conn.GetResult().ToListData<ExpenseTypeModel>();
                }
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
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpenseType_GetAll);
                    return conn.GetResult().ToListData<ExpenseTypeModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ExpenseTypeModel Get_ByExpenseID(int expenseId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpenseType_GetExpense);
                    conn.InsertParameter("ID", expenseId);

                    return conn.GetResult().ToListData<ExpenseTypeModel>().FirstOrDefault();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }   

        public ExpenseTypeModel Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpenseType_GetId);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<ExpenseTypeModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(ExpenseTypeModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpenseType_Update);
                    LoadParameters(model, conn);
                    conn.InsertParameter("ID", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(ExpenseTypeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
