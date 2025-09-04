using System;
using System.Data;
using DAO.Resources;
using AUXILIAR;


namespace DAO.Classes
{
    public class StatusDAO
    {
        public DataTable Get(int objectId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Status_Get);
                    conn.InsertParameter("OBJ", objectId);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }       
    }
}
