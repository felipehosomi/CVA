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
    public class ItemsService : IDisposable
    {
        private readonly IServiceLayerRepository<SAPB1.Item> _reporsitory;

        public ItemsService()
        {
            _reporsitory = new ServiceLayerRepositories<SAPB1.Item>("Items");
        }

        public SAPB1.Item Get(string filter)
        {
            try
            {
                return _reporsitory.Get(filter);
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
            //catch (CatchWebException e)
            //{
            //    throw e;
            //}
        }

        public void FuckingGetAll()
        {
            try
            {
                var test = _reporsitory.GetAll();
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
        }

        public void FuckingAdd()
        {
            var item = new SAPB1.Item
            {
                ItemCode = "ABCD",
                ItemName = "ABCDTest",
                ItemType = "itItems",
            };

            try
            {
                //var test = _reporsitory.Add(item);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
        }

        public void FuckingEdit()
        {
            var item = new SAPB1.Item();
            try
            {
                item = _reporsitory.Get("('ABCD')");
                item.ItemCode = "ABCDTest1";
               // var test = _reporsitory.Edit(item, item.ItemCode);
            }
            catch (CatchWebException e)
            {
                return;
            }
        }

        public void FuckingDelete()
        {
            var item = new SAPB1.Item();
            try
            {
                item = _reporsitory.Get("?$filter=ItemCode eq 'ABCD'");
                _reporsitory.Delete(item, item.ItemCode);
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
