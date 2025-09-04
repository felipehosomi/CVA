using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace CVA.TomTicketDownload.BLL
{
    public class UserFieldsBLL
    {

        SqlConnection conn { get; set; }

        public UserFieldsBLL()
        {
            conn = new SqlConnection();
            OpenConnection();
        }
        public void OpenConnection()
        {
            conn.Close();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MeuContext"].ConnectionString;
            conn.Open();
        }


        public void CreateTable()
        {

            try
            {
                using (conn)
                {
                    OpenConnection();

                    var cmd = new SqlCommand();
                    cmd.Connection = conn;

                    #region Cria a tabela - CVA_TOMTICKET
                    cmd.CommandText = @"CREATE TABLE chamados(id int IDENTITY(1,1) PRIMARY KEY,
                    IdChamado NVARCHAR(500),
                    Protocolo INT,
                    Titulo NVARCHAR(500),
                    Mimetype NVARCHAR(500),
                    EmailCliente NVARCHAR(500),
                    Prioridade INT,
                    TempoTrabalho INT,
                    TempoAbertura NVARCHAR(500),
                    DataCriacao NVARCHAR(500),
                    Deadline NVARCHAR(500),
                    ValorItemHora INT,
                    ValorItemHoraExtra INT,
                    ValorFinal INT,
                    ValorFinalExtra INT,
                    NomeCliente NVARCHAR(500),
                    TipoChamado NVARCHAR(500),
                    AvaliadoProblemaResolvido NVARCHAR(500),
                    AvaliadoAtendimento NVARCHAR(500),
                    AvaliacaoComentario NVARCHAR(500),
                    DataPrimeiraResposta NVARCHAR(500),
                    DataEncerramento NVARCHAR(500),
                    UltimaSituacao INT,
                    DataUltimaSituacao NVARCHAR(500),
                    SlaInicio NVARCHAR(500),
                    SlaDeadline NVARCHAR(500),
                    SlaInicializacaoCumprido NVARCHAR(500),
                    SlaDeadlineCumprido NVARCHAR(500),
                    DescSituacao NVARCHAR(500),
                    Categoria NVARCHAR(500),
                    Departamento NVARCHAR(500),
                    Atendente NVARCHAR(500),
                    IdCliente NVARCHAR(500),
                    Status NVARCHAR(500),
                    DataUltimoStatus NVARCHAR(500),
                    NomeOrganizacao NVARCHAR(500),
                    Historico NVARCHAR(500),
                    TempoAtendimento NVARCHAR(500),
                    HistoricoStatus NVARCHAR(500))";

                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Historico
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = (@"CREATE TABLE historicos(id int IDENTITY(1,1) PRIMARY KEY,
                        IdHistorico NVARCHAR(500),
                        Origem NVARCHAR(500),
                        Atendente NVARCHAR(500),
                        Data_hora NVARCHAR(500))");
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region HistoricoStatus
                    cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = (@"CREATE TABLE historico_status(id int IDENTITY(1,1) PRIMARY KEY,
                        IdStatus NVARCHAR(500),
                       Status NVARCHAR(500),
                       Atendente_inicio NVARCHAR(500),
                       Atendente_fim NVARCHAR(500),
                       Inicio NVARCHAR(500),
                       ProtolocoChamado NVARCHAR(500),
                        TempoAtendimento NVARCHAR(500),
                      Fim NVARCHAR(500))");
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Log
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = (@"CREATE TABLE log_tomticket(id int IDENTITY(1,1) PRIMARY KEY,
                        ProtolocoChamado NVARCHAR(500),
                        Data NVARCHAR(500),
                        Motivo NVARCHAR(500))");
                    cmd.ExecuteNonQuery();
                    #endregion




                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

