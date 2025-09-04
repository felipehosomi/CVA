using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using AUXILIAR;

namespace DAO.Classes
{
    public class MaritalStatusDAO
    {
        public List<MaritalStatusModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.MaritalStatus_Get);
                    return conn.GetResult().ToListData<MaritalStatusModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MaritalStatusModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(MaritalStatusModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(MaritalStatusModel model)
        {
            throw new NotImplementedException();
        }
    }
}
