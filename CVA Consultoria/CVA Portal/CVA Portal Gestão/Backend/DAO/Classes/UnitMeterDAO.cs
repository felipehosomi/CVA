using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using AUXILIAR;

namespace DAO.Classes
{
    public class UnitMeterDAO
    {
        public List<UnitMeterModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.UnitMeter_Get);
                    return conn.GetResult().ToListData<UnitMeterModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UnitMeterModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(UnitMeterModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(UnitMeterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
