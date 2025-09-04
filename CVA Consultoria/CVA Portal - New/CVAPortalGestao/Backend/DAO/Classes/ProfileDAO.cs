using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Resources;
using AUXILIAR;

namespace DAO.Classes
{
    public class ProfileDAO
    {
        public List<ProfileModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile_Get);
                    return conn.GetResult().ToListData<ProfileModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save(ProfileModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("PER", model.Name);
                    conn.InsertParameter("DSCR", model.Description);
                    conn.InsertParameter("SIG", model.Initials);
                    conn.InsertParameter("FIN_ACC", model.FinancialAccess);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProfileModel GetUser(int userId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile_GetUser);
                    conn.InsertParameter("ID", userId);
                    return conn.GetResult().ToListData<ProfileModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Profile1_Save(ProfileModel profile, int viewID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile1_Insert);
                    conn.InsertParameter("USR", profile.User.Id);
                    conn.InsertParameter("STU", profile.Status.Id);
                    conn.InsertParameter("VIEW_ID", viewID);
                    conn.InsertParameter("ID", profile.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProfileModel Get_ByID(int profileID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile_GetId);
                    conn.InsertParameter("ID", profileID);

                    return conn.GetResult().ToListData<ProfileModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(ProfileModel profile)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile_Update);
                    conn.InsertParameter("USR", profile.User.Id);
                    conn.InsertParameter("STU", profile.Status.Id);
                    conn.InsertParameter("PER", profile.Name);
                    conn.InsertParameter("DSCR", profile.Description);
                    conn.InsertParameter("FIN_ACC", profile.FinancialAccess);
                    conn.InsertParameter("ID", profile.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Profile1_Inactive(int profileID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile1_Inactive);
                    conn.InsertParameter("PER_ID", profileID);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Profile1_Update(ProfileModel profile, int userViewID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Profile1_Update);
                    conn.InsertParameter("VIEW_ID", userViewID);
                    conn.InsertParameter("PER_ID", profile.Id);
                    conn.InsertParameter("USR", profile.User.Id);
                    conn.InsertParameter("STU", profile.Status.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(ProfileModel model)
        {
            throw new NotImplementedException();
        }

        public ProfileModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
