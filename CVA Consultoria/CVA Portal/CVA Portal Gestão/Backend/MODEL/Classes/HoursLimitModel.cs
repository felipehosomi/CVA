using MODEL.Interface;
using System;

namespace MODEL.Classes
{
    public class HoursLimitModel : IModel
    {
        public int Colaborador { get; set; }
        public string ColaboradorNome { get; set; }
        public DateTime Horas { get; set; }
    }
}
