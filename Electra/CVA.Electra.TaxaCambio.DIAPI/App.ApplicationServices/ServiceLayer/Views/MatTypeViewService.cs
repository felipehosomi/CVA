using App.Domain.ValueObjects.Views;
using App.Repository.Exception;
using App.Repository.Generic;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class MatTypeViewService : IDisposable
    {
        private readonly IServiceLayerRepository<MatTypeView> _reporsitory;

        public MatTypeViewService()
        {
            _reporsitory = new ServiceLayerRepositories<MatTypeView>("Odata/application.xsodata/MatTypeView?$format=json", CallType.OData);
        }


        public ObservableCollection<MatTypeView> GetAll(string filterDocs)
        {

            var retFilter = string.Empty;

            if (!filterDocs.IsNullOrEmpty())
                retFilter = $"&$filter=" + filterDocs;

            var ret = _reporsitory.GetAll(retFilter);

            if (ret.IsNullOrEmpty())
                return null;
            else
                return new ObservableCollection<MatTypeView>(ret);

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
