using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace PackIndicator.Models
{
    class WMSIntegrationFile
    {
        public class Header
        {
            [Index(1)]
            public string CardCode { get; set; }

            [Index(2)]
            public string CardFName { get; set; }

            [Index(3)]
            public string County { get; set; }

            [Index(4)]
            public string State { get; set; }

            [Index(5)]
            public string Street { get; set; }

            [Index(6)]
            public string Block { get; set; }

            [Index(7)]
            public string ZipCode { get; set; }

            [Index(8)]
            public int DocNum { get; set; }

            [Index(9)]
            public DateTime ShipDate { get; set; }

            [Index(10)]
            public double Quantity { get; set; }

            [Index(11)]
            public string RouteCode { get; set; }

            [Index(12)]
            public string FileType { get; set; }

            [Ignore]
            public int DocEntry { get; set; }

            [Ignore]
            public string StorageCategory { get; set; }

            [Ignore]
            public List<Items> Items { get; set; }

            public Header()
            {
                Items = new List<Items>();
            }
        }

        public class Items
        {
            [Index(1)]
            public double Quantity { get; set; }

            [Index(2)]
            public string ItemCode { get; set; }

            [Index(3)]
            public double Weight { get; set; }

            [Index(5)]
            public string ItemName { get; set; }

            [Index(6)]
            public double Price { get; set; }

            [Index(7)]
            public int PickEntry { get; set; }

            [Index(8)]
            public int AbsEntry { get; set; }

            [Index(9)]
            public int DocNum { get; set; }

            [Ignore]
            public int UomEntry { get; set; }
        }
    }
}