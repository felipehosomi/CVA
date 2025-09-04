using System;

namespace EmailAutorizacao.MODEL
{
    public class EmailMessageModel
    {
        public int CreateTS { get; set; } // Usado apenas no select
        public string UserName { get; set; }
        public string Email { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public double DocTotal { get; set; }
        public string BPLName { get; set; }
    }
}
