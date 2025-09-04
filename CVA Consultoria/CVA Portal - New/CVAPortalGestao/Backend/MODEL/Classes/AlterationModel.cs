using System;

namespace MODEL.Classes
{
    public class AlterationModel
    {
        public DateTime Data { get; set; }
        public String Descricao { get; set; }
        public String FaseNome { get;set;}
        public String EspecialidadeNome { get;set;}
        public String ColaboradorNome { get;set;}
        public int HorasAdicionadas { get; set; }
    }
}
