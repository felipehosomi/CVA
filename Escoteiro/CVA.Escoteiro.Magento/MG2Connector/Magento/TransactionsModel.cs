using System.Collections.Generic;

namespace CVA.Escoteiro.Magento.Models.Magento
{
    public class TransactionsModel
    {
        public List<Item> items { get; set; }
        public SearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }

        public class Item
        {
            public int transaction_id { get; set; }
            public int order_id { get; set; }
            public int payment_id { get; set; }
            public string txn_id { get; set; }
            public object parent_txn_id { get; set; }
            public string txn_type { get; set; }
            public int is_closed { get; set; }
            public List<string> additional_information { get; set; }
            public string created_at { get; set; }
            public List<object> child_transactions { get; set; }
        }

        public class Filter
        {
            public string field { get; set; }
            public string value { get; set; }
            public string condition_type { get; set; }
        }

        public class FilterGroup
        {
            public List<Filter> filters { get; set; }
        }

        public class SearchCriteria
        {
            public List<FilterGroup> filter_groups { get; set; }
            public int current_page { get; set; }
        }
    }
}