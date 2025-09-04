using System;
using System.Collections.Generic;
using System.Linq;
using CVA_Obj_Shared.Interfaces;
using CVA_Obj_Shared.Sap;
using CVA_Rep_DAL;
using SAPbobsCOM;
using System.Xml;

namespace CVA_Rep_Service
{
    public class OrigemBLL : IDisposable
    {
        private bool disposed;
        private Company oCompany;

        public OrigemBLL(CVA_BAS _base)
        {
            if (_base.DB_TYP != null)
            {
                var conn = B1Connection.Instance;
                oCompany = conn.Connect(_base.UNAME, _base.PAS, _base.COMP, _base.SRVR, false, _base.DB_UNAME,
                    _base.DB_PAS, (BoDataServerTypes)_base.DB_TYP, _base.DB_SRVR);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateCentroCusto(int cvaRegId, ref Dictionary<int, CentroCustoService> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach(var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetCentroCusto(cvaReg.CODE, cvaReg.FUNC));
            }
        }

        public void CreateCondicaoPagamento(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetCondicaoPagamento(cvaReg.CODE));
            }
        }

        public void CreateContasBancarias(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetContasBancarias(cvaReg.CODE));
            }
        }

        public void CreateFormaPagamento(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetFormaPagamento(cvaReg.CODE));
            }
        }

        public void CreateGrupoItem(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetGrupoItem(cvaReg.CODE));
            }
        }

        public void CreateGrupoParceiroNegocio(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetGrupoParceiroNegocio(cvaReg.CODE));
            }
        }

        public void CreateTipoImposto(ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            GetTipoImposto(ref dict);
        }

        public void CreateAliquotaImposto(ref Dictionary<string, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            GetAliquotaImposto(ref dict);
        }

        public void CreateCodigoImposto(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetCodigoImposto(cvaReg.CODE));
            }
        }

        public void CreateIndicador(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetIndicador(cvaReg.CODE));
            }
        }

        public void CreateItem(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetItem(cvaReg.CODE));
            }
        }

        public void CreateParceiroNegocio(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetParceiroNegocio(cvaReg.CODE));
            }
        }

        public void CreatePlanoContas(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetPlanoContas(cvaReg.CODE));
            }
        }

        public void CreateUsuario(int cvaRegId, ref Dictionary<int, UsuarioService> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetUsuario(cvaReg.CODE));
            }
        }

        public void CreateUtilizacao(int cvaRegId, ref Dictionary<int, UtilizacaoService> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetUtilizacao(cvaReg.CODE));
            }
        }

        public void CreateVendedoresCompradores(int cvaRegId, ref Dictionary<int, string> dict)
        {
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

            var oUnitOfWork = new UnitOfWork();
            var reg = oUnitOfWork.CvaRegRepository.Get(r => r.ID == cvaRegId).ToArray();
            foreach (var cvaReg in reg)
            {
                dict.Add(cvaReg.ID, GetVendedoresCompradores(cvaReg.CODE));
            }
        }

        private CentroCustoService GetCentroCusto(object key, int? func)
        {
            var oUnitOfWork = new UnitOfWork();
            var cvaFunc = oUnitOfWork.CvaFuncRepository.GetByID(Convert.ToInt32(func));

            if (cvaFunc.FUNC != "D")
            {
                CompanyService oCompanyService = oCompany.GetCompanyService();
                ProfitCentersService oProfitCenterSerivce =
                    (ProfitCentersService)
                        oCompanyService.GetBusinessService(ServiceTypes.ProfitCentersService);
                ProfitCenterParams oProfitCentersParams =
                    (ProfitCenterParams)
                        oProfitCenterSerivce.GetDataInterface(
                            ProfitCentersServiceDataInterfaces.pcsProfitCenterParams);
                oProfitCentersParams.CenterCode = key.ToString();
                ProfitCenter oProfitCenter =
                    oProfitCenterSerivce.GetProfitCenter(oProfitCentersParams);

                var centroCusto = new CentroCustoService
                {
                    Active = oProfitCenter.Active,
                    FactorCode = oProfitCenter.CenterCode,
                    FactorDescription = oProfitCenter.CenterName,
                    InWhichDimension = oProfitCenter.InWhichDimension,
                    EffectiveTo = oProfitCenter.EffectiveTo,
                    EffectiveFrom = oProfitCenter.Effectivefrom,
                    GroupCode = oProfitCenter.GroupCode,
                    CostCenterType = oProfitCenter.CostCenterType
                };

                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenter);
                //oProfitCenter = null;
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCentersParams);
                //oProfitCentersParams = null;
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oProfitCenterSerivce);
                //oProfitCenterSerivce = null;
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompanyService);
                //oCompanyService = null;

                return centroCusto;
            }
            else
            {
                var centroCusto = new CentroCustoService
                {
                    FactorCode = key.ToString()
                };

                return centroCusto;
            }
        }

        private string GetCondicaoPagamento(object key)
        {
            var sXml = string.Empty;
            PaymentTermsTypes oPaymentTermsType =
                (PaymentTermsTypes) oCompany.GetBusinessObject(BoObjectTypes.oPaymentTermsTypes);

            if (oPaymentTermsType.GetByKey(Convert.ToInt32(key)))
            {
                sXml = oPaymentTermsType.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oPaymentTermsType);
            //oPaymentTermsType = null;

            return sXml;
        }

        private string GetContasBancarias(object key)
        {
            var sXml = string.Empty;
            HouseBankAccounts oHouseBankAccounts =
                (HouseBankAccounts)oCompany.GetBusinessObject(BoObjectTypes.oHouseBankAccounts);

            if (oHouseBankAccounts.GetByKey(Convert.ToInt32(key)))
            {
                sXml = oHouseBankAccounts.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oHouseBankAccounts);
            //oHouseBankAccounts = null;

            return sXml;
        }

        private string GetFormaPagamento(object key)
        {
            var sXml = string.Empty;
            WizardPaymentMethods oWizardPaymentMethods =
                (WizardPaymentMethods) oCompany.GetBusinessObject(BoObjectTypes.oWizardPaymentMethods);

            if (oWizardPaymentMethods.GetByKey(key.ToString()))
            {
                sXml = oWizardPaymentMethods.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWizardPaymentMethods);
            //oWizardPaymentMethods = null;

            return sXml;
        }

        private string GetGrupoItem(object key)
        {
            var sXml = string.Empty;
            ItemGroups oItemGroups =
                (ItemGroups) oCompany.GetBusinessObject(BoObjectTypes.oItemGroups);

            if (oItemGroups.GetByKey(Convert.ToInt32(key)))
            {
                sXml = oItemGroups.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oItemGroups);
            //oItemGroups = null;

            return sXml;
        }

        private string GetGrupoParceiroNegocio(object key)
        {
            var sXml = string.Empty;
            BusinessPartnerGroups oBusinessPartnersGroup =
                (BusinessPartnerGroups) oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartnerGroups);

            if (oBusinessPartnersGroup.GetByKey(Convert.ToInt32(key)))
            {
                sXml = oBusinessPartnersGroup.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartnersGroup);
            //oBusinessPartnersGroup = null;

            return sXml;
        }

        private void GetTipoImposto(ref Dictionary<int, string> dict)
        {
            SalesTaxAuthoritiesTypes oSalesTaxAuthoritiesTypes =
                (SalesTaxAuthoritiesTypes) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxAuthoritiesTypes);
            Recordset oRecordset = (Recordset) oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT AbsId FROM OSTT");

            oSalesTaxAuthoritiesTypes.Browser.Recordset = oRecordset;

            if (oSalesTaxAuthoritiesTypes.Browser.RecordCount > 0)
            {
                while (!oSalesTaxAuthoritiesTypes.Browser.EoF)
                {
                    var key = oSalesTaxAuthoritiesTypes.Numerator;

                    var sXml = oSalesTaxAuthoritiesTypes.GetAsXML();

                    dict.Add(key, sXml);

                    oSalesTaxAuthoritiesTypes.Browser.MoveNext();
                }
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxAuthoritiesTypes);
            //oSalesTaxAuthoritiesTypes = null;
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oRecordset);
            //oRecordset = null;
        }

        private void GetAliquotaImposto(ref Dictionary<string, string> dict)
        {
            SalesTaxAuthorities oSalesTaxAuthorities =
                (SalesTaxAuthorities) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxAuthorities);
            Recordset oRecordset = (Recordset) oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT Code FROM OSTA");

            oSalesTaxAuthorities.Browser.Recordset = oRecordset;

            if (oSalesTaxAuthorities.Browser.RecordCount > 0)
            {
                while (!oSalesTaxAuthorities.Browser.EoF)
                {
                    var key = oSalesTaxAuthorities.Code + "|" + oSalesTaxAuthorities.Type;

                    var sXml = oSalesTaxAuthorities.GetAsXML();

                    dict.Add(key, sXml);

                    oSalesTaxAuthorities.Browser.MoveNext();
                }
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxAuthorities);
            //oSalesTaxAuthorities = null;
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oRecordset);
            //oRecordset = null;
        }

        private string GetCodigoImposto(object key)
        {
            var sXml = string.Empty;

            SalesTaxCodes oSalesTaxCodes = (SalesTaxCodes) oCompany.GetBusinessObject(BoObjectTypes.oSalesTaxCodes);

            if (oSalesTaxCodes.GetByKey(key.ToString()))
            {
                sXml = oSalesTaxCodes.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesTaxCodes);
            //oSalesTaxCodes = null;

            return sXml;
        }

        private string GetIndicador(object key)
        {
            var sXml = string.Empty;
            FactoringIndicators oFactoringIndicator =
                (FactoringIndicators) oCompany.GetBusinessObject(BoObjectTypes.oFactoringIndicators);

            if (oFactoringIndicator.GetByKey(key.ToString()))
            {
                sXml = oFactoringIndicator.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oFactoringIndicator);
            //oFactoringIndicator = null;

            return sXml;
        }

        private string GetItem(object key)
        {
            var sXml = string.Empty;
            Items oItems = (Items) oCompany.GetBusinessObject(BoObjectTypes.oItems);

            if (oItems.GetByKey(key.ToString()))
            {
                sXml = oItems.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oItems);
            //oItems = null;

            return sXml;
        }

        private string GetParceiroNegocio(object key)
        {
            var sXml = string.Empty;
            BusinessPartners oBusinessPartners = (BusinessPartners) oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            if (oBusinessPartners.GetByKey(key.ToString()))
            {
                sXml = oBusinessPartners.GetAsXML();
                AtualizaPnFatherCard(ref sXml, oBusinessPartners.FatherCard);
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartners);
            //oBusinessPartners = null;

            return sXml;
        }

        private string GetPlanoContas(object key)
        {
            var sXml = string.Empty;
            ChartOfAccounts oChartOfAccounts = (ChartOfAccounts) oCompany.GetBusinessObject(BoObjectTypes.oChartOfAccounts);

            if (oChartOfAccounts.GetByKey(key.ToString()))
            {
                sXml = oChartOfAccounts.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oChartOfAccounts);
            //oChartOfAccounts = null;

            return sXml;
        }

        private UsuarioService GetUsuario(object key)
        {
            var usuario = new UsuarioService();
            Users oUsers = (Users) oCompany.GetBusinessObject(BoObjectTypes.oUsers);
            var userid = 0;
            var passwd = string.Empty;
            string[] sKey;

            if (key.ToString().Contains("|"))
            {
                sKey = key.ToString().Split('|');
                if (sKey.Length >= 2)
                {
                    userid = Convert.ToInt32(sKey[0]);
                    passwd = sKey[1];
                }
            }
            else
                userid = Convert.ToInt32(key);
            
            if (oUsers.GetByKey(Convert.ToInt32(userid)))
            {
                usuario.Branch = oUsers.Branch;
                usuario.CashLimit = (int)oUsers.CashLimit;
                usuario.Defaults = oUsers.Defaults;
                usuario.Department = oUsers.Department;
                usuario.eMail = oUsers.eMail;
                usuario.FaxNumber = oUsers.FaxNumber;
                usuario.Locked = (int)oUsers.Locked;
                usuario.MaxCashAmtForIncmngPayts = oUsers.MaxCashAmtForIncmngPayts;
                usuario.MaxDiscountGeneral = oUsers.MaxDiscountGeneral;
                usuario.MaxDiscountPurchase = oUsers.MaxDiscountPurchase;
                usuario.MaxDiscountSales = oUsers.MaxDiscountSales;
                usuario.MobilePhoneNumber = oUsers.MobilePhoneNumber;
                usuario.Superuser = (int)oUsers.Superuser;

                for (var i = 0; i < oUsers.UserBranchAssignment.Count; i++)
                {
                    oUsers.UserBranchAssignment.SetCurrentLine(i);
                    var branch = new FiliaisUsuarioService();
                    branch.BPLID = oUsers.UserBranchAssignment.BPLID;
                    usuario.UserBranchAssignment.Add(branch);
                }

                usuario.UserCode = oUsers.UserCode;
                usuario.UserName = oUsers.UserName;
                usuario.UserPassword = passwd;

                for (var i = 0; i < oUsers.UserPermission.Count; i++)
                {
                    oUsers.UserPermission.SetCurrentLine(i);
                    var permission = new PermissoesUsuarioService();
                    permission.Permission = (int)oUsers.UserPermission.Permission;
                    permission.PermissionID = oUsers.UserPermission.PermissionID;
                    usuario.UserPermission.Add(permission);
                }

                if (!string.IsNullOrEmpty(passwd))
                {
                    oUsers.UserPassword = passwd;
                    oUsers.Update();
                }
                

            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oUsers);
            //oUsers = null;

            return usuario;
        }

        private UtilizacaoService GetUtilizacao(object key)
        {
            var utilizacao = new UtilizacaoService();
            NotaFiscalUsage oNotaFiscalUsage = (NotaFiscalUsage) oCompany.GetBusinessObject(BoObjectTypes.oNotaFiscalUsage);

            if (oNotaFiscalUsage.GetByKey(Convert.ToInt32(key)))
            {
                utilizacao.Description = oNotaFiscalUsage.Description;
                utilizacao.IncomingImportCFOPCode = oNotaFiscalUsage.IncomingImportCFOPCode;
                utilizacao.IncomingInStateCFOPCode = oNotaFiscalUsage.IncomingInStateCFOPCode;
                utilizacao.IncomingOutStateCFOPCode = oNotaFiscalUsage.IncomingOutStateCFOPCode;
                utilizacao.OutgoingExportCFOPCode = oNotaFiscalUsage.OutgoingExportCFOPCode;
                utilizacao.OutgoingInStateCFOPCode = oNotaFiscalUsage.OutgoingInStateCFOPCode;
                utilizacao.OutgoingOutStateCFOPCode = oNotaFiscalUsage.OutgoingOutStateCFOPCode;
                utilizacao.ThirdParty = (int)oNotaFiscalUsage.ThirdParty;
                utilizacao.Usage = oNotaFiscalUsage.Usage;

                for (int i = 0; i < oNotaFiscalUsage.UserFields.Fields.Count; i++)
                {
                    var campos = new CamposUsuarioService();
                    campos.Nome = oNotaFiscalUsage.UserFields.Fields.Item(i).Name;
                    campos.Valor = oNotaFiscalUsage.UserFields.Fields.Item(i).Value;
                    utilizacao.CamposUsuario.Add(campos);
                }
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oNotaFiscalUsage);
            //oNotaFiscalUsage = null;

            return utilizacao;
        }

        private string GetVendedoresCompradores(object key)
        {
            var sXml = string.Empty;
            SalesPersons oSalesPersons = (SalesPersons)oCompany.GetBusinessObject(BoObjectTypes.oSalesPersons);

            if (oSalesPersons.GetByKey(Convert.ToInt32(key)))
            {
                sXml = oSalesPersons.GetAsXML();
            }
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oSalesPersons);
            //oSalesPersons = null;

            return sXml;
        }

        private void AtualizaPnFatherCard(ref string sXml, string fatherCard)
        {
            var sFile = $"{System.IO.Path.GetTempPath()}\\tmp.xml";

            var doc = new XmlDocument();

            doc.LoadXml(sXml);
            var root = doc.DocumentElement;
            var newChild = doc.CreateElement("FatherCard");
            root.SelectSingleNode("/BOM/BO/BusinessPartners/row").AppendChild(newChild);

            root.SelectSingleNode("/BOM/BO/BusinessPartners/row/FatherCard").InnerText = fatherCard;

            doc.Save(sFile);

            doc = new XmlDocument();
            doc.Load(sFile);

            sXml = doc.InnerXml;
            System.IO.File.Delete(sFile);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (oCompany.Connected)
                    {
                        oCompany.Disconnect();
                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
                        //oCompany = null;
                    }
                }

                disposed = true;
            }
        }
    }
}