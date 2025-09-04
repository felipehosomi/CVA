using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class StatusReportModel : IModel
    {
        public ProjectModel Projeto { get; set; }
        public DateTime Data { get; set; }        
        public string Descricao { get; set; }
        public string PontosAtencao { get; set; }
        public string PlanoDeAcao { get; set; }
        public string Conquistas { get; set; }
        public string ProximosPassos { get; set; }
        public CollaboratorModel GerenteProjeto { get; set;}
        public string HorasOrcadas { get; set; }
        public string HorasConsumidas { get; set; }
        public string Concluido { get; set; }   
        public List<StepModel> Fases { get; set; }
    }
}
