using DAO.Resources;
using MODEL.Classes;
using System;
using AUXILIAR;
using System.Data;

namespace DAO.Classes
{
    public class SubPeriodDAO
    {
        public int Save(SubPeriodModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    if (model.CollaboratorId.HasValue && model.CollaboratorId != 0)
                        conn.InsertParameter("COL_ID", model.CollaboratorId);
                    else
                        conn.InserParameter("COL_ID", System.Data.SqlDbType.Int, DBNull.Value);
                    
                    if (model.ClientId.HasValue && model.ClientId != 0)
                        conn.InsertParameter("CLI_ID", model.ClientId);
                    else
                        conn.InserParameter("CLI_ID", System.Data.SqlDbType.Int, DBNull.Value);

                    if (model.ProjectId.HasValue && model.ProjectId != 0)
                        conn.InsertParameter("PRJ_ID", model.ProjectId);
                    else
                        conn.InserParameter("PRJ_ID", System.Data.SqlDbType.Int, DBNull.Value);

                    conn.InsertParameter("DAT_INI", model.DateFrom);
                    conn.InsertParameter("DAT_FIM", model.DateTo);
                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get(int? colId, int? clientId, int? projectId, DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_Get);
                    if (colId.HasValue)
                    {
                        conn.InsertParameter("COL_ID", colId);
                    }
                    else
                    {
                        conn.InserParameter("COL_ID", System.Data.SqlDbType.Int, DBNull.Value);
                    }
                    if (clientId.HasValue)
                    {
                        conn.InsertParameter("CLI_ID", clientId);
                    }
                    else
                    {
                        conn.InserParameter("CLI_ID", System.Data.SqlDbType.Int, DBNull.Value);
                    }
                    if (projectId.HasValue)
                    {
                        conn.InsertParameter("PRJ_ID", projectId);
                    }
                    else
                    {
                        conn.InserParameter("PRJ_ID", System.Data.SqlDbType.Int, DBNull.Value);
                    }
                    if (dateFrom.HasValue)
                    {
                        conn.InsertParameter("DAT_INI", dateFrom);
                    }
                    else
                    {
                        conn.InserParameter("DAT_INI", System.Data.SqlDbType.DateTime, DBNull.Value);
                    }
                    if (dateTo.HasValue)
                    {
                        conn.InsertParameter("DAT_FIM", dateTo);
                    }
                    else
                    {
                        conn.InserParameter("DAT_FIM", System.Data.SqlDbType.DateTime, DBNull.Value);
                    }

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SetStatus(int periodId, int statusId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_UpdateStatus);
                    conn.InsertParameter("ID", periodId);
                    conn.InsertParameter("STU", statusId);
                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SetStatusOnList(string periodIdList, int statusId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.SubPeriod_UpdateListStatus);
                    conn.InsertParameter("ID_LIST", periodIdList);
                    conn.InsertParameter("STU", statusId);
                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(SubPeriodModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(SubPeriodModel model)
        {
            throw new NotImplementedException();
        }

        public SubPeriodModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
