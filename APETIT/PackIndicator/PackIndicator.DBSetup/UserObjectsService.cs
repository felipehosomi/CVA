using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SAPbobsCOM;

namespace PackIndicator.DBSetup
{
    public class UserObjectsService
    {
        public static bool OnlyAdd = false;

        public static Company oCompany;

        public static string CreateUserTable(string UserTableName, string UserTableDesc, SAPbobsCOM.BoUTBTableType UserTableType, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserTablesMD oUserTableMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
            GC.Collect();

            string Retorno = "Criação/Atualização da tabela de usuário " + UserTableName + ": ";

            oUserTableMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

            // Remove a arroba do usertable Name
            UserTableName = UserTableName.Replace("@", "");

            bool bUpdate = oUserTableMD.GetByKey(UserTableName);

            oUserTableMD.TableName = UserTableName;
            oUserTableMD.TableDescription = UserTableDesc;
            oUserTableMD.TableType = UserTableType;

            if (bUpdate)
            {
                if (!OnlyAdd)
                {
                    CodErro = oUserTableMD.Update();
                }
                else
                {

                    CodErro = 0;
                }
            }
            else
            {
                CodErro = oUserTableMD.Add();
            }

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            //}
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
            GC.Collect();
            tbxLog.AppendText("\r\n" + Retorno);
            return Retorno;
        }

        public static string RemoveUserTable(string UserTableName, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserTablesMD oUserTableMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
            GC.Collect();

            oUserTableMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

            // Remove a arroba do usertable Name
            UserTableName = UserTableName.Replace("@", "");

            string Retorno = " - Remoção da tabela de usuário " + UserTableName + ": ";
            if (oUserTableMD.GetByKey(UserTableName))
            {
                CodErro = oUserTableMD.Remove();
                if (CodErro != 0)
                {
                    oCompany.GetLastError(out CodErro, out MsgErro);
                    Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
                }
                else
                {
                    MsgErro = "";
                    Retorno += "OK";
                }
                return Retorno;
            }
            else
            {
                CodErro = 0;
                MsgErro = "";
                Retorno += "OK";
            }

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")";
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            GC.Collect();
            tbxLog.AppendText("\r\n" + UserTableName + " " + Retorno);
            return Retorno;
        }

        public static string CreateUserField(string TableName, string FieldName, string FieldDescription, SAPbobsCOM.BoFieldTypes oType, SAPbobsCOM.BoFldSubTypes oSubType, int FieldSize, ref TextBox tbxLog, string DefaultValue = null)
        {
            return CreateUserField(TableName, FieldName, FieldDescription, oType, oSubType, FieldSize, false, ref tbxLog, DefaultValue);
        }

