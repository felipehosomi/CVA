using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models
{
    public class VoucherModel : PagingModel
    {
        public List<Voucher> vouchers { get; set; }
        public string Error { get; set; }

        public class Voucher
        {
            public string codigo_sap { get; set; }
            public string data_criacao
            {
                get
                {
                    return CreateDate.ToString("dd/MM/yyyy") + " " + CreateTS.ToString().PadLeft(6, '0').Insert(2, ":").Insert(5, ":");
                }
            }
            public object data_validade { get; set; }
            public int identificador { get; set; }
            public double valor { get; set; }
            public double saldo { get; set; }
            [JsonIgnore]
            public Int64 TotalRecords { get; set; }
            [JsonIgnore]
            public DateTime CreateDate { get; set; }
            [JsonIgnore]
            public int CreateTS { get; set; }
            [JsonIgnore]
            public Int64 RN { get; set; }
        }

    }
}