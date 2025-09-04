using System;
using System.Collections.Generic;
using DAO.Resources;
using MODEL.Classes;
using AUXILIAR;

namespace DAO.Classes
{
    public class UserViewDAO 
    {
        public List<UserViewModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.View_Get);
                    return conn.GetResult().ToListData<UserViewModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserViewModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserViewModel> Get_ByProfileID(int profileID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.View_GetProfile);
                    conn.InsertParameter("ID", profileID);

                    return conn.GetResult().ToListData<UserViewModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(UserViewModel model)
        {
            throw new NotImplementedException();
        }

        public int Update(UserViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
