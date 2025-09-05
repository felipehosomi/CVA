using System;
using App.ApplicationServices.ServiceLayer;

namespace App.ApplicationServices.Addon
{
    public class CurrencyService
    {
        #region [ Atualizar Currency ]

        public static void AtualizarCurrency(string sMoeda, double dRate, DateTime dtData)
        {
            try
            {
                using (var sBOBobService_SetCurrencyRate = new SBOBobService_SetCurrencyRate())
                {
                    var obj = new CurrencySBO();
                    obj.Currency = sMoeda;
                    obj.Rate = dRate;
                    obj.RateDate = dtData.ToString("yyyyMMdd");
                    sBOBobService_SetCurrencyRate.Add(obj);
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
