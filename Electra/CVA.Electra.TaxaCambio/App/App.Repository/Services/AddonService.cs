using App.Infrastructure;
using App.Infrastructure.Caching;
using App.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Repository.Services
{
    public class AddonService
    {
        public static string DatabaseName;
        static public SAPbouiCOM.Application uiApplication;
        static public SAPbobsCOM.Company diCompany;
        static public bool isRunning = false;
        static public string serviceLayerURL;
        private static readonly IConfigurationRepository _configurationRepository = new AppSettingsConfigurationRepository();
        //cache
        private static readonly ICacheStorage _cacheStorage = new SystemRuntimeCacheStorage();

        public static void GetApplication()
        {
            SAPbouiCOM.SboGuiApi sboGuiApi;
            String sConnectionString;
            try
            {
                sboGuiApi = new SAPbouiCOM.SboGuiApi();
                sConnectionString = Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));
                sboGuiApi.Connect(sConnectionString);
                uiApplication = sboGuiApi.GetApplication(-1);
                Marshal.FinalReleaseComObject(sboGuiApi);
                isRunning = true;
                DatabaseName = uiApplication.Company.DatabaseName;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public static string GetServiceLayerDI()
        {
            try
            {
                diCompany = uiApplication.Company.GetDICompany();
                var Servidor = diCompany.SLDServer.Split(':');
                string NomeServer = Servidor[1].Replace("//", "");
                int iTeste;
                if (int.TryParse(NomeServer, out iTeste))
                {
                    NomeServer = Servidor[0];
                }
                return string.Format(@"https://{0}:50000/b1s/v1", NomeServer);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private static string _GetConnectionContextService()
        {
            try
            {
                string connectionContext = string.Empty;
                try
                {
                    if (isRunning)
                    {
                        var ret = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerURL"));
                        if (string.IsNullOrEmpty(ret))
                            ret = GetServiceLayerDI();

                        var serviceLayerURL = ret;
                        connectionContext = uiApplication.Company.GetServiceLayerConnectionContext(serviceLayerURL);
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
                if (connectionContext == string.Empty)
                {
                    throw new System.Exception("UI API - Get service layer connection context error");
                }
                return connectionContext;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static string GetAddonConnectionContext()
        {
            const string key = "AddonConnectionContext";
            var response = _cacheStorage.Retrieve<string>(key);
            if (response == null)
            {
                if (isRunning)
                    try { response = _GetConnectionContextService(); }
                    catch (System.Exception) { throw; }

                var cacheDurationSeconds = Convert.ToInt32(Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("CheckConnectionCacheDuration")));
                _cacheStorage.Store(key, response, TimeSpan.FromSeconds(cacheDurationSeconds * 60));
            }

            return response;
        }

        public static object ExecuteSqlScalar(string query)
        {
            SAPbobsCOM.Recordset objRS;
            try
            {
                objRS = (SAPbobsCOM.Recordset)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                objRS.DoQuery(query);

                if (objRS.RecordCount > 0)
                {
                    return objRS.Fields.Item(0).Value;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                objRS = null;
            }
        }

        public static void ConnectDIbyConfig()
        {
            try
            {
                if (diCompany == null)
                {
                    diCompany = new SAPbobsCOM.Company();
                }
                if (!diCompany.Connected)
                {
                    var UserName = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerUser"));
                    var Password = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerPassword"));
                    var CompanyDB = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase"));
                    var HanaConnectionString = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("HanaConnectionString"));
                    var DbUserName = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataUser"));
                    var DbPassword = Cryptography.DecryptStringAES(_configurationRepository.GetConfigurationValue<string>("OdataPassword"));
                    var LicenseServer = _configurationRepository.GetConfigurationValue<string>("LicenseServer");
                    var Server = HanaConnectionString.Split('=')[1];
                    diCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    diCompany.Server = Server;
                    diCompany.LicenseServer = LicenseServer;
                    diCompany.DbUserName = DbUserName;
                    diCompany.DbPassword = DbPassword;
                    diCompany.CompanyDB = CompanyDB;
                    diCompany.UserName = UserName;
                    diCompany.Password = Password;
                    diCompany.UseTrusted = false;
                    var ret = diCompany.Connect();
                    if (ret != 0)
                    {
                        var erroText = diCompany.GetLastErrorDescription();
                        MessageBox.Show(erroText);
                        Environment.Exit(0);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
