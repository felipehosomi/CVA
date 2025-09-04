using CVA.AddOn.Control.Logic.DAO.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.BLL.CVACommon
{
    public class TimerBLL
    {
        TimerDAO _timerDAO { get; set; }

        public TimerBLL()
        {
            _timerDAO = new TimerDAO();
        }

        public void RestartTimer()
        {
            _timerDAO.RestartTimer();
        }
    }
}
