using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fibra.ItemPricing.Models
{
    public partial class UserFieldsMD
    {
        [JsonProperty("odata.metadata")]
        public Uri OdataMetadata { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Size")]
        public int? Size { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("SubType")]
        public string SubType { get; set; }

        [JsonProperty("LinkedTable")]
        public object LinkedTable { get; set; }

        [JsonProperty("DefaultValue")]
        public object DefaultValue { get; set; }

        [JsonProperty("TableName")]
        public string TableName { get; set; }

        [JsonProperty("FieldID")]
        public int? FieldId { get; set; }

        [JsonProperty("EditSize")]
        public int? EditSize { get; set; }

        [JsonProperty("Mandatory")]
        public string Mandatory { get; set; }

        [JsonProperty("LinkedUDO")]
        public object LinkedUdo { get; set; }

        [JsonProperty("LinkedSystemObject")]
        public string LinkedSystemObject { get; set; }

        [JsonProperty("ValidValuesMD")]
        public List<ValidValuesMD> ValidValuesMd { get; set; }
    }

    public partial class ValidValuesMD
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}
