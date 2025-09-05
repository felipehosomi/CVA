using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkaSapWs.Data
{
    public class Database
    {
        public static void VerifyData()
        {
            using (var helper = new SqlHelper("ska"))
            {

            }
        }

        private static string GetCreateTableExportQuery()
        {
            var builder = new StringBuilder();
            builder.AppendLine("IF NOT EXISTS ( ");
            builder.AppendLine("SELECT * FROM SYS.TABLES T ");
            builder.AppendLine("JOIN SYS.SCHEMAS S ON (T.SCHEMA_ID = S.SCHEMA_ID) ");
            builder.AppendLine("WHERE S.NAME = 'dbo' AND T.NAME = 'CVA_LOG_EXPORT' ");
            builder.AppendLine(") ");
            builder.AppendLine("CREATE TABLE dbo.[CVA_LOG_EXPORT] ( ");
            builder.AppendLine("LOGID INT IDENTITY PRIMARY KEY, ");
            builder.AppendLine("BELNR_ID NUMERIC(19,6), ");
            builder.AppendLine("BELPOS_ID NUMERIC(19,6), ");
            builder.AppendLine("POS_ID NUMERIC(19,6), ");
            builder.AppendLine("TYP NCHAR(2), ");
            builder.AppendLine("RESOURCENPOS_ID INT, ");
            builder.AppendLine("PERS_ID NCHAR(40), ");
            builder.AppendLine("ANFZEIT DATETIME, ");
            builder.AppendLine("ENDZEIT DATETIME, ");
            builder.AppendLine("ZEIT NUMERIC(19,6), ");
            builder.AppendLine("MENGE_GUT_RM NUMERIC(19,6), ");
            builder.AppendLine("MENGE_SCHLECHT_RM NUMERIC(19,6), ");
            builder.AppendLine("ABGKZ NCHAR(2), ");
            builder.AppendLine("manualbooking NCHAR(40), ");
            builder.AppendLine("APLATZ_ID NCHAR(40), ");
            builder.AppendLine("KSTST_ID NCHAR(40), ");
            builder.AppendLine("GRUND NVARCHAR(510), ");
            builder.AppendLine("DocDate DATETIME, ");
            builder.AppendLine("Project NVARCHAR(40), ");
            builder.AppendLine("TIMETYPE_ID NVARCHAR(40), ");
            builder.AppendLine("EXTERNAL_COST NUMERIC(19,6), ");
            builder.AppendLine("BatchNum NVARCHAR(80), ");
            builder.AppendLine("UDF1 NVARCHAR(100), ");
            builder.AppendLine("UDF2 NVARCHAR(100), ");
            builder.AppendLine("UDF3 NVARCHAR(100), ");
            builder.AppendLine("UDF4 NVARCHAR(100), ");
            builder.AppendLine("WKZ_ID NVARCHAR(40), ");
            builder.AppendLine("LOGACTION CHAR(1) NOT NULL, ");
            builder.AppendLine("LOGSTATUS CHAR(1) NOT NULL, ");
            builder.AppendLine("LOGDATE DATETIME NOT NULL, ");
            builder.AppendLine("LOGMSG NVARCHAR(MAX) NOT NULL");
            builder.AppendLine(")");

            return builder.ToString();
        }
    }
}
