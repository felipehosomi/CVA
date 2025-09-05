using CVA.AddOn.Common.Controllers;

namespace CVA.View.MaxFlex.Model
{
    public class TabelaPrecoSerieModel
    {
        [ModelController(UIFieldName = "Cód. Item")]
        public string ItemCode { get; set; }

        [ModelController(UIFieldName = "SysNumber")]
        public int SysSerial { get; set; }

        [ModelController(UIFieldName = "Preço Novo")]
        public double Price { get; set; }
    }
}
