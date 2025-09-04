using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_PlanoContas.DAO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_PlanoContas
{
    public class PlanoContas : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public PlanoContas()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<PlanoContas>();
        }


        public override string Name => "CVA_Obj_PlanoContas";

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

                    var sFile = $"{Path.GetTempPath()}\\PlanoContas.xml";
                    File.WriteAllText(sFile, registro.Value);

                    ChartOfAccounts oChardOfAccounts =
                        (ChartOfAccounts) oCompany.GetBusinessObjectFromXML(sFile, 0);

                    if (oChardOfAccounts.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oChardOfAccounts);
                    //oChardOfAccounts = null;
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
                Recordset rst = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                rst.DoQuery(SQL.Date_GetNullValue);

                foreach (var registro in listaRegistros)
                {
                    var sFile = $"{Path.GetTempPath()}\\PlanoContas.xml";
                    File.WriteAllText(sFile, registro.Value);

                    //System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    //xmlDoc.Load(xmlFile);
                    //xmlDoc.SelectSingleNode("xml/stockgroup/name.list/name").InnerText = "New Value";
                    //xmlDoc.Save(xmlFile);
                    ChartOfAccounts oChartOfAccounts = (ChartOfAccounts) oCompany.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
                    
                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oChartOfAccounts.GetByKey(_reg.CODE);
                    BoYesNoEnum blockManualPosting = oChartOfAccounts.BlockManualPosting;

                    BoYesNoEnum validFor = oChartOfAccounts.ValidFor;
                    DateTime validFrom = oChartOfAccounts.ValidFrom;
                    DateTime validTo = oChartOfAccounts.ValidTo;
                    string validRemarks = oChartOfAccounts.ValidRemarks;

                    BoYesNoEnum frozenFor = oChartOfAccounts.FrozenFor;
                    DateTime frozenFrom = oChartOfAccounts.FrozenFrom;
                    DateTime frozenTo = oChartOfAccounts.FrozenTo;
                    string frozenRemarks = oChartOfAccounts.FrozenRemarks;

                    oChartOfAccounts.Browser.ReadXml(sFile, 0);

                    oChartOfAccounts.BlockManualPosting = blockManualPosting;
                    oChartOfAccounts.ValidFor = validFor;
                    if (validFrom.Year > 1899)
                    {
                        oChartOfAccounts.ValidFrom = validFrom;
                    }
                    else
                    {

                        oChartOfAccounts.ValidFrom = rst.Fields.Item(0).Value;
                    }
                    if (validTo.Year > 1899)
                    {
                        oChartOfAccounts.ValidTo = validTo;
                    }
                    else
                    {
                        oChartOfAccounts.ValidTo = rst.Fields.Item(0).Value;
                    }
                    oChartOfAccounts.ValidRemarks = validRemarks;

                    oChartOfAccounts.FrozenFor = frozenFor;
                    if (frozenFrom.Year > 1899)
                    {
                        oChartOfAccounts.FrozenFrom = frozenFrom;
                    }
                    else
                    {
                        oChartOfAccounts.FrozenFrom = rst.Fields.Item(0).Value;
                    }
                    if (frozenTo.Year > 1899)
                    {
                        oChartOfAccounts.FrozenTo = frozenTo;
                    }
                    else
                    {
                        oChartOfAccounts.FrozenTo = rst.Fields.Item(0).Value;
                    }
                    oChartOfAccounts.FrozenRemarks = frozenRemarks;

                    if (oChartOfAccounts.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oChartOfAccounts);
                    //oChartOfAccounts = null;
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
                ChartOfAccounts oChartOfAccounts =
                    (ChartOfAccounts)oCompany.GetBusinessObject(BoObjectTypes.oChartOfAccounts);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                ret = oChartOfAccounts.GetByKey(_reg.CODE);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oChartOfAccounts);
                //oChartOfAccounts = null;
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
                foreach (var registro in listaRegistros)
                {
                    ChartOfAccounts oChartOfAccounts =
                        (ChartOfAccounts) oCompany.GetBusinessObject(BoObjectTypes.oChartOfAccounts);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oChartOfAccounts.GetByKey(_reg.CODE);

                    if (oChartOfAccounts.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oChartOfAccounts);
                    //oChartOfAccounts = null;
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