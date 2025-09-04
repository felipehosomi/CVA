using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Apetit.Servico.Consolidacao.Class
{
    class Conexao
    {
        public static SAPbobsCOM.Company oCompany;

        //==================================================================================================================================//
        static public Boolean ConectarB1()
        //==================================================================================================================================//
        {
            int lErrCode = 0;
            string sErrMsg = "";
            Boolean conectado = false, useTrusted = true;
            SAPbobsCOM.BoDataServerTypes dbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;

            try
            {
                oCompany = new SAPbobsCOM.Company();

                oCompany.SLDServer = Class.Acesso.SLDServer;
                oCompany.CompanyDB = Class.Acesso.CompanyDB;
                oCompany.UserName = Class.Acesso.UserName;
                oCompany.Password = Class.Acesso.Password;
                oCompany.Server = Class.Acesso.Server;
                //oCompany.DbUserName = Class.Acesso.DbUserName;
                //oCompany.DbPassword = Class.Acesso.DbPassword; 
                oCompany.UseTrusted = false;
                oCompany.DbServerType = dbServerType;
                if (Class.Acesso.Language == "SP")
                    oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
                else if (Class.Acesso.Language == "PT")
                    oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br;
                else if (Class.Acesso.Language == "EN")
                    oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;

                lErrCode = oCompany.Connect();
                if (lErrCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    throw new Exception(sErrMsg);
                }
                if (oCompany.Connected)
                {
                    conectado = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return conectado;
        }

        //==================================================================================================================================//
        public static object ExecuteSqlObj(string Query)
        //==================================================================================================================================//
        {
            try
            {
                object obj = null;

                if ((oCompany == null) || (!oCompany.Connected))
                    Class.Conexao.ConectarB1();

                if (oCompany.Connected)
                {

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
                }
                else
                {
                    throw new Exception("Sem conexão com o B1");
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static System.Data.DataTable ExecuteSqlDataTable(string Query)
        {
            System.Data.DataTable table2;
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                Recordset businessObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                businessObject.DoQuery(Query);
                if (!businessObject.EoF)
                {
                    businessObject.MoveFirst();
                    int index = 0;
                    while (index < businessObject.Fields.Count)
                    {
                        table.Columns.Add(businessObject.Fields.Item(index).Name);
                        index++;
                    }
                    for (int i = 0; i < businessObject.RecordCount; i++)
                    {
                        DataRow row = table.NewRow();
                        for (index = 0; index < businessObject.Fields.Count; index++)
                        {
                            row[index] = Convert.ToString((dynamic)businessObject.Fields.Item(index).Value);
                        }
                        table.Rows.Add(row);
                        businessObject.MoveNext();
                    }
                }
                //Marshal.ReleaseComObject(businessObject);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                table2 = table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table2;
        }

        public static object ExecuteSqlScalar(string Query)
        {
            object obj3;
            try
            {
                object obj2 = null;
                Recordset businessObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                businessObject.DoQuery(Query);
                if (!businessObject.EoF)
                {
                    obj2 = businessObject.Fields.Item(0).Value;
                }
                //Marshal.ReleaseComObject(businessObject);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                obj3 = obj2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj3;
        }

        public static void ExecuteSqlNonQuery(string command)
        {
            try
            {
                var recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery(command);
                
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //==================================================================================================================================//
        static public void gravaLog(string msg)
        //==================================================================================================================================//
        {
            try
            {
                StreamWriter sw;
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
                string str3 = path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!File.Exists(str3))
                    sw = File.CreateText(str3);
                else
                    sw = File.AppendText(str3);

                if (msg != "")
                    sw.WriteLine(String.Format(new CultureInfo("pt-BR"), "[" + DateTime.Now.ToString() + "] - " + msg));
                else
                    sw.WriteLine();

                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }


    }
}
