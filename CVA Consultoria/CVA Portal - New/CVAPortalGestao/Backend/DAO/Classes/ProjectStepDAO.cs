using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class ProjectStepDAO
    {
        public int Save(ProjectStepModel projectStep)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_Insert);
                    InsertParameters(projectStep, conn);
                    conn.InsertParameter("IS_PRJ", projectStep.IsProjectStep);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<ProjectStepModel> Get(int isProject)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_Get);
                    conn.InsertParameter("IS_PRJ", isProject);

                    return conn.GetResult().ToListData<ProjectStepModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectStepModel> Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_GetAll);
                    return conn.GetResult().ToListData<ProjectStepModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(ProjectStepModel projectStep)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_Update);
                    InsertParameters(projectStep, conn);
                    conn.InsertParameter("ID", projectStep.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProjectStepModel Get_ByID(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_GetID);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<ProjectStepModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsertParameters(ProjectStepModel projectStep, Connection conn)
        {
            conn.InsertParameter("USR", projectStep.User.Id);
            conn.InsertParameter("STU", projectStep.Status.Id);
            conn.InsertParameter("COD", projectStep.Code);
            conn.InsertParameter("NOM", projectStep.Name);
            conn.InsertParameter("DSCR", projectStep.Description);
        }

        public DataTable Get_ProjectSteps(int id, int user)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_Get_ProjectSteps);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("ID_COL", user);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(ProjectStepModel model)
        {
            throw new NotImplementedException();
        }
    }
}
