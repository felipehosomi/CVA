using DAO.Resources;
using System;
using MODEL.Classes;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{

    public class StepItemDAO
    {
        public int Insert(StepItemModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StepItem_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_FAS", model.Fase.Id);
                    conn.InsertParameter("ID_ESP", model.Especialidade.Id);
                    conn.InsertParameter("ID_COL", model.Colaborador.Id);
                    conn.InsertParameter("CST_ORC", model.CustoOrcado);
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Remove_ForProject(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StepItem_Remove_ForProject);
                    conn.InsertParameter("ID_FAS", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_ForProject(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StepItem_Get_ForProject);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(StepItemModel model)
        {
            throw new NotImplementedException();
        }

        public StepItemModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
