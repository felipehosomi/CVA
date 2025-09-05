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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class BranchesService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.Branch> _reporsitory;

        public BranchesService()
        {
            _reporsitory = new ServiceLayerRepositories<SAPB1.Branch>("Branches");
        }

        public void Get([Optional] string filter)
        {
            try
            {
                var test = _reporsitory.Get(filter);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }            
        }

        public void GetAll([Optional] string filter)
        {
            try
            {
                var test = _reporsitory.GetAll(filter);
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
