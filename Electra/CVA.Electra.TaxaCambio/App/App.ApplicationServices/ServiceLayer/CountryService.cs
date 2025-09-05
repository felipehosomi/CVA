using App.Repository.Interfaces;
using App.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class CountryService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.Country> _repository;
        public CountryService()
        {
            _repository = new ServiceLayerRepositories<SAPB1.Country>("Countries");
        }

        public IEnumerable<SAPB1.Country> GetAll()
        {
            try
            {
               return _repository.GetAll("?$orderby=Name");
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
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
