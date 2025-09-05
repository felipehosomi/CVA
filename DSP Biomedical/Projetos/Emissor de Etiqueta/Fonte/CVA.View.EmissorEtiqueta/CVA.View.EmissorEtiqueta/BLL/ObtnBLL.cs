using CVA.View.EmissorEtiqueta.MODEL;
using CVA.View.EmissorEtiqueta.SERVICE;
using System;
using System.Collections.Generic;

namespace CVA.View.EmissorEtiqueta.BLL
{
    public class ObtnBLL
    {
        private OBTNService _otbnService { get; set; }

        /// <summary>
        /// Constructor method with dependency injection (Control by Dover)
        /// </summary>
        /// <param name="obtnService"></param>
        public ObtnBLL(OBTNService obtnService)
        {
            this._otbnService = obtnService;
        }

        /// <summary>
        /// Retur List of OBTN class from itemCode.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public List<OBTN> GetLoteByItem(string itemCode)
        {
            try
            {
                if (string.IsNullOrEmpty(itemCode)) return null;
                return _otbnService.GetLoteByItem(itemCode);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
