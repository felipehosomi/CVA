using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using AUXILIAR;

namespace DAO.Classes
{
    public class GenreDAO 
    {
        public List<GenreModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Genre_Get);
                    return conn.GetResult().ToListData<GenreModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GenreModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(GenreModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(GenreModel model)
        {
            throw new NotImplementedException();
        }
    }
}
