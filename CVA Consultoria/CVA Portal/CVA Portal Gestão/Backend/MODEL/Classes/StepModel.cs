using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class StepModel : IModel
    {
        public int StepId { get; set; }
        public int ProjectId { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataPrevista { get; set; }
        public string CustoOrcado { get; set; }
        public string CustoReal { get; set; }
        public string HorasOrcadas { get; set; }
        public string HorasConsumidas { get; set; }
        public string Concluido { get; set; }
    }
}
