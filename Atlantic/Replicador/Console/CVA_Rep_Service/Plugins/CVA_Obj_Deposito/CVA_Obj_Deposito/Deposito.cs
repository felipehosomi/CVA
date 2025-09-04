using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CVA_Obj_Shared.Extensions;
using CVA_Obj_Shared.Interfaces;
using CVA_Obj_Shared.Sap;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;

namespace CVA_Obj_Deposito
{
    public class Deposito : IPlugin
    {
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;
        public string Name => "CVA_Obj_Deposito";
        public CVA_BAS oSource { get; set; }
        public CVA_BAS oDestination { get; set; }
        private Company oCompanySource { get; set; }
        private Company oCompanyDestination { get; set; }

        public Deposito()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Deposito>();

            try
            {
                oCompanySource = new B1Connection(oSource.UNAME, oSource.PAS, oSource.COMP, oSource.SRVR, (oSource.USE_TRU == 0), "", "", BoDataServerTypes.dst_MSSQL2014, "").oCompany;
                oCompanyDestination = new B1Connection(oDestination.UNAME, oDestination.PAS, oDestination.COMP, oDestination.SRVR, (oDestination.USE_TRU == 0), "", "", BoDataServerTypes.dst_MSSQL2014, "").oCompany;
            }
            catch (ReplicadorException ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public Deposito(CVA_BAS source, CVA_BAS destination)
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Deposito>();

            try
            {
                oSource = source;
                oDestination = destination;
                oCompanySource = new B1Connection(oSource.UNAME, oSource.PAS, oSource.COMP, oSource.SRVR, (oSource.USE_TRU == 0), "", "", BoDataServerTypes.dst_MSSQL2014, "").oCompany;
                oCompanyDestination = new B1Connection(oDestination.UNAME, oDestination.PAS, oDestination.COMP, oDestination.SRVR, (oDestination.USE_TRU == 0), "", "", BoDataServerTypes.dst_MSSQL2014, "").oCompany;
            }
            catch (ReplicadorException ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        ~Deposito()
        {
            oCompanySource.Disconnect();
            oCompanyDestination.Disconnect();
            oCompanySource = null;
            oCompanyDestination = null;
            oSource = null;
            oDestination = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public void Create(object key)
        {
            try
            {
                oCompanySource.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
                var oWarehousesSource = (Warehouses) oCompanySource.GetBusinessObject(BoObjectTypes.oWarehouses);

                if (oWarehousesSource.GetByKey(key.ToString()))
                {
                    var sXml = oWarehousesSource.GetAsXML();
                    var sFile = $"{Path.GetTempPath()}\\Deposito.xml";
                    File.WriteAllText(sFile, sXml);

                    oCompanyDestination.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

                    if(!oCompanyDestination.InTransaction)
                        oCompanyDestination.StartTransaction();

                    var oWarehousesDestination = (Warehouses) oCompanyDestination.GetBusinessObjectFromXML(sFile, 0);
                    oWarehousesDestination.UserFields.Fields.Item("U_CheckSum").Value =
                        oWarehousesSource.GetHash<HMACMD5>();

                    var bll = new CVA_REG_LOG_Repository();
                    var log = new CVA_REG_LOG();
                    var reg = new CVA_REG_Repository();
                    var obj = new CVA_OBJ_Repository();
                    log.BAS = oSource.ID;
                    log.INS = DateTime.Now;
                    var cvaReg = reg.GetAll()
                        .FirstOrDefault(
                            p =>
                                (p.STU == 3) && (p.BAS == oSource.ID) &&
                                (p.OBJ == obj.GetAll().FirstOrDefault(q => q.OBJ == Name).ID) &&
                                (p.CODE == key.ToString()));
                    if (cvaReg != null)
                        log.REG =
                            cvaReg.ID;

                    if (oWarehousesDestination.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompanyDestination.GetLastError(out errCode, out errMsg);

                        log.STU = 5;
                        log.MSG = $"Erro ao replicar Depósito {key}: {errCode} - {errMsg}";
                        bll.Add(log);

                        if(oCompanyDestination.InTransaction)
                            oCompanyDestination.EndTransaction(BoWfTransOpt.wf_RollBack);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }

                    if(oCompanyDestination.InTransaction)
                        oCompanyDestination.EndTransaction(BoWfTransOpt.wf_Commit);

                    log.STU = 4;
                    log.MSG = $"Depósito replicado: {key}";
                    bll.Add(log);
                    logger.Info($"Depósito replicado: {key}");
                }
                else
                {
                    throw new ReplicadorException($"Depósito não encontrado na base Origem: {key}.");
                }
            }
            catch (ReplicadorException ex)
            {
                logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
            }
        }

        public object RetrieveSource(object key)
        {
            var oWarehouses = (Warehouses) oCompanySource.GetBusinessObject(BoObjectTypes.oWarehouses);
            oWarehouses.GetByKey(key.ToString());
            return oWarehouses;
        }

        public object RetrieveDestination(object key)
        {
            var oWarehouses = (Warehouses)oCompanyDestination.GetBusinessObject(BoObjectTypes.oWarehouses);
            oWarehouses.GetByKey(key.ToString());
            return oWarehouses;
        }

        public void Update(object key)
        {
            try
            {
                oCompanySource.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
                var oWarehousesSource = (Warehouses)oCompanySource.GetBusinessObject(BoObjectTypes.oWarehouses);

                if (oWarehousesSource.GetByKey(key.ToString()))
                {
                    var sXml = oWarehousesSource.GetAsXML();
                    var sFile = $"{Path.GetTempPath()}\\Deposito.xml";
                    File.WriteAllText(sFile, sXml);

                    oCompanyDestination.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

                    if (!oCompanyDestination.InTransaction)
                        oCompanyDestination.StartTransaction();

                    var oWarehousesDestination = (Warehouses)oCompanyDestination.GetBusinessObjectFromXML(sFile, 0);

                    //if (oWarehousesDestination.UserFields.Fields.Item("U_CheckSum").Value.ToString() !=
                    //    oWarehousesSource.GetHash<HMACMD5>())
                    //    throw new ReplicadorException("Objeto Origem difere do Objeto Destino.");

                    var bll = new CVA_REG_LOG_Repository();
                    var log = new CVA_REG_LOG();
                    var reg = new CVA_REG_Repository();
                    var obj = new CVA_OBJ_Repository();
                    log.BAS = oSource.ID;
                    log.INS = DateTime.Now;
                    var cvaReg = reg.GetAll()
                        .FirstOrDefault(
                            p =>
                                (p.STU == 3) && (p.BAS == oSource.ID) &&
                                (p.OBJ == obj.GetAll().FirstOrDefault(q => q.OBJ == Name).ID) &&
                                (p.CODE == key.ToString()));
                    if (cvaReg != null)
                        log.REG =
                            cvaReg.ID;

                    if (oWarehousesDestination.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompanyDestination.GetLastError(out errCode, out errMsg);

                        log.STU = 5;
                        log.MSG = $"Erro ao atualizar Depósito {key}: {errCode} - {errMsg}";
                        bll.Add(log);

                        if (oCompanyDestination.InTransaction)
                            oCompanyDestination.EndTransaction(BoWfTransOpt.wf_RollBack);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }

                    if (oCompanyDestination.InTransaction)
                        oCompanyDestination.EndTransaction(BoWfTransOpt.wf_Commit);

                    log.STU = 4;
                    log.MSG = $"Depósito atualizado: {key}";
                    bll.Add(log);
                    logger.Info($"Depósito atualizado: {key}");
                }
                else
                {
                    throw new ReplicadorException($"Depósito não encontrado na base Origem: {key}.");
                }
            }
            catch (ReplicadorException ex)
            {
                logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
            }
        }

        public void Delete(object key)
        {
            try
            {
                if (!oCompanyDestination.InTransaction)
                    oCompanyDestination.StartTransaction();

                var oWarehousesDestination = (Warehouses)oCompanyDestination.GetBusinessObject(BoObjectTypes.oWarehouses);

                if (oWarehousesDestination.GetByKey(key.ToString()))
                {
                    var bll = new CVA_REG_LOG_Repository();
                    var log = new CVA_REG_LOG();
                    var reg = new CVA_REG_Repository();
                    var obj = new CVA_OBJ_Repository();
                    log.BAS = oSource.ID;
                    log.INS = DateTime.Now;
                    var cvaReg = reg.GetAll()
                        .FirstOrDefault(
                            p =>
                                (p.STU == 3) && (p.BAS == oSource.ID) &&
                                (p.OBJ == obj.GetAll().FirstOrDefault(q => q.OBJ == Name).ID) &&
                                (p.CODE == key.ToString()));
                    if (cvaReg != null)
                        log.REG =
                            cvaReg.ID;

                    if (oWarehousesDestination.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompanyDestination.GetLastError(out errCode, out errMsg);

                        log.STU = 5;
                        log.MSG = $"Erro ao remover Depósito {key}: {errCode} - {errMsg}";
                        bll.Add(log);

                        if (oCompanyDestination.InTransaction)
                            oCompanyDestination.EndTransaction(BoWfTransOpt.wf_RollBack);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }

                    if (oCompanyDestination.InTransaction)
                        oCompanyDestination.EndTransaction(BoWfTransOpt.wf_Commit);

                    log.STU = 4;
                    log.MSG = $"Depósito removido: {key}";
                    bll.Add(log);
                    logger.Info($"Depósito removido: {key}");
                }
                else
                {
                    throw new ReplicadorException($"Depósito não encontrado na base Destino: {key}.");
                }
            }
            catch (ReplicadorException ex)
            {
                logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
            }
        }
    }
}