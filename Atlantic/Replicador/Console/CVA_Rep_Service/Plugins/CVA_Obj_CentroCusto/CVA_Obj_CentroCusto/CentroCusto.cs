using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_CentroCusto
{
    public class CentroCusto : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public CentroCusto()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<CentroCusto>();
        }

        public override string Name => "CVA_Obj_CentroCusto";

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

        public override int Create(Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var reg in listaRegistros)
                {
                    if (Exists(reg, oCompany))
                        continue;

                    CompanyService oCompanyService = oCompany.GetCompanyService();
                    ProfitCentersService oProfitCenterService =
                        (ProfitCentersService)
                            oCompanyService.GetBusinessService(ServiceTypes.ProfitCentersService);

                    ProfitCenter oProfitCenter =
                        (ProfitCenter)
                            oProfitCenterService.GetDataInterface(
                                ProfitCentersServiceDataInterfaces.pcsProfitCenter);

                    oProfitCenter.CenterCode = reg.Value.FactorCode;
                    oProfitCenter.CenterName = reg.Value.FactorDescription;
                    oProfitCenter.Active = reg.Value.Active;
                    oProfitCenter.GroupCode = reg.Value.GroupCode;
                    oProfitCenter.InWhichDimension = reg.Value.InWhichDimension;
                    oProfitCenter.CostCenterType = reg.Value.CostCenterType;
                    oProfitCenter.EffectiveTo = reg.Value.EffectiveTo;
                    oProfitCenter.Effectivefrom = reg.Value.EffectiveFrom;

                    oProfitCenterService.AddProfitCenter(oProfitCenter);

                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenter);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenterService);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompanyService);
                    //oCompanyService = null;
                    //oProfitCenterService = null;
                    //oProfitCenter = null;
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

        public override int Update(Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var reg in listaRegistros)
                {
                    CompanyService oCompanyService = oCompany.GetCompanyService();
                    ProfitCentersService oProfitCentersService =
                        (ProfitCentersService)
                            oCompanyService.GetBusinessService(ServiceTypes.ProfitCentersService);
                    ProfitCenterParams oProfitCentersParams =
                        (ProfitCenterParams)
                            oProfitCentersService.GetDataInterface(
                                ProfitCentersServiceDataInterfaces.pcsProfitCenterParams);
                    oProfitCentersParams.CenterCode = reg.Value.FactorCode;
                    ProfitCenter oProfitCenter =
                        oProfitCentersService.GetProfitCenter(oProfitCentersParams);

                    oProfitCenter.CenterCode = reg.Value.FactorCode;
                    oProfitCenter.CenterName = reg.Value.FactorDescription;
                    oProfitCenter.Active = reg.Value.Active;
                    oProfitCenter.GroupCode = reg.Value.GroupCode;
                    oProfitCenter.InWhichDimension = reg.Value.InWhichDimension;
                    oProfitCenter.CostCenterType = reg.Value.CostCenterType;
                    oProfitCenter.EffectiveTo = reg.Value.EffectiveTo;
                    oProfitCenter.Effectivefrom = reg.Value.EffectiveFrom;

                    oProfitCentersService.UpdateProfitCenter(oProfitCenter);

                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenter);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersParams);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersService);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompanyService);
                    //oCompanyService = null;
                    //oProfitCentersService = null;
                    //oProfitCentersParams = null;
                    //oProfitCenter = null;

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

        private int Update(KeyValuePair<int, CentroCustoService> reg, Company oCompany)
        {
            try
            {
                CompanyService oCompanyService = oCompany.GetCompanyService();
                ProfitCentersService oProfitCentersService =
                    (ProfitCentersService)
                        oCompanyService.GetBusinessService(ServiceTypes.ProfitCentersService);
                ProfitCenterParams oProfitCentersParams =
                    (ProfitCenterParams)
                        oProfitCentersService.GetDataInterface(
                            ProfitCentersServiceDataInterfaces.pcsProfitCenterParams);
                oProfitCentersParams.CenterCode = reg.Value.FactorCode;
                ProfitCenter oProfitCenter =
                    oProfitCentersService.GetProfitCenter(oProfitCentersParams);

                oProfitCenter.CenterCode = reg.Value.FactorCode;
                oProfitCenter.CenterName = reg.Value.FactorDescription;
                oProfitCenter.Active = reg.Value.Active;
                oProfitCenter.GroupCode = reg.Value.GroupCode;
                oProfitCenter.InWhichDimension = reg.Value.InWhichDimension;
                oProfitCenter.CostCenterType = reg.Value.CostCenterType;
                oProfitCenter.EffectiveTo = reg.Value.EffectiveTo;
                oProfitCenter.Effectivefrom = reg.Value.EffectiveFrom;

                oProfitCentersService.UpdateProfitCenter(oProfitCenter);

                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenter);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersParams);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersService);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompanyService);
                //oCompanyService = null;
                //oProfitCentersService = null;
                //oProfitCentersParams = null;
                //oProfitCenter = null;
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

        private bool Exists(KeyValuePair<int, CentroCustoService> reg, Company oCompany)
        {
            var ret = false;

            try
            {
                CompanyService oCompanyService = oCompany.GetCompanyService();
                ProfitCentersService oProfitCentersService =
                    (ProfitCentersService)
                        oCompanyService.GetBusinessService(ServiceTypes.ProfitCentersService);
                ProfitCenterParams oProfitCentersParams =
                    (ProfitCenterParams)
                        oProfitCentersService.GetDataInterface(
                            ProfitCentersServiceDataInterfaces.pcsProfitCenterParams);
                oProfitCentersParams.CenterCode = reg.Value.FactorCode;
                ProfitCenter oProfitCenter =
                    oProfitCentersService.GetProfitCenter(oProfitCentersParams);

                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenter);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersParams);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersService);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompanyService);
                //oCompanyService = null;
                //oProfitCentersService = null;
                //oProfitCentersParams = null;
                //oProfitCenter = null;

                ret = true;
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
            throw new NotImplementedException();
        }

        public override int Delete(Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var reg in listaRegistros)
                {
                    CompanyService oCompanyService = oCompany.GetCompanyService();
                    ProfitCentersService oProfitCentersService =
                        (ProfitCentersService)
                            oCompanyService.GetBusinessService(ServiceTypes.ProfitCentersService);
                    ProfitCenterParams oProfitCentersParams =
                        (ProfitCenterParams)
                            oProfitCentersService.GetDataInterface(
                                ProfitCentersServiceDataInterfaces.pcsProfitCenterParams);
                    oProfitCentersParams.CenterCode = reg.Value.FactorCode;

                    oProfitCentersService.DeleteProfitCenter(oProfitCentersParams);

                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersParams);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersService);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompanyService);
                    //oCompanyService = null;
                    //oProfitCentersService = null;
                    //oProfitCentersParams = null;

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