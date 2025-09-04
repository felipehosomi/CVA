using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Producao
{
    public class EtapaItinerarioModel
    {
        [ModelController]
        public int AbsEntry { get; set; }

        [ModelController]
        public string Code { get; set; }

        [ModelController]
        public string Desc { get; set; }
    }
}
