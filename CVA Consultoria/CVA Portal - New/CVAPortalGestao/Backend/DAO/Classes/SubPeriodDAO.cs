using DAO.Resources;
using MODEL.Classes;
using System;
using AUXILIAR;
using System.Data;

namespace DAO.Classes
{
    public class SubPeriodDAO
    {
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

    }
}
