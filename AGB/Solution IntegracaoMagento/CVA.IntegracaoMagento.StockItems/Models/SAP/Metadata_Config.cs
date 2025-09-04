using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.StockItems.Models.SAP
{
    public class Metadata_Config
    {
        public class CVA_CONFIG_MAG
        {
            public string Code { get; set; }
            public object Name { get; set; }
            public int DocEntry { get; set; }
            public string Canceled { get; set; }
            public string Object { get; set; }
            public object LogInst { get; set; }
            public int UserSign { get; set; }
            public string Transfered { get; set; }
            public string CreateDate { get; set; }
            public string CreateTime { get; set; }
            public string UpdateDate { get; set; }
            public string UpdateTime { get; set; }
            public string DataSource { get; set; }
            public string U_Utilizacao { get; set; }
            public string U_Despesa { get; set; }
            public string U_ApiUrl { get; set; }
            public string U_ApiUsuario { get; set; }
            public string U_ApiSenha { get; set; }
            public string U_ApiClientId { get; set; }
            public string U_ApiClientSecret { get; set; }

            public IList<CVACONFIGMAG1Collection> CVA_CONFIG_MAG1Collection { get; set; }
        }

        public class CVACONFIGMAG1Collection
        {
            public string Code { get; set; }
            public int LineId { get; set; }
            public string Object { get; set; }
            public object LogInst { get; set; }
            public string U_FilialSap { get; set; }
            public string U_FilialMagento { get; set; }
            public string U_Deposito { get; set; }
        }

        public class Content
        {
            [JsonProperty("@odata.context")]
            public string odatacontext { get; set; }

            [JsonProperty("value")]
            public IList<CVA_CONFIG_MAG> value { get; set; }
        }
    }
}
