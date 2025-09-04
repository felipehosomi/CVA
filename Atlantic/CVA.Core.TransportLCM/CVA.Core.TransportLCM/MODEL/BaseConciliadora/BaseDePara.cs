using CVA.Core.TransportLCM.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.MODEL.BaseConciliadora
{
    public class BaseDePara
    {
        public int ID { get; set; }

        [ModelHelper(ColumnName = "BASE_DE")]
        public int BaseDe { get; set; }

        [ModelHelper(ColumnName = "FILIAL_DE")]
        public int FilialDe { get; set; }

        [ModelHelper(ColumnName = "FILIAL_PARA")]
        public int FilialPara { get; set; }

        [ModelHelper(ColumnName = "CNPJ_FILIAL_DE")]
        public string CnpjFilialDe { get; set; }

        [ModelHelper(ColumnName = "CNPJ_FILIAL_PARA")]
        public string CnpjFilialPara { get; set; }

        [ModelHelper(ColumnName = "NOME")]
        public string Nome { get; set; }
    }
}
