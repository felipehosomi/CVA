using Fibra.ItemPricing.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Fibra.ItemPricing.Controllers
{
    internal class ItemsController
    {
        internal async Task SetPrice(string itemCode, int priceList, double cost)
        {
            var item = await ItemPricing.SLConnection.Request($"Items('{itemCode}')").GetAsync<ItemMasterData>();
            item.ItemPrices.Where(x => x.PriceList == priceList).FirstOrDefault().Price = cost;
            await ItemPricing.SLConnection.Request($"Items('{itemCode}')").PatchAsync(item);
        }
    }
}