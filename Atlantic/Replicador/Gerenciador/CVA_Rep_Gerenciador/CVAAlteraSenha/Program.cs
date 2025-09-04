using CVA_Rep_DAL;
using CVAAlteraSenha.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVAAlteraSenha
{
    class Program
    {
        static void Main(string[] args)
        {
            var bases = new ConciliadorDAL();
            var dbs = bases.Bases_GetAll();
            
            foreach (var db in dbs)
            {
                B12DAL _dao = new B12DAL(db.USERNAME, db.PASSWD, db.BASE, db.LICENSE_SERVER, Convert.ToBoolean(db.USE_TRUSTED),//Instancia a conexão
                       db.DB_USERNAME, db.DB_PASSWD, SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014, db.DB_SERVER);

                _dao.Connect();
            }
           
        }
    }
}
