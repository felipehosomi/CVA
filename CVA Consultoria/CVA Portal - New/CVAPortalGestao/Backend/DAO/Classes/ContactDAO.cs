using DAO.Resources;
using MODEL.Classes;
using System;
using AUXILIAR;

namespace DAO.Classes
{
    public class ContactDAO 
    {
        public ContactModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(ContactModel model)
        {
            throw new NotImplementedException();
        }

        public int Save(ContactModel contact)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Contact_Insert);
                    conn.InsertParameter("USR", contact.User.Id);
                    conn.InsertParameter("STU", contact.Status.Id);
                    conn.InsertParameter("NOM", contact.Name);
                    conn.InsertParameter("EMA", contact.Email);
                    conn.InsertParameter("TEL", contact.Phone);
                    conn.InsertParameter("CEL", contact.CellPhone);
                    conn.InsertParameter("SIT", contact.Site);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(ContactModel contact)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Contact_Update);
                    conn.InsertParameter("USR", contact.User.Id);
                    conn.InsertParameter("STU", contact.Status.Id);
                    conn.InsertParameter("NOM", contact.Name);
                    conn.InsertParameter("EMA", contact.Email);
                    conn.InsertParameter("TEL", contact.Phone);
                    conn.InsertParameter("CEL", contact.CellPhone);
                    conn.InsertParameter("SIT", contact.Site);
                    conn.InsertParameter("ID", contact.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
