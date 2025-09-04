using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using AUXILIAR;
using System.Data;

namespace DAO.Classes
{
    public class SpecialtyDAO
    {
        public int Save(SpecialtyModel specialty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_Insert);
                    conn.InsertParameter("USR", specialty.User.Id);
                    conn.InsertParameter("STU", specialty.Status.Id);
                    conn.InsertParameter("FUNC", specialty.Name);
                    conn.InsertParameter("DSCR", specialty.Description);
                    conn.InsertParameter("VAL", specialty.Value);
                    conn.InsertParameter("TIP_ESP", specialty.TipoEspecialidade.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_Get);
                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public int Update(SpecialtyModel specialty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_Update);
                    conn.InsertParameter("USR", specialty.User.Id);
                    conn.InsertParameter("STU", specialty.Status.Id);
                    conn.InsertParameter("FUNC", specialty.Name);
                    conn.InsertParameter("DSCR", specialty.Description);
                    conn.InsertParameter("VAL", specialty.Value);
                    conn.InsertParameter("ID", specialty.Id);
                    conn.InsertParameter("TIP_ESP", specialty.TipoEspecialidade.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> Get_Specialty(int projectId, int stepId, int user)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_Get_Specialty);
                    conn.InsertParameter("ID_PRJ", projectId);
                    conn.InsertParameter("ID_FAS", stepId);
                    conn.InsertParameter("ID_COL", user);


                    return conn.GetResult().ToListData<SpecialtyModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyTypeModel> Get_TiposEspecialidade()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_Get_TiposEspecialidade);

                    return conn.GetResult().ToListData<SpecialtyTypeModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_ByID(int specialtyID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_GetId);
                    conn.InsertParameter("ID", specialtyID);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Specialty_Get_All);
                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> GetByCollaborator(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {//apagarumdesse
                    conn.CreateDataAdapter(StoredProcedure.Specialty_GetByCollaborator);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<SpecialtyModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(SpecialtyModel model)
        {
            throw new NotImplementedException();
        }

        public SpecialtyModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
