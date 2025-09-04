using System;
using System.Data;
using DAO.Resources;
using AUXILIAR;

namespace DAO.Classes
{
    public class UfDAO
    {
        public DataTable Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Uf_Get_All);
                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}