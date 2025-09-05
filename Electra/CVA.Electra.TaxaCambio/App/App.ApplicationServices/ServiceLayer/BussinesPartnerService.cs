using App.Repository.Exception;
using App.Repository.Generic;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class BusinessPartnerService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.BusinessPartner> _repository;

        public BusinessPartnerService()
        {
            _repository = new ServiceLayerRepositories<SAPB1.BusinessPartner>("BusinessPartners");
        }

        public void Get()
        {
            try
            {
                var test = _repository.Get("('C0000001')");
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }            
        }

        public void GetAll()
        {
            try
            {
                var test = _repository.GetAll();
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
        }

        public IEnumerable<SAPB1.BusinessPartner> GetAll(string filter)
        {
            try
            {
                return _repository.GetAll(filter);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
            }
        }

        public void Add()
        {
            try
            {
                //var test = _repository.Add(bank);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
        }

        public void Edit()
        {
            try
            {
                //var test = _repository.Edit(businessPartner, businessPartner.CardCode);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
        }

        public void Delete()
        {
            try
            {
                //_repository.Delete(businessPartner, businessPartner.CardCode);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
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
