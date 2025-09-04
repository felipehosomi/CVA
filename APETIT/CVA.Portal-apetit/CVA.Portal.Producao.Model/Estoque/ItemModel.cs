using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Estoque
{
    public class ItemModel
    {
        [ModelController(ColumnName = "ItemCode")]
        public string ItemCode { get; set; }

        [ModelController(ColumnName = "ItemName")]
        public string ItemName { get; set; }

        public string Item
        {
            get
            {
                return ItemCode + " - " + ItemName;
            }
        }
    }
}
