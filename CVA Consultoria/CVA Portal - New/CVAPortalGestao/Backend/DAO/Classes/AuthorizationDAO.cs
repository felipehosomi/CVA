using AUXILIAR;
using DAO.Resources;
using MODEL.Classes;
using System;
using System.Data;

namespace DAO.Classes
{
    public class AuthorizationDAO
    {
        #region Dias
        public DataTable Get_DiasAutorizados(int idCol)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_Get);
                    conn.InsertParameter("ID_COL", idCol);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddDiaAutorizado(AuthorizedDayModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_COL", model.Colaborador);
                    conn.InsertParameter("DAT_INI", model.De);
                    conn.InsertParameter("DAT_FIM", model.Ate);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public int RemoveDiaAutorizado(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Dias

        #region Despesas
        public DataTable Get_DespesasAutorizados(int idCol)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpensePeriod_Get);
                    conn.InsertParameter("ID_COL", idCol);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddDespesaAutorizado(AuthorizedDayModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpensePeriod_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_COL", model.Colaborador);
                    conn.InsertParameter("DAT_INI", model.De);
                    conn.InsertParameter("DAT_FIM", model.Ate);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveDespesaAutorizado(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ExpensePeriod_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Despesas

        #region Horas
        public DataTable Get_HorasAutorizadas(int idCol)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.AuthorizedHours_Get);
                    conn.InsertParameter("ID", idCol);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddHorasAutorizadas(AuthorizedHoursModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.AuthorizedHours_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("COL", model.Colaborador);
                    conn.InsertParameter("DAT", model.Data.Date);
                    conn.InsertParameter("MAX_HRS", model.Horas);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveHorasAutorizadas(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.AuthorizedHours_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Get_LimiteHoras(int idCol)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.HoursLimit_Get);
                    conn.InsertParameter("ID_COL", idCol);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddLimiteHoras(HoursLimitModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.HoursLimit_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID", model.Colaborador);
                    conn.InsertParameter("MAX_HRS", model.Horas);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveLimiteHoras(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.HoursLimit_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Horas
    }
}