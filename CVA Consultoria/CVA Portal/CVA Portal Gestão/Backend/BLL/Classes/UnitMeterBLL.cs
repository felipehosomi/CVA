using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class UnitMeterBLL
    {
        #region Atributos
        private UnitMeterDAO _unitMeterDAO { get; set; }
        #endregion

        #region Atributos
        public UnitMeterBLL()
        {
            this._unitMeterDAO = new UnitMeterDAO();
        }
        #endregion

        public List<UnitMeterModel> Get()
        {
            try
            {
                return _unitMeterDAO.Get();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
