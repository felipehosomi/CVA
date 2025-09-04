using System;
using System.Collections.Generic;
using System.Text;
using SAPbobsCOM;
using CVA.View.Apetit.Cardapio.Helpers;

namespace Addon.CVA.View.Apetit.Cardapio.Helpers
{
    public static class FormatedSearch
    {
        public static void CriaFormated()
        {
            int idCategoria = CreateCategory("Addon Apetit");

            #region QUERY
            string strSql = $@"
                CREATE LOCAL TEMPORARY TABLE #tmp_table_ret
                (
	                {"Code".Aspas()} nvarchar(50),
                    { "TipoLoja".Aspas()} nvarchar(50),
	                { "Selected".Aspas()} nvarchar(1)
                );

                insert into #tmp_table_ret
                SELECT 
                    { "ItmsGrpNam".Aspas()} as { "Code".Aspas()},		
		            { "ItmsGrpNam".Aspas()} as { "TipoLoja".Aspas()},
	                    MAP(element_number,
                            1, { "QryGroup1".Aspas()},
                        2, { "QryGroup2".Aspas()},
                        3, { "QryGroup3".Aspas()},
                        4, { "QryGroup4".Aspas()},
                        5, { "QryGroup5".Aspas()},
                        6, { "QryGroup6".Aspas()},
                        7, { "QryGroup7".Aspas()},
                        8, { "QryGroup8".Aspas()},
                        9, { "QryGroup9".Aspas()},
                        10, { "QryGroup10".Aspas()},
                        11, { "QryGroup11".Aspas()},
                        12, { "QryGroup12".Aspas()},
                        13, { "QryGroup13".Aspas()},
                        14, { "QryGroup14".Aspas()},
                        15, { "QryGroup15".Aspas()},
                        16, { "QryGroup16".Aspas()},
                        17, { "QryGroup17".Aspas()},
                        18, { "QryGroup18".Aspas()},
                        19, { "QryGroup19".Aspas()},
                        20, { "QryGroup20".Aspas()},
                        21, { "QryGroup21".Aspas()},
                        22, { "QryGroup22".Aspas()},
                        23, { "QryGroup23".Aspas()},
                        24, { "QryGroup24".Aspas()},
                        25, { "QryGroup25".Aspas()},
                        26, { "QryGroup26".Aspas()},
                        27, { "QryGroup27".Aspas()},
                        28, { "QryGroup28".Aspas()},
                        29, { "QryGroup29".Aspas()},
                        30, { "QryGroup30".Aspas()},
                        31, { "QryGroup31".Aspas()},
                        32, { "QryGroup32".Aspas()},
                        33, { "QryGroup33".Aspas()},
                        34, { "QryGroup34".Aspas()},
                        35, { "QryGroup35".Aspas()},
                        36, { "QryGroup36".Aspas()},
                        37, { "QryGroup37".Aspas()},
                        38, { "QryGroup38".Aspas()},
                        39, { "QryGroup39".Aspas()},
                        40, { "QryGroup40".Aspas()},
                        41, { "QryGroup41".Aspas()},
                        42, { "QryGroup42".Aspas()},
                        43, { "QryGroup43".Aspas()},
                        44, { "QryGroup44".Aspas()},
                        45, { "QryGroup45".Aspas()},
                        46, { "QryGroup46".Aspas()},
                        47, { "QryGroup47".Aspas()},
                        48, { "QryGroup48".Aspas()},
                        49, { "QryGroup49".Aspas()},
                        50, { "QryGroup50".Aspas()},
                        51, { "QryGroup51".Aspas()},
                        52, { "QryGroup52".Aspas()},
                        53, { "QryGroup53".Aspas()},
                        54, { "QryGroup54".Aspas()},
                        55, { "QryGroup55".Aspas()},
                        56, { "QryGroup56".Aspas()},
                        57, { "QryGroup57".Aspas()},
                        58, { "QryGroup58".Aspas()},
                        59, { "QryGroup59".Aspas()},
                        60, { "QryGroup60".Aspas()},
                        61, { "QryGroup61".Aspas()},
                        62, { "QryGroup62".Aspas()},
                        63, { "QryGroup63".Aspas()},
                        64, { "QryGroup64".Aspas()}) AS { "Selected".Aspas()}
                FROM OITM AS TM, SERIES_GENERATE_INTEGER(1, 1, 65) 
	                INNER JOIN OITG AS TG ON element_number = { "ItmsTypCod".Aspas()}
                WHERE { "ItemCode".Aspas()} = RTRIM(LTRIM($[$mtxItens.it_prd]))
                ORDER BY element_number;

                SELECT { "Code".Aspas()}, { "TipoLoja".Aspas()}
                FROM #tmp_table_ret WHERE {"Selected".Aspas()} = 'Y';

                DROP TABLE #tmp_table_ret;
            ;";
            #endregion

            CreateFormattedSearches(strSql, "Busca TipoLoja", idCategoria, "CVACICLO", "mtxItens", "it_tploja");
        }

        //public static void BuscaTipoLojaProduto(string codProduto, string codTpLoja, out string code, out string name)
        //{
        //    try
        //    {
        //        code = "";
        //        name = "";

