using CVA.ImportacaoLCM.Controller;
using CVA.ImportacaoLCM.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.BLL
{
     public class UserFieldsBLL
    {
         public static void CreateUserFields()
         {
             UserObjectController userObjectController = new UserObjectController();
             string[] findColumns;

             if (GetFieldByName("CVA_ContaDominio", "OACT") == -1)
             {
                 userObjectController.InsertUserField("OACT", "CVA_ContaDominio", "Conta contábil domínio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 150);
             }
             if (GetFieldByName("CVA_ParceiroNegocio", "OACT") == -1)
             {
                 userObjectController.InsertUserField("OACT", "CVA_ParceiroNegocio", "Parceiro de Negócio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 150);
             }
             if (GetFieldByName("CVA_CcustoDominio", "OOCR") == -1)
             {
                 userObjectController.InsertUserField("OOCR", "CVA_CcustoDominio", "Centro de custo domínio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 150);
             }

         }
         public static bool ExisteTB(string TBName)
         {
             SAPbobsCOM.UserTablesMD oUserTable;
             oUserTable = (SAPbobsCOM.UserTablesMD)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
             //UserTablesMD oUserTable = new UserTablesMD(ref oDiCompany);            
             bool ret = oUserTable.GetByKey(TBName);
             int errCode; string errMsg;
             SBOApp.oCompany.GetLastError(out errCode, out errMsg);

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
             SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
             SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

             try
             {
                string sqlQuery = string.Empty;
                if (SBOApp.oCompany.DbServerType == BoDataServerTypes.dst_HANADB)
                    sqlQuery = string.Format(@"SELECT T0.""TableID"", T0.""FieldID"" FROM ""CUFD"" T0 WHERE T0.""TableID"" = '{0}' AND T0.""AliasID"" = '{1}'", userTable, Name);
                else
                    sqlQuery = string.Format("SELECT T0.TableID, T0.FieldID FROM CUFD T0 WHERE T0.TableID = '{0}' AND T0.AliasID = '{1}'", userTable, Name);

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
