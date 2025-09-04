using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_Utilizacao
{
    public class Utilizacao : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public Utilizacao()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Utilizacao>();
        }

        public override string Name => "CVA_Obj_Utilizacao";

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

        private bool Exists(KeyValuePair<int, UtilizacaoService> registro, Company oCompany)
        {
            var ret = false;

            try
            {
                NotaFiscalUsage oNotaFiscalUsage =
                    (NotaFiscalUsage)oCompany.GetBusinessObject(BoObjectTypes.oNotaFiscalUsage);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                ret = oNotaFiscalUsage.GetByKey(Convert.ToInt32(_reg.CODE));
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oNotaFiscalUsage);
                //oNotaFiscalUsage = null;
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

        public override int Create(Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    if (Exists(registro, oCompany))
                        continue;
                    
                    NotaFiscalUsage oNotaFiscalUsage = (NotaFiscalUsage) oCompany.GetBusinessObject(BoObjectTypes.oNotaFiscalUsage);

                    oNotaFiscalUsage.Description = registro.Value.Description;
                    oNotaFiscalUsage.IncomingImportCFOPCode = registro.Value.IncomingImportCFOPCode;
                    oNotaFiscalUsage.IncomingInStateCFOPCode = registro.Value.IncomingInStateCFOPCode;
                    oNotaFiscalUsage.IncomingOutStateCFOPCode = registro.Value.IncomingOutStateCFOPCode;
                    oNotaFiscalUsage.OutgoingExportCFOPCode = registro.Value.OutgoingExportCFOPCode;
                    oNotaFiscalUsage.OutgoingInStateCFOPCode = registro.Value.OutgoingInStateCFOPCode;
                    oNotaFiscalUsage.OutgoingOutStateCFOPCode = registro.Value.OutgoingOutStateCFOPCode;
                    oNotaFiscalUsage.ThirdParty = (BoYesNoEnum)registro.Value.ThirdParty;
                    oNotaFiscalUsage.Usage = registro.Value.Usage;
                    var i = 0;
                    foreach (var item in registro.Value.CamposUsuario)
                    {
                        if (oNotaFiscalUsage.UserFields.Fields.Item(i).Name == item.Nome)
                            oNotaFiscalUsage.UserFields.Fields.Item(i).Value = item.Valor;
                        i++;
                    }

                    if (oNotaFiscalUsage.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oNotaFiscalUsage);
                    //oNotaFiscalUsage = null;
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

        public override int Update(Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    NotaFiscalUsage oNotaFiscalUsage =
                        (NotaFiscalUsage) oCompany.GetBusinessObject(BoObjectTypes.oNotaFiscalUsage);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oNotaFiscalUsage.GetByKey(Convert.ToInt32(_reg.CODE));
                    oNotaFiscalUsage.Description = registro.Value.Description;
                    oNotaFiscalUsage.IncomingImportCFOPCode = registro.Value.IncomingImportCFOPCode;
                    oNotaFiscalUsage.IncomingInStateCFOPCode = registro.Value.IncomingInStateCFOPCode;
                    oNotaFiscalUsage.IncomingOutStateCFOPCode = registro.Value.IncomingOutStateCFOPCode;
                    oNotaFiscalUsage.OutgoingExportCFOPCode = registro.Value.OutgoingExportCFOPCode;
                    oNotaFiscalUsage.OutgoingInStateCFOPCode = registro.Value.OutgoingInStateCFOPCode;
                    oNotaFiscalUsage.OutgoingOutStateCFOPCode = registro.Value.OutgoingOutStateCFOPCode;
                    oNotaFiscalUsage.ThirdParty = (BoYesNoEnum)registro.Value.ThirdParty;
                    oNotaFiscalUsage.Usage = registro.Value.Usage;
                    var i = 0;
                    foreach (var item in registro.Value.CamposUsuario)
                    {
                        if (oNotaFiscalUsage.UserFields.Fields.Item(i).Name == item.Nome)
                            oNotaFiscalUsage.UserFields.Fields.Item(i).Value = item.Valor;
                        i++;
                    }

                    if (oNotaFiscalUsage.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oNotaFiscalUsage);
                    //oNotaFiscalUsage = null;
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

        public override int Delete(Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    NotaFiscalUsage oNotaFiscalUsage =
                        (NotaFiscalUsage) oCompany.GetBusinessObject(BoObjectTypes.oNotaFiscalUsage);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oNotaFiscalUsage.GetByKey(Convert.ToInt32(_reg.CODE));

                    if (oNotaFiscalUsage.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oNotaFiscalUsage);
                    //oNotaFiscalUsage = null;
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

        public override int Create(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new NotImplementedException();
        }

        public override int Update(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new NotImplementedException();
        }

        public override int Delete(Dictionary<int, string> listaRegistros, Company oCompany)
        {
            throw new NotImplementedException();
        }

        private static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return $"{ex.InnerException.Message} > {GetInnerException(ex.InnerException)} ";
            return string.Empty;
        }
    }
}