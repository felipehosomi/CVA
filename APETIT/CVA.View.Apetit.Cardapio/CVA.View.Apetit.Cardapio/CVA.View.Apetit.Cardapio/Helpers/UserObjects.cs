using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Xml.Linq;
using System.IO;

namespace CVA.View.Apetit.Cardapio.Helpers
{
    public class UserObjects
    {
        public static bool ExistsThenRemove(string udoCode)
        {
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var ret = udo.GetByKey(udoCode);
            if (ret) udo.Remove();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            return false;
        }

        public static bool Exists(string udoCode)
        {
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var ret = udo.GetByKey(udoCode);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            return ret;
        }

        public static void Create(string xmlFilePath)
        {
            B1Connection.Instance.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            udo.Browser.ReadXml(xmlFilePath, 0);
            var udoCode = udo.Code;
            
            if (udo.Add() != 0)
            {
                int errCode;
                string errMsg;

                B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

                throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            AddData(udoCode);
        }

        private static void AddData(string udoCode)
        {
            var oCompanyService = B1Connection.Instance.Company.GetCompanyService();

            if (udoCode == "UDOTIPO")
            {
                var oGeneralService = oCompanyService.GetGeneralService("UDOTIPO");
                var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "1");
                oGeneralData.SetProperty("Name", "REPRESENTANTE");
                oGeneralData.SetProperty("U_TIPO", "N");
                var oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOTIPO");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "2");
                oGeneralData.SetProperty("Name", "VENDEDOR");
                oGeneralData.SetProperty("U_TIPO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData); 
            }

            if (udoCode == "UDOCRIT")
            {
                var oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "1");
                oGeneralData.SetProperty("Name", "Vendedor (obrigatório)");
                oGeneralData.SetProperty("U_POS", "1");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                var oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "2");
                oGeneralData.SetProperty("Name", "Item");
                oGeneralData.SetProperty("U_POS", "2");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "3");
                oGeneralData.SetProperty("Name", "Grupo de itens");
                oGeneralData.SetProperty("U_POS", "3");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "4");
                oGeneralData.SetProperty("Name", "Centro de custo");
                oGeneralData.SetProperty("U_POS", "4");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "5");
                oGeneralData.SetProperty("Name", "Fabricante");
                oGeneralData.SetProperty("U_POS", "5");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "6");
                oGeneralData.SetProperty("Name", "Cliente");
                oGeneralData.SetProperty("U_POS", "6");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "7");
                oGeneralData.SetProperty("Name", "Cidade");
                oGeneralData.SetProperty("U_POS", "7");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "8");
                oGeneralData.SetProperty("Name", "Estado");
                oGeneralData.SetProperty("U_POS", "8");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "9");
                oGeneralData.SetProperty("Name", "Setor");
                oGeneralData.SetProperty("U_POS", "9");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData); 
            }
        }

        private static void ExportXml(string udoCode)
        {
            B1Connection.Instance.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var filename = $"{AppDomain.CurrentDomain.BaseDirectory}\\{udoCode}.xml";

            if (udo.GetByKey(udoCode))
            {
                udo.SaveXML(ref filename);                
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);
        }

        //public static void Create(string fileName)
        //{
        //    var xml = XDocument.Load(fileName);

        //    var strXml = xml.ToString();

        //    strXml = strXml.Replace(" xmlns=\"\"", "");
        //    File.WriteAllText(fileName, strXml);

        //    xml = XDocument.Load(fileName);

        //    foreach (var bo in xml.Descendants("BO"))
        //    {
        //        UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
        //        var code = "";
        //        var name = "";
        //        var tableName = "";
        //        var logTableName = "";
        //        var objectType = "";
        //        var manageSeries = "";
        //        var canDelete = "";
        //        var canClose = "";
        //        var canCancel = "";
        //        var extensionName = "";
        //        var canFind = "";
        //        var canYearTransfer = "";
        //        var canCreateDefaultForm = "";
        //        var canLog = "";
        //        var overwriteDllfile = "";
        //        var useUniqueFormType = "";
        //        var canArchive = "";
        //        var menuItem = "";
        //        var menuCaption = "";
        //        var fatherMenuId = "";
        //        var position = "";
        //        var enableEnhancedForm = "";
        //        var rebuildEnhancedform = "";
        //        var formSrf = "";
        //        var menuUid = "";
        //        var canApprove = "";
        //        var templateId = "";

        //        foreach (var userObjects in bo.Descendants("UserObjectsMD"))
        //        {
        //            foreach (var row in userObjects.Descendants("row"))
        //            {
        //                code = row.Element("Code").Value;
        //                name = row.Element("Name").Value;
        //                tableName = row.Element("TableName").Value;
        //                logTableName = row.Element("LogTableName").Value;
        //                objectType = row.Element("ObjectType").Value;
        //                manageSeries = row.Element("ManageSeries").Value;
        //                canDelete = row.Element("CanDelete").Value;
        //                canClose = row.Element("CanClose").Value;
        //                canCancel = row.Element("CanCancel").Value;
        //                extensionName = row.Element("ExtensionName").Value;
        //                canFind = row.Element("CanFind").Value;
        //                canYearTransfer = row.Element("CanYearTransfer").Value;
        //                canCreateDefaultForm = row.Element("CanCreateDefaultForm").Value;
        //                canLog = row.Element("CanLog").Value;
        //                overwriteDllfile = row.Element("OverwriteDllfile").Value;
        //                useUniqueFormType = row.Element("UseUniqueFormType").Value;
        //                canArchive = row.Element("CanArchive").Value;
        //                menuItem = row.Element("MenuItem").Value;
        //                menuCaption = row.Element("MenuCaption").Value;
        //                fatherMenuId = row.Element("FatherMenuID").Value;
        //                position = row.Element("Position").Value;
        //                enableEnhancedForm = row.Element("EnableEnhancedForm").Value;
        //                rebuildEnhancedform = row.Element("RebuildEnhancedForm").Value;
        //                formSrf = row.Element("FormSRF").Value;
        //                menuUid = row.Element("MenuUID").Value;
        //                canApprove = row.Element("CanApprove").Value;
        //                templateId = row.Element("TemplateID").Value;
        //            }
        //        }

        //        if (Exists(code))
        //        {
        //            Update(fileName);
        //            return;
        //        }

        //        udo.CanApprove = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canApprove);
        //        udo.CanArchive = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canArchive);
        //        udo.CanCancel = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canCancel);
        //        udo.CanClose = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canClose);
        //        udo.CanCreateDefaultForm = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canCreateDefaultForm);
        //        udo.CanDelete = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canDelete);
        //        udo.CanFind = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canFind);
        //        udo.CanLog = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canLog);
        //        udo.CanYearTransfer = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canYearTransfer);
        //        udo.Code = code;
        //        udo.EnableEnhancedForm = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), enableEnhancedForm);
        //        udo.ExtensionName = extensionName;
        //        udo.FatherMenuID = int.Parse(fatherMenuId);
        //        udo.FormSRF = formSrf;
        //        udo.LogTableName = logTableName;
        //        udo.ManageSeries = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), manageSeries);
        //        udo.MenuCaption = menuCaption;
        //        udo.MenuItem = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), menuItem);
        //        udo.MenuUID = menuUid;
        //        udo.Name = name;
        //        udo.ObjectType = (BoUDOObjType)Enum.Parse(typeof(BoUDOObjType), objectType);
        //        udo.OverwriteDllfile = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), overwriteDllfile);
        //        udo.Position = int.Parse(position);
        //        udo.RebuildEnhancedForm = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), rebuildEnhancedform);
        //        udo.TableName = tableName;
        //        udo.TemplateID = templateId;
        //        udo.UseUniqueFormType = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), useUniqueFormType);

        //        foreach (var  childTables in bo.Descendants("UserObjectMD_ChildTables"))
        //        {
        //            foreach (var row in childTables.Descendants("row"))
        //            {
        //                var child_tableName = row.Element("TableName").Value;
        //                var child_logTableName = row.Element("LogTableName").Value;
        //                var child_objectName = row.Element("ObjectName").Value;

        //                udo.ChildTables.Add();
        //                udo.ChildTables.LogTableName = child_logTableName;
        //                udo.ChildTables.ObjectName = child_objectName;
        //                udo.ChildTables.TableName = child_tableName;
        //            }
        //        }

        //        foreach (var findColumns in bo.Descendants("UserObjectMD_FindColumns"))
        //        {
        //            foreach (var row in findColumns.Descendants("row"))
        //            {
        //                var columnAlias = row.Element("ColumnAlias").Value;
        //                var columnDescription = row.Element("ColumnDescription").Value;

        //                udo.FindColumns.Add();
        //                udo.FindColumns.ColumnAlias = columnAlias;
        //                udo.FindColumns.ColumnDescription = columnDescription;
        //            }
        //        }

        //        foreach (var formColumns in bo.Descendants("UserObjectMD_FormColumns"))
        //        {
        //            foreach (var row in formColumns.Descendants("row"))
        //            {
        //                var sonNumber = int.Parse(row.Element("SonNumber").Value.ToString());
        //                var formColumnAlias = row.Element("FormColumnAlias").Value;
        //                var formColumnDescription = row.Element("FormColumnDescription").Value;
        //                var editable = row.Element("Editable").Value; 

        //                udo.FormColumns.Add();
        //                udo.FormColumns.SonNumber = sonNumber;
        //                udo.FormColumns.FormColumnAlias = formColumnAlias;
        //                udo.FormColumns.FormColumnDescription = formColumnDescription;
        //                udo.FormColumns.Editable = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), editable);
        //            }
        //        }

        //        foreach (var enchancedFormColumns in bo.Descendants("UserObjectMD_EnhancedFormColumns"))
        //        {
        //            foreach (var row in enchancedFormColumns.Descendants("row"))
        //            {
        //                var columnNumber = int.Parse(row.Element("ColumnNumber").Value.ToString());
        //                var childNumber = int.Parse(row.Element("ChildNumber").Value.ToString());
        //                var columnAlias = row.Element("ColumnAlias").Value;
        //                var columnDescription = row.Element("ColumnDescription").Value;
        //                var columnIsUsed = row.Element("ColumnIsUsed").Value;
        //                var editable = row.Element("Editable").Value;

        //                udo.EnhancedFormColumns.Add();
        //                udo.EnhancedFormColumns.ColumnNumber = columnNumber;
        //                udo.EnhancedFormColumns.ChildNumber = childNumber;
        //                udo.EnhancedFormColumns.ColumnAlias = columnAlias;
        //                udo.EnhancedFormColumns.ColumnDescription = columnDescription;
        //                udo.EnhancedFormColumns.ColumnIsUsed = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), columnIsUsed);
        //                udo.EnhancedFormColumns.Editable = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), editable);
        //            }
        //        }

        //        if (udo.Add() != 0)
        //        {
        //            int errCode;
        //            string errMsg;

        //            B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

        //            throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
        //        }
        //    }
        //}

        //private static void Update(string fileName)
        //{
        //    var xml = XDocument.Load(fileName);

        //    var strXml = xml.ToString();

        //    strXml = strXml.Replace(" xmlns=\"\"", "");
        //    File.WriteAllText(fileName, strXml);

        //    xml = XDocument.Load(fileName);

        //    foreach (var bo in xml.Descendants("BO"))
        //    {
        //        UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
        //        var code = "";
        //        var name = "";
        //        var tableName = "";
        //        var logTableName = "";
        //        var objectType = "";
        //        var manageSeries = "";
        //        var canDelete = "";
        //        var canClose = "";
        //        var canCancel = "";
        //        var extensionName = "";
        //        var canFind = "";
        //        var canYearTransfer = "";
        //        var canCreateDefaultForm = "";
        //        var canLog = "";
        //        var overwriteDllfile = "";
        //        var useUniqueFormType = "";
        //        var canArchive = "";
        //        var menuItem = "";
        //        var menuCaption = "";
        //        var fatherMenuId = "";
        //        var position = "";
        //        var enableEnhancedForm = "";
        //        var rebuildEnhancedform = "";
        //        var formSrf = "";
        //        var menuUid = "";
        //        var canApprove = "";
        //        var templateId = "";

        //        foreach (var userObjects in bo.Descendants("UserObjectsMD"))
        //        {
        //            foreach (var row in userObjects.Descendants("row"))
        //            {
        //                code = row.Element("Code").Value;
        //                name = row.Element("Name").Value;
        //                tableName = row.Element("TableName").Value;
        //                logTableName = row.Element("LogTableName").Value;
        //                objectType = row.Element("ObjectType").Value;
        //                manageSeries = row.Element("ManageSeries").Value;
        //                canDelete = row.Element("CanDelete").Value;
        //                canClose = row.Element("CanClose").Value;
        //                canCancel = row.Element("CanCancel").Value;
        //                extensionName = row.Element("ExtensionName").Value;
        //                canFind = row.Element("CanFind").Value;
        //                canYearTransfer = row.Element("CanYearTransfer").Value;
        //                canCreateDefaultForm = row.Element("CanCreateDefaultForm").Value;
        //                canLog = row.Element("CanLog").Value;
        //                overwriteDllfile = row.Element("OverwriteDllfile").Value;
        //                useUniqueFormType = row.Element("UseUniqueFormType").Value;
        //                canArchive = row.Element("CanArchive").Value;
        //                menuItem = row.Element("MenuItem").Value;
        //                menuCaption = row.Element("MenuCaption").Value;
        //                fatherMenuId = row.Element("FatherMenuID").Value;
        //                position = row.Element("Position").Value;
        //                enableEnhancedForm = row.Element("EnableEnhancedForm").Value;
        //                rebuildEnhancedform = row.Element("RebuildEnhancedForm").Value;
        //                formSrf = row.Element("FormSRF").Value;
        //                menuUid = row.Element("MenuUID").Value;
        //                canApprove = row.Element("CanApprove").Value;
        //                templateId = row.Element("TemplateID").Value;
        //            }
        //        }

        //        udo.GetByKey(code);

        //        udo.CanApprove = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canApprove);
        //        udo.CanArchive = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canArchive);
        //        udo.CanCancel = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canCancel);
        //        udo.CanClose = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canClose);
        //        udo.CanCreateDefaultForm = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canCreateDefaultForm);
        //        udo.CanDelete = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canDelete);
        //        udo.CanFind = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canFind);
        //        udo.CanLog = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canLog);
        //        udo.CanYearTransfer = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), canYearTransfer);
        //        udo.EnableEnhancedForm = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), enableEnhancedForm);
        //        udo.ExtensionName = extensionName;
        //        udo.FatherMenuID = int.Parse(fatherMenuId);
        //        udo.FormSRF = formSrf;
        //        udo.LogTableName = logTableName;
        //        udo.ManageSeries = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), manageSeries);
        //        udo.MenuCaption = menuCaption;
        //        udo.MenuItem = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), menuItem);
        //        udo.MenuUID = menuUid;
        //        udo.Name = name;
        //        udo.ObjectType = (BoUDOObjType)Enum.Parse(typeof(BoUDOObjType), objectType);
        //        udo.OverwriteDllfile = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), overwriteDllfile);
        //        udo.Position = int.Parse(position);
        //        udo.RebuildEnhancedForm = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), rebuildEnhancedform);
        //        udo.TableName = tableName;
        //        udo.TemplateID = templateId;
        //        udo.UseUniqueFormType = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), useUniqueFormType);

        //        foreach (var childTables in bo.Descendants("UserObjectMD_ChildTables"))
        //        {
        //            var lst = new List<int>();

        //            for (var i = 0; i < udo.ChildTables.Count; i++)
        //            {
        //                udo.ChildTables.SetCurrentLine(i);

        //                var row = childTables.Descendants("row").Where(c =>
        //                c.Element("TableName").Value == udo.ChildTables.TableName
        //                && c.Element("LogTableName").Value == udo.ChildTables.LogTableName
        //                && c.Element("ObjectName").Value == udo.ChildTables.ObjectName);

        //                if (row.ToList().Count > 0)
        //                {
        //                    udo.ChildTables.TableName = row.FirstOrDefault().Element("TableName").Value;
        //                    udo.ChildTables.LogTableName = row.FirstOrDefault().Element("LogTableName").Value;
        //                    udo.ChildTables.ObjectName = row.FirstOrDefault().Element("ObjectName").Value;
        //                    lst.Add(i);
        //                }
        //            }

        //            if (lst.Count != childTables.Descendants("row").Count())
        //            {
        //                var i = 0;

        //                foreach (var row in childTables.Descendants("row"))
        //                {
        //                    var child_tableName = row.Element("TableName").Value;
        //                    var child_logTableName = row.Element("LogTableName").Value;
        //                    var child_objectName = row.Element("ObjectName").Value;

        //                    if (!lst.Contains(i))
        //                    {
        //                        udo.ChildTables.Add();
        //                        udo.ChildTables.LogTableName = child_logTableName;
        //                        udo.ChildTables.ObjectName = child_objectName;
        //                        udo.ChildTables.TableName = child_tableName;
        //                    }

        //                    i++;
        //                }
        //            }
        //        }

        //        foreach (var findColumns in bo.Descendants("UserObjectMD_FindColumns"))
        //        {
        //            var lst = new List<int>();

        //            for (var i = 0; i < udo.FindColumns.Count; i++)
        //            {
        //                udo.FindColumns.SetCurrentLine(i);

        //                var row = findColumns.Descendants("row").Where(c =>
        //                c.Element("ColumnAlias").Value == udo.FindColumns.ColumnAlias
        //                && c.Element("ColumnDescription").Value == udo.FindColumns.ColumnDescription);

        //                if (row.ToList().Count > 0)
        //                {
        //                    udo.FindColumns.ColumnAlias = row.FirstOrDefault().Element("ColumnAlias").Value;
        //                    udo.FindColumns.ColumnDescription = row.FirstOrDefault().Element("ColumnDescription").Value;
        //                    lst.Add(i);
        //                }
        //            }

        //            if (lst.Count != findColumns.Descendants("row").Count())
        //            {
        //                var i = 0;

        //                foreach (var row in findColumns.Descendants("row"))
        //                {
        //                    var ColumnAlias = row.Element("ColumnAlias").Value;
        //                    var ColumnDescription = row.Element("ColumnDescription").Value;

        //                    if (!lst.Contains(i))
        //                    {
        //                        udo.FindColumns.Add();
        //                        udo.FindColumns.ColumnAlias = ColumnAlias;
        //                        udo.FindColumns.ColumnDescription = ColumnDescription;
        //                    }

        //                    i++;
        //                }
        //            }
        //        }

        //        foreach (var formColumns in bo.Descendants("UserObjectMD_FormColumns"))
        //        {
        //            var lst = new List<int>();

        //            for (var i = 0; i < udo.FormColumns.Count; i++)
        //            {
        //                udo.FormColumns.SetCurrentLine(i);

        //                var row = formColumns.Descendants("row").Where(c =>
        //                int.Parse(c.Element("SonNumber").Value.ToString()) == udo.FormColumns.SonNumber
        //                && c.Element("FormColumnAlias").Value == udo.FormColumns.FormColumnAlias
        //                && c.Element("FormColumnDescription").Value == udo.FormColumns.FormColumnDescription
        //                && (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), c.Element("Editable").Value) == udo.FormColumns.Editable);

        //                if (row.ToList().Count > 0)
        //                {
        //                    var sonNumber = int.Parse(row.FirstOrDefault().Element("SonNumber").Value.ToString());
        //                    var formColumnAlias = row.FirstOrDefault().Element("FormColumnAlias").Value;
        //                    var formColumnDescription = row.FirstOrDefault().Element("FormColumnDescription").Value;
        //                    var editable = row.FirstOrDefault().Element("Editable").Value;

        //                    udo.FormColumns.SonNumber = sonNumber;
        //                    udo.FormColumns.FormColumnAlias = formColumnAlias;
        //                    udo.FormColumns.FormColumnDescription = formColumnDescription;
        //                    udo.FormColumns.Editable = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), editable);
        //                    lst.Add(i);
        //                }
        //            }

        //            if (lst.Count != formColumns.Descendants("row").Count())
        //            {
        //                var i = 0;

        //                foreach (var row in formColumns.Descendants("row"))
        //                {
        //                    var sonNumber = int.Parse(row.Element("SonNumber").Value.ToString());
        //                    var formColumnAlias = row.Element("FormColumnAlias").Value;
        //                    var formColumnDescription = row.Element("FormColumnDescription").Value;
        //                    var editable = row.Element("Editable").Value;

        //                    if (!lst.Contains(i))
        //                    {
        //                        udo.FormColumns.Add();
        //                        udo.FormColumns.SonNumber = sonNumber;
        //                        udo.FormColumns.FormColumnAlias = formColumnAlias;
        //                        udo.FormColumns.FormColumnDescription = formColumnDescription;
        //                        udo.FormColumns.Editable = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), editable);
        //                    }

        //                    i++;
        //                }
        //            }
        //        }

        //        foreach (var enchancedFormColumns in bo.Descendants("UserObjectMD_EnhancedFormColumns"))
        //        {
        //            var lst = new List<int>();

        //            for (var i = 0; i < udo.EnhancedFormColumns.Count; i++)
        //            {
        //                udo.EnhancedFormColumns.SetCurrentLine(i);

        //                var row = enchancedFormColumns.Descendants("row").Where(c =>
        //                int.Parse(c.Element("ColumnNumber").Value.ToString()) == udo.EnhancedFormColumns.ColumnNumber
        //                && int.Parse(c.Element("ChildNumber").Value.ToString()) == udo.EnhancedFormColumns.ChildNumber
        //                && c.Element("ColumnAlias").Value == udo.EnhancedFormColumns.ColumnAlias
        //                && c.Element("ColumnDescription").Value == udo.EnhancedFormColumns.ColumnDescription
        //                && (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), c.Element("ColumnIsUsed").Value) == udo.EnhancedFormColumns.ColumnIsUsed
        //                && (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), c.Element("Editable").Value) == udo.EnhancedFormColumns.Editable);

        //                if (row.ToList().Count > 0)
        //                {
        //                    var columnNumber = int.Parse(row.FirstOrDefault().Element("ColumnNumber").Value.ToString());
        //                    var childNumber = int.Parse(row.FirstOrDefault().Element("ChildNumber").Value.ToString());
        //                    var columnAlias = row.FirstOrDefault().Element("ColumnAlias").Value;
        //                    var columnDescription = row.FirstOrDefault().Element("ColumnDescription").Value;
        //                    var columnIsUsed = row.FirstOrDefault().Element("ColumnIsUsed").Value;
        //                    var editable = row.FirstOrDefault().Element("Editable").Value;

        //                    udo.EnhancedFormColumns.ColumnNumber = columnNumber;
        //                    udo.EnhancedFormColumns.ChildNumber = childNumber;
        //                    udo.EnhancedFormColumns.ColumnAlias = columnAlias;
        //                    udo.EnhancedFormColumns.ColumnDescription = columnDescription;
        //                    udo.EnhancedFormColumns.ColumnIsUsed = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), columnIsUsed);
        //                    udo.EnhancedFormColumns.Editable = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), editable);
        //                    lst.Add(i);
        //                }
        //            }

        //            if (lst.Count != enchancedFormColumns.Descendants("row").Count())
        //            {
        //                var i = 0;

        //                foreach (var row in enchancedFormColumns.Descendants("row"))
        //                {
        //                    var columnNumber = int.Parse(row.Element("ColumnNumber").Value.ToString());
        //                    var childNumber = int.Parse(row.Element("ChildNumber").Value.ToString());
        //                    var columnAlias = row.Element("ColumnAlias").Value;
        //                    var columnDescription = row.Element("ColumnDescription").Value;
        //                    var columnIsUsed = row.Element("ColumnIsUsed").Value;
        //                    var editable = row.Element("Editable").Value;

        //                    if (!lst.Contains(i))
        //                    {
        //                        udo.EnhancedFormColumns.Add();
        //                        udo.EnhancedFormColumns.ColumnNumber = columnNumber;
        //                        udo.EnhancedFormColumns.ChildNumber = childNumber;
        //                        udo.EnhancedFormColumns.ColumnAlias = columnAlias;
        //                        udo.EnhancedFormColumns.ColumnDescription = columnDescription;
        //                        udo.EnhancedFormColumns.ColumnIsUsed = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), columnIsUsed);
        //                        udo.EnhancedFormColumns.Editable = (BoYesNoEnum)Enum.Parse(typeof(BoYesNoEnum), editable);
        //                    }

        //                    i++;
        //                }
        //            }
        //        }

        //        if (udo.Update() != 0)
        //        {
        //            int errCode;
        //            string errMsg;

        //            B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

        //            throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
        //        }
        //    }
        //}
    }
}
