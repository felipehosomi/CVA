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
    public class SearchPurchaseOrderListViewService : IDisposable
    {
        private readonly IServiceLayerRepository<SearchPurchaseOrderView> _reporsitory;

        public SearchPurchaseOrderListViewService()
        {
            _reporsitory = new ServiceLayerRepositories<SearchPurchaseOrderView>("Odata/application.xsodata/SearchPurchaseOrderView?$format=json", CallType.OData);
        }

        
        public ObservableCollection<SearchPurchaseOrderView> GetAll(int? docnum, string cardname, DateTime? dateTimeDe, DateTime? dateTimeAte)
        {

            var filter = new StringBuilder();

            if (docnum.HasValue && docnum > 0)
                filter.Append($"DocNum eq {docnum} and ");

            if (!cardname.IsNullOrEmpty())
                filter.Append($"(indexof(toupper(CardName), '{cardname.ToUpper()}') gt -1 or CNPJ eq '{cardname}') and ");

            if (dateTimeDe.HasValue && dateTimeAte.HasValue)
            {
                filter.Append($"DocDateOrder ge datetime'{dateTimeDe.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}' and DocDateOrder le datetime'{dateTimeAte.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}' and ");
            }
            else if (dateTimeDe.HasValue)
            {
                filter.Append($"DocDateOrder ge datetime'{dateTimeDe.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}' and ");
            }
            else if (dateTimeAte.HasValue)
            {
                filter.Append($"DocDateOrder le datetime'{dateTimeAte.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}' and ");
            }
            
            var retFilter = string.Empty;
            if (filter.ToString().EndsWith("and "))
                retFilter = filter.ToString().Remove(filter.Length-4, 4);
            else
                retFilter = filter.ToString();

            if (!retFilter.IsNullOrEmpty())
                retFilter = $"&$filter=" + retFilter;

            var ret = _reporsitory.GetAll(retFilter);
            if (ret.IsNullOrEmpty())
                return null;
            else
                return new ObservableCollection<SearchPurchaseOrderView>(ret);

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
