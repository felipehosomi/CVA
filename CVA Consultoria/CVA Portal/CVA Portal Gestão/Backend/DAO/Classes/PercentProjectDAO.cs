using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using AUXILIAR;

namespace DAO.Classes
{
    public class PercentProjectDAO 
    {
        public List<PercentProjectModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PercentProject_Get);
                    return conn.GetResult().ToListData<PercentProjectModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PercentProjectModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(PercentProjectModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(PercentProjectModel model)
        {
            throw new NotImplementedException();
        }
    }
}
