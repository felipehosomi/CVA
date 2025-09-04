using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CVA_Rep_DAL
{
    public class USER
    {
        public string USERID { get; set; }
        public string USER_CODE { get; set; }
        public string E_Mail { get; set; }
        public string U_NAME { get; set; }

    } // Modelo de Usuário
    public class UsuarioDAL
    {
        ConciliadorDAL _conciliadorDAL = new ConciliadorDAL();

        public List<USER> GetUsers(CVA_BASES lastBase) //Retorna todos os usuários da base consolidadora
        {
            B1DAL _dao = new B1DAL(lastBase.USERNAME, lastBase.PASSWD, lastBase.BASE, lastBase.LICENSE_SERVER, Convert.ToBoolean(lastBase.USE_TRUSTED),//Instancia a conexão
                lastBase.DB_USERNAME, lastBase.DB_PASSWD, SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014, lastBase.DB_SERVER);                             //com a base do B1
            try
            {
                var usersList = new List<USER>();

                _dao.Connect();// Conecta com a base B1

                Recordset rst = (Recordset)_dao.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                rst.DoQuery("SELECT USERID,USER_CODE, U_NAME, E_Mail FROM OUSR WHERE Locked = 'N'");//Faz a busca dos usuários ativos
                
                while (!rst.EoF) //Adiciona o usuário na lista de usuários
                {
                    var user = new USER();
                    user.USERID = rst.Fields.Item("USERID").Value.ToString();
                    user.USER_CODE = rst.Fields.Item("USER_CODE").Value.ToString();
                    user.U_NAME = rst.Fields.Item("U_NAME").Value.ToString();
                    user.E_Mail = rst.Fields.Item("E_Mail").Value.ToString();

                    usersList.Add(user);
                    rst.MoveNext();
                }

                _dao.oCompany.Disconnect();
                GC.SuppressFinalize(_dao.oCompany);
                GC.Collect();

                return usersList;//Retorna a lista de usuários
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}