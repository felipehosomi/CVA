using AUXILIAR;
using DAO;
using MODEL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ChamadoBLL
    {
        public List<string> logList = new List<string>();
        public LogWriter _logWriter = new LogWriter();
        public ChamadoDAO _chamadoDAO = new ChamadoDAO();

        public void Save(List<ChamadoModel> chamados)
        {
            foreach (var c in chamados)
            {
                if (!_chamadoDAO.CheckIfSaved(c))
                    logList.Add(_chamadoDAO.Save(c));
                else
                    logList.Add(_chamadoDAO.Update(c));
            }
            _logWriter.WriteLog(logList);
        }
    }
}