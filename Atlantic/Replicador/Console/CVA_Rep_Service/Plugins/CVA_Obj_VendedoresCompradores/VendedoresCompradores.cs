using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;

namespace CVA_Obj_VendedoresCompradores
{
    public class VendedoresCompradores : IPlugin
    {
        private readonly ILogger _logger;
        private readonly ILogService _logService = Log4NetService.Instance;
        private bool _disposed;

        public VendedoresCompradores()
        {
            _logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            _logger = _logService.GetLogger<VendedoresCompradores>();
        }

        public override string Name => "CVA_Obj_VendedoresCompradores";

        public override void Dispose()
        {
            Close();
        }

        public override void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                _disposed = true;
            }
        }

        public override int Create(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    if (Exists(registro, oCompany))
                        continue;

                    var sFile = $"{Path.GetTempPath()}\\VendedoresCompradores.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesPersons oSalesPersons = (SalesPersons) oCompany.GetBusinessObjectFromXML(sFile, 0);

                    if (oSalesPersons.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesPersons);
                    //oSalesPersons = null;
                }
            }
            catch (ReplicadorException ex)
            {
                _logger.Error(GetInnerException(ex), ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Fatal(GetInnerException(ex), ex);
                throw;
            }

            return 0;
        }

        public override int Update(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    var sFile = $"{Path.GetTempPath()}\\VendedoresCompradores.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesPersons oSalesPersons = (SalesPersons) oCompany.GetBusinessObject(BoObjectTypes.oSalesPersons);

                    var oUnitOfWork = new UnitOfWork();
                    var reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oSalesPersons.GetByKey(Convert.ToInt32(reg.CODE));
                    oSalesPersons.Browser.ReadXml(sFile, 0);

                    if (oSalesPersons.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesPersons);
                    //oSalesPersons = null;
                }
            }
            catch (ReplicadorException ex)
            {
                _logger.Error(GetInnerException(ex), ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Fatal(GetInnerException(ex), ex);
                throw;
            }

            return 0;
        }

        public override int Delete(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    SalesPersons oSalesPersons = (SalesPersons) oCompany.GetBusinessObject(BoObjectTypes.oSalesPersons);

                    var oUnitOfWork = new UnitOfWork();
                    var reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oSalesPersons.GetByKey(Convert.ToInt32(reg.CODE));

                    if (oSalesPersons.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesPersons);
                    //oSalesPersons = null;
                }
            }
            catch (ReplicadorException ex)
            {
                _logger.Error(GetInnerException(ex), ex);

                throw;
            }
            catch (Exception ex)
            {
                _logger.Fatal(GetInnerException(ex), ex);

                throw;
            }

            return 0;
        }

        private static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return $"{ex.InnerException.Message} > {GetInnerException(ex.InnerException)} ";
            return string.Empty;
        }

        private bool Exists(KeyValuePair<int, string> registro, Company oCompany)
        {
            bool ret;

            try
            {
                SalesPersons oSalesPersons = (SalesPersons) oCompany.GetBusinessObject(BoObjectTypes.oSalesPersons);

                var oUnitOfWork = new UnitOfWork();
                var reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                ret = oSalesPersons.GetByKey(Convert.ToInt32(reg.CODE));
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesPersons);
                //oSalesPersons = null;
            }
            catch (ReplicadorException)
            {
                ret = false;
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }
    }
}
