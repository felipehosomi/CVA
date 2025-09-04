using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_CondicaoPagamento
{
    public class CondicaoPagamento : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public CondicaoPagamento()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<CondicaoPagamento>();
        }

        public override string Name => "CVA_Obj_CondicaoPagamento";

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
                foreach (var reg in listaRegistros)
                {
                    if (Exists(reg, oCompany))
                        continue;

                    var sFile = $"{Path.GetTempPath()}\\CondicaoPagamento.xml";
                    File.WriteAllText(sFile, reg.Value);

                    PaymentTermsTypes oPaymentTermsTypes =
                        (PaymentTermsTypes) oCompany.GetBusinessObjectFromXML(sFile, 0);

                    if (oPaymentTermsTypes.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oPaymentTermsTypes);
                    //oPaymentTermsTypes = null;
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
                oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

                foreach (var reg in listaRegistros)
                {
                    var sFile = $"{Path.GetTempPath()}\\CondicaoPagamento.xml";
                    File.WriteAllText(sFile, reg.Value);

                    PaymentTermsTypes oPaymentTermsTypes =
                        (PaymentTermsTypes) oCompany.GetBusinessObject(BoObjectTypes.oPaymentTermsTypes);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                    oPaymentTermsTypes.GetByKey(Convert.ToInt32(_reg.CODE));
                    oPaymentTermsTypes.Browser.ReadXml(sFile, 0);

                    if (oPaymentTermsTypes.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oPaymentTermsTypes);
                    //oPaymentTermsTypes = null;
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

        private bool Exists(KeyValuePair<int, string> reg, Company oCompany)
        {
            var ret = false;

            try
            {
                oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

                PaymentTermsTypes oPaymentTermsTypes =
                    (PaymentTermsTypes)oCompany.GetBusinessObject(BoObjectTypes.oPaymentTermsTypes);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                ret = oPaymentTermsTypes.GetByKey(Convert.ToInt32(_reg.CODE));
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oPaymentTermsTypes);
                //oPaymentTermsTypes = null;
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
            try
            {
                oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

                foreach (var reg in listaRegistros)
                {
                    PaymentTermsTypes oPaymentTermsTypes =
                        (PaymentTermsTypes) oCompany.GetBusinessObject(BoObjectTypes.oPaymentTermsTypes);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                    oPaymentTermsTypes.GetByKey(Convert.ToInt32(_reg.CODE));

                    if (oPaymentTermsTypes.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oPaymentTermsTypes);
                    //oPaymentTermsTypes = null;
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

        private static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return $"{ex.InnerException.Message} > {GetInnerException(ex.InnerException)} ";
            return string.Empty;
        }
    }
}