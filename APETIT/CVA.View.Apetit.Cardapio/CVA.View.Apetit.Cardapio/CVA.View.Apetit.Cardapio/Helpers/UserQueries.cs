using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.View.Apetit.Cardapio.Helpers
{
    public class UserQueries
    {
        public static void AssignFormattedSearch(string queryName, string theQuery, string formId, string itemId, string colId = "-1")
        {
            var oFms = (FormattedSearches)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = $@"SELECT * 
                            FROM {"CSHS".Aspas()} as T0 
                                INNER JOIN {"OUQR".Aspas()} AS T1 ON 
                                        T0.{"QueryId".Aspas()} = T1.{"IntrnalKey".Aspas()} 
                            WHERE   T0.{"FormID".Aspas()} = '{formId}' 
                                AND T0.{"ItemID".Aspas()} = '{itemId}' 
                                AND T0.{"ColID".Aspas()} = '{colId}' ;";

            oRecordset.DoQuery(query);

            if(oRecordset.RecordCount == 0)
            {
                int queryId = CreateQuery(queryName, theQuery);
                oFms.Action = BoFormattedSearchActionEnum.bofsaQuery;
                oFms.FormID = formId;
                oFms.ItemID = itemId;
                oFms.ColumnID = colId;
                oFms.QueryID = queryId;
                oFms.FieldID = itemId;

                if (colId.Equals("-1"))
                    oFms.ByField = BoYesNoEnum.tYES;
                else
                    oFms.ByField = BoYesNoEnum.tNO;

                long lRetCode = oFms.Add();

                if(lRetCode == -2035)
                {
                    oRecordset.DoQuery($@"SELECT 
                                            TOP 1 T0.{"IndexID".Aspas()} 
                                        FROM {"CSHS".Aspas()} AS T0 
                                        WHERE   T0.{"FormID".Aspas()} = '{formId}' 
                                            AND T0.{"ItemID".Aspas()} = '{itemId}' 
                                            AND T0.{"ColID".Aspas()} = '{colId}'
                                     ");

                    if(oRecordset.RecordCount > 0)
                    {
                        oFms.GetByKey((int)oRecordset.Fields.Item(0).Value);
                        oFms.Action = BoFormattedSearchActionEnum.bofsaQuery;
                        oFms.FormID = formId;
                        oFms.ItemID = itemId;
                        oFms.ColumnID = colId;
                        oFms.QueryID = queryId;
                        oFms.FieldID = itemId;

                        if (colId.Equals("-1"))
                            oFms.ByField = BoYesNoEnum.tYES;
                        else
                            oFms.ByField = BoYesNoEnum.tNO;

                        lRetCode = oFms.Update();
                    }
                }

                if (lRetCode != 0)
                    throw new Exception(B1Connection.Instance.Company.GetLastErrorDescription());
            }
            else
            {
                CreateQuery(queryName, theQuery);
            }
        }

        public static void RemoveFormattedSearch(string queryName, string itemId, string formId)
        {
            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            SAPbobsCOM.UserQueries oQuery = (SAPbobsCOM.UserQueries)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserQueries);
            FormattedSearches oFms = (FormattedSearches)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oFormattedSearches);

            oRecordset.DoQuery($"SELECT {"IndexID".Aspas()} FROM {"CSHS".Aspas()} WHERE {"ItemID".Aspas()} = '{itemId}' AND {"FormID".Aspas()} = '{formId}'");

            if (oRecordset.RecordCount > 0)
            {
                oFms.GetByKey(Convert.ToInt32(oRecordset.Fields.Item(0).Value));
                oFms.Remove();
            }

            oRecordset.DoQuery($"SELECT {"IntrnalKey".Aspas()}, {"QCategory".Aspas()} FROM {"OUQR".Aspas()} WHERE {"QName".Aspas()} = '{queryName}' and {"QCategory".Aspas()} = {GetSysCatID()}");

            if (oRecordset.RecordCount > 0)
            {
                oQuery.GetByKey(Convert.ToInt32(oRecordset.Fields.Item(0).Value), Convert.ToInt32(oRecordset.Fields.Item(1).Value));
                oQuery.Remove();
            }
        }

        public static bool ExistsQuery(string query)
        {
            query = query.Replace("'", "''");
            bool exists = false;

            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordset.DoQuery($"SELECT TOP 1 1 FROM {"OUQR".Aspas()} WHERE CAST({"QString".Aspas()} AS VARCHAR) = '{query}'");

            if (oRecordset.RecordCount > 0)
                exists = true;

            return exists;
        }

        public static int CreateQuery(string QueryName, string TheQuery)
        {
            int ret = 0;
            ret = -1;

            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            SAPbobsCOM.UserQueries oQuery = (SAPbobsCOM.UserQueries)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserQueries);

            oRecordset.DoQuery($"SELECT TOP 1 {"IntrnalKey".Aspas()} FROM {"OUQR".Aspas()} WHERE {"QCategory".Aspas()}={GetSysCatID()} AND {"QName".Aspas()}='{QueryName}'");
            if (oRecordset.RecordCount > 0)
            {
                ret = (int)oRecordset.Fields.Item(0).Value;
                oQuery.GetByKey(ret, GetSysCatID());
                oQuery.Query = TheQuery;
                oQuery.Update();
            }
            else
            {
                oQuery.QueryCategory = GetSysCatID();
                oQuery.QueryDescription = QueryName;
                oQuery.Query = TheQuery;

                if (oQuery.Add() != 0)
                    throw new Exception(B1Connection.Instance.Company.GetLastErrorDescription());

                string newKey = B1Connection.Instance.Company.GetNewObjectKey();

                if (newKey.Contains('\t'))
                    newKey = newKey.Split('\t')[0];

                ret = Convert.ToInt32(newKey);
            }

            return ret;
        }

        public static int GetSysCatID()
        {
            int ret = -3;
            Recordset oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordset.DoQuery($"SELECT TOP 1 {"CategoryId".Aspas()} FROM {"OQCN".Aspas()} WHERE {"CatName".Aspas()} = 'Geral'");
            if (oRecordset.RecordCount > 0)
                ret = Convert.ToInt32(oRecordset.Fields.Item(0).Value);

            return ret;
        }
    }
}
