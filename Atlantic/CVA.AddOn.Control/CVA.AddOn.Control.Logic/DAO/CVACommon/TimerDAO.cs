using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.DAO.CVACommon
{
    public class TimerDAO
    {
        public void RestartTimer()
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            sqlHelper.ExecuteNonQuery(Resources.Query.Timer_Restart);
        }
    }
}
