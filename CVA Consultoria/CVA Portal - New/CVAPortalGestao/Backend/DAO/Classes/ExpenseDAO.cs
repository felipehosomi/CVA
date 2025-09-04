using DAO.Resources;
using MODEL.Classes;
using System;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class ExpenseDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Expense_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_ByUser(int id, int mes, int ano)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Expense_Get_ByUser);
                    conn.InsertParameter("USR", id);
                    conn.InsertParameter("Mes", mes);
                    conn.InsertParameter("Ano", ano);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert(ExpenseModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Expense_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", 1);
                    conn.InsertParameter("ID_PRJ", model.Projeto.Id);
                    conn.InsertParameter("DAT", model.Data);
                    conn.InsertParameter("ID_TIP_DESP", model.TipoDespesa.Id);
                    conn.InsertParameter("DOC_NUM", model.NumNota);
                    conn.InsertParameter("DOC_NUM_TOT", Convert.ToDecimal(model.ValorNota));
                    if (!string.IsNullOrEmpty(model.ValorDespesa))
                        conn.InsertParameter("VLR_REM_USR", Convert.ToDecimal(model.ValorDespesa));
                    else
                        conn.InserParameter("VLR_REM_USR", SqlDbType.Decimal, DBNull.Value);

                    if (string.IsNullOrEmpty(model.Quilometragem))
                        conn.InserParameter("KIL", SqlDbType.Int, DBNull.Value);
                    else
                        conn.InsertParameter("KIL", Convert.ToDecimal(model.Quilometragem));

                    conn.InsertParameter("DSCR", model.Descricao);
                    conn.InsertParameter("VLR_REM", Convert.ToDecimal(model.ValorReembolso));


                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Update(ExpenseModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Expense_Update);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("DAT", model.Data);
                    conn.InsertParameter("ID_TIP_DESP", model.TipoDespesa.Id);
                    conn.InsertParameter("DOC_NUM", model.NumNota);
                    conn.InsertParameter("DOC_NUM_TOT", Convert.ToDecimal(model.ValorNota));

                    if (!string.IsNullOrEmpty(model.ValorDespesa))
                        conn.InsertParameter("VLR_REM_USR", Convert.ToDecimal(model.ValorDespesa));
                    else
                        conn.InserParameter("VLR_REM_USR", SqlDbType.Decimal, DBNull.Value);

                    if (string.IsNullOrEmpty(model.Quilometragem))
                        conn.InserParameter("KIL", SqlDbType.Int, DBNull.Value);
                    else
                        conn.InsertParameter("KIL", model.Quilometragem);

                    conn.InsertParameter("DSCR", model.Descricao);
                    conn.InsertParameter("VLR_REM", Convert.ToDecimal(model.ValorReembolso));
                    conn.InsertParameter("ID", model.Id);
                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Expense_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Search(int col, int cli, int prj, DateTime? de, DateTime? ate)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Expense_Extract);
                    conn.InsertParameter("COL", col);
                    conn.InsertParameter("CLI", cli);
                    conn.InsertParameter("PRJ", prj);

                    if (de != null)
                        conn.InsertParameter("DE", de);
                    else
                        conn.InserParameter("DE", SqlDbType.DateTime, DBNull.Value);

                    if (ate != null)
                        conn.InsertParameter("ATE", ate);
                    else
                        conn.InserParameter("ATE", SqlDbType.DateTime, DBNull.Value);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }
    }
}