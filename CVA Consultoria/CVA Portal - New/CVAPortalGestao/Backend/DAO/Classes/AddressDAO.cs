using DAO.Resources;
using MODEL.Classes;
using System;
using AUXILIAR;

namespace DAO.Classes
{
    public class AddressDAO
    {
        public int Insert(AddressModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Address_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("STT", model.Street);
                    conn.InsertParameter("STTNO", model.StreetNo);
                    conn.InsertParameter("BLC", model.Block);
                    conn.InsertParameter("CIT", model.City);
                    conn.InsertParameter("UF", model.Uf.Id);
                    conn.InsertParameter("ZIP", model.ZipCode);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int Update(AddressModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Address_Update);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("STT", model.Street);
                    conn.InsertParameter("STTNO", model.StreetNo);
                    conn.InsertParameter("BLC", model.Block);
                    conn.InsertParameter("CIT", model.City);
                    conn.InsertParameter("UF", model.Uf.Id);
                    conn.InsertParameter("ZIP", model.ZipCode);
                    conn.InsertParameter("ID", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}