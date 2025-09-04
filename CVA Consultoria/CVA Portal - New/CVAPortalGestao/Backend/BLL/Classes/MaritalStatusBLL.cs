using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class MaritalStatusBLL
    {
        #region Atributos
        private MaritalStatusDAO MaritalStatusDAO { get; set; }
        #endregion

        #region Construtor
        public MaritalStatusBLL()
        {
            this.MaritalStatusDAO = new MaritalStatusDAO();
        }
        #endregion

        public List<MaritalStatusModel> Get()
        {
            try
            {
                return MaritalStatusDAO.Get();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
