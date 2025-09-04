using SAPbobsCOM;
using System;

namespace CVA.AddOn.Control.Logic.MODEL
{
    public class AccountModel
    {
        public string AcctCode { get; set; }

        public bool ValidFor { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string ValidRemarks { get; set; }

        public bool FrozenFor { get; set; }
        public DateTime? FrozenFrom { get; set; }
        public DateTime? FrozenTo { get; set; }
        public string FrozenRemarks { get; set; }
    }
}
