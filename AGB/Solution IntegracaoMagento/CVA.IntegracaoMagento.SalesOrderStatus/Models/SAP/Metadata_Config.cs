using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVA.IntegracaoMagento.SalesOrderStatus.Models.SAP
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
            public IList<CVACONFIGMAG2Collection> CVA_CONFIG_MAG2Collection { get; set; }
            public IList<CVACONFIGMAG3Collection> CVA_CONFIG_MAG3Collection { get; set; }
        }

        public class CVACONFIGMAG1Collection
        {
            public string Code { get; set; }
            public int LineId { get; set; }
            public string Object { get; set; }
            public object LogInst { get; set; }
            public string U_FilialSap { get; set; }
            public string U_FilialMagento { get; set; }
            public string U_DepositoMagento { get; set; }
            public string U_Deposito { get; set; }
        }

        public class CVACONFIGMAG2Collection
        {
            public string Code { get; set; }
            public int LineId { get; set; }
            public string Object { get; set; }
            public object LogInst { get; set; }
            public string U_CondSap { get; set; }
            public string U_CondMagento { get; set; }
            public string U_Adiant { get; set; }
        }

        public class CVACONFIGMAG3Collection
        {
            public string Code { get; set; }
            public int LineId { get; set; }
            public string Object { get; set; }
            public object LogInst { get; set; }
            public string U_FormaSap { get; set; }
            public string U_Conta { get; set; }
            public string U_FormaMagento { get; set; }
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
