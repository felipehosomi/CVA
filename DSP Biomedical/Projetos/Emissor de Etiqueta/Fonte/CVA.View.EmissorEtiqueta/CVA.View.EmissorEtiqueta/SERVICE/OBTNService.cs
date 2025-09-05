using CVA.View.EmissorEtiqueta.MODEL;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;

namespace CVA.View.EmissorEtiqueta.SERVICE
{
    public class OBTNService
    {
        private BusinessOneDAO _businessOneDAO { get; set; }


        /// <summary>
        /// Contructor Method with dependency injection (Control by Dover Framework)
        /// </summary>
        /// <param name="businessOneDAO"></param>
        public OBTNService(BusinessOneDAO businessOneDAO)
        {
            this._businessOneDAO = businessOneDAO;
        }

        /// <summary>
        /// Load and execute query defined in code, and return list of OBTN object
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public List<OBTN> GetLoteByItem(string itemCode)
        {
            try
            {
                var query = String.Format("SELECT ItemCode, DistNumber FROM OBTN WHERE ItemCode = '{0}'",itemCode);
                return _businessOneDAO.ExecuteSqlForList<OBTN>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}