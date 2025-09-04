using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_Imposto
{
    public class Imposto : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public Imposto()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Imposto>();
        }

        public override string Name => "CVA_Obj_Imposto";

        public override int Create(Dictionary<int, string> listaCodigosImposto,
            Dictionary<int, string> listaTiposImposto, Dictionary<string, string> listaAliquotasImposto,
            Company oCompany)
        {
            try
            {
                foreach (var registro in listaTiposImposto)
                {
                    var sFile = $"{Path.GetTempPath()}\\TipoImposto.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesTaxAuthoritiesTypes oSalesTaxAuthoritiesTypes =
                        (SalesTaxAuthoritiesTypes) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxAuthoritiesTypes);

                    if (!oSalesTaxAuthoritiesTypes.GetByKey(registro.Key))
                    {
                        oSalesTaxAuthoritiesTypes =
                            (SalesTaxAuthoritiesTypes) oCompany.GetBusinessObjectFromXML(sFile, 0);

                        if (oSalesTaxAuthoritiesTypes.Add() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    else
                    {
                        oSalesTaxAuthoritiesTypes.Browser.ReadXml(sFile, 0);

                        if (oSalesTaxAuthoritiesTypes.Update() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxAuthoritiesTypes);
                    //oSalesTaxAuthoritiesTypes = null;
                }

                foreach (var registro in listaAliquotasImposto)
                {
                    var sFile = $"{Path.GetTempPath()}\\AliquotaImposto.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesTaxAuthorities oSalesTaxAuthorities =
                        (SalesTaxAuthorities) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxAuthorities);

                    var s = registro.Key.Split('|');

                    if (!oSalesTaxAuthorities.GetByKey(s[0], Convert.ToInt32(s[1])))
                    {
                        oSalesTaxAuthorities =
                            (SalesTaxAuthorities) oCompany.GetBusinessObjectFromXML(sFile, 0);

                        if (oSalesTaxAuthorities.Add() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    else
                    {
                        oSalesTaxAuthorities.Browser.ReadXml(sFile, 0);

                        if (oSalesTaxAuthorities.Update() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxAuthorities);
                    //oSalesTaxAuthorities = null;
                }

                foreach (var registro in listaCodigosImposto)
                {
                    if (Exists(registro, oCompany))
                        continue;

                    var sFile = $"{Path.GetTempPath()}\\CodigoImposto.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesTaxCodes oSalesTaxCodes =
                        (SalesTaxCodes) oCompany.GetBusinessObjectFromXML(sFile, 0);

                    if (oSalesTaxCodes.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxCodes);
                    //oSalesTaxCodes = null;
                }
            }
            catch (ReplicadorException ex)
            {
                logger.Error(GetInnerException(ex), ex);

                throw;
            }
            catch (Exception ex)
            {
                logger.Fatal(GetInnerException(ex), ex);

                throw;
            }

            return 0;
        }

        public override int Update(Dictionary<int, string> listaCodigosImposto,
            Dictionary<int, string> listaTiposImposto, Dictionary<string, string> listaAliquotasImposto,
            Company oCompany)
        {
            try
            {
                foreach (var registro in listaTiposImposto)
                {
                    var sFile = $"{Path.GetTempPath()}\\TipoImposto.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesTaxAuthoritiesTypes oSalesTaxAuthoritiesTypes =
                        (SalesTaxAuthoritiesTypes) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxAuthoritiesTypes);

                    if (!oSalesTaxAuthoritiesTypes.GetByKey(registro.Key))
                    {
                        oSalesTaxAuthoritiesTypes =
                            (SalesTaxAuthoritiesTypes) oCompany.GetBusinessObjectFromXML(sFile, 0);

                        if (oSalesTaxAuthoritiesTypes.Add() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    else
                    {
                        oSalesTaxAuthoritiesTypes.Browser.ReadXml(sFile, 0);

                        if (oSalesTaxAuthoritiesTypes.Update() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxAuthoritiesTypes);
                    //oSalesTaxAuthoritiesTypes = null;
                }

                foreach (var registro in listaAliquotasImposto)
                {
                    var sFile = $"{Path.GetTempPath()}\\AliquotaImposto.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesTaxAuthorities oSalesTaxAuthorities =
                        (SalesTaxAuthorities) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxAuthorities);

                    var s = registro.Key.Split('|');

                    if (!oSalesTaxAuthorities.GetByKey(s[0], Convert.ToInt32(s[1])))
                    {
                        oSalesTaxAuthorities =
                            (SalesTaxAuthorities) oCompany.GetBusinessObjectFromXML(sFile, 0);

                        if (oSalesTaxAuthorities.Add() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    else
                    {
                        oSalesTaxAuthorities.Browser.ReadXml(sFile, 0);

                        if (oSalesTaxAuthorities.Update() != 0)
                        {
                            int errCode;
                            string errMsg;

                            oCompany.GetLastError(out errCode, out errMsg);

                            throw new ReplicadorException($"{errCode} - {errMsg}");
                        }
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxAuthorities);
                    //oSalesTaxAuthorities = null;
                }

                foreach (var registro in listaCodigosImposto)
                {
                    var sFile = $"{Path.GetTempPath()}\\CodigoImposto.xml";
                    File.WriteAllText(sFile, registro.Value);

                    SalesTaxCodes oSalesTaxCodes = (SalesTaxCodes) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxCodes);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oSalesTaxCodes.GetByKey(_reg.CODE);
                    oSalesTaxCodes.Browser.ReadXml(sFile, 0);

                    if (oSalesTaxCodes.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxCodes);
                    //oSalesTaxCodes = null;
                }
            }
            catch (ReplicadorException ex)
            {
                logger.Error(GetInnerException(ex), ex);

                throw;
            }
            catch (Exception ex)
            {
                logger.Fatal(GetInnerException(ex), ex);

                throw;
            }

            return 0;
        }

        private bool Exists(KeyValuePair<int, string> registro, Company oCompany)
        {
            var ret = false;

            try
            {
                SalesTaxCodes oSalesTaxCodes = (SalesTaxCodes)oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxCodes);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                ret = oSalesTaxCodes.GetByKey(_reg.CODE);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxCodes);
                //oSalesTaxCodes = null;
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

        public override int Delete(Dictionary<int, string> listaCodigosImposto,
            Dictionary<int, string> listaTiposImposto, Dictionary<string, string> listaAliquotasImposto,
            Company oCompany)
        {
            throw new NotImplementedException();
        }

        public override int Delete(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new ReplicadorException("Objetos de Imposto nãp permitem exclusão.");
        }

        public override int Create(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new NotImplementedException();
        }

        public override int Update(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override void Dispose()
        {
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }

                disposed = true;
            }
        }

        private static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return $"{ex.InnerException.Message} > {GetInnerException(ex.InnerException)} ";
            return string.Empty;
        }
    }
}