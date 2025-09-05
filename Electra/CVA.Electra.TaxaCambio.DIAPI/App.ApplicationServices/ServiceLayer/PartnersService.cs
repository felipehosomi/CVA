using App.ApplicationServices.Addon;
using App.Domain.ValueObjects;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class PartnersService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.BusinessPartner> _repository;

        public PartnersService()
        {
            _repository = new ServiceLayerRepositories<SAPB1.BusinessPartner>("BusinessPartners");
        }
        
        public List<SAPB1.BusinessPartner> GetAll(string filter = "")
        {
            try
            {
                return (List<SAPB1.BusinessPartner>)_repository.GetAll(filter);
            }
            catch (CatchWebException)
            {
                throw;
            }
        }

        public SAPB1.BusinessPartner Get(string filter = "")
        {
            try
            {
                return _repository.Get(filter);
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw ex;
                }
                return null;
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
