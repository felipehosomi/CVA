using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SAPbobsCOM;
using App.Repository.Services;

namespace App.ApplicationServices.Addon
{
    public class FormattedSearchService
    {
        public FormattedSearchService()
        {
        }

        public int RetornaKeyFormattedSearches(string FormUID ,string ItemUID, string ColumnID) 
        {
        string  strSql;
        int Ret;
     
        try 
	        {
                strSql = "SELECT DISTINCT \"IndexID\"";
                strSql += " FROM CSHS";
                strSql += " WHERE \"FormID\" = '" + FormUID + "' AND" ;
                strSql += " \"ItemID\" = '" + ItemUID + "' AND \"ColID\" = '" + ColumnID +"'";
                string sRet = Convert.ToString(AddonService.ExecuteSqlScalar(strSql));
                if (sRet != "")
                {
                    Ret = Convert.ToInt32( sRet);
                }
                else
                {
                    Ret = 0;
                }               

                if (Ret > 0)
                   return Ret;
                else
                  return -1;
    	    }
	        catch (Exception)
	        {
		
		        throw;
	        }
        }

        public int RetornaQueryId(int Category, string QueryName)
        {
            //SAPbobsCOM.Recordset oRsDados; 
            string strSql;
            int Ret;
            try
            {
                //oRsDados = (SAPbobsCOM.Recordset)di.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                strSql = $"SELECT \"IntrnalKey\" FROM OUQR WHERE \"QCategory\" = {Category} AND \"QName\" = '{QueryName}'";
                //strSql = "SELECT INTRNALKEY";
                //strSql += " FROM OUQR";
                //strSql += " WHERE QCATEGORY = " + Category + " AND";
                //strSql += " QNAME = '" + QueryName + "'";

                //oRsDados.DoQuery(strSql);
                string sRet = Convert.ToString(AddonService.ExecuteSqlScalar(strSql));
                if (sRet != "")
                {
                    Ret = Convert.ToInt32(sRet);
                }
                else
                {
                    Ret = 0;
                }
                //Ret =Convert.ToInt32(oRsDados.Fields.Item("INTRNALKEY").Value);

                return Ret;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DelFormattedSearch(string QueryName, int QueryCategory)
        {
            
            UserQueries oUserQueries;
            int iRet;
            string errMessage;
            int QueryKey;            
            
            try
            {
                oUserQueries = (SAPbobsCOM.UserQueries)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                QueryKey = RetornaQueryId(QueryCategory, QueryName);

                if (QueryKey > 0)
                {
                    oUserQueries.GetByKey(QueryKey, QueryCategory);
                    iRet = oUserQueries.Remove();

                    if (iRet != 0)
                    {
                        AddonService.diCompany.GetLastError(out iRet, out errMessage);
                        throw new Exception(errMessage);
                    }
                }

                if (oUserQueries != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserQueries);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateFormattedSearches(string Query, string QueryName, int QueryCategory, string FormID, string ItemID, string ColumnID,BoYesNoEnum oRefresh,string FieldID)
        {

            FormattedSearches oFormattedSearch;
            UserQueries oUserQueries;
            int lRet;
            string errMessage;
            int QueryKey;
            int FormatKey;
            bool bRet = true;
            try
            {

                oUserQueries = (SAPbobsCOM.UserQueries)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                QueryKey = RetornaQueryId(QueryCategory, QueryName);

                if (QueryKey > 0)
                {
                    oUserQueries.GetByKey(QueryKey, QueryCategory);
                    oUserQueries.Query = Query;
                    oUserQueries.QueryCategory = QueryCategory;
                    oUserQueries.QueryDescription = QueryName;
                    lRet = oUserQueries.Update();
                }
                else
                {
                    oUserQueries.Query = Query;
                    oUserQueries.QueryCategory = QueryCategory;
                    oUserQueries.QueryDescription = QueryName;
                    lRet = oUserQueries.Add();
                }

                if (lRet != 0)
                {
                    bRet = false;
                    AddonService.diCompany.GetLastError(out lRet, out errMessage);
                    throw new Exception(errMessage);
                }

                oFormattedSearch = (SAPbobsCOM.FormattedSearches)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);
                FormatKey = RetornaKeyFormattedSearches(FormID, ItemID, ColumnID);

                if (FormatKey != -1)
                {
                    oFormattedSearch.GetByKey(FormatKey);
                }

                
                oFormattedSearch.Action = BoFormattedSearchActionEnum.bofsaQuery;
                oFormattedSearch.FormID = FormID;
                oFormattedSearch.ItemID = ItemID;
                oFormattedSearch.ColumnID = ColumnID;//"-1";
                oFormattedSearch.QueryID = RetornaQueryId(QueryCategory, QueryName);

                if (oRefresh == BoYesNoEnum.tYES)
                {
                    oFormattedSearch.ByField = BoYesNoEnum.tYES;
                    oFormattedSearch.Refresh = oRefresh;
                    oFormattedSearch.FieldID = FieldID;
                }
                else if (oRefresh == BoYesNoEnum.tNO && !FieldID.IsNullOrEmpty())
                {
                    oFormattedSearch.ByField = oRefresh;
                    oFormattedSearch.Refresh = BoYesNoEnum.tYES;
                    oFormattedSearch.FieldID = FieldID;
                }
               
                if (FormatKey == -1)
                {
                    lRet = oFormattedSearch.Add();
                }
                else
                {
                    lRet = oFormattedSearch.Update();
                }

                if (lRet != 0)
                {
                    bRet = false;
                    AddonService.diCompany.GetLastError(out lRet, out errMessage);
                    throw new Exception(errMessage);
                }

                if (oFormattedSearch != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFormattedSearch);
                }
                if (oUserQueries != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserQueries);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                return bRet;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool CreateSearches(string Query, string QueryName, int QueryCategory)
        {
            bool bRet = true;
            int lRet;
            string errMessage;
            int QueryKey;
            UserQueries oUserQueries;

            try
            {
                oUserQueries = (SAPbobsCOM.UserQueries)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                QueryKey = RetornaQueryId(QueryCategory, QueryName);

                if (QueryKey > 0)
                {
                    oUserQueries.GetByKey(QueryKey, QueryCategory);
                    oUserQueries.Query = Query;
                    oUserQueries.QueryCategory = QueryCategory;
                    oUserQueries.QueryDescription = QueryName;
                    lRet = oUserQueries.Update();
                }
                else
                {
                    oUserQueries.Query = Query;
                    oUserQueries.QueryCategory = QueryCategory;
                    oUserQueries.QueryDescription = QueryName;
                    lRet = oUserQueries.Add();
                }

                if (lRet != 0)
                {
                    bRet = false;
                    AddonService.diCompany.GetLastError(out lRet, out errMessage);
                    throw new Exception(errMessage);
                }
                return bRet;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public int CreateCategory(string NomeCategoria)
        {
            try
            {
                //verifica se existe a categoria
                string sQuery;
                sQuery = "select \"CategoryId\" from oqcn where \"CatName\" = '" + NomeCategoria + "'";
                string catID = Convert.ToString(AddonService.ExecuteSqlScalar(sQuery));
                //se não existe cria a categoria
                if (catID.Trim() != "")
                {
                    return Convert.ToInt32(catID.Trim());
                }
                else
                {
                    SAPbobsCOM.QueryCategories oQCat;
                    oQCat = (SAPbobsCOM.QueryCategories)AddonService.diCompany.GetBusinessObject(BoObjectTypes.oQueryCategories);
                    oQCat.Name = NomeCategoria;
                    int iRet = oQCat.Add();
                    System.Threading.Thread.Sleep(2000);
                    if (oQCat != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oQCat);
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();


                    return (iRet);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

