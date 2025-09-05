using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapSkaWs.Data
{
    public class Database
    {
        public static void VerifyData()
        {
            using (var helper = new SqlHelper("ska"))
            {
                helper.ExecuteNonQuery(GetCreateTableImportQuery());
            }
        }

        public static void InsertLog(SspImport item, string message = "", string action = "A", string status = "I")
        {
            //using (var helper = new SqlHelper("ska"))
            //{
            //    using(var cmd = new SqlCommand(GetInsertQuery(), helper.GetConnection()))
            //    {
            //        string plandtini = item.PLANDTINI == null ? string.Empty : ((DateTime)item.PLANDTINI).ToString("yyyy-MM-dd HH:mm:ss");
            //        string plandtfim = item.PLANDTFIM == null ? string.Empty : ((DateTime)item.PLANDTFIM).ToString("yyyy-MM-dd HH:mm:ss");
            //        string tstamp = item.TSTAMP == null ? string.Empty : ((DateTime)item.TSTAMP).ToString("yyyy-MM-dd HH:mm:ss");

            //        cmd.Parameters.AddWithValue("@OP", item.OP);
            //        cmd.Parameters.AddWithValue("@OPER", item.OPER);
            //        cmd.Parameters.AddWithValue("@CODPECA", item.CODPECA);
            //        cmd.Parameters.AddWithValue("@MAQ", item.MAQ);
            //        cmd.Parameters.AddWithValue("@PLANDTINI", plandtini);
            //        cmd.Parameters.AddWithValue("@PLANDTFIM", plandtfim);
            //        cmd.Parameters.AddWithValue("@PLANQTY", item.PLANQTY);
            //        cmd.Parameters.AddWithValue("@CYCQTY", item.CYCQTY);
            //        cmd.Parameters.AddWithValue("@PLANTMUNIT", item.PLANTMUNIT);
            //        cmd.Parameters.AddWithValue("@PLANTMSETUP", item.PLANTMSETUP);
            //        cmd.Parameters.AddWithValue("@ACAO", item.ACAO);
            //        cmd.Parameters.AddWithValue("@STATUS", item.STATUS);
            //        cmd.Parameters.AddWithValue("@BELPOS_ID", item.BELPOS_ID);
            //        cmd.Parameters.AddWithValue("@POS_ID", item.POS_ID);
            //        cmd.Parameters.AddWithValue("@TSTAMP", tstamp);
            //        cmd.Parameters.AddWithValue("@LOGACTION", action);
            //        cmd.Parameters.AddWithValue("@LOGSTATUS", status);
            //        cmd.Parameters.AddWithValue("@LOGDATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //        cmd.Parameters.AddWithValue("@LOGMSG", message);

            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }

        private static string GetCreateTableImportQuery()
        {
            var builder = new StringBuilder();
            builder.AppendLine("IF NOT EXISTS ( ");
            builder.AppendLine("SELECT * FROM SYS.TABLES T ");
            builder.AppendLine("JOIN SYS.SCHEMAS S ON (T.SCHEMA_ID = S.SCHEMA_ID) ");
            builder.AppendLine("WHERE S.NAME = 'dbo' AND T.NAME = 'CVA_LOG_IMPORT' ");
            builder.AppendLine(") ");
            builder.AppendLine("CREATE TABLE dbo.[CVA_LOG_IMPORT] ( ");
            builder.AppendLine("LOGID INT IDENTITY PRIMARY KEY, ");
            builder.AppendLine("OP CHAR(250) NULL, ");
            builder.AppendLine("OPER CHAR(250) NULL, ");
            builder.AppendLine("CODPECA CHAR(250) NULL, ");
            builder.AppendLine("MAQ CHAR(250) NULL, ");
            builder.AppendLine("PLANDTINI DATETIME NULL, ");
            builder.AppendLine("PLANDTFIM DATETIME NULL, ");
            builder.AppendLine("PLANQTY INT NULL, ");
            builder.AppendLine("CYCQTY INT NULL, ");
            builder.AppendLine("PLANTMUNIT FLOAT NULL, ");
            builder.AppendLine("PLANTMSETUP FLOAT NULL, ");
            builder.AppendLine("ACAO BIT NULL, ");
            builder.AppendLine("STATUS BIT NULL, ");
            builder.AppendLine("BELPOS_ID CHAR(250) NULL, ");
            builder.AppendLine("POS_ID CHAR(250) NULL, ");
            builder.AppendLine("TSTAMP DATETIME NULL, ");
            builder.AppendLine("LOGACTION CHAR(1) NOT NULL, ");
            builder.AppendLine("LOGSTATUS CHAR(1) NOT NULL, ");
            builder.AppendLine("LOGDATE DATETIME NOT NULL, ");
            builder.AppendLine("LOGMSG NVARCHAR(MAX) NOT NULL");
            builder.AppendLine(")");

            return builder.ToString();
        }

        private static string GetInsertQuery()
        {
            var builder = new StringBuilder();
            builder.AppendLine("SET DATEFORMAT 'ymd'; INSERT INTO [dbo].[CVA_LOG_IMPORT]( ");
            builder.Append("OP, OPER, CODPECA, MAQ, PLANDTINI, PLANDTFIM, PLANQTY, CYCQTY, PLANTMUNIT, PLANTMSETUP, ACAO, STATUS, BELPOS_ID, POS_ID, TSTAMP, LOGACTION, LOGSTATUS, LOGDATE, LOGMSG");
            builder.Append(")");
            builder.AppendLine("VALUES (");
            builder.Append("@OP, @OPER, @CODPECA, @MAQ, @PLANDTINI, @PLANDTFIM, @PLANQTY, @CYCQTY, @PLANTMUNIT, @PLANTMSETUP, @ACAO, @STATUS, @BELPOS_ID, @POS_ID, @TSTAMP, @LOGACTION, @LOGSTATUS, @LOGDATE, @LOGMSG");
            builder.Append(")");

            return builder.ToString();
        }
    }
}
