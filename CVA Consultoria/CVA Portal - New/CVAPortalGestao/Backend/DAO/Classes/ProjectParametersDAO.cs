using AUXILIAR;
using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;

namespace DAO.Classes
{
    public class ProjectParametersDAO
    {
        public ProjectParametersModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProjectParametersModel> Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectParameters_Get_All);
                    
                    return conn.GetResult().ToListData<ProjectParametersModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Insert(ProjectParametersModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectParameters_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("EQP", model.Equipe);
                    conn.InsertParameter("DE", model.De);
                    conn.InsertParameter("ATE", model.Ate);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Update(ProjectParametersModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectParameters_Update);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("EQP", model.Equipe);
                    conn.InsertParameter("DE", model.De);
                    conn.InsertParameter("ATE", model.Ate);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
