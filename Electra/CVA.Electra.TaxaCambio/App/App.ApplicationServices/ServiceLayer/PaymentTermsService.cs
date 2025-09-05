using App.ApplicationServices.Addon;
using App.Domain.ValueObjects;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class PaymentTermsService
    {
        private readonly IServiceLayerRepository<SAPB1.PaymentTermsType> _repository;

        public PaymentTermsService()
        {
            _repository = new ServiceLayerRepositories<SAPB1.PaymentTermsType>("PaymentTermsTypes");
        }

        public List<SAPB1.PaymentTermsType> GetAll()
        {
            try
            {
                return (List<SAPB1.PaymentTermsType>)_repository.GetAll();
            }
            catch (CatchWebException)
            {
                throw;
            }
        }
    }
}
