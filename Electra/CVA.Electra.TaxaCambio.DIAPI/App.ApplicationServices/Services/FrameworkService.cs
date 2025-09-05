using App.Infrastructure;
using App.Infrastructure.Caching;
using App.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.Services
{
    static class FrameworkService
    {
        private static readonly ICacheStorage _cacheStorage = new SystemRuntimeCacheStorage();
        private static readonly IConfigurationRepository _configurationRepository = new AppSettingsConfigurationRepository();
        public static string Database;

        public static string GetHanaConnectionString()
        {
            const string key = "HanaConnectionString";
            var response = _cacheStorage.Retrieve<string>(key);
            if (response == null)
            {
                response = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("HanaConnectionString"));
                response += ";UserName=" + Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataUser"));
                response += ";Password=" + Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataPassword"));

                var cacheDurationSeconds = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ConfigCacheDuration")).ToInt();
                _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
            }
            Database = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase"));

            return response;
        }
    }
}
