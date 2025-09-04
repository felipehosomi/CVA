using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_Indicador
{
    public class Indicador : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public Indicador()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Indicador>();
        }

        public override string Name => "CVA_Obj_Indicador";
        
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

        public override int Create(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    if (Exists(registro, oCompany))
                        continue;

                    var sFile = $"{Path.GetTempPath()}\\Indicador.xml";
                    File.WriteAllText(sFile, registro.Value);

                    FactoringIndicators oFactoringIndicators =
                        (FactoringIndicators) oCompany.GetBusinessObjectFromXML(sFile, 0);

                    if (oFactoringIndicators.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oFactoringIndicators);
                    //oFactoringIndicators = null;
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

        public override int Update(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    var sFile = $"{Path.GetTempPath()}\\Indicador.xml";
                    File.WriteAllText(sFile, registro.Value);

                    FactoringIndicators oFactoringIndicators =
                        (FactoringIndicators) oCompany.GetBusinessObject(BoObjectTypes.oFactoringIndicators);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oFactoringIndicators.GetByKey(_reg.CODE);
                    oFactoringIndicators.Browser.ReadXml(sFile, 0);

                    if (oFactoringIndicators.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oFactoringIndicators);
                    //oFactoringIndicators = null;
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
                FactoringIndicators oFactoringIndicators =
                    (FactoringIndicators)oCompany.GetBusinessObject(BoObjectTypes.oFactoringIndicators);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                ret = oFactoringIndicators.GetByKey(_reg.CODE);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oFactoringIndicators);
                //oFactoringIndicators = null;
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

        public override int Delete(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new ReplicadorException("Objeto Indicador não pode ser removido.");
        }

        private static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return $"{ex.InnerException.Message} > {GetInnerException(ex.InnerException)} ";
            return string.Empty;
        }
    }
}