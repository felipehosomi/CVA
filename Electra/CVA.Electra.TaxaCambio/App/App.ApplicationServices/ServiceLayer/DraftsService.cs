using App.Repository.Exception;
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
using SAPB1;

namespace App.ApplicationServices.ServiceLayer
{
    class DraftsService
    {
        private readonly IServiceLayerRepository<SAPB1.Document> _repository;

        public DraftsService()
        {
            _repository = new ServiceLayerRepositories<SAPB1.Document>("Drafts");
        }

        public SAPB1.Document Add( SAPB1.Document document)
        {
            try
            {
                return _repository.Add(document);
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return null;
            }
        }
        
        internal void AddList(List<Document> lisDocs)
        {
            throw new NotImplementedException();
        }
    }
}
