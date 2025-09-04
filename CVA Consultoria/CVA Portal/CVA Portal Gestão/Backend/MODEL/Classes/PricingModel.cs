using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class PricingModel : IModel
    {
        public double PorcentagemBackoffice { get; set; }
        public double PorcentagemRisco { get; set; }
        public double PorcentagemMargem { get; set; }
        public double PorcentagemComissao { get; set; }
        public double PorcentagemImposto { get; set; }
        public List<PricingItemModel> ItensPricing { get; set; }





        //public string PorcentagemBackoffice { get; set; }
        //public string PorcentagemRisco { get; set; }
        //public string PorcentagemMargem { get; set; }
        //public string PorcentagemComissao { get; set; }
        //public string PorcentagemImposto { get; set; }
        //public List<PricingItemModel> ItensPricing { get; set; }

        //public string SomaHoras { get; set; }
        //public string SomaCusto { get; set; }
                        
        //public string SomaBackoffice { get; set; }
        //public string SomaRisco { get; set; }
        //public string SomaMargem { get; set; }
        //public string SomaComissao { get; set; }
        //public string SomaImposto { get; set; }

        //public string SomaSubTotal { get; set; }
        //public string SomaDespesas { get; set; }
        //public string SomaComDespesas {get;set;}
        //public string SomaComImpostos { get; set; }



        //public string HotelDiarias { get; set; }
        //public string HotelValor { get; set; }
        //public string HotelTotal { get; set; }

        //public string KmTrechos { get; set; }
        //public string KmDistancia { get; set; }
        //public string KmValor { get; set; }
        //public string KmTotal { get; set; }


        //public string AlimentacaoDias { get; set; }
        //public string AlimentacaoValor { get; set; }
        //public string AlimentacaoTotal { get; set; }

        //public string DeslocamentoHoras { get; set; }
        //public string DeslocamentoValor { get; set; }
        //public string DeslocamentoTotal { get; set; }

        //public string AereoTrechos { get; set; }
        //public string AereoValor { get; set; }
        //public string AereoTotal { get; set; }




    }
}
