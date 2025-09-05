using System;
using System.Linq;

namespace CVA.View.Comissionamento.Helpers
{
    static class Extensions
    {
        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }

    }
}
