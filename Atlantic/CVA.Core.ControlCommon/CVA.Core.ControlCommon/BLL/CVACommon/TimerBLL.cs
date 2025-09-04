using CVA.Core.ControlCommon.SERVICE.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL.CVACommon
{
    public class TimerBLL
    {
        TimerDAO _timerDAO { get; set; }

        public TimerBLL(TimerDAO timerDAO)
        {
            _timerDAO = timerDAO;
        }

        public void RestartTimer()
        {
            _timerDAO.RestartTimer();
        }
    }
}
