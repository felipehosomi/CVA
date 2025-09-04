using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SZS.Common.Enums;

namespace SZS.Common
{
    public class SZApp
    {
        public static string ServerName { get; set; }

        public static string DatabaseName { get; set; }

        public static string DBUserName { get; set; }

        public static string DBPassword { get; set; }

        public static AppTypeEnum AppType { get; set; }

        public static DatabaseTypeEnum DatabaseType { get; set; }

        public static void FillConnectionParameters()
        {

        }
    }
}
