using Newtonsoft.Json;
using System;

namespace Fibra.ItemPricing.Models
{
    public partial class UserTablesMd
    {
        [JsonProperty("odata.metadata")]
        public Uri OdataMetadata { get; set; }

        [JsonProperty("TableName")]
        public string TableName { get; set; }

        [JsonProperty("TableDescription")]
        public string TableDescription { get; set; }

        [JsonProperty("TableType")]
        public string TableType { get; set; }

        [JsonProperty("Archivable")]
        public string Archivable { get; set; }

        [JsonProperty("ArchiveDateField")]
        public object ArchiveDateField { get; set; }
    }
}
