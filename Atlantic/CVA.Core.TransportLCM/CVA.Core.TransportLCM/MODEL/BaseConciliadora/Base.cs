using CVA.Core.TransportLCM.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.MODEL.BaseConciliadora
{
    public class Base
    {
        public int ID { get; set; }

        [ModelHelper(ColumnName = "BASE")]
        public string BaseName { get; set; }

        [ModelHelper(ColumnName = "TIPO")]
        public int Type { get; set; }

        [ModelHelper(ColumnName = "DB_SERVER")]
        public string DBServer { get; set; }

        [ModelHelper(ColumnName = "USERNAME")]
        public string UserName { get; set; }

        [ModelHelper(ColumnName = "PASSWD")]
        public string Password { get; set; }

        [ModelHelper(ColumnName = "DB_USERNAME")]
        public string DBUserName { get; set; }

        [ModelHelper(ColumnName = "DB_PASSWD")]
        public string DBPassword { get; set; }

        [ModelHelper(ColumnName = "LICENSE_SERVER")]
        public string LicenseServer { get; set; }

        [ModelHelper(ColumnName = "DB_TYPE")]
        public int DBType { get; set; }

        [ModelHelper(ColumnName = "USE_TRUSTED")]
        public int UseTrusted { get; set; }
    }
}
