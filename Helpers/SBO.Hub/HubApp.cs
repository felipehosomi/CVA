using SBO.Hub.Enums;

namespace SBO.Hub
{
    public class HubApp
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
