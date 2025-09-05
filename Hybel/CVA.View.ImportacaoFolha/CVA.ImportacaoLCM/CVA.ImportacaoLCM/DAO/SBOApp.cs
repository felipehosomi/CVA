using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.DAO
{
    public class SBOApp
    {
        #region
        public static SAPbouiCOM.SboGuiApi oSboGuiApi = null;
        public static SAPbouiCOM.Application SBO_Application = null;
        public static SAPbobsCOM.Company oCompany = null;



        public long lRetErr;
        public string sErrMsg;
        public int iErrCode;

        #endregion

        public SBOApp()
        {
            try
            {
                SetApplicationUI();

            }
            catch
            {
                throw;
            }
        }

        public void SetApplicationUI()
        {


            try
            {

                oCompany = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
                string sCookie = string.Empty;
                string sInfo = string.Empty;
                sCookie = oCompany.GetContextCookie();
                sInfo = SAPbouiCOM.Framework.Application.SBO_Application.Company.GetConnectionContext(sCookie);

                iErrCode = oCompany.GetLastErrorCode();
                if (iErrCode == 0)
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Conectado a Empresa: " + oCompany.CompanyName, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    oCompany.GetLastError(out iErrCode, out sErrMsg);
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Erro: " + sErrMsg + " - CODE: " + Convert.ToString(iErrCode), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string TranslateToHana(string sql)
        {

            return sql;

        }
        public static DataTable ExecuteSqlDataTable(string Query)
        {
            try
            {
                DataTable oDT = new DataTable();
                SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRS.DoQuery(Query);
                if (!oRS.EoF)
                {
                    oRS.MoveFirst();
                    for (int i = 0; i < oRS.Fields.Count; i++)
                    {
                        oDT.Columns.Add(oRS.Fields.Item(i).Name);
                    }
                    DataRow oDR;
                    for (int x = 0; x < oRS.RecordCount; x++)
                    {
                        oDR = oDT.NewRow();

                        for (int i = 0; i < oRS.Fields.Count; i++)
                        {
                            oDR[i] = Convert.ToString(oRS.Fields.Item(i).Value);
                        }
                        oDT.Rows.Add(oDR);
                        oRS.MoveNext();
                    }
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                return oDT;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static object ExecuteSqlScalar(string Query)
        {
            try
            {
                object obj = null;
                SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRS.DoQuery(Query);
                if (!oRS.EoF)
                {
                    obj = oRS.Fields.Item(0).Value;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
