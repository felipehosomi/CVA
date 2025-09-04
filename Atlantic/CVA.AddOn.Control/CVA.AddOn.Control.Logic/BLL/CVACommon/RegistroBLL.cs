using CVA.AddOn.Control.Logic.DAO.CVACommon;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.BLL.CVACommon
{
    public class RegistroBLL
    {
        RegistroDAO _registroDAO { get; set; }
        TimerBLL _timerBLL { get; set; }

        public RegistroBLL()
        {
            _registroDAO = new RegistroDAO();
            _timerBLL = new TimerBLL();
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

        public bool IsFirstErrorObject(string objectCode, CVAObjectEnum objectType)
        {
            return _registroDAO.IsFirstErrorObject(objectCode, objectType);
        }

        public void RestartErrorReg(CVAObjectEnum objectType)
        {
            _registroDAO.UpdateError(objectType);
            _timerBLL.RestartTimer();
        }
    }
}
