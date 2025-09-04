using MODEL.Interface;
using System;

namespace MODEL.Classes
{
    public class OportunittyStepsModel : IModel
    {
        public ProjectStepModel ProjectStep { get; set; }
        public string  Description { get; set; }
        public DateTime DateConclusion { get; set; }
        public DateTime DateInit { get; set; }
        public string CustoOrcado { get; set; }
        public string HorasOrcadas { get; set; }
        public string CustoReal { get; set; }
        public string HorasConsumidas { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataPrevista { get; set; }
    }
} 