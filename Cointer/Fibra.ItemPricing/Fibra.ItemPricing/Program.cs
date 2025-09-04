namespace Fibra.ItemPricing
{
    class Program
    {
        static void Main(string[] args)
        {
            var itemPrincing = new ItemPricing();
            itemPrincing.SetItemPrice().Wait();
        }
    }
}