        //        var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
        //        var sqlQuery = $"CALL {"spc_CVA_ProcuraTipoLoja".Aspas()}('{codProduto}', '{codTpLoja}')";
        //        oRecordset.DoQuery(sqlQuery);

        //        if (!oRecordset.EoF)
        //        {
        //            code = oRecordset.Fields.Item("Code").Value.ToString();
        //            name = oRecordset.Fields.Item("TipoLoja").Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static int RetornaKeyFormattedSearches(string FormUID, string ItemUID, string ColumnID)
        {
            try
            {
                string strSql;
                int Ret = -1;

                strSql = $@"
                    SELECT 
                        DISTINCT {"IndexID".Aspas()} 
                    FROM CSHS 
                    WHERE   {"FormID".Aspas()} = '{FormUID}' 
                            AND {"ItemID".Aspas()} = '{ItemUID}' 
                            AND {"ColID".Aspas()} = '{ColumnID}'
                ";

                var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordset.DoQuery(strSql);

                int sRet = (dynamic)oRecordset.Fields.Item(0).Value;

                if (sRet != 0) Ret = Convert.ToInt32(sRet);

                return Ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int RetornaQueryId(int Category, string QueryName)
        {
            int Ret = 0;

            try
            {
                //string strSql = $"SELECT INTRNALKEY FROM OUQR WHERE QCATEGORY = '{Category}' AND QNAME = '{QueryName}'";                
                string strSql = $"SELECT {"IntrnalKey".Aspas()} FROM OUQR WHERE {"QCategory".Aspas()} = '{Category}' AND {"QName".Aspas()} = '{QueryName}' ;";
                var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordset.DoQuery(strSql);

                int sRet = (dynamic)oRecordset.Fields.Item(0).Value;
                if (sRet != 0) Ret = Convert.ToInt32(sRet);
            }
            catch { }

            return Ret;
        }

        public static bool CreateFormattedSearches(string Query, string QueryName, int QueryCategory, string FormID, string ItemID, string ColumnID)
        {
            SAPbobsCOM.FormattedSearches oFormattedSearch;
            SAPbobsCOM.UserQueries oUserQueries;

            int lRet = 0;
            string errMessage;
            int QueryKey;
            int FormatKey;
            bool bRet = true;

            try
            {
                oUserQueries = (SAPbobsCOM.UserQueries)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserQueries);
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
                    B1Connection.Instance.Company.GetLastError(out lRet, out errMessage);
                    throw new Exception(errMessage);
                }

                oFormattedSearch = (FormattedSearches)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
                FormatKey = RetornaKeyFormattedSearches(FormID, ItemID, ColumnID);

                if (FormatKey == -1)
                {
                    oFormattedSearch.ByField = BoYesNoEnum.tYES;
                    oFormattedSearch.Action = BoFormattedSearchActionEnum.bofsaQuery;
                    oFormattedSearch.FormID = FormID;
                    oFormattedSearch.ItemID = ItemID;
                    oFormattedSearch.ColumnID = ColumnID;//"-1";
                    oFormattedSearch.QueryID = RetornaQueryId(QueryCategory, QueryName);
                    lRet = oFormattedSearch.Add();
                }
                else
                {
                    oFormattedSearch.GetByKey(FormatKey);
                    oFormattedSearch.ByField = BoYesNoEnum.tYES;
                    oFormattedSearch.Action = BoFormattedSearchActionEnum.bofsaQuery;
                    oFormattedSearch.FormID = FormID;
                    oFormattedSearch.ItemID = ItemID;
                    oFormattedSearch.ColumnID = ColumnID;//"-1";
                    oFormattedSearch.QueryID = RetornaQueryId(QueryCategory, QueryName);

                    lRet = oFormattedSearch.Update();
                }

                if (lRet != 0)
                {
                    bRet = false;
                    B1Connection.Instance.Company.GetLastError(out lRet, out errMessage);
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

            }
            catch
            {
            }

            return bRet;
        }

        public static int CreateCategory(string NomeCategoria)
        {
            int iRet = 0;
            try
            {
                //verifica se existe a categoria
                string sQuery = $@"select {"CategoryId".Aspas()} from {"OQCN".Aspas()} where {"CatName".Aspas()} = '{NomeCategoria}' ;";

                var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordset.DoQuery(sQuery);

                int catID = (dynamic)oRecordset.Fields.Item(0).Value;

                //string catID = Convert.ToString(B1Connections.ExecuteSqlScalar(sQuery));
                //se não existe cria a categoria
                if (catID != 0)
                    return catID;
                else
                {
                    SAPbobsCOM.QueryCategories oQCat;

                    oQCat = (SAPbobsCOM.QueryCategories)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oQueryCategories);
                    //fim teste
                    oQCat.Name = NomeCategoria;
                    iRet = oQCat.Add();

                    string errMessage;
                    B1Connection.Instance.Company.GetLastError(out iRet, out errMessage);
                    if (oQCat != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oQCat);
                    }

                    string strCode = "";
                    B1Connection.Instance.Company.GetNewObjectCode(out strCode);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    return Convert.ToInt32(strCode);
                }
            }
            catch { }

            return (iRet);
        }
    }
}
