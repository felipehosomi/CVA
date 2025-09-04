using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class OpportunityModel : IModel
    {
        public int Convertida { get; set; }
        public string Tag { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ClientModel Cliente { get; set; }
        public PercentProjectModel Temperatura { get; set; }
        public ProjectTypeModel TipoProjeto { get; set; }
        public DateTime DataPrevista { get; set; }
        public ContactModel ContatoComercial { get; set; }
        public CollaboratorModel Responsavel { get; set; }
        public CollaboratorModel Vendedor { get; set; }
        public CollaboratorModel Tecnico { get; set; }
        public string ResponsavelDespesa { get; set; }
        public List<OportunittyStepsModel> Fases { get; set; }
        public List<OportunittyObservationModel> Detalhes { get; set; }
        public PricingModel Pricing { get; set; }
        public string ValorOportunidade { get; set; }
        public string CustoOrcado { get; set; }
        public string HorasOrcadas { get; set; }
        public string IngressoLiquido { get; set; }
        public string RiscoGerenciavel { get; set; }
        public string IngressoTotal { get; set; }
    }
}