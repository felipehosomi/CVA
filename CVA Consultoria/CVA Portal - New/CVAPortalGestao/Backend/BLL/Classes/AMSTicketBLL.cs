using DAO.Classes;
using MODEL.Classes;
using System;
using System.Collections.Generic;

namespace BLL.Classes
{
    public class AMSTicketBLL
    {
        #region Atributos
        ProjectDAO _projectDAO { get; set; }
        #endregion

        #region Construtor
        public AMSTicketBLL()
        {
            _projectDAO = new ProjectDAO();
        }
        #endregion

        public List<AMSTicketModel> GetTicketsByProject(int projectId)
        {
            return _projectDAO.GetTicketsByProject(projectId);
        }

        public List<AMSTicketModel> GetTicketsByProject(int projectId, DateTime date)
        {
            return _projectDAO.GetTicketsByProject(projectId, date);
        }

        public List<AMSTicketModel> GetTicketsByProject(int projectId, DateTime date, int numTicket)
        {
            return _projectDAO.GetTicketsByProject(projectId, date, numTicket);
        }
    }
}
