using AUXILIAR;
using MODEL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAO
{
    public class ChamadoDAO
    {
        #region Atributos
        string connectionString { get; set; }
        SqlConnection conn { get; set; }
        XmlReader _xmlReader { get; set; }
        #endregion

        #region Construtor
        public ChamadoDAO()
        {
            this.conn = new SqlConnection();
            this._xmlReader = new XmlReader();
            this.connectionString = _xmlReader.readConnectionString();
        }
        #endregion

        public string Save(ChamadoModel chamado)
        {
            var numChamado = Convert.ToInt32(chamado.protocolo);
            try
            {
                using (conn)
                {
                    conn.Close();
                    conn.ConnectionString = connectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"INSERT INTO CVA_TCK_IMP VALUES(
                                                                    '{chamado.prd}',
                                                                     GETDATE(),
                                                                     NULL,
                                                                     1,
                                                                     1,
                                                                    '{numChamado}',
                                                                    '{chamado.titulo.Replace("'","''")}',
                                                                    '{chamado.nomeorganizacao}',
                                                                    '{chamado.nomecliente}',
                                                                     'P',
                                                                    (SELECT ID FROM CVA_CLI WHERE DSCR_AMS = '{chamado.nomeorganizacao}'),
                                                                    '{chamado.departamento}',
                                                                     {chamado.ultimasituacao},
                                                                    convert(datetime, '{chamado.dataultimasituacao}', 103))";

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return $@"{numChamado} - {chamado.nomeorganizacao}: Salvo com sucesso.";
            }
            catch (SqlException ex)
            {
                return $@"{numChamado} - {chamado.nomeorganizacao} {ex.Message.ToString()}";
            }
        }

        public string Update(ChamadoModel chamado)
        {
            var numChamado = Convert.ToInt32(chamado.protocolo);
            try
            {
                using (conn)
                {
                    conn.Close();
                    conn.ConnectionString = connectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"UPDATE CVA_TCK_IMP SET
                                               PRD = '{chamado.prd}',
                                               UPD = GETDATE(),
                                               STU = 1,
                                               USR = 1,
                                               NUM_TCK = '{numChamado}',
                                               DSCR = '{chamado.titulo.Replace("'","''")}',
                                               CLI_AMS = '{chamado.nomeorganizacao}',
                                               REQ_AMS = '{chamado.nomecliente}',
                                               STU_AMS = 'P',
                                               CLI_ID = (SELECT ID FROM CVA_CLI WHERE DSCR_AMS = '{chamado.nomeorganizacao}'),
                                               CAT = '{chamado.departamento}',
                                               COD_LAST_STATUS = {chamado.ultimasituacao},
                                               DATE_LAST_STATUS = convert(datetime, '{chamado.dataultimasituacao}', 103)
                                          WHERE NUM_TCK = '{numChamado}'
                                                AND CLI_AMS = '{chamado.nomeorganizacao}'
                                                AND PRD  = '{chamado.prd}'";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return $@"{numChamado} - {chamado.nomeorganizacao}: Atualizado com sucesso.";
            }
            catch (SqlException ex)
            {
                return $@"{numChamado} - {chamado.nomeorganizacao} {ex.Message.ToString()}";
            }
        }

        public bool CheckIfSaved(ChamadoModel chamado)
        {
            var numChamado = Convert.ToInt32(chamado.protocolo);
            try
            {
                using (conn)
                {
                    conn.Close();
                    conn.ConnectionString = connectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT ID FROM CVA_TCK_IMP WHERE NUM_TCK = '{numChamado}'
                                                                      AND CLI_AMS = '{chamado.nomeorganizacao}'
                                                                      AND PRD = '{chamado.prd}'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();

                        if (tb.Rows.Count <= 0)
                            return false;
                        else
                            return true;
                    }
                }
            }
            catch (SqlException)
            {
                return true;
            }
        }
    }
}
