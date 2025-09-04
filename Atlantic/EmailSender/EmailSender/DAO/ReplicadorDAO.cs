using EmailSender.RESOURCES;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.DAO
{
    class ReplicadorDAO
    {
        XMLReaderREP reader = new XMLReaderREP();
        public SqlConnection conn = new SqlConnection();

        string sqlSelect = "SELECT * FROM CVA_TIM";

        public int CheckStatus()
        {
            int status = 0;
            conn.Close();
            conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            SqlCommand commandSelect = new SqlCommand(sqlSelect, conn);
            SqlDataReader result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                status = Convert.ToInt32(result["STU"].ToString());
            }
            
            conn.Close();

            return status;
        }

        public List<string> ReadLog()
        {
            var list = new List<string>();
            SqlCommand commandSelect;
            SqlDataReader result;

            var id = 0;

            conn.Close();
            conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            commandSelect = new SqlCommand("SELECT MAX(ID) AS 'ID' FROM CVA_REG_LOG WHERE STU = 5" , conn);
            result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                id = Convert.ToInt32(result["ID"].ToString());
            }

            conn.Close();
            conn.Open();

            commandSelect = new SqlCommand("SELECT T0.MSG, T1.CODE, T1.BAS_ERR, T2.DSCR FROM CVA_REG_LOG T0 INNER JOIN CVA_REG T1 ON T0.REG = T1.ID INNER JOIN CVA_OBJ T2 ON T1.OBJ = T2.ID WHERE T0.ID = " + id, conn);
            result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                list.Add(result["BAS_ERR"].ToString());
                list.Add(result["MSG"].ToString());
                list.Add(result["CODE"].ToString());
                list.Add(result["DSCR"].ToString());
            }

            conn.Close();

            return list;
        }
    }
}
