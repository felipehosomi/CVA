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
    class ConsolidadorDAO
    {
        XMLReaderCON reader = new XMLReaderCON();
        public SqlConnection conn = new SqlConnection();


        public int CheckStatus()
        {
            int id = 0;
            int status = 2;

            conn.Close();
            conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            SqlCommand commandSelect = new SqlCommand("SELECT MAX(ID) AS 'ID' FROM CVA_REG WHERE STU = 2", conn);
            SqlDataReader result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                if (!String.IsNullOrEmpty(result["ID"].ToString()))
                    id = Convert.ToInt32(result["ID"].ToString());
                else
                    return 3;
            }

            conn.Close();
            conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            SqlCommand commandSelect2 = new SqlCommand("SELECT * FROM CVA_EML_SENT WHERE REG = " + id, conn);
            SqlDataReader result2 = commandSelect2.ExecuteReader();
            while (result2.Read())
            {
                status = 3;
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
            //conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            commandSelect = new SqlCommand("SELECT MAX(ID) AS 'ID' FROM CVA_REG WHERE STU = 2", conn);
            result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                if (!String.IsNullOrEmpty(result["ID"].ToString()))
                    id = Convert.ToInt32(result["ID"].ToString());
            }

            conn.Close();
            conn.Open();

            commandSelect = new SqlCommand(@"SELECT T0.CODE, T2.BASE, T1.MSG FROM CVA_REG T0 
                                                INNER JOIN CVA_REG_LOG T1 ON T0.ID = T1.REG
                                                INNER JOIN CVA_BASES T2 ON T0.BAS_ERR = T2.ID
                                            WHERE T0.ID =" + id, conn);

            result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                list.Add(result["BASE"].ToString());
                list.Add(result["MSG"].ToString());
                list.Add(result["CODE"].ToString());
            }

            conn.Close();
            //conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            commandSelect = new SqlCommand($"INSERT INTO CVA_EML_SENT VALUES({id})", conn);
            result = commandSelect.ExecuteReader();


            conn.Close();

            return list;
        }
    }
}
