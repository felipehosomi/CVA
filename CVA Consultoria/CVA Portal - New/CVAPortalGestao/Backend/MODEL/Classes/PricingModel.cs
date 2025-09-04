using MODEL.Interface;
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
        public List<PricingItemModel> Itens { get; set; }
    }
}
