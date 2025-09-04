using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.MODEL
{
    public class EmailTagsModel
    {
        public string CompanyDB { get; set; }
        public string CompanyName { get; set; }
        public string Action { get; set; }
        public string CntctType { get; set; }
        public string CntctSbjct { get; set; }
        public string User { get; set; }
        public int ClgCode { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Details { get; set; }
        public DateTime Recontact { get; set; }
        public DateTime EndDate { get; set; }
        public string DocType { get; set; }
        public string DocNum { get; set; }
        public string Email { get; set; }
    }
}
