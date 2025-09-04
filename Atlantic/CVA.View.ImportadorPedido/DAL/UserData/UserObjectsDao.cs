using System;
using SAPbobsCOM;
using DAL.Connection;
using System.Runtime.InteropServices;

namespace DAL.UserData
{
    public class UserObjectsDao
    {
        public static bool Exists(string udoCode)
        {
            var udo = (UserObjectsMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var ret = udo.GetByKey(udoCode);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            return ret;
        }

        public static void Create(string xmlFilePath)
        {
            ConnectionDao.Instance.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            var udo = (UserObjectsMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            udo.Browser.ReadXml(xmlFilePath, 0);
            var udoCode = udo.Code;

            if (udo.Add() != 0)
            {
                int errCode;
                string errMsg;

                ConnectionDao.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

                throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);
        }

        public static void ExportXml(string udoCode)
        {
            ConnectionDao.Instance.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            var udo = (UserObjectsMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var filename = $"{AppDomain.CurrentDomain.BaseDirectory}\\{udoCode}.xml";

            if (udo.GetByKey(udoCode))
            {
                udo.SaveXML(ref filename);
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);
        }

        public static void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType)
        {
            // se não preenchido um table name separado, usa o mesmo do objeto
            if (String.IsNullOrEmpty(TableName))
                TableName = ObjectName;

            UserObjectsMD UserObjectsMD = (UserObjectsMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            // Remove a arroba do usertable Name
            TableName = TableName.Replace("@", "");

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            UserObjectsMD.Code = ObjectName;
            UserObjectsMD.Name = ObjectDesc;
            UserObjectsMD.ObjectType = ObjectType;
            UserObjectsMD.TableName = TableName;

            UserObjectsMD.CanArchive = BoYesNoEnum.tNO;
            UserObjectsMD.CanCancel = BoYesNoEnum.tNO;
            UserObjectsMD.CanClose = BoYesNoEnum.tNO;
            UserObjectsMD.CanCreateDefaultForm = BoYesNoEnum.tNO;
            UserObjectsMD.CanDelete = BoYesNoEnum.tNO;
            UserObjectsMD.CanFind = BoYesNoEnum.tYES;
            UserObjectsMD.CanLog = BoYesNoEnum.tYES;
            UserObjectsMD.CanYearTransfer = BoYesNoEnum.tNO;


            UserObjectsMD.FindColumns.ColumnAlias = "Code";
            UserObjectsMD.FindColumns.ColumnDescription = "Código";
            UserObjectsMD.FindColumns.Add();

            UserObjectsMD.FindColumns.ColumnAlias = "Name";
            UserObjectsMD.FindColumns.ColumnDescription = "Descrição";
            UserObjectsMD.FindColumns.Add();


            if (!bUpdate)
            {
                if (UserObjectsMD.Add() != 0)
                {
                    int errCode;
                    string errMsg;
                    ConnectionDao.Instance.Company.GetLastError(out errCode, out errMsg);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(UserObjectsMD);

                    throw new Exception($"Erro ao criar objeto: ({errCode}) {errMsg}.");
                }
            }
        }

        public static void AddChildTableToUserObject(string ObjectName, string ChildTableName)
        {
            UserObjectsMD UserObjectsMD = (UserObjectsMD)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            // Remove a arroba do usertable Name
            ChildTableName = ChildTableName.Replace("@", "");

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            bool JaAdicionada = false;
            for (int i = 0; i < UserObjectsMD.ChildTables.Count; i++)
            {
                UserObjectsMD.ChildTables.SetCurrentLine(i);
                if (ChildTableName == UserObjectsMD.ChildTables.TableName)
                {
                    JaAdicionada = true;
                    break;
                }
            }

            if (!JaAdicionada)
            {
                UserObjectsMD.ChildTables.Add();
                UserObjectsMD.ChildTables.TableName = ChildTableName;

                if (UserObjectsMD.Update() != 0)
                {
                    int errCode;
                    string errMsg;
                    ConnectionDao.Instance.Company.GetLastError(out errCode, out errMsg);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(UserObjectsMD);

                    throw new Exception($"Erro ao adicionar tabela filho ao objeto: ({errCode}) {errMsg}.");
                }
            }

            Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            GC.Collect();
        }
    }
}
