using CVA.View.EmissorEtiqueta.MODEL;
using CVA.View.EmissorEtiqueta.Resources;
using Dover.Framework.DAO;
using System;

namespace CVA.View.EmissorEtiqueta.SERVICE
{
    public class OITMService
    {
        private BusinessOneDAO _businessOneDAO { get; set; }

        /// <summary>
        /// Contructor Method with depedency injection (Control by Dover Framework)
        /// </summary>
        /// <param name="businessOneDAO"></param>
        public OITMService(BusinessOneDAO businessOneDAO)
        {
            this._businessOneDAO = businessOneDAO;
        }

        /// <summary>
        /// Get description defined in user field allocated in OITM table. Load and execute query.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string Get_ItemDescription(string itemCode)
        {
            try
            {
                var query = String.Format(@"
                                            SELECT 
                                                U_DSP_DescEtiqueta 
                                            FROM OITM T0 
                                            WHERE T0.ItemCode = '{0}'", itemCode);
                return _businessOneDAO.ExecuteSqlForObject<string>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PRINT GetEtiqueta(string itemCode)
        {
            var query = String.Format(Query.Etiqueta_Componente, itemCode);
            return _businessOneDAO.ExecuteSqlForObject<PRINT>(query);
        }
    }
}