using MODEL.Classes;
using MODEL.Interface;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class ChangeRequestModel : IModel
    {
        public ProjectModel Projeto { get; set; }
        public string Codigo { get; set; } 
        public string Versao { get; set; } 
        public string Autor { get; set; } 
        public string Situacao { get; set; } 
        public string GPI { get; set; } 
        public string GPE { get; set; }
        public string Departamento { get; set; } 
        public string Processo { get; set; }
        public string Descricao { get; set; } 
        public string Motivos { get; set; } 
        public string Recomendacoes { get; set; }
        public string ImpactosPositivos { get; set; } 
        public string ImpactosNegativos { get; set; }
        public List<ChangeRequestRecursoModel> RecursosSolicitados { get; set; } 
  
    }

    public class ChangeRequestRecursoModel
    {
        public ChangeRequestModel ChangeRequest { get; set; }
        public int RecursoFase { get; set; }
        public string RecursoFaseNome { get; set; }
        public int RecursoEspecialidade { get; set; }
        public string RecursoEspecialidadeNome{ get; set; }
        public string RecursoHorasSolicitadas { get; set; }
        public string RecursoSolicitante { get; set; }
        public string RecursoNecessidade { get; set; }
    }
}