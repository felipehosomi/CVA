using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using CVA.Core.ControlCommon.SERVICE.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL.CVACommon
{
    public class RegistroBLL
    {
        RegistroDAO _registroDAO { get; set; }
        TimerBLL _timerBLL { get; set; }

        public RegistroBLL(RegistroDAO registroDAO, TimerBLL timerBLL)
        {
            _registroDAO = registroDAO;
            _timerBLL = timerBLL;
        }

        public void RestartErrorReg(string objectCode, CVAObjectEnum objectType)
        {
            _registroDAO.UpdateError(objectCode, objectType);
            _timerBLL.RestartTimer();
        }

        public void Insert(string objectCode, CVAObjectEnum objectType, FuncaoEnum funcao)
        {
            Registro registro = new Registro();
            registro.CodigoBase = StaticKeys.Base.ID;

            registro.Insert = DateTime.Now;
            registro.Status = 3;
            registro.Codigo = objectCode;
            registro.Objeto = (int)objectType;
            registro.Funcao = (int)funcao;

            _registroDAO.Insert(registro);
        }

        public Registro Get(string objectCode, CVAObjectEnum objectType)
        {
            return _registroDAO.Get(objectCode, objectType);
        }
    }
}
