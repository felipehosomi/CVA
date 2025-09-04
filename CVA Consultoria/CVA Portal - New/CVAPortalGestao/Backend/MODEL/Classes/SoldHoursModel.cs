using System;

namespace MODEL.Classes
{
    public class SoldHoursModel
    {
        public string Especialidade { get; set; }
        public int Horas { get; set; }
        public string HorasFormat
        {
            get
            {
                var ts = TimeSpan.FromHours(Horas);
                ts = TimeSpan.FromMinutes(1 * Math.Ceiling(ts.TotalMinutes / 1));
                return $"{((int)ts.TotalHours).ToString().PadLeft(2, '0')}:{ts:mm}";
            }
        }
        public Double HorasConsumidas { get; set; }
        public string HorasConsumidasFormat
        {
            get
            {
                var ts = TimeSpan.FromHours(HorasConsumidas);
                ts = TimeSpan.FromMinutes(1 * Math.Ceiling(ts.TotalMinutes / 1));
                return $"{((int)ts.TotalHours).ToString().PadLeft(2, '0')}:{ts:mm}";
            }
        } 
    }
}
