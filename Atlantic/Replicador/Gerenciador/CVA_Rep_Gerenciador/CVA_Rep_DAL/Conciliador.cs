using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CVA_Rep_DAL
{
    public class ConciliadorDAL
    {
        private SqlConnection oConnection { get; set; }

        public ConciliadorDAL()
        {
            OpenConnection();
        }

        private void OpenConnection()
        {
            if (oConnection == null)
            {
                oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CVA_ATL_CON"].ConnectionString);
                oConnection.Open(); 
            }
        }

        public void CloseConnection()
        {
            if(oConnection != null)
            {
                if(oConnection.State == System.Data.ConnectionState.Open)
                {
                    oConnection.Close();
                }
            }
        }

        public List<CVA_BASES> Bases_GetAll()
        {
            var ret = new List<CVA_BASES>();

            if(oConnection != null)
            {
                if(oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("SELECT * FROM CVA_BASES", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var cvaBases = new CVA_BASES();

                        cvaBases.ID = Convert.ToInt32(reader["ID"]);
                        cvaBases.BASE = reader["BASE"].ToString();
                        cvaBases.TIPO = Convert.ToInt32(reader["TIPO"]);
                        cvaBases.DB_SERVER = reader["DB_SERVER"].ToString();
                        cvaBases.USERNAME = reader["USERNAME"].ToString();
                        cvaBases.PASSWD = reader["PASSWD"].ToString();
                        cvaBases.DB_USERNAME = reader["DB_USERNAME"].ToString();
                        cvaBases.DB_PASSWD = reader["DB_PASSWD"].ToString();
                        cvaBases.LICENSE_SERVER = reader["LICENSE_SERVER"].ToString();
                        cvaBases.DB_TYPE = Convert.ToInt32(reader["DB_TYPE"]);
                        cvaBases.USE_TRUSTED = Convert.ToInt32(reader["USE_TRUSTED"]);

                        ret.Add(cvaBases);
                    }

                    reader.Close();
                    cmd.Dispose();                    
                }
            }

            return ret;
        }

        public CVA_BASES Bases_GetById(int id)
        {
            var cvaBases = new CVA_BASES();

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand($"SELECT * FROM CVA_BASES WHERE ID = {id}", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cvaBases.ID = Convert.ToInt32(reader["ID"]);
                        cvaBases.BASE = reader["BASE"].ToString();
                        cvaBases.TIPO = Convert.ToInt32(reader["TIPO"]);
                        cvaBases.DB_SERVER = reader["DB_SERVER"].ToString();
                        cvaBases.USERNAME = reader["USERNAME"].ToString();
                        cvaBases.PASSWD = reader["PASSWD"].ToString();
                        cvaBases.DB_USERNAME = reader["DB_USERNAME"].ToString();
                        cvaBases.DB_PASSWD = reader["DB_PASSWD"].ToString();
                        cvaBases.LICENSE_SERVER = reader["LICENSE_SERVER"].ToString();
                        cvaBases.DB_TYPE = Convert.ToInt32(reader["DB_TYPE"]);
                        cvaBases.USE_TRUSTED = Convert.ToInt32(reader["USE_TRUSTED"]);
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return cvaBases;
        }

        public int Bases_Insert(CVA_BASES cvaBases)
        {
            var ret = 0;

            if(oConnection != null)
            {
                if(oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("INSERT INTO CVA_BASES VALUES(@BASE, @TIPO, @DB_SERVER, @USERNAME, @PASSWD, @DB_USERNAME, @DB_PASSWD, @LICENSE_SERVER, @DB_TYPE, @USE_TRUSTED)", oConnection);
                    cmd.Parameters.AddWithValue("@BASE", cvaBases.BASE);
                    cmd.Parameters.AddWithValue("@TIPO", cvaBases.TIPO);
                    cmd.Parameters.AddWithValue("@DB_SERVER", cvaBases.DB_SERVER);
                    cmd.Parameters.AddWithValue("@USERNAME", cvaBases.USERNAME);
                    cmd.Parameters.AddWithValue("@PASSWD", cvaBases.PASSWD);
                    cmd.Parameters.AddWithValue("@DB_USERNAME", cvaBases.DB_USERNAME);
                    cmd.Parameters.AddWithValue("@DB_PASSWD", cvaBases.DB_PASSWD);
                    cmd.Parameters.AddWithValue("@LICENSE_SERVER", cvaBases.LICENSE_SERVER);
                    cmd.Parameters.AddWithValue("@DB_TYPE", cvaBases.DB_TYPE);
                    cmd.Parameters.AddWithValue("@USE_TRUSTED", cvaBases.USE_TRUSTED);

                    ret = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public int Bases_Update(CVA_BASES cvaBases)
        {
            var ret = 0;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("UPDATE CVA_BASES SET BASE = @BASE, TIPO = @TIPO, DB_SERVER = @DB_SERVER, USERNAME = @USERNAME, PASSWD = @PASSWD, DB_USERNAME = @DB_USERNAME, DB_PASSWD = @DB_PASSWD, LICENSE_SERVER = @LICENSE_SERVER, DB_TYPE = @DB_TYPE, USE_TRUSTED = @USE_TRUSTED WHERE ID = @ID", oConnection);
                    cmd.Parameters.AddWithValue("@ID", cvaBases.ID);
                    cmd.Parameters.AddWithValue("@BASE", cvaBases.BASE);
                    cmd.Parameters.AddWithValue("@TIPO", cvaBases.TIPO);
                    cmd.Parameters.AddWithValue("@DB_SERVER", cvaBases.DB_SERVER);
                    cmd.Parameters.AddWithValue("@USERNAME", cvaBases.USERNAME);
                    cmd.Parameters.AddWithValue("@PASSWD", cvaBases.PASSWD);
                    cmd.Parameters.AddWithValue("@DB_USERNAME", cvaBases.DB_USERNAME);
                    cmd.Parameters.AddWithValue("@DB_PASSWD", cvaBases.DB_PASSWD);
                    cmd.Parameters.AddWithValue("@LICENSE_SERVER", cvaBases.LICENSE_SERVER);
                    cmd.Parameters.AddWithValue("@DB_TYPE", cvaBases.DB_TYPE);
                    cmd.Parameters.AddWithValue("@USE_TRUSTED", cvaBases.USE_TRUSTED);

                    ret = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public int Bases_Delete(CVA_BASES cvaBases)
        {
            var ret = 0;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("DELETE FROM CVA_BASES WHERE ID = @ID", oConnection);
                    cmd.Parameters.AddWithValue("@ID", cvaBases.ID);

                    ret = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public List<CVA_BASES_DE_PARA> DePara_GetAll()
        {
            var ret = new List<CVA_BASES_DE_PARA>();

            if(oConnection != null)
            {
                if(oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("SELECT ID, BASE_DE, FILIAL_DE, NOME, CNPJ_FILIAL_DE, CNPJ_FILIAL_PARA, FILIAL_PARA FROM CVA_BASES_DE_PARA", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var cvaBases = new CVA_BASES_DE_PARA();

                        cvaBases.ID = Convert.ToInt32(reader["ID"]);
                        cvaBases.BASE_DE = Convert.ToInt32(reader["BASE_DE"]);
                        cvaBases.FILIAL_DE = Convert.ToInt32(reader["FILIAL_DE"]);
                        cvaBases.NOME = reader["NOME"].ToString();
                        cvaBases.CNPJ_FILIAL_DE = reader["CNPJ_FILIAL_DE"].ToString();
                        cvaBases.CNPJ_FILIAL_PARA = reader["CNPJ_FILIAL_PARA"].ToString();
                        cvaBases.FILIAL_PARA = Convert.ToInt32(reader["FILIAL_PARA"]);

                        ret.Add(cvaBases);
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public CVA_BASES_DE_PARA DePara_GetById(int id)
        {
            var cvaBases = new CVA_BASES_DE_PARA();
            
            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand($"SELECT ID, BASE_DE, NOME, CNPJ_FILIAL_DE, CNPJ_FILIAL_PARA, FILIAL_DE, FILIAL_PARA FROM CVA_BASES_DE_PARA WHERE ID = {id}", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cvaBases.ID = Convert.ToInt32(reader["ID"]);
                        cvaBases.BASE_DE = Convert.ToInt32(reader["BASE_DE"]);
                        cvaBases.NOME = reader["NOME"].ToString();
                        cvaBases.CNPJ_FILIAL_DE = reader["CNPJ_FILIAL_DE"].ToString();
                        cvaBases.CNPJ_FILIAL_PARA = reader["CNPJ_FILIAL_PARA"].ToString();
                        cvaBases.FILIAL_DE = Convert.ToInt32(reader["FILIAL_DE"]);
                        cvaBases.FILIAL_PARA = Convert.ToInt32(reader["FILIAL_PARA"]);
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return cvaBases;

        }

        public string DePara_GetCNPJFilialConciliadora(int id)
        {
            var ret = string.Empty;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var baseOrigem = Bases_GetAll().Where(b => b.TIPO == 1).FirstOrDefault();
                    var cmd = new SqlCommand($"SELECT T0.BPLId, T0.BPLName, T0.TaxIdNum FROM [{baseOrigem.BASE}].[dbo].[OBPL] T0 WHERE T0.Disabled = 'N' AND T0.BPLId = {id}", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ret = reader["TaxIdNum"].ToString();
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public int DePara_Insert(CVA_BASES_DE_PARA dePara)
        {
            var ret = 0;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("INSERT INTO CVA_BASES_DE_PARA VALUES(@BASE_DE, @FILIAL_DE, @CNPJ_DE, @CNPJ_PARA, @FILIAL_PARA, @NOME)", oConnection);
                    cmd.Parameters.AddWithValue("@BASE_DE", dePara.BASE_DE);
                    cmd.Parameters.AddWithValue("@FILIAL_DE", dePara.FILIAL_DE);
                    cmd.Parameters.AddWithValue("@CNPJ_DE", dePara.CNPJ_FILIAL_DE);
                    cmd.Parameters.AddWithValue("@CNPJ_PARA", dePara.CNPJ_FILIAL_PARA);
                    cmd.Parameters.AddWithValue("@FILIAL_PARA", dePara.FILIAL_PARA);
                    cmd.Parameters.AddWithValue("@NOME", dePara.NOME);

                    ret = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public int DePara_Update(CVA_BASES_DE_PARA dePara)
        {
            var ret = 0;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("UPDATE CVA_BASES_DE_PARA SET BASE_DE = @BASE_DE, FILIAL_DE = @FILIAL_DE, CNPJ_FILIAL_DE = @CNPJ_DE, CNPJ_FILIAL_PARA = @CNPJ_PARA, FILIAL_PARA = @FILIAL_PARA, NOME = @NOME WHERE ID = @ID", oConnection);
                    cmd.Parameters.AddWithValue("@ID", dePara.ID);
                    cmd.Parameters.AddWithValue("@BASE_DE", dePara.BASE_DE);
                    cmd.Parameters.AddWithValue("@FILIAL_DE", dePara.FILIAL_DE);
                    cmd.Parameters.AddWithValue("@CNPJ_DE", dePara.CNPJ_FILIAL_DE);
                    cmd.Parameters.AddWithValue("@CNPJ_PARA", dePara.CNPJ_FILIAL_PARA);
                    cmd.Parameters.AddWithValue("@FILIAL_PARA", dePara.FILIAL_PARA);
                    cmd.Parameters.AddWithValue("@NOME", dePara.NOME);

                    ret = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public int DePara_Delete(CVA_BASES_DE_PARA dePara)
        {
            var ret = 0;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var cmd = new SqlCommand("DELETE FROM CVA_BASES_DE_PARA WHERE ID = @ID", oConnection);
                    cmd.Parameters.AddWithValue("@ID", dePara.ID);

                    ret = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public Dictionary<int, string> DePara_GetFiliaisConciliadora()
        {
            var ret = new Dictionary<int, string>();

            if(oConnection != null)
            {
                if(oConnection.State == System.Data.ConnectionState.Open)
                {
                    var baseConciliadora = Bases_GetAll().Where(b => b.TIPO == 1).FirstOrDefault();
                    var cmd = new SqlCommand($"SELECT T0.BPLId, T0.BPLName, T0.TaxIdNum FROM [{baseConciliadora.BASE}].[dbo].[OBPL] T0 WHERE T0.Disabled = 'N'", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(Convert.ToInt32(reader["BPLId"]), $"{reader["BPLName"].ToString()}");
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public List<int> DePara_GetFiliaisOrigem(int id)
        {
            var ret = new List<int>();

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var baseOrigem = Bases_GetById(id);
                    var cmd = new SqlCommand($"SELECT T0.BPLId, T0.BPLName, T0.TaxIdNum FROM [{baseOrigem.BASE}].[dbo].[OBPL] T0 WHERE T0.Disabled = 'N'", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(Convert.ToInt32(reader["BPLId"]));
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public string DePara_GetNomeFilialOrigem(int idBase, int idFilial)
        {
            var ret = string.Empty;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var baseOrigem = Bases_GetById(idBase);
                    var cmd = new SqlCommand($"SELECT T0.BPLId, T0.BPLName, T0.TaxIdNum FROM [{baseOrigem.BASE}].[dbo].[OBPL] T0 WHERE T0.Disabled = 'N' AND T0.BPLId = {idFilial}", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ret = reader["BPLName"].ToString();
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return ret;
        }

        public string DePara_GetCNPJFilialOrigem(int idBase, int idFilial)
        {
            var ret = string.Empty;

            if (oConnection != null)
            {
                if (oConnection.State == System.Data.ConnectionState.Open)
                {
                    var baseOrigem = Bases_GetById(idBase);
                    var cmd = new SqlCommand($"SELECT T0.BPLId, T0.BPLName, T0.TaxIdNum FROM [{baseOrigem.BASE}].[dbo].[OBPL] T0 WHERE T0.Disabled = 'N' AND T0.BPLId = {idFilial}", oConnection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ret = reader["TaxIdNum"].ToString();
                    }

                    reader.Close();
                    cmd.Dispose();
                }
            }

            return ret;
        }
    }

    public class CVA_BASES
    {
        public int ID { get; set; }
        public string BASE { get; set; }
        public int TIPO { get; set; }
        public string DB_SERVER { get; set; }
        public string USERNAME { get; set; }
        public string PASSWD { get; set; }
        public string DB_USERNAME { get; set; }
        public string DB_PASSWD { get; set; }
        public string LICENSE_SERVER { get; set; }
        public int DB_TYPE { get; set; }
        public int USE_TRUSTED { get; set; }
    }

    public class CVA_BASES_DE_PARA
    {
        public int ID { get; set; }
        public int BASE_DE { get; set; }
        public int FILIAL_DE { get; set; }
        public string NOME { get; set; }
        public int FILIAL_PARA { get; set; }
        public string CNPJ_FILIAL_DE { get; set; }
        public string CNPJ_FILIAL_PARA { get; set; }
    }
}
