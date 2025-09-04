using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class ResourceModel : IModel
    {
        public int ColaboradorId { get; set; }
        public string ColaboradorNome { get; set; }
        public int FaseId { get; set; }
        public string FaseNome { get; set; }
        public int EspecialidadeId { get; set; }
        public string EspecialidadeNome { get; set; }
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
        public double HorasConsumidas { get; set; }
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
