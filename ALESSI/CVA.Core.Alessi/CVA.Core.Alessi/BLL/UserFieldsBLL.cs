using CVA.AddOn.Common;
using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.IO;

namespace CVA.Core.Alessi.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            if (GetFieldByName("CVA_Frete_Estimado", "INV1") == -1)
            {
                userObjectController.InsertUserField("INV1", "CVA_Frete_Estimado", "Frete Estimado", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 254);
            }
            if (GetFieldByName("CodRed", "OACT") == -1)
            {
                userObjectController.InsertUserField("OACT", "CodRed", "CodRed", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            }
            if (GetFieldByName("PN_CodRed", "OCRD") == -1)
            {
                userObjectController.InsertUserField("OCRD", "PN_CodRed", "PN_CodRed", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            }
            if (GetFieldByName("CVA_ForaMultiplo", "ORDR") == -1)
            {
                userObjectController.InsertUserField("ORDR", "CVA_ForaMultiplo", "Permite fora multiplo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.AddValidValueToUserField("ORDR", "CVA_ForaMultiplo", "1", "Sim");
                userObjectController.AddValidValueToUserField("ORDR", "CVA_ForaMultiplo", "2", "Não", true);
            }

            if (!ExisteTB("CVA_MKT_NF"))
            {
                userObjectController.CreateUserTable("CVA_MKT_NF", "CVA - Tipos Marketing NF", BoUTBTableType.bott_NoObject);
                userObjectController.InsertUserField("@CVA_MKT_NF", "Margem", "Margem", BoFieldTypes.db_Float, BoFldSubTypes.st_Rate, 10);
            }

            #region CVA_IMP_DEFAULT
            if (!ExisteTB("CVA_IMP_DEFAULT"))
            {
                userObjectController.CreateUserTable("CVA_IMP_DEFAULT", "CVA - Valores Default", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_IMP_DEFAULT", "Layout", "Nome Layout", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, null, true);
                userObjectController.InsertUserField("@CVA_IMP_DEFAULT", "ObjType", "Tipo Objeto", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);

                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "ObjType", "17", "Pedido Venda");
                userObjectController.InsertUserField("@CVA_IMP_DEFAULT", "DI_Obj", "Objeto da DI", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true, "1");
                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "DI_Obj", "1", "Cabeçalho");
                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "DI_Obj", "2", "Linha");

                userObjectController.InsertUserField("@CVA_IMP_DEFAULT", "DI_Type", "Tipo Campo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "DI_Type", "1", "Alfanumérico", true);
                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "DI_Type", "2", "Data");
                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "DI_Type", "3", "Inteiro");
                userObjectController.AddValidValueToUserField("@CVA_IMP_DEFAULT", "DI_Type", "4", "Decimal");

                userObjectController.InsertUserField("@CVA_IMP_DEFAULT", "DI_Field", "Nome Campo DI", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, null, true);
                userObjectController.InsertUserField("@CVA_IMP_DEFAULT", "Value", "Valor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, null, true);

                userObjectController.CreateUserObject("CVA_IMP_DEFAULT", "Valores Default", "@CVA_IMP_DEFAULT", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("@CVA_IMP_DEFAULT");
            }
            #endregion

            #region CVA_IMPORT_LOG
            if (!ExisteTB("CVA_IMPORT_LOG"))
            {

                userObjectController.CreateUserTable("CVA_IMPORT_LOG", "CVA - Log Importação", BoUTBTableType.bott_NoObject);

                userObjectController.InsertUserField("@CVA_IMPORT_LOG", "Date", "Data Importação", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_IMPORT_LOG", "Hour", "Hora Importação", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);
                userObjectController.InsertUserField("@CVA_IMPORT_LOG", "User", "Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
                userObjectController.InsertUserField("@CVA_IMPORT_LOG", "Line", "Linha arquivo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_IMPORT_LOG", "Description", "Descrição", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 1000);

            }
            #endregion

            #region CVA_DOC_MAP / CVA_DOC_MAP_ITEM
            if (!ExisteTB("CVA_DOC_MAP"))
            {

                userObjectController.CreateUserTable("CVA_DOC_MAP", "CVA - Mapeamento Documentos", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_DOC_MAP", "Layout", "Nome Layout", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, null, true, "1");
                userObjectController.InsertUserField("@CVA_DOC_MAP", "ObjType", "Tipo Objeto", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP", "ObjType", "17", "Pedido Venda");

                userObjectController.InsertUserField("@CVA_DOC_MAP", "Identifier", "Identificador Linha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);

                userObjectController.InsertUserField("@CVA_DOC_MAP", "LineType", "Tipo Registro", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP", "LineType", "1", "Cabeçalho");
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP", "LineType", "2", "Linha");
                //userObjectController.AddValidValueToUserField("@CVA_DOC_MAP", "DI_Object", "3", "Lote");
                //userObjectController.AddValidValueToUserField("@CVA_DOC_MAP", "DI_Object", "4", "Série");
                //userObjectController.AddValidValueToUserField("@CVA_DOC_MAP", "DI_Object", "5", "Parcela");

                userObjectController.CreateUserObject("CVA_DOC_MAP", "Mapeamento Documentos", "@CVA_DOC_MAP", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("@CVA_DOC_MAP");

                userObjectController.CreateUserTable("CVA_DOC_MAP_ITEM", "CVA - Mapeamento Campos", BoUTBTableType.bott_MasterDataLines);
                userObjectController.AddChildTableToUserObject("CVA_DOC_MAP", "CVA_DOC_MAP_ITEM");

                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "DI_Field", "Nome Campo DI", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, null, true);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "DI_Type", "Tipo Campo DI", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP_ITEM", "DI_Type", "1", "Alfanumérico");
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP_ITEM", "DI_Type", "2", "Data");
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP_ITEM", "DI_Type", "3", "Inteiro");
                userObjectController.AddValidValueToUserField("@CVA_DOC_MAP_ITEM", "DI_Type", "4", "Decimal");

                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "From", "Posição De", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "To", "Posição Até", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "Size", "Tamanho", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, null, true);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "Decimal", "Casas decimais", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "Format", "Formato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "Parameter", "Nome Parâmetro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
                userObjectController.InsertUserField("@CVA_DOC_MAP_ITEM", "Query", "Consulta", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 1000);
            }
            #endregion

            #region CVA_NFE_FRETE
            if (!ExisteTB("CVA_NFE_FRETE"))
            {

                userObjectController.CreateUserTable("CVA_NFE_FRETE", "CVA - Frete Nota Entrada", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_NFE_FRETE", "DocEntry", "ID Nota Entrada", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_NFE_FRETE", "DocNum", "Nr. Nota Entrada", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

                userObjectController.CreateUserObject("CVA_NFE_FRETE", "CVA - Frete Nota Entrada", "@CVA_NFE_FRETE", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("@CVA_IMP_DEFAULT");

                userObjectController.CreateUserTable("CVA_NFE_FRETE_ITEM", "CVA - Frete - NF's", BoUTBTableType.bott_MasterDataLines);
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "DocDate", "Data NF Saída", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "DocNum", "Nr. Doc Nota Saída", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20); // Alpha para funcionar o CFL
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "Serial", "Nr. Nota Saída", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "CardCode", "Cód. PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "CardName", "Nome PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "DocTotal", "Valor NF Saída", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
                userObjectController.InsertUserField("@CVA_NFE_FRETE_ITEM", "ShipCode", "Transportadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);

                userObjectController.AddChildTableToUserObject("CVA_NFE_FRETE", "CVA_NFE_FRETE_ITEM");
            }
            #endregion


            #region CVA_Juros

            if (!ExisteTB("CVA_Juros"))
            {
                userObjectController.CreateUserTable("CVA_JUROS", "CVA - Juros", BoUTBTableType.bott_NoObject);
                userObjectController.InsertUserField("@CVA_JUROS", "Juros", "Juros", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Percentage, 10);
            }
            #endregion

            //StreamWriter sw = new StreamWriter("c:\\CVA Consultoria\\log.txt");
            //sw.WriteLine(userObjectController.Log);
            //sw.Close();
        }
        public static bool ExisteTB(string TBName)
        {
            SAPbobsCOM.UserTablesMD oUserTable;
            oUserTable = (SAPbobsCOM.UserTablesMD)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            //UserTablesMD oUserTable = new UserTablesMD(ref oDiCompany);            
            bool ret = oUserTable.GetByKey(TBName);
            int errCode; string errMsg;
            SBOApp.Company.GetLastError(out errCode, out errMsg);

            TBName = null;
            errMsg = null;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTable);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return (ret);
        }
        public static int GetFieldByName(string Name, string userTable)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {


                string sqlQuery = string.Format("SELECT T0.TableID, T0.FieldID FROM CUFD T0 WHERE T0.TableID = '{0}' AND T0.AliasID = '{1}'", userTable, Name);

                oRecordSet.DoQuery(sqlQuery);

                if (oRecordSet.RecordCount == 1)
                {
                    return Convert.ToInt32(oRecordSet.Fields.Item("FieldID").Value);
                }
                else
                {
                    return -1;
                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
            }
        }
    }
}
