using Conferencia.DAO;
using Conferencia.Model;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.BLL
{
    public class LoginBLL
    {
        public List<Login> GeLogin(string Logon, string Password)
        {
            var lista = new List<Login>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetLogin, Logon,Password), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new Login();

                    model.Logon = row["Code"].ToString();
                    model.Password = row["Name"].ToString();
                    model.Autorizacao = row["U_CVA_Auto"].ToString();

                    lista.Add(model);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }
    }
}
