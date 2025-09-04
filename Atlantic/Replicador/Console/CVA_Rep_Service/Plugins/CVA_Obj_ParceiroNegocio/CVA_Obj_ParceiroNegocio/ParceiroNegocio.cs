using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
using System.Xml.Linq;
using System.Xml;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_ParceiroNegocio
{
    public class ParceiroNegocio : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public ParceiroNegocio()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<ParceiroNegocio>();
        }

        public override string Name => "CVA_Obj_ParceiroNegocio";

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

                    var sFile = $"{Path.GetTempPath()}\\ParceiroNegocio.xml";

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    File.WriteAllText(sFile, GetXml(oCompany, registro.Value, sFile, _reg.CODE, true));

                    BusinessPartners oBusinessPartners =
                        (BusinessPartners)oCompany.GetBusinessObjectFromXML(sFile, 0);
                    oBusinessPartners.Series = (oBusinessPartners.CardType == BoCardTypes.cCustomer) ? 1 : 2;

                    if (oBusinessPartners.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    File.Delete(sFile);
                    Update(registro, oCompany);
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartners);
                    //oBusinessPartners = null;
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
                    var sFile = $"{Path.GetTempPath()}\\ParceiroNegocio.xml";
                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    RemoveEnderecos(oCompany, _reg.CODE);

                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForFullGCApproach();
                    GC.WaitForFullGCComplete();

                    BusinessPartners oBusinessPartners =
                        (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);


                    File.WriteAllText(sFile, GetXml(oCompany, registro.Value, sFile, _reg.CODE, false));

                    oBusinessPartners.GetByKey(_reg.CODE);
                    oBusinessPartners.Browser.ReadXml(sFile, 0);
                    oBusinessPartners.Series = (oBusinessPartners.CardType == BoCardTypes.cCustomer) ? 1 : 2;

                    if (oBusinessPartners.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartners);
                    //oBusinessPartners = null;
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

        private void Update(KeyValuePair<int, string> registro, Company oCompany)
        {
            try
            {
                var sFile = $"{Path.GetTempPath()}\\ParceiroNegocio.xml";
                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                RemoveEnderecos(oCompany, _reg.CODE);

                BusinessPartners oBusinessPartners =
                    (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                File.WriteAllText(sFile, GetXml(oCompany, registro.Value, sFile, _reg.CODE, false));

                oBusinessPartners.GetByKey(_reg.CODE);
                oBusinessPartners.Browser.ReadXml(sFile, 0);
                oBusinessPartners.Series = (oBusinessPartners.CardType == BoCardTypes.cCustomer) ? 1 : 2;

                if (oBusinessPartners.Update() != 0)
                {
                    int errCode;
                    string errMsg;

                    oCompany.GetLastError(out errCode, out errMsg);

                    throw new ReplicadorException($"{errCode} - {errMsg}");
                }
                File.Delete(sFile);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartners);
                //oBusinessPartners = null;
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
        }

        private bool Exists(KeyValuePair<int, string> registro, Company oCompany)
        {
            var ret = false;

            try
            {
                BusinessPartners oBusinessPartners =
                    (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                ret = oBusinessPartners.GetByKey(_reg.CODE);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartners);
                //oBusinessPartners = null;

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
                    BusinessPartners oBusinessPartners =
                        (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oBusinessPartners.GetByKey(_reg.CODE);

                    if (oBusinessPartners.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBusinessPartners);
                    //oBusinessPartners = null;
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

        private string GetXml(Company oCompany, string xml, string sFile, string bpCode, bool isInserting)
        {
            var doc = new XmlDocument();
            XmlNode node;
            XmlNode element;

            doc.LoadXml(xml);

            var root = doc.DocumentElement;
            var nodeToFind = root.SelectSingleNode("/BOM/BO/BPBranchAssignment");

            if (nodeToFind == null)
            {
                node = doc.SelectSingleNode("/BOM/BO");
                element = doc.CreateNode(XmlNodeType.Element, "BPBranchAssignment", null);
                node.AppendChild(element);
                root.AppendChild(node);
                doc.Save(sFile);

                doc = new XmlDocument();
                doc.Load(sFile);
            }

            if (isInserting)
            {
                nodeToFind = root.SelectSingleNode("/BOM/BO/BPBankAccounts");

                if (nodeToFind != null)
                {
                    node = doc.SelectSingleNode("/BOM/BO");
                    element = doc.GetElementsByTagName("BPBankAccounts")[0];
                    node.RemoveChild(element);

                    root.SelectSingleNode("/BOM/BO/BusinessPartners/row/DefaultAccount").InnerText = string.Empty;
                    root.SelectSingleNode("/BOM/BO/BusinessPartners/row/DefaultBranch").InnerText = string.Empty;
                    root.SelectSingleNode("/BOM/BO/BusinessPartners/row/DefaultBankCode").InnerText = string.Empty;

                    //element = doc.CreateNode(XmlNodeType.Element, "BPBankAccounts", null);
                    //node.AppendChild(element);
                    //root.AppendChild(node);
                    doc.Save(sFile);

                    doc = new XmlDocument();
                    doc.Load(sFile);
                }
            }

            //else
            //{
            //    nodeToFind = root.SelectSingleNode("/BOM/BO/BPFiscalTaxID");

            //    if (nodeToFind != null)
            //    {
            //        foreach(XmlNode row in doc.SelectNodes("/BOM/BO/BPFiscalTaxID"))
            //        {
            //            foreach(XmlNode item in row.ChildNodes)
            //            {
            //                if (item.ChildNodes.Count == 3)
            //                    row.RemoveChild(item);
            //            }
            //        }

            //        doc.Save(sFile);

            //        doc = new XmlDocument();
            //        doc.Load(sFile);
            //    }
            //}

            //nodeToFind = root.SelectSingleNode("/BOM/BO/ContactEmployees");

            //if (nodeToFind != null)
            //{
            //    foreach (XmlNode row in doc.SelectNodes("/BOM/BO/ContactEmployees"))
            //    {
            //        var bp = string.Empty;
            //        var addr = string.Empty;

            //        foreach (XmlNode item in row.ChildNodes)
            //        {
            //            var j = 0;
            //            foreach (XmlNode i in item.ChildNodes)
            //            {
            //                if (i.Name != "NFeRcpn")
            //                {
            //                    if (j >= item.ChildNodes.Count)
            //                    {
            //                        XmlElement e = i.OwnerDocument.CreateElement("NFeRcpn");
            //                        e.InnerText = "tYES";
            //                        doc.Save(sFile);

            //                        doc = new XmlDocument();
            //                        doc.Load(sFile);
            //                        break;
            //                    }
            //                }
            //                j++;
            //            }
            //        }
            //    }
            //}
            nodeToFind = root.SelectSingleNode("/BOM/BO/BusinessPartners/row/AttachmentEntry");
            if (nodeToFind != null)
            {
                foreach (XmlNode row in doc.SelectNodes("/BOM/BO/BusinessPartners"))
                {
                    foreach(XmlNode item in row.ChildNodes)
                    {
                        foreach(XmlNode i in item.ChildNodes)
                        {
                            if(i.Name == "AttachmentEntry")
                            {
                                item.RemoveChild(i);
                                doc.Save(sFile);
                                doc = new XmlDocument();
                                doc.Load(sFile);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (XmlNode row in doc.SelectNodes("/BOM/BO/BPBranchAssignment"))
            {
                row.RemoveAll();
            }

            BusinessPlaces oBPL = (BusinessPlaces)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPlaces);
            Recordset oRecordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT BPLId FROM OBPL WHERE Disabled = 'N'");

            oBPL.Browser.Recordset = oRecordset;

            using (XmlWriter writer = doc.SelectSingleNode("/BOM/BO/BPBranchAssignment").CreateNavigator().AppendChild())
            {
                while (!oBPL.Browser.EoF)
                {
                    writer.WriteStartElement("row");
                    writer.WriteElementString("BPCode", bpCode);
                    writer.WriteElementString("BPLID", oBPL.BPLID.ToString());
                    writer.WriteEndElement();

                    oBPL.Browser.MoveNext();
                }
            }

            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oRecordset);
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oBPL);
            //oBPL = null;
            //oRecordset = null;

            return doc.InnerXml;
        }

        private static void RemoveEnderecos(Company oCompany, string code)
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();

            BusinessPartners oBP = (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            if (oBP.GetByKey(code))
            {
                var totalEnderecos = oBP.Addresses.Count;

                for (int i = 0; i < totalEnderecos; i++)
                {
                    oBP.Addresses.SetCurrentLine(0);
                    oBP.Addresses.Delete();
                }

                oBP.Update();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP);
            oBP = null;

            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForFullGCApproach();
            GC.WaitForFullGCComplete();
        }
    }
}