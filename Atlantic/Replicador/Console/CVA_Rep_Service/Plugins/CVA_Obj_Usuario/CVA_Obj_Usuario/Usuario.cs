using System;
using System.Collections.Generic;
using System.IO;
using CVA_Obj_Shared.Interfaces;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
using System.Xml;
// ReSharper disable PossibleInvalidOperationException

namespace CVA_Obj_Usuario
{
    public class Usuario : IPlugin
    {
        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private bool disposed;

        public Usuario()
        {
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Usuario>();
        }

        public override string Name => "CVA_Obj_Usuario";

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

        public override int Create(Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    if (Exists(registro, oCompany))
                        continue;

                    var usuario = registro.Value;

                    Users oUsers = (Users) oCompany.GetBusinessObject(BoObjectTypes.oUsers);

                    oUsers.Branch = usuario.Branch;
                    oUsers.CashLimit = (BoYesNoEnum)usuario.CashLimit;
                    oUsers.Defaults = usuario.Defaults;
                    oUsers.Department = usuario.Department;
                    oUsers.eMail = usuario.eMail;
                    oUsers.FaxNumber = usuario.FaxNumber;
                    oUsers.Locked = (BoYesNoEnum)usuario.Locked;
                    oUsers.MaxCashAmtForIncmngPayts = usuario.MaxCashAmtForIncmngPayts;
                    oUsers.MaxDiscountGeneral = usuario.MaxDiscountGeneral;
                    oUsers.MaxDiscountPurchase = usuario.MaxDiscountPurchase;
                    oUsers.MaxDiscountSales = usuario.MaxDiscountSales;
                    oUsers.MobilePhoneNumber = usuario.MobilePhoneNumber;
                    oUsers.Superuser = (BoYesNoEnum)usuario.Superuser;
                    
                    BusinessPlaces oBPL = (BusinessPlaces)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPlaces);
                    Recordset oRecordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    oRecordset.DoQuery("SELECT BPLId FROM OBPL WHERE Disabled = 'N'");

                    oBPL.Browser.Recordset = oRecordset;

                    var j = 0;

                    while (!oBPL.Browser.EoF)
                    {
                        try
                        {
                            oUsers.UserBranchAssignment.SetCurrentLine(j);
                            oUsers.UserBranchAssignment.BPLID = oBPL.BPLID;
                        }
                        catch (Exception)
                        {
                            oUsers.UserBranchAssignment.Add();
                            oUsers.UserBranchAssignment.SetCurrentLine(j);
                            oUsers.UserBranchAssignment.BPLID = oBPL.BPLID;
                        }

                        j++;
                        oBPL.Browser.MoveNext();
                    }

                    oUsers.UserCode = usuario.UserCode;
                    oUsers.UserName = usuario.UserName;
                    oUsers.UserPassword = usuario.UserPassword;

                    //for (var i = 0; i < usuario.UserPermission.Count; i++)
                    //{
                    //    oUsers.UserPermission.Permission = (BoPermission)usuario.UserPermission[i].Permission;
                    //    oUsers.UserPermission.PermissionID = usuario.UserPermission[i].PermissionID;
                    //    oUsers.UserPermission.Add();
                    //}

                    if (oUsers.Add() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oUsers);
                    //oUsers = null;
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

        public override int Update(Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    Users oUsers =
                        (Users) oCompany.GetBusinessObject(BoObjectTypes.oUsers);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    var usuario = registro.Value;

                    var userid = 0;
                    var passwd = string.Empty;
                    string[] sKey;

                    if (_reg.CODE.ToString().Contains("|"))
                    {
                        sKey = _reg.CODE.ToString().Split('|');
                        if (sKey.Length >= 2)
                        {
                            userid = Convert.ToInt32(sKey[0]);
                            passwd = sKey[1];
                        }
                    }
                    else
                        userid = Convert.ToInt32(_reg.CODE);

                    oUsers.GetByKey(userid);
                    oUsers.Branch = usuario.Branch;
                    oUsers.CashLimit = (BoYesNoEnum)usuario.CashLimit;
                    oUsers.Defaults = usuario.Defaults;
                    oUsers.Department = usuario.Department;
                    oUsers.eMail = usuario.eMail;
                    oUsers.FaxNumber = usuario.FaxNumber;
                    oUsers.Locked = (BoYesNoEnum)usuario.Locked;
                    oUsers.MaxCashAmtForIncmngPayts = usuario.MaxCashAmtForIncmngPayts;
                    oUsers.MaxDiscountGeneral = usuario.MaxDiscountGeneral;
                    oUsers.MaxDiscountPurchase = usuario.MaxDiscountPurchase;
                    oUsers.MaxDiscountSales = usuario.MaxDiscountSales;
                    oUsers.MobilePhoneNumber = usuario.MobilePhoneNumber;
                    oUsers.Superuser = (BoYesNoEnum)usuario.Superuser;

                    BusinessPlaces oBPL = (BusinessPlaces)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPlaces);
                    Recordset oRecordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    oRecordset.DoQuery("SELECT BPLId FROM OBPL WHERE Disabled = 'N'");

