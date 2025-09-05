namespace App.Repository.Services
{
    using System;
    using SAPbobsCOM;
    using SAPbouiCOM;
    using System.Runtime.InteropServices;

    public sealed class DIExecuteService
    {
        
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            string returnValue = System.Text.Encoding.Unicode.GetString(encodedDataAsBytes);

            return returnValue;
        }
    
        public static System.Data.DataTable ExecuteSqlDataTable(string Query)
        {
            try
            {
                System.Data.DataTable oDT = new System.Data.DataTable();
                Recordset oRS = (Recordset)AddonService.diCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Query);
                if (!oRS.EoF)
                {
                    oRS.MoveFirst();
                    for (int i = 0; i < oRS.Fields.Count; i++)
                    {
                        oDT.Columns.Add(oRS.Fields.Item(i).Name);
                    }
                    System.Data.DataRow oDR;
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

                Marshal.ReleaseComObject(oRS);
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
                var oRS = (Recordset)AddonService.diCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Query);
                if (!oRS.EoF)
                {
                    if (oRS.Fields.Item(0).IsNull() == BoYesNoEnum.tNO)
                        obj = oRS.Fields.Item(0).Value;
                }

                Marshal.ReleaseComObject(oRS);
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

        public static System.Data.DataTable ExecuteSqlXml(string Query)
        {
            Recordset oRS = null;
            System.Data.DataTable oDT = new System.Data.DataTable();
            System.Data.DataSet oDS = null;
            System.IO.Stream vStream = null;

            try
            {
                oDT = new System.Data.DataTable();
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                oRS = (Recordset)AddonService.diCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Query);

                var vVal = oRS.GetAsXML();

                if (oRS.RecordCount > 0)
                {
                    using (oDS = new System.Data.DataSet("DataValue"))
                    {
                        using (vStream = new System.IO.MemoryStream())
                        {
                            xmlDoc.LoadXml(vVal);
                            xmlDoc.Save(vStream);
                            vStream.Position = 0;
                            oDS.ReadXml(vStream);
                        }

                        oDT = oDS.Tables["Row"];
                    }
                }

                return oDT;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Marshal.ReleaseComObject(oRS);
                oRS = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}


