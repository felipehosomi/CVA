using System;
using System.Collections.Generic;

using MODEL.Classes;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class ProjectModel : IModel
    {
        public string Tag { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataPrevista { get; set; }
        public ClientModel Cliente { get; set; }
        public ProjectTypeModel TipoProjeto { get; set; }
        public string ValorProjeto { get; set; }
        public string CustoOrcado { get; set; }
        public string CustoReal { get; set; }
        public string HorasOrcadas { get; set; }
        public string HorasConsumidas { get; set; }
        public string IngressoLiquido { get; set; }
        public string RiscoGerenciavel { get; set; }
        public string IngressoTotal { get; set; }
        public string ResponsavelDespesa { get; set; }
        public List<MemberModel> Membros { get; set; }
        public List<CollaboratorModel> Recursos { get; set; }
        public List<StepModel> Fases { get; set; }
        public List<StepItemModel> Itens { get; set; }
        public PricingModel Pricing { get; set; }
        public List<SpecialtyRuleModel> SpecialtyRules { get; set; }
        public CollaboratorModel Gerente { get; set; }
        public List<StatusReportModel> StatusReport { get; set; }
    }
}
