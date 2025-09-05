using CVA.View.EmissorEtiqueta.MODEL;
using CVA.View.EmissorEtiqueta.SERVICE;
using System;
namespace CVA.View.EmissorEtiqueta.BLL
{
    public class OitmBLL
    {
        public OITMService _oitmService { get; set; }

        /// <summary>
        /// Constructor Method with dependency injection (Control by Dover)
        /// </summary>
        /// <param name="oitmService"></param>
        public OitmBLL(OITMService oitmService)
        {
            this._oitmService = oitmService;
        }

        /// <summary>
        /// Get Description from Item throught itemCode
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string GetItemDescription(string itemCode)
        {
            try
            {
                var result = _oitmService.Get_ItemDescription(itemCode);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PRINT GetEtiqueta(string itemCode)
        {
            return _oitmService.GetEtiqueta(itemCode);
        }
    }
}
