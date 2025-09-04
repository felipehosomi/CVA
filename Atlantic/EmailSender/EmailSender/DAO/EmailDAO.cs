using EmailSender.DAO;
using EmailSender.RESOURCES;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmailSender
{
    class EmailDAO
    {
        XMLReaderREP reader = new XMLReaderREP();
        public SqlConnection conn = new SqlConnection();

        string sqlSelect = "SELECT * FROM CVA_EML";
        
        public List<string> GetEmails()
        {
            List<string> emails = new List<string>();

            conn.ConnectionString = reader.readConnectionString();
            conn.Open();

            SqlCommand commandSelect = new SqlCommand(sqlSelect, conn);
            SqlDataReader result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                emails.Add(result["EMAIL"].ToString());
            }
            conn.Close();

            return emails;
        }
    }
}

