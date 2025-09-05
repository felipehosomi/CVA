using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPB1;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using App.Repository.Exception;
using App.Repository.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.ApplicationServices.ServiceLayer
{
    public class JournalEntriesService : IDisposable
    {
        
        private readonly IServiceLayerRepository<SAPB1.JournalEntry> _repository;

        public JournalEntriesService()
        {
            _repository = new ServiceLayerRepositories<SAPB1.JournalEntry>("JournalEntries");
        }

        public SAPB1.JournalEntry Add(SAPB1.JournalEntry journalEntry)
        {
            try
            {
                return  _repository.Add(journalEntry);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }

            return null;
        }
        
        public SAPB1.JournalEntry Get(string filter)
        {
            try
            {
                return _repository.Get(filter);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
            return null;
        }

        public IEnumerable<SAPB1.JournalEntry> GetAll(string filter)
        {
            try
            {
                return _repository.GetAll(filter);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
            }
            return null;
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
