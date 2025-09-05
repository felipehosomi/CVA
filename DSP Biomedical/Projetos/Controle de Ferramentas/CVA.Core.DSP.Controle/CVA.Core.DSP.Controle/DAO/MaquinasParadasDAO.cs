using CVA.Core.DSP.Controle.Auxiliar;
using CVA.Core.DSP.Controle.HELPER;
using CVA.Core.DSP.Controle.MODEL;
using CVA.Core.DSP.Controle.Resources.Select;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CVA.Core.DSP.Controle.DAO
{
    public class MaquinasParadasDAO
    {
        public BusinessOneDAO _businessOneDAO { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public Company _company { get; set; }

        SqlConnection conn { get; set; }
        XMLReader xmlReader { get; set; }

        public MaquinasParadasDAO()
        {
            //this._businessOneDAO =  _businessOneDAO;
            xmlReader = new XMLReader();
            conn = new SqlConnection();
            OpenConnection();
        }

        public void OpenConnection()
        {
            conn.Close();
            conn.ConnectionString = xmlReader.readConnectionString();
            conn.Open();

        }

        #region GetMaquinas

        public DataTable GetMaquinas()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"select ResName,ResCode from orsc where ResType = 'M'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public List<MaquinaModel> GetMaquinas()
        //{
        //    var query = String.Format(Query.GetMaquinas);
        //    return _businessOneDAO.ExecuteSqlForList<MaquinaModel>(query);
        //}


        #endregion

        #region GetMotivos
        public DataTable GetMotivos()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT code, name FROM [@CVA_MOTIVO]";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public List<MotivoModel> GetMotivo()
        //{
        //    var query = String.Format(Query.GetMotivos);
        //    return _businessOneDAO.ExecuteSqlForList<MotivoModel>(query);
        //}
        #endregion

        #region GetOperador

        public DataTable GetOperador()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT EMPID,FIRSTNAME+' '+ LASTNAME as 'NOME' FROM OHEM";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //    public List<OperadorModel> GetOperador()
        //{
        //    var query = String.Format(Query.GetOperador);
        //    return _businessOneDAO.ExecuteSqlForList<OperadorModel>(query);
        //}
        #endregion

        #region GetProduto

        public DataTable GetProduto()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT distinct owor.Itemcode as 'CODE'
	                                         , oitm.ItemName as 'NAME'
	 
                                         FROM owor
                                         INNER JOIN OITM
	                                        ON owor.ItemCode = oitm.itemcode
	                                        where owor.ItemCode = oitm.itemcode";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public List<ProdutoModel> GetProduto()
        //{
        //    var query = String.Format(Query.GetProduto);
        //    return _businessOneDAO.ExecuteSqlForList<ProdutoModel>(query);
        //}

        #endregion

        #region BuscaMaxValueMaqParadas

        public DataTable BuscaMaxValueMaqParadas()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"select isnull(max(code),0)+1 as 'Code', isnull(max(name),0)+1 as 'Name' from [@CVA_MAQ_PARADAS]";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public DataTable GetMAxValue()
        //{
        //    var query = String.Format(Query.GetMaxValue);
        //    return _businessOneDAO.ExecuteSqlForObject<DataTable>(query);
        //}

        #endregion

        #region GetMaquinaGrid

        public DataTable BuscaMaquinaParada(int code)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"select* from[@CVA_MAQ_PARADAS] where code = '{0}' ";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public List<MaxValue> BuscaMaquinaParada(string code)
        //{
        //    var query = String.Format(Query.BuscaMaquinaParada,code);
        //    return _businessOneDAO.ExecuteSqlForList<MaxValue>(query);
        //}


        #endregion


        public bool Insert(CampoMaquinaModel model)
        {
            try
            {
                var table = _company.UserTables.Item("CVA_MAQ_PARADAS");

                table.Code = model.Code.ToString().Trim();
                table.Name = model.Name.ToString().Trim();
                table.UserFields.Fields.Item("U_Maquina").Value = model.maquina.Trim();
                table.UserFields.Fields.Item("U_operador").Value = model.operador.Trim();
                table.UserFields.Fields.Item("U_produto").Value = model.produto.Trim();
                table.UserFields.Fields.Item("U_motivo").Value = model.motivo.Trim();
                table.UserFields.Fields.Item("U_lote").Value = model.lote.Trim();
                table.UserFields.Fields.Item("U_horaIni").Value = model.horaInicial;
                
                table.UserFields.Fields.Item("U_dtInicio").Value = model.dataInicial;
               
                table.UserFields.Fields.Item("U_duracao").Value = model.duracao.Trim();
                table.UserFields.Fields.Item("U_status").Value = "A";

                if (String.IsNullOrEmpty(model.dataFinal) &&  String.IsNullOrEmpty(model.horaFinal))
                {
                    model.dataFinal = "01/01/0001 00:00:00";
                    model.horaFinal = "00:00";
                    table.UserFields.Fields.Item("U_datafinal1").Value = model.dataFinal;
                    table.UserFields.Fields.Item("U_horafinal1").Value = model.horaFinal;
                }
                else
                {
                    //table.UserFields.Fields.Item("U_horaFim").Value = model.horaFinal;
                    //table.UserFields.Fields.Item("U_dtFim").Value = model.dataFinal;

                    table.UserFields.Fields.Item("U_datafinal1").Value = model.dataFinal;
                    table.UserFields.Fields.Item("U_horafinal1").Value = model.horaFinal;

                }

                    if (table.Add() != 0)
                {
                    int ErrorCode;
                    string errorMsg;

                    _company.GetLastError(out ErrorCode, out errorMsg);

                    _application.SetStatusBarMessage("Cadastro não efetuado " + ErrorCode + " " + errorMsg, SAPbouiCOM.BoMessageTime.bmt_Short);
                    BusinessOneLog.Add("Erro ao adicionar motivo : " + errorMsg, true);
                    return false;
                }

                _application.SetStatusBarMessage("Cadastrado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(CampoMaquinaModel model)
        {
            try
            {
                var table = _company.UserTables.Item("CVA_MAQ_PARADAS");
                table.GetByKey(model.Code.ToString());
                //table.Code = model.Code.ToString().Trim();
                //table.Name = model.Code.ToString().Trim();
                table.UserFields.Fields.Item("U_Maquina").Value = model.maquina.Trim();
                table.UserFields.Fields.Item("U_operador").Value = model.operador.Trim();
                table.UserFields.Fields.Item("U_produto").Value = model.produto.Trim();
                table.UserFields.Fields.Item("U_motivo").Value = model.motivo.Trim();
                table.UserFields.Fields.Item("U_lote").Value = model.lote.Trim();
                table.UserFields.Fields.Item("U_horaIni").Value = model.horaInicial;
                table.UserFields.Fields.Item("U_horafinal1").Value = model.horaFinal;
                table.UserFields.Fields.Item("U_dtInicio").Value = model.dataInicial;
                table.UserFields.Fields.Item("U_datafinal1").Value = model.dataFinal;
                table.UserFields.Fields.Item("U_duracao").Value = model.duracao.Trim();
                table.UserFields.Fields.Item("U_status").Value = "A";

                if (table.Update() != 0)
                {
                    int ErrorCode;
                    string errorMsg;

                    _company.GetLastError(out ErrorCode, out errorMsg);

                    _application.SetStatusBarMessage("Cadastro não Atualizado " + ErrorCode + " " + errorMsg, SAPbouiCOM.BoMessageTime.bmt_Short);
                    BusinessOneLog.Add("Erro ao adicionar motivo : " + errorMsg, true);
                    return false;
                }

                _application.SetStatusBarMessage("Cadastrado Atualizado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool Delete(CampoMaquinaModel model)
        {
            try
            {
                var table = _company.UserTables.Item("CVA_MAQ_PARADAS");
                table.GetByKey(model.Code.ToString());

                table.UserFields.Fields.Item("U_Maquina").Value = model.maquina.Trim();
                table.UserFields.Fields.Item("U_operador").Value = model.operador.Trim();
                table.UserFields.Fields.Item("U_produto").Value = model.produto.Trim();
                table.UserFields.Fields.Item("U_motivo").Value = model.motivo.Trim();
                table.UserFields.Fields.Item("U_lote").Value = model.lote.Trim();
                table.UserFields.Fields.Item("U_horaIni").Value = model.horaInicial;
                table.UserFields.Fields.Item("U_dtInicio").Value = model.dataInicial;
                table.UserFields.Fields.Item("U_duracao").Value = model.duracao.Trim();
                table.UserFields.Fields.Item("U_status").Value = "I";
                table.UserFields.Fields.Item("U_horaFim").Value = model.horaFinal;
                table.UserFields.Fields.Item("U_dtFim").Value = model.dataFinal;


                if (table.Update() != 0)
                {
                    int ErrorCode;
                    string errorMsg;

                    _company.GetLastError(out ErrorCode, out errorMsg);

                    _application.SetStatusBarMessage("Apontamento não Deletado " + ErrorCode + " " + errorMsg, SAPbouiCOM.BoMessageTime.bmt_Short);
                    BusinessOneLog.Add("Erro ao adicionar motivo : " + errorMsg, true);
                    return false;
                }

                _application.SetStatusBarMessage("Apontamento deletado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}