                    oBPL.Browser.Recordset = oRecordset;

                    var j = 0;

                    while (!oBPL.Browser.EoF)
                    {
                        try
                        {
                            oUsers.UserBranchAssignment.SetCurrentLine(j);
                            oUsers.UserBranchAssignment.BPLID = oBPL.BPLID;
                        }
                        catch (Exception)
                        {
                            oUsers.UserBranchAssignment.Add();
                            oUsers.UserBranchAssignment.SetCurrentLine(j);
                            oUsers.UserBranchAssignment.BPLID = oBPL.BPLID;
                        }

                        j++;
                        oBPL.Browser.MoveNext();
                    }

                    oUsers.UserCode = usuario.UserCode;
                    oUsers.UserName = usuario.UserName;

                    if (!string.IsNullOrEmpty(passwd))
                        oUsers.UserPassword = usuario.UserPassword;
                    
                    //for (var i = 0; i < usuario.UserPermission.Count; i++)
                    //{
                    //    oUsers.UserPermission.SetCurrentLine(i);
                    //    oUsers.UserPermission.Permission = (BoPermission)usuario.UserPermission[i].Permission;
                    //    oUsers.UserPermission.PermissionID = usuario.UserPermission[i].PermissionID;
                    //    oUsers.UserPermission.Add();
                    //}

                    if (oUsers.Update() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oUsers);
                    //oUsers = null;
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

        private bool Exists(KeyValuePair<int, UsuarioService> registro, Company oCompany)
        {
            var ret = false;

            try
            {
                Users oUsers =
                    (Users)oCompany.GetBusinessObject(BoObjectTypes.oUsers);

                var oUnitOfWork = new UnitOfWork();
                var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);                

                ret = oUsers.GetByKey(Convert.ToInt32(_reg.CODE));
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oUsers);
                //oUsers = null;

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

        public override int Delete(Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            try
            {
                foreach (var registro in listaRegistros)
                {
                    Users oUsers =
                        (Users) oCompany.GetBusinessObject(BoObjectTypes.oUsers);

                    var oUnitOfWork = new UnitOfWork();
                    var _reg = oUnitOfWork.CvaRegRepository.GetByID(registro.Key);

                    oUsers.GetByKey(Convert.ToInt32(_reg.CODE));

                    if (oUsers.Remove() != 0)
                    {
                        int errCode;
                        string errMsg;

                        oCompany.GetLastError(out errCode, out errMsg);

                        throw new ReplicadorException($"{errCode} - {errMsg}");
                    }
                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oUsers);
                    //oUsers = null;
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

        private string GetXml(Company oCompany, string xml, string sFile)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var root = doc.DocumentElement;
            var nodeToFind = root.SelectSingleNode("/BOM/BO/UserBranchAssignment");
            var userCode = root.SelectSingleNode("/BOM/BO/Users/row/UserCode");

            if (nodeToFind == null)
            {
                var node = doc.SelectSingleNode("/BOM/BO");
                var element = doc.CreateNode(XmlNodeType.Element, "UserBranchAssignment", null);
                node.AppendChild(element);
                root.AppendChild(node);
                doc.Save(sFile);

                doc = new XmlDocument();
                doc.Load(sFile);
            }

            foreach (XmlNode row in doc.SelectNodes("/BOM/BO/UserBranchAssignment"))
            {
                row.RemoveAll();
            }

            BusinessPlaces oBPL = (BusinessPlaces)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPlaces);
            Recordset oRecordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT BPLId FROM OBPL WHERE Disabled = 'N'");

            oBPL.Browser.Recordset = oRecordset;

            using (XmlWriter writer = doc.SelectSingleNode("/BOM/BO/UserBranchAssignment").CreateNavigator().AppendChild())
            {
                while (!oBPL.Browser.EoF)
                {
                    writer.WriteStartElement("row");
                    writer.WriteElementString("UserCode", userCode.InnerText);
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
    }
}