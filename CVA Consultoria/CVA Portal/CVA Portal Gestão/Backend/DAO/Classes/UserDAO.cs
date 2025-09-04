using System;
using System.Collections.Generic;
using System.Linq;
using MODEL.Classes;
using DAO.Resources;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class UserDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_GetId);
                    conn.InserParameter("ID", System.Data.SqlDbType.Int, id);

                    return conn.GetResult();//.ToListData<UserModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> Get_All()
        {
            using (Connection conn = new Connection())
            {
                conn.CreateDataAdapter(StoredProcedure.User_Get);
                return conn.GetResult().ToListData<UserModel>();
            }
        }

        public int Insert(UserModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("NOM", model.Name);
                    conn.InsertParameter("PAS", model.Password);
                    conn.InsertParameter("EML", model.Email);
                    conn.InsertParameter("CON", 0);
                    conn.InsertParameter("COL_ID", model.Collaborator.Id);
                    conn.InsertParameter("PER_ID", model.Profile.Id);
                    conn.InsertParameter("FIL_ID", "3");
                    conn.InsertParameter("PRJ_BND", 0);
                    conn.InsertParameter("EXT_USR", "N");

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(UserModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_Update);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("NOM", model.Name);
                    if (string.IsNullOrEmpty(model.Password))
                        conn.InserParameter("PAS", System.Data.SqlDbType.VarChar, DBNull.Value);
                    else
                        conn.InsertParameter("PAS", model.Password);
                    conn.InsertParameter("EML", model.Email);
                    conn.InsertParameter("FIL_ID", "3");
                    conn.InsertParameter("PRJ_BND", 0);
                    conn.InsertParameter("EXT_USR", "N");
                    conn.InsertParameter("PER_ID", model.Profile.Id);
                    conn.InsertParameter("ID", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int IsExist(string email)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_Exist);
                    conn.InsertParameter("EML", email);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable User1_Get(int userID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User1_Get);
                    conn.InsertParameter("ID_USR", userID);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public int Update_ByUSer(UserModel user)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_UpdateByUser);
                    conn.InsertParameter("PAS", user.Password);
                    conn.InsertParameter("ID", user.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> GetUsers()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_Get);
                    return conn.GetResult().ToListData<UserModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
