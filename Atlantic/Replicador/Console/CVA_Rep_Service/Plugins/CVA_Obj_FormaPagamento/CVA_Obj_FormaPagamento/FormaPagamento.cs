using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_FormaPagamento
{
    public class FormaPagamento : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public FormaPagamento()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<FormaPagamento>();
        }

        public override string Name => "CVA_Obj_FormaPagamento";

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
                    if(Exists(reg, oCompany))
                    {
                        //Update(reg, oCompany);
                        continue;
                    }

                    var sFile = $"{Path.GetTempPath()}\\FormaPagamento.xml";
                    File.WriteAllText(sFile, reg.Value);

                    WizardPaymentMethods oWizardPaymentMethods =
                        (WizardPaymentMethods) oCompany.GetBusinessObjectFromXML(sFile, 0);

                    if (oWizardPaymentMethods.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWizardPaymentMethods);
                    //oWizardPaymentMethods = null;
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
                foreach (var reg in listaRegistros)
                {
                    var sFile = $"{Path.GetTempPath()}\\FormaPagamento.xml";
                    File.WriteAllText(sFile, reg.Value);

                    WizardPaymentMethods oWizardPaymentMethods =
                        (WizardPaymentMethods) oCompany.GetBusinessObject(BoObjectTypes.oWizardPaymentMethods);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                    oWizardPaymentMethods.GetByKey(_reg.CODE);
                    oWizardPaymentMethods.Browser.ReadXml(sFile, 0);

                    if (oWizardPaymentMethods.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWizardPaymentMethods);
                    //oWizardPaymentMethods = null;
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

        private int Update(KeyValuePair<int, string> reg, Company oCompany)
        {
            try
            {
                var sFile = $"{Path.GetTempPath()}\\FormaPagamento.xml";
                File.WriteAllText(sFile, reg.Value);

                WizardPaymentMethods oWizardPaymentMethods =
                    (WizardPaymentMethods)oCompany.GetBusinessObject(BoObjectTypes.oWizardPaymentMethods);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                oWizardPaymentMethods.GetByKey(_reg.CODE);
                oWizardPaymentMethods.Browser.ReadXml(sFile, 0);

                if (oWizardPaymentMethods.Update() != 0)
                {
                    int errCode;
                    string errMsg;

                    oCompany.GetLastError(out errCode, out errMsg);

                    throw new ReplicadorException($"{errCode} - {errMsg}");
                }
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWizardPaymentMethods);
                //oWizardPaymentMethods = null;
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
                WizardPaymentMethods oWizardPaymentMethods =
                    (WizardPaymentMethods)oCompany.GetBusinessObject(BoObjectTypes.oWizardPaymentMethods);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                ret = oWizardPaymentMethods.GetByKey(_reg.CODE);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWizardPaymentMethods);
                //oWizardPaymentMethods = null;
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
                foreach (var reg in listaRegistros)
                {
                    WizardPaymentMethods oWizardPaymentMethods =
                        (WizardPaymentMethods) oCompany.GetBusinessObject(BoObjectTypes.oWizardPaymentMethods);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(reg.Key);

                    oWizardPaymentMethods.GetByKey(_reg.CODE);

                    if (oWizardPaymentMethods.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWizardPaymentMethods);
                    //oWizardPaymentMethods = null;

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