        public static string CreateUserField(string TableName, string FieldName, string FieldDescription, SAPbobsCOM.BoFieldTypes oType, SAPbobsCOM.BoFldSubTypes oSubType, int FieldSize, bool oMandadory, ref TextBox tbxLog, string DefaultValue = null)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));
            //int lRetCode = 0;
            bool bUpdate;

            string Retorno = " - Criação/Atualização do Campo " + TableName + "." + FieldName + ": ";

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            string Sql = $@"SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{TableName}' AND ""AliasID"" = '{FieldName}'";
            string FieldID = QueryForValue(Sql);

            if (FieldID != null)
            {
                bUpdate = oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldID));
            }
            else
                bUpdate = false;

            oUserFieldsMD.TableName = TableName;
            oUserFieldsMD.Name = FieldName;
            oUserFieldsMD.Description = FieldDescription;
            oUserFieldsMD.Type = oType;
            oUserFieldsMD.SubType = oSubType;
            oUserFieldsMD.Mandatory = oMandadory ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

            if (!String.IsNullOrEmpty(DefaultValue))
            {
                oUserFieldsMD.DefaultValue = DefaultValue;
            }

            if (FieldSize > 0)
                oUserFieldsMD.EditSize = FieldSize;

            if (bUpdate)
            {
                if (!OnlyAdd)
                {
                    CodErro = oUserFieldsMD.Update();
                }
                else
                {
                    CodErro = 0;
                }
            }
            else
            {
                CodErro = oUserFieldsMD.Add();
            }

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")";
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            tbxLog.AppendText("\r\n" + FieldName + " " + Retorno);
            return Retorno;
        }

        public static string SetUserFieldLinkedUDO(string TableName, string FieldName, string linkedUDO, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            string Retorno = " - Definição de Link para UDO do Campo " + TableName + "." + FieldName + ": ";

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            string Sql = $@"SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{TableName}' AND ""AliasID"" = '{FieldName}'";
            string FieldID = QueryForValue(Sql);

            oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldID));
            oUserFieldsMD.LinkedUDO = linkedUDO;
            CodErro = oUserFieldsMD.Update();

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            tbxLog.AppendText(FieldName + " " + Retorno);
            return Retorno;
        }

        public static string SetUserFieldLinkedTable(string TableName, string FieldName, string linkedTable, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            string Retorno = " - Definição de Link para UDO do Campo " + TableName + "." + FieldName + ": ";

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            string Sql = $@"SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{TableName}' AND ""AliasID"" = '{FieldName}'";
            string FieldID = QueryForValue(Sql);

            oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldID));
            oUserFieldsMD.LinkedTable = linkedTable;
            CodErro = oUserFieldsMD.Update();

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            tbxLog.AppendText(FieldName + " " + Retorno);
            return Retorno;
        }

        public static string RemoveUserField(string TableName, string FieldName, out int CodErro, out string MsgErro)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));
            //int lRetCode = 0;
            bool bUpdate;

            string Retorno = " - Remoção do Campo " + TableName + "." + FieldName + ": ";

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            //if (!TableName.StartsWith("@"))
            //    TableName = "@" + TableName;

            string Sql = $@"SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{TableName}' AND ""AliasID"" = '{FieldName}'";
            string FieldID = QueryForValue(Sql);

            if (FieldID != null)
            {
                bUpdate = oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldID));
                CodErro = oUserFieldsMD.Remove();
                if (CodErro != 0)
                {
                    oCompany.GetLastError(out CodErro, out MsgErro);
                    Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
                }
                else
                {
                    MsgErro = "";
                    Retorno += "OK";
                }
            }
            else
            {
                MsgErro = "";
                CodErro = 0;
                Retorno += " Tabela/Campo não encontrado " + Environment.NewLine;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();

            return Retorno;
        }

        public static string AddValidValueToUserField(string TableName, string FieldName, string Value, string Description, ref TextBox tbxLog)
        {
            // se não foi passado o parâmetro de "É Valor Padrão" trata como não
            // chamando a função que realmente insere o valor como "false" a variável IsDefault
            return AddValidValueToUserField(TableName, FieldName, Value, Description, false, ref tbxLog);
        }

        public static string AddValidValueToUserField(string TableName, string FieldName, string Value, string Description, bool IsDefault, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));
            //int lRetCode = 0;
            bool bUpdate;

            string Retorno = " - Criação de Valor válido " + TableName + "." + FieldName + ": ";

            string Sql = $@"SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{TableName}' AND ""AliasID"" = '{FieldName.Replace("U_", "")}' ";
            string FieldID = QueryForValue(Sql);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            GC.Collect();
            oUserFieldsMD = null;
            oUserFieldsMD = ((SAPbobsCOM.UserFieldsMD)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)));

            bUpdate = oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldID));

            Sql = $@"SELECT COUNT(1) 
                       FROM CUFD 
                      INNER JOIN UFD1 ON CUFD.""TableID"" = UFD1.""TableID"" 
                        AND CUFD.""FieldID"" = UFD1.""FieldID""
                      WHERE CUFD.""TableID"" = '{TableName}' 
                        AND CUFD.""AliasID"" = '{FieldName.Replace("U_", "")}' 
                        AND LENGTH(UFD1.""FldValue"") > 0";

            string ContaValoresValidos = QueryForValue(Sql);

            if (Convert.ToInt32(ContaValoresValidos) > 0)
            {

                Sql = $@"SELECT UFD1.""IndexID""
                           FROM CUFD
                          INNER JOIN UFD1 ON CUFD.""TableID"" = UFD1.""TableID""
                            AND CUFD.""FieldID"" = UFD1.""FieldID"" 
                          WHERE CUFD.""TableID"" = '{TableName}' 
                            AND CUFD.""AliasID"" = '{FieldName.Replace("U_", "")}' 
                            AND UFD1.""FldValue"" = '{Value}'";

                string IndexId = QueryForValue(Sql);

                if (IndexId == null)
                    oUserFieldsMD.ValidValues.Add();

                if (IndexId != null)
                    oUserFieldsMD.ValidValues.SetCurrentLine(Convert.ToInt32(IndexId));
            }

            oUserFieldsMD.ValidValues.Value = Value;
            oUserFieldsMD.ValidValues.Description = Description;
            if (IsDefault)
                oUserFieldsMD.DefaultValue = Value;

            if (!OnlyAdd)
            {
                CodErro = oUserFieldsMD.Update();
            }
            else
            {
                CodErro = 0;
            }

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")";
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);

            GC.Collect();
            tbxLog.AppendText(Environment.NewLine + FieldName + " " + Retorno);
            return Retorno;
        }

        public static string CreateUserObject(string ObjectName, string ObjectDesc, string TableName,
            BoUDOObjType ObjectType, bool CanArchive, bool CanCancel, bool CanClose, bool CanCreateDefaultForm,
            bool CanDelete, bool CanFind, bool CanLog, bool CanYearTransfer, bool ManageSeries, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            // se não preenchido um table name separado, usa o mesmo do objeto
            if (TableName.Length == 0)
                TableName = ObjectName;

            string Retorno = " - Criação/Atualização do Objeto de usuário " + ObjectName + ": ";

            var UserObjectsMD = (UserObjectsMD)oCompany.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            // Remove a arroba do usertable Name
            TableName = TableName.Replace("@", "");

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            UserObjectsMD.Code = ObjectName;
            UserObjectsMD.Name = ObjectDesc;
            UserObjectsMD.ObjectType = ObjectType;
            UserObjectsMD.TableName = TableName;
            //UserObjectsMD.CanArchive = GetSapBoolean(CanArchive);
            UserObjectsMD.CanCancel = GetSapBoolean(CanCancel);
            UserObjectsMD.CanClose = GetSapBoolean(CanClose);
            UserObjectsMD.CanCreateDefaultForm = GetSapBoolean(CanCreateDefaultForm);
            UserObjectsMD.CanDelete = GetSapBoolean(CanDelete);
            UserObjectsMD.CanFind = GetSapBoolean(CanFind);
            UserObjectsMD.CanLog = GetSapBoolean(CanLog);
            if (CanLog) UserObjectsMD.LogTableName = String.Format("A{0}", TableName);
            UserObjectsMD.CanYearTransfer = GetSapBoolean(CanYearTransfer);
            UserObjectsMD.ManageSeries = GetSapBoolean(ManageSeries);

            if (UserObjectsMD.CanFind == BoYesNoEnum.tYES)
            {
                var userTable = oCompany.UserTables.Item(TableName);

                switch (UserObjectsMD.ObjectType)
                {
                    case BoUDOObjType.boud_Document:
                        UserObjectsMD.FindColumns.ColumnAlias = "DocEntry";
                        UserObjectsMD.FindColumns.Add();
                        break;
                    case BoUDOObjType.boud_MasterData:
                        UserObjectsMD.FindColumns.ColumnAlias = "Code";
                        UserObjectsMD.FindColumns.Add();
                        UserObjectsMD.FindColumns.ColumnAlias = "Name";
                        UserObjectsMD.FindColumns.Add();
                        break;
                }

                for (int i = 0; i < userTable.UserFields.Fields.Count; i++)
                {
                    UserObjectsMD.FindColumns.ColumnAlias = userTable.UserFields.Fields.Item(i).Name;
                    UserObjectsMD.FindColumns.Add();
                }
            }

            if (bUpdate)
            {
                if (!OnlyAdd)
                {
                    CodErro = UserObjectsMD.Update();
                }
                else
                {
                    CodErro = 0;
                }
            }
            else
            {
                CodErro = UserObjectsMD.Add();
            }

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            Marshal.ReleaseComObject(UserObjectsMD);
            GC.Collect();
            tbxLog.Text += Environment.NewLine + ObjectName + " " + Retorno;
            return Retorno;
        }

        public static string RemoveUserObject(string ObjectName, out int CodErro, out string MsgErro)
        {
            string Retorno = " - Remoção de objeto " + ObjectName + ": ";

            SAPbobsCOM.UserObjectsMD UserObjectsMD = (SAPbobsCOM.UserObjectsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);

            CodErro = 0;
            if (bUpdate)
                CodErro = UserObjectsMD.Remove();

            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")" + Environment.NewLine;
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(UserObjectsMD);
            GC.Collect();

            return Retorno;
        }

        public static string AddChildTableToUserObject(string ObjectName, string ChildTableName, ref TextBox tbxLog)
        {
            // se não preenchido um table name separado, usa o mesmo do objeto
            int CodErro;
            string Retorno = " - Inserção de tabela filha (" + ChildTableName + ") ao objeto " + ObjectName + ": ";

            SAPbobsCOM.UserObjectsMD UserObjectsMD = (SAPbobsCOM.UserObjectsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);

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
            }

            CodErro = !OnlyAdd ? UserObjectsMD.Update() : 0;

            if (CodErro != 0)
            {
                Retorno += "FALHA (" + oCompany.GetLastErrorDescription() + ")";
            }
            else
            {
                Retorno += "OK";
            }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(UserObjectsMD);
            GC.Collect();
            tbxLog.Text += Environment.NewLine + ObjectName + @" " + Retorno;
            return Retorno;
        }

        public static string QueryForValue(string Sql)
        {
            SAPbobsCOM.Recordset oRecordset = (SAPbobsCOM.Recordset)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset));
            string Retorno = null;
            try
            {
                oRecordset.DoQuery(Sql);

                // Executa e, caso exista ao menos um registro, devolve o mesmo.
                // retorna sempre o primeiro campo da consulta (SEMPRE)
                if (!oRecordset.EoF)
                {
                    Retorno = oRecordset.Fields.Item(0).Value.ToString();
                }

            }

            catch
            {

            }

            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                GC.Collect();
            }

            return Retorno;
        }

        public static string CreateUserKey(string KeyName, string TableName, string Fields, bool isUnique, ref TextBox tbxLog)
        {
            int CodErro;
            string MsgErro;
            SAPbobsCOM.UserKeysMD oUserKeysMD = (SAPbobsCOM.UserKeysMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserKeys);

            oUserKeysMD.TableName = TableName;
            oUserKeysMD.KeyName = KeyName;

            string[] arrAux = Fields.Split(Convert.ToChar(","));
            for (int i = 0; i < arrAux.Length; i++)
            {
                if (i > 0)
                    oUserKeysMD.Elements.Add();

                oUserKeysMD.Elements.ColumnAlias = arrAux[i].Trim();

            }

            oUserKeysMD.Unique = GetSapBoolean(isUnique);

            string Retorno = "";

            CodErro = oUserKeysMD.Add();
            if (CodErro != 0)
            {
                oCompany.GetLastError(out CodErro, out MsgErro);
                Retorno += "FALHA (" + MsgErro + ")";
            }
            else
            {
                MsgErro = "";
                Retorno += "OK";
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserKeysMD);
            GC.Collect();
            tbxLog.Text += Environment.NewLine + KeyName + " ";
            return Retorno;
        }

        public static SAPbobsCOM.BoYesNoEnum GetSapBoolean(bool Variavel)
        {
            if (Variavel)
                return SAPbobsCOM.BoYesNoEnum.tYES;
            else
                return SAPbobsCOM.BoYesNoEnum.tNO;

        }
    }
}
