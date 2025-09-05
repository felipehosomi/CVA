using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiros.Magento.AddOn.Model.Magento
{
    public class CategoryModel
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
        public int position { get; set; }
        public int level { get; set; }
        public int product_count { get; set; }
        public List<CategoryModel> children_data { get; set; }
    }
}
