using MODEL.Interface;
using System;

namespace MODEL.Classes
{
    public class OpportunityModel : IModel
    {
        public string Codigo { get; set; }
        public string Tag { get; set; }
        public string Nome { get; set; }
        public ClientModel Cliente { get; set; }
        public DateTime DataPrevista { get; set; }
        public PercentProjectModel Temperatura { get; set; }
        public int Convertida { get; set; }
        public ProjectTypeModel TipoProjeto { get; set; }
        public CollaboratorModel Vendedor { get; set; }
        public string ResponsavelDespesa { get; set; }
        public string ValorOportunidade { get; set; }
        public string CustoOrcado { get; set; }
        public string HorasOrcadas { get; set; }
        public string IngressoLiquido { get; set; }
        public string RiscoGerenciavel { get; set; }
        public string IngressoTotal { get; set; }
        public PricingModel Pricing { get; set; }
    }
}