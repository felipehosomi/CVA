using System;
using System.Data;
using MODEL.Classes;
using DAO.Resources;
using AUXILIAR;

namespace DAO.Classes
{
    public class NoteDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Note_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public DataTable Get_UserNotes(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Note_GetNotesByUser);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public DataTable Get_ProjectNotes(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Get_Notes_By_Project);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public DataTable Get_StepNotes(int id1, int id2)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Get_Notes_By_StepProject);
                    conn.InsertParameter("ID_PRJ", id1);
                    conn.InsertParameter("ID_FAS", id2);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public DataTable Get_DayNotes(int idUser, DateTime date)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Notes_Get_By_Day);
                    conn.InsertParameter("USR", idUser);
                    conn.InsertParameter("DAT", date);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public DataTable Search(NoteFilterModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Note_Search);
                    conn.InsertParameter("USR", model.UserId);
                    conn.InsertParameter("ID_PRJ", model.ProjectId);
                    conn.InsertParameter("STU_ID", model.StatusId);
                    conn.InsertParameter("CLI_ID", model.ClientId);
                    Insert_DateParameter(model, conn);

                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public int Insert(NoteModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Note_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", 1);
                    conn.InsertParameter("DSCR", model.Description);
                    conn.InsertParameter("SOL", model.Requester);
                    conn.InsertParameter("DAT", model.Date.Date);
                    conn.InsertParameter("ID_PRJ", model.Project.Id);
                    conn.InsertParameter("ID_SPC ", model.Specialty.Id);
                    conn.InsertParameter("ID_CHA", model.Ticket.Code);
                    conn.InsertParameter("VLR", model.Value);
                    conn.InsertParameter("ID_FAS", model.Step.Id);
                    Insert_HoursParameter(model, conn);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public int Update(NoteModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Note_Update);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("DSCR", model.Description);
                    conn.InsertParameter("SOL", model.Requester);
                    conn.InsertParameter("ID_PRJ", model.Project.Id);
                    conn.InsertParameter("ID_SPC ", model.Specialty.Id);
                    conn.InsertParameter("ID_CHA", model.Ticket.Code);
                    conn.InsertParameter("VLR", model.Value);
                    conn.InsertParameter("ID_FAS", model.Step.Id);
                    conn.InsertParameter("ID", model.Id);
                    Insert_HoursParameter(model, conn);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public int Remove(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Note_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #region Auxiliares
        private void Insert_HoursParameter(NoteModel model, Connection conn)
        {
            if (model.InitHour == DateTime.MinValue || model.InitHour == null)
                conn.InsertParameter("HOR_INI", DBNull.Value);
            else
                conn.InsertParameter("HOR_INI", model.InitHour);

            if (model.IntervalHour == DateTime.MinValue || model.IntervalHour == null)
                conn.InsertParameter("HOR_INT", DBNull.Value);
            else
                conn.InsertParameter("HOR_INT", model.IntervalHour);

            if (model.FinishHour == DateTime.MinValue || model.FinishHour == null)
                conn.InsertParameter("HOR_SAI", DBNull.Value);
            else
                conn.InsertParameter("HOR_SAI", model.FinishHour);
        }

        private void Insert_DateParameter(NoteFilterModel model, Connection conn)
        {
            if (model.InitialDate != null)
                conn.InsertParameter("DATA_INICIAL", model.InitialDate);
            else
                conn.InserParameter("DATA_INICIAL", SqlDbType.DateTime, DBNull.Value);

            if (model.FinishDate != null)
                conn.InsertParameter("DATA_FINAL", model.FinishDate);
            else
                conn.InserParameter("DATA_FINAL", SqlDbType.DateTime, DBNull.Value);
        }
        #endregion
    }
}