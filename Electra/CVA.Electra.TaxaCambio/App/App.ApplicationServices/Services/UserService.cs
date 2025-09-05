using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.Services
{
    static public class UserService
    {
        static public bool UsuarioQualidade(string username)
        {
            try
            {
                HanaService oHanaService = new HanaService();
                var Database = oHanaService.Database;
                var sql = $"SELECT 1 FROM \"{Database}\".OUSR T0  " +
                    $"INNER JOIN \"{Database}\".OHEM T1 ON T0.\"USERID\" = T1.\"userId\" " +
                    $"INNER JOIN \"{Database}\".OHPS T2 ON T1.\"position\" = T2.\"posID\" " +
                    $"WHERE T0.\"USER_CODE\"  = '{username}'  AND  T2.\"name\" = 'Qualidade'";
                return Convert.ToString( oHanaService.ExecuteScalar(sql)).Trim() == "1";
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
