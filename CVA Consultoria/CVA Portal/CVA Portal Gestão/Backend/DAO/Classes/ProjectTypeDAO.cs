using MODEL.Classes;
using System;
using System.Data;
using DAO.Resources;
using AUXILIAR;

namespace DAO.Classes
{
    public class ProjectTypeDAO
    {
        public DataTable Get(int id)
        {
            using (Connection conn = new Connection())
            {
                conn.CreateDataAdapter(StoredProcedure.ProjectType_Get);
                conn.InsertParameter("ID", id);

                return conn.GetResult();
            }
        }

        public DataTable Get_All()
        {
            using (Connection conn = new Connection())
            {
                conn.CreateDataAdapter(StoredProcedure.ProjectType_Get_All);
                return conn.GetResult();
            }
        }

        public int Insert(ProjectTypeModel model)
        {
            using (Connection conn = new Connection())
            {
                conn.CreateDataAdapter(StoredProcedure.ProjectType_Insert);
                conn.InsertParameter("USR", model.User.Id);
                conn.InsertParameter("NOM", model.Nome);
                conn.InsertParameter("AMS", Convert.ToInt32(model.AMS));
                conn.InsertParameter("TEA", model.Equipe);
                conn.InsertParameter("DSC", model.Descricao);

                return conn.GetResult().Success();
            }
        }

        public int Update(ProjectTypeModel model)
        {
            using (Connection conn = new Connection())
            {
                conn.CreateDataAdapter(StoredProcedure.ProjectType_Update);
                conn.InsertParameter("ID", model.Id);
                conn.InsertParameter("USR", model.User.Id);
                conn.InsertParameter("NOM", model.Nome);
                conn.InsertParameter("AMS", Convert.ToInt32(model.AMS));
                conn.InsertParameter("TEA", model.Equipe);
                conn.InsertParameter("DSC", model.Descricao);

                return conn.GetResult().Success();
            }
        }

        public int Remove(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectType_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
