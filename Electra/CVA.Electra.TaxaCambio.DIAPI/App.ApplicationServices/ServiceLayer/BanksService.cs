using App.Repository.Exception;
using App.Repository.Generic;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class BanksService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.Bank> _reporsitory;

        public BanksService()
        {
            _reporsitory = new ServiceLayerRepositories<SAPB1.Bank>("Banks");
        }

        public void FuckingGet()
        {
            try
            {
                var test = _reporsitory.Get("?$filter=BankCode eq '000'");
            }
            catch (CatchWebException)
            {
                throw;
            }
        }

        public void FuckingGetAll()
        {
            try
            {
                var test = _reporsitory.GetAll();
            }
            catch (CatchWebException)
            {
                throw;
            }
        }

        public void FuckingAdd()
        {
            var bank = new SAPB1.Bank
            {
                BankCode = "9993",
                BankName = "Banco HSBC S.A.",
                CountryCode = "BR",
                PostOffice = "tNO",
                AbsoluteEntry = 74
            };

            try
            {
                //var test = _reporsitory.Add(bank);
            }
            catch (CatchWebException)
            {
                throw;
            }
        }

        public void FuckingEdit()
        {
            var bank = new SAPB1.Bank();
            try
            {
                bank = _reporsitory.Get("?$filter=BankCode eq '9993'");
                bank.BankName = "ABC";
               // var test = _reporsitory.Edit(bank, bank.BankCode);
            }
            catch (CatchWebException)
            {
                throw;
            }
        }

        public void FuckingDelete()
        {
            var bank = new SAPB1.Bank();
            try
            {
                bank = _reporsitory.Get("?$filter=BankCode eq '9993'");
                _reporsitory.Delete(bank, bank.BankCode);
            }
            catch (CatchWebException)
            {
                throw;
            }
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BanksService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
