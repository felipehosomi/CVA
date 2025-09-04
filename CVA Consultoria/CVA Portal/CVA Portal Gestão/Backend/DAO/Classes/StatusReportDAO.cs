using MODEL.Classes;
using System;
using DAO.Resources;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class StatusReportDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_All(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Get_All);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_ParcialHours_Orc(int id, DateTime data)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Get_ParcialHours_Orc);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_StatusReport_Info(int id, DateTime date)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_StatusReportInfo);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("DAT", date.AddDays(1));

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_ParcialHours_Con(int id, DateTime data)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Get_ParcialHours_Con);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("DAT", data);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(StatusReportModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_StatusReport_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_PRJ", model.Projeto.Id);
                    conn.InsertParameter("ID_GER", model.GerenteProjeto.Id);
                    conn.InsertParameter("DAT", model.Data);
                    conn.InsertParameter("HOR_ORC", Math.Round(Convert.ToDouble(model.HorasOrcadas), 2));
                    conn.InsertParameter("HOR_CON", Math.Round(Convert.ToDouble(model.HorasConsumidas), 2));
                    conn.InsertParameter("DSC", model.Descricao);
                    conn.InsertParameter("PON_ATC", model.PontosAtencao);
                    conn.InsertParameter("PLN_ACA", model.PlanoDeAcao);
                    conn.InsertParameter("CQT", model.Conquistas);
                    conn.InsertParameter("PRX_PAS", model.ProximosPassos);
                    conn.InsertParameter("PCT_CON", model.Concluido);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Steps(StatusReportModel model, StepModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Insert_Steps);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", 1);
                    conn.InsertParameter("ID_STU_RPT", model.Id);
                    conn.InsertParameter("ID_FAS_PRJ", item.StepId);
                    conn.InsertParameter("HOR_ORC", item.HorasOrcadas);
                    conn.InsertParameter("HOR_CON", item.HorasConsumidas);
                    conn.InsertParameter("PCT_CON", item.Concluido);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }
    }
}