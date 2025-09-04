using MODEL.Interface;

namespace MODEL.Classes
{
    public class PricingItemModel : IModel
    {
        public SpecialtyModel Especialidade { get; set; }
        public string Colaborador { get; set; }

        public double EspecialidadeHoras { get; set; }
        public double EspecialidadeValor { get; set; }
        public double EspecialidadeCusto { get; set; }

        public double ValorBackoffice { get; set; }
        public double ValorRisco { get; set; }
        public double ValorMargem { get; set; }
        public double ValorComissao { get; set; }
        public double ValorImposto { get; set; }
               
        public double HotelDiarias { get; set; }
        public double HotelValor { get; set; }
        public double HotelTotal { get; set; }
        
        public double KmTrechos { get; set; }
        public double KmDistancia { get; set; }
        public double KmValor { get; set; }
        public double KmTotal { get; set; }
   
        public double AlimentacaoDias { get; set; }
        public double AlimentacaoValor { get; set; }
        public double AlimentacaoTotal { get; set; }

        public double DeslocamentoHoras { get; set; }
        public double DeslocamentoValor { get; set; }
        public double DeslocamentoTotal { get; set; }

        public double AereoTrechos { get; set; }
        public double AereoValor { get; set; }
        public double AereoTotal { get; set; }
   
        public double RecursoSubTotal { get; set; }
        public double RecursoDespesas { get; set; }
        public double RecursoValorComDespesas { get; set; }
        public double RecursoValorComImpostos { get; set; }
        public double RecursoValorHorasSemDespesa { get; set; }
        public double RecursoValorHorasComDespesa { get; set; }
    }
}
