using System;
using System.Linq;
using DAO.Resources;

using MODEL.Classes;
using AUXILIAR;

namespace DAO.Classes
{
    public class LoginDAO
    {
           public int AlterPassword(string email, string password)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_AlterPassword);
                    conn.InsertParameter("EMA", email);
                    conn.InsertParameter("PAS", password);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int LogOff(int userId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Logoff);
                    conn.InserParameter("USR", System.Data.SqlDbType.Int, userId);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel LogIn(string email)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Login);
                    conn.InserParameter("EML", System.Data.SqlDbType.VarChar, email);

                    return conn.GetResult().ToListData<UserModel>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
