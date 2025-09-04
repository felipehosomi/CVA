using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

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
    }
}
