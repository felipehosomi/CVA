using CVA.Core.ControlCommon.HELPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.SERVICE.CVACommon
{
    public class TimerDAO
    {
        public void RestartTimer()
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConnectionString);
            sqlHelper.ExecuteNonQuery(Resource.Query.Timer_Restart);
        }
    }
}
