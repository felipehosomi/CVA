using MODEL.Interface;
using System;

namespace MODEL.Classes
{
    public class AuthorizedDayModel : IModel
    {
        public int Colaborador { get; set; }
        public string ColaboradorNome { get; set; }
        public DateTime De { get; set; }
        public DateTime Ate { get; set; }
    }
}