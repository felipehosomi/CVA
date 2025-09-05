using App.ApplicationServices.ServiceLayer;
using App.Infrastructure;
using App.Infrastructure.Caching;
using App.Infrastructure.Configuration;
using App.Repository.Generic;
using App.Repository.Services;
using SAPB1;
using System;
using System.Net;

namespace App.ApplicationServices.Services
{
    public class ConnectionService
    {
        private static readonly Lazy<ConnectionService> Lazy = new Lazy<ConnectionService>(() => new ConnectionService());
        public static ConnectionService Instance => Lazy.Value;

        private static readonly IConfigurationRepository _configurationRepository = new AppSettingsConfigurationRepository();
        private static readonly ICacheStorage _cacheStorage = new SystemRuntimeCacheStorage();

        public bool Connection()
        {
            try
            {
                if (AddonService.isRunning)
                {
                    new Uri(ConnectionCacheService.GetServiceLayerUrl()).LoginConnectionString();
                }
                else
                {
                    var obj = new
                    {
                        UserName = _configurationRepository.GetConfigurationValue<string>("ServiceLayerUser"),
                        Password = _configurationRepository.GetConfigurationValue<string>("ServiceLayerPassword"),
                        CompanyDB = _configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")
                    };

                    new Uri(ConnectionCacheService.GetServiceLayerUrl() + "/Login").Login<B1Session>(obj);
                }

                if (ConnectionCacheService.CheckConnection(true))
                    return true;
                else
                    return false;
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return false;
            }
        }

        public bool Connection(string sServiceLayerPassword)
        {
            try
            {
                if (AddonService.isRunning)
                {
                    new Uri(ConnectionCacheService.GetServiceLayerUrl()).LoginConnectionString();
                }
                else
                {
                    var obj = new
                    {
                        UserName = _configurationRepository.GetConfigurationValue<string>("ServiceLayerUser"),
                        Password = _configurationRepository.GetConfigurationValue<string>("ServiceLayerPassword"),
                        CompanyDB = _configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")
                    };

                    new Uri(ConnectionCacheService.GetServiceLayerUrl() + "/Login").Login<B1Session>(obj);
                }

                if (ConnectionCacheService.CheckConnection(true))
                    return true;
                else
                    return false;
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return false;
            }
        }

        public bool Disconnection()
        {
            try
            {
                if(ConnectionCacheService.CheckConnection(true))
                    new Uri(ConnectionCacheService.GetServiceLayerUrl() + "/Logout").Post<object>();
                return true;
            }
            catch (WebException e)
            {
                CatchWebException.ExceptionError(e);
                return false;
            }
        }
        

    }
}
