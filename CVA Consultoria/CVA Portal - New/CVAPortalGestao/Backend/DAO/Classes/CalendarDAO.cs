using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Resources;
using AUXILIAR;

namespace DAO.Classes
{
    public class CalendarDAO
    {
        public CalendarModel Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Calendar_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<CalendarModel>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CalendarModel> Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Calendar_Get_All);

                    return conn.GetResult().ToListData<CalendarModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(CalendarModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Calendar_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("NOM", model.Name);
                    conn.InsertParameter("DSCR", model.Description);
                    conn.InsertParameter("ANO", model.InitialDate);
                    conn.InsertParameter("ANOF", model.FinishDate);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(CalendarModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Calendar_Update);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("NOM", model.Name);
                    conn.InsertParameter("DSCR", model.Description);
                    conn.InsertParameter("ANO", model.InitialDate);
                    conn.InsertParameter("ANOF", model.FinishDate);
                    conn.InsertParameter("ID", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteHolidays(int calendarID)
        {
            using (Connection conn = new Connection())
            {
                conn.CreateDataAdapter(StoredProcedure.CalendarHolidays_Remove);
                conn.InsertParameter("ID", calendarID);

                return conn.GetResult().Success();
            }
        }

        public int SaveHolidays(Calendar1Model holidays)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.CalendarHolidays_Insert);
                    conn.InserParameter("USR", System.Data.SqlDbType.Int, holidays.User.Id);
                    conn.InserParameter("DAT", System.Data.SqlDbType.DateTime, holidays.Date);
                    conn.InserParameter("FER", System.Data.SqlDbType.VarChar, holidays.Description);
                    conn.InserParameter("CAL", System.Data.SqlDbType.Int, holidays.CalendarId);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public List<Calendar1Model> Get_Holidays(int calendarID)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.CalendarHolidays_Get);
                    conn.InsertParameter("ID", calendarID);
                    return conn.GetResult().ToListData<Calendar1Model>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public List<Calendar1Model> Get_All_Holidays(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Holidays_Get_All);
                    conn.InsertParameter("FROM", dateFrom);
                    conn.InsertParameter("TO", dateTo);

                    return conn.GetResult().ToListData<Calendar1Model>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}