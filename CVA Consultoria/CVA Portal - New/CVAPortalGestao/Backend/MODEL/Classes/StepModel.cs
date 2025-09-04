using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class StepModel : IModel
    {
        public int ProjectId { get; set; }  
        public int StepId { get; set; }  
        public DateTime DataInicio { get; set; }  
        public DateTime DataPrevista { get; set; }  
        public string HorasOrcadas { get; set; }
        public string HorasOrcadasFormat
        {
            get
            {
                if (double.TryParse(HorasOrcadas, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out double horas))
                {
                    var ts = TimeSpan.FromHours(horas);
                    ts = TimeSpan.FromMinutes(1 * Math.Ceiling(ts.TotalMinutes / 1));
                    return $"{((int)ts.TotalHours).ToString().PadLeft(2, '0')}:{ts:mm}";
                }
                return null;
            }
        }
        public string HorasConsumidas { get; set; }
        public string HorasConsumidasFormat
        {
            get
            {
                if (double.TryParse(HorasConsumidas, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out double horas))
                {
                    var ts = TimeSpan.FromHours(horas);
                    ts = TimeSpan.FromMinutes(1 * Math.Ceiling(ts.TotalMinutes / 1));
                    return $"{((int)ts.TotalHours).ToString().PadLeft(2, '0')}:{ts:mm}";
                }
                return null;
            }
        }
        public string Liberada { get; set; }


        /* VERIFICAR ESTE CAMPO */
        public string Nome { get; set; }
    }
}
