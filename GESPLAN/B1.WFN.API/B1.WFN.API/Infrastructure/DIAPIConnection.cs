using B1.WFN.API.Models;
using Microsoft.Extensions.Options;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B1.WFN.API.Infrastructure
{

    public class DIAPIConnection
    {
        private readonly Company _company;
        public string _LastError;

        public DIAPIConnection(IOptions<DIAPIConnectionSettings> settings)
        {
            var DIAPISettings = settings.Value;

            var _server = string.IsNullOrEmpty(DIAPISettings.ServerInstance) ? Crypto.Decrypt(DIAPISettings.Server) : $"{Crypto.Decrypt(DIAPISettings.Server)}\\{Crypto.Decrypt(DIAPISettings.ServerInstance)}";
            try
            {

                _company = new Company()
                {
                    Server = _server,
                    CompanyDB = Crypto.Decrypt(DIAPISettings.CompanyDB),
                    DbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), DIAPISettings.DbServerType, true),
                    UserName = Crypto.Decrypt(DIAPISettings.UserName),
                    Password = Crypto.Decrypt(DIAPISettings.Password),
                    language = (BoSuppLangs)Enum.Parse(typeof(BoSuppLangs), DIAPISettings.Language, true),
                    SLDServer = Crypto.Decrypt(DIAPISettings.SLDServer),
                    UseTrusted = DIAPISettings.UseTrusted
                };
                var code = _company.Connect();
                if (code != 0)
                {
                    throw new Exception(code.ToString() + " - " + _company.GetLastErrorDescription());
                }

            }
            catch (Exception ex)
            {
                _company = null;
                _LastError = ex.Message;
            }

        }

        public Company Connection
        {
            get
            {
                return _company;
            }
        }
    }
}
