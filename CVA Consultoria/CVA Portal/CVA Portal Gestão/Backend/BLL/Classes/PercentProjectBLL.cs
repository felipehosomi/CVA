using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class PercentProjectBLL
    {
        #region Atributos
        private PercentProjectDAO _percentProjectDAO { get; set; }
        #endregion

        #region Construtor
        public PercentProjectBLL()
        {
            this._percentProjectDAO = new PercentProjectDAO();
        }
        #endregion

        public List<PercentProjectModel> Get()
        {
            try
            {
                return _percentProjectDAO.Get();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
