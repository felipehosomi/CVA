using MODEL.Interface;
using System;

namespace MODEL.Classes
{
    public class AuthorizedHoursModel : IModel
    {
        public int Colaborador { get; set; }
        public string ColaboradorNome { get; set; }
        public DateTime Horas { get; set; }
        public DateTime Data { get; set; }
    }
}
