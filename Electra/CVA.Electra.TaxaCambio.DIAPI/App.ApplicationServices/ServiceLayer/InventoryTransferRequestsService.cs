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
    public class InventoryTransferRequestsService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.StockTransfer> _reporsitory;

        public InventoryTransferRequestsService()
        {
            _reporsitory = new ServiceLayerRepositories<SAPB1.StockTransfer>("InventoryTransferRequests");
        }

        public SAPB1.StockTransfer Get(int id)
        {
            try
            {
                var retFilter = $"?$filter= DocNum eq " + id;

                return _reporsitory.GetAll(retFilter).FirstOrDefault();
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
            }
        }

        public SAPB1.StockTransfer Edit(int id, SAPB1.StockTransfer stockTransfer)
        {
            try
            {
                return _reporsitory.Edit(stockTransfer,id, false);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
            }
        }

        public IEnumerable<SAPB1.StockTransfer> GetAll()
        {
            try
            {
                return _reporsitory.GetAll();
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
            }
        }

        public SAPB1.StockTransfer Add(SAPB1.StockTransfer stockTransfer)
        {
            try
            {
                return _reporsitory.Add(stockTransfer);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
            }
        }

        public bool Close(SAPB1.StockTransfer stockTransfer)
        {
            try
            {
                _reporsitory.Close(stockTransfer.DocEntry);
                return true;
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return false;
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
