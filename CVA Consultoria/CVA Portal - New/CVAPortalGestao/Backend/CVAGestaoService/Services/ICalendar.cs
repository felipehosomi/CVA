using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface ICalendar
    {
        [OperationContract]
        MessageModel SaveCalendarHeader(CalendarModel calendar);

        [OperationContract]
        List<CalendarModel> GetCalendar();

        [OperationContract]
        CalendarModel GetCalendar_ById(int calendarID);


        [OperationContract]
        List<StatusModel> CalendarStatus();
    }
}