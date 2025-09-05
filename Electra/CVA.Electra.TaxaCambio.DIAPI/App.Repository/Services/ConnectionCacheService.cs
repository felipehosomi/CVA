using App.Infrastructure.Caching;
using App.Infrastructure.Configuration;
using App.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using App.Repository.Repositories;
using App.Infrastructure;
using SAPB1;

namespace App.Repository.Services
{
    public class ConnectionCacheService
    {
        //cache
        private static readonly ICacheStorage _cacheStorage = new SystemRuntimeCacheStorage();
        private static readonly IConfigurationRepository _configurationRepository = new AppSettingsConfigurationRepository();

        public static bool CheckConnection([Optional] bool force)
        {
            try
            {
                const string key = "CheckConnection";
                var response = _cacheStorage.Retrieve<bool>(key);
                if (!response || force)
                {
                    response = !string.IsNullOrEmpty(new Uri(GetServiceLayerUrl() + "/LicenseService_GetInstallationNumber").Post<string>());

                    var cacheDurationSeconds = Convert.ToInt32(_configurationRepository.GetConfigurationValue<string>("CheckConnectionCacheDuration"));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public static string GetServiceLayerUrl(CallType callType = CallType.ServiceLayer)
        {
            try
            {
                switch (callType)
                {
                    case CallType.ServiceLayer:
                        return GetServiceLayerUrl();
                    default:
                        return GetServiceLayerODataUrl();
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static string GetServiceLayerUrl()
        {
            try
            {
                const string key = "ServiceLayerUrl";
                var response = _cacheStorage.Retrieve<string>(key);
                if (response == null)
                {
                    response = _configurationRepository.GetConfigurationValue<string>("ServiceLayerURL");
                    if (string.IsNullOrEmpty(response))
                        response = AddonService.GetServiceLayerDI();

                    if (response.EndsWith("/")) response = response.Remove(response.ToString().LastIndexOf('/'), 1);

                    var cacheDurationSeconds = Convert.ToInt32(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration"));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static string GetServiceLayerODataUrl()
        {
            try
            {
                const string key = "OdataUrl";
                var response = _cacheStorage.Retrieve<string>(key);
                if (response == null)
                {
                    response = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataUrl"));

                    if (response.EndsWith("/")) response = response.Remove(response.ToString().LastIndexOf('/'), 1);

                    var cacheDurationSeconds = Convert.ToInt32(Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration")));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static void ConnectionServiceLayer()
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
                        UserName = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerUser")),
                        Password = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerPassword")),
                        CompanyDB = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase"))
                    };

                    new Uri(ConnectionCacheService.GetServiceLayerUrl() + "/Login").Login<B1Session>(obj);
                }
            }
            catch (WebException e)
            {
                throw e;
            }
        }

        public static string GetBasicAuthorization()
        {
            try
            {
                const string key = "BasicAuthorization";
                var response = _cacheStorage.Retrieve<string>(key);
                if (response == null)
                {
                    var username = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataUser"));
                    var password = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataPassword"));

                    response = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

                    var cacheDurationSeconds = Convert.ToInt32(Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration")));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static string GetDataBaseName()
        {
            try
            {
                const string key = "DataBaseName";
                var response = _cacheStorage.Retrieve<string>(key);
                if (response == null)
                {
                    response = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase"));

                    var cacheDurationSeconds = Convert.ToInt32(Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration")));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static string GetHanaConnectionString()
        {
            try
            {
                const string key = "HanaConnectionString";
                var response = _cacheStorage.Retrieve<string>(key);
                if (response == null)
                {
                    response = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("HanaConnectionString"));
                    response += ";UserName=" + Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataUser"));
                    response += ";Password=" + Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataPassword"));

                    var cacheDurationSeconds = Convert.ToInt32(Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration")));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static string GetKeyDelimiterCommand()
        {
            try
            {
                const string key = "KeyDelimiterCommand";
                var response = _cacheStorage.Retrieve<string>(key);
                if (response == null)
                {
                    response = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("KeyDelimiterCommand"));

                    var cacheDurationSeconds = Convert.ToInt32(Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration")));
                    _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
                }

                return response;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}
