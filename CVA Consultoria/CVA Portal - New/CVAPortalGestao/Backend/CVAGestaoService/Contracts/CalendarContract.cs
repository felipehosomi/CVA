using System.Collections.Generic;
using System.Runtime.Serialization;

using MODEL.Classes;
using BLL.Classes;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class CalendarContract
    {
        private CalendarBLL CalendarBLL { get; set; }

        public CalendarContract()
        {
            this.CalendarBLL = new CalendarBLL();
        }

        public MessageModel SaveCalendarHeader(CalendarModel calendar)
        {
            return CalendarBLL.Save(calendar);
        }

        public CalendarModel Get_ByID(int calendarID)
        {
            return CalendarBLL.GetById(calendarID);
        }

        public List<CalendarModel> GetCalendar()
        {
            return CalendarBLL.Get();
        }


        public List<StatusModel> GetSpecificStatus()
        {
            return CalendarBLL.GetSpecificStatus();
        }
    }
}