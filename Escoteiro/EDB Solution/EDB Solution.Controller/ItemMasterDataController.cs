using EDB_Solution.Controller.Client;
using EDB_Solution.Model.Magento;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Configuration;

namespace EDB_Solution.Controller
{
    public class ItemMasterDataController
    {
        public static string Token;
        public static string Api = ConfigurationManager.AppSettings["ApiMagento"];
        public static string User = ConfigurationManager.AppSettings["ApiMagentoUser"];
        public static string Password = ConfigurationManager.AppSettings["ApiMagentoPassWord"];
        public static CategoryModel Categories { get; set; }

        static string GetToken(string userName, string password)
        {
            var client = new ClientMagento(Api);
            return client.GetAdminToken(userName, password);
        }

        public static void GetCategories()
        {
            // Obtém o token de acesso
            Token = GetToken(User, Password);

            var magento = new ClientMagento(Api, Token);

            Categories = magento.GetListCategories(Token);
        }

        public static int SetItemCategories(DataTable dtSelCats, string itemCode)
        {
            var cva_oict = CommonController.Company.UserTables.Item("CVA_OICT");

            for (var i = 0; i < dtSelCats.Rows.Count; i++)
            {
                cva_oict.Code = String.Format("{0}_{1}", itemCode, dtSelCats.GetValue("ID", i));
                cva_oict.Name = String.Format("{0}_{1}", itemCode, dtSelCats.GetValue("ID", i));

                cva_oict.UserFields.Fields.Item("U_ID").Value = dtSelCats.GetValue("ID", i).ToString();
                cva_oict.UserFields.Fields.Item("U_Name").Value = dtSelCats.GetValue("Name", i);
                cva_oict.UserFields.Fields.Item("U_ItemCode").Value = itemCode;

                if (cva_oict.Add() != 0)
                {
                    return -1;
                }
            }

            return 0;
        }

        public static int SetItemCategories(DataTable dtCategories, string itemCode, int lineNum, bool setAllCategories)
        {
            var cva_oict = CommonController.Company.UserTables.Item("CVA_OICT");

            for (var i = 0; i < dtCategories.Rows.Count; i++)
            {
                if (dtCategories.GetValue("Select", i).ToString() != "Y") continue;

                if (!setAllCategories &&
                    dtCategories.GetValue("Items", i).ToString().Length > 0 &&
                    !dtCategories.GetValue("Items", i).ToString().Contains(String.Concat("\u00A0", lineNum.ToString(), "\u00A0"))) continue;

                cva_oict.Code = String.Format("{0}_{1}", itemCode, dtCategories.GetValue("ID", i));
                cva_oict.Name = String.Format("{0}_{1}", itemCode, dtCategories.GetValue("ID", i));

                cva_oict.UserFields.Fields.Item("U_ID").Value = dtCategories.GetValue("ID", i).ToString();
                cva_oict.UserFields.Fields.Item("U_Name").Value = dtCategories.GetValue("Name", i).ToString().Replace("▼", "").TrimStart('\u00A0').TrimStart(' ');
                cva_oict.UserFields.Fields.Item("U_ItemCode").Value = itemCode;

                if (cva_oict.Add() != 0)
                {
                    return -1;
                }
            }

            return 0;
        }

        public static void GetSelectedItemCategories(DataTable dtCategories, string itemCode, int lineNum, string selectValue = "Y")
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = $@"select ""U_ID"", ""U_Name"" 
                             from ""@CVA_OICT"" 
                            where ""U_ItemCode"" = '{itemCode}'";
            recordset.DoQuery(query);

            for (var i = 0; i < recordset.RecordCount; i++)
            {
                for (var y = 0; y < dtCategories.Rows.Count; y++)
                {
                    if (dtCategories.GetValue("ID", y).ToString() != recordset.Fields.Item("U_ID").Value.ToString()) continue;

                    var items = dtCategories.GetValue("Items", y).ToString();

                    if (selectValue == "Y")
                    {
                        dtCategories.SetValue("Select", y, selectValue);

                        if (!dtCategories.GetValue("Items", y).ToString().Contains(String.Concat("\u00A0", lineNum.ToString(), "\u00A0")))
                        {
                            dtCategories.SetValue("Items", y, String.Concat(items, "\u00A0", lineNum.ToString(), "\u00A0"));
                        }
                    }
                    else
                    {
                        var removeString = String.Concat("\u00A0", lineNum, "\u00A0");
                        var index = items.IndexOf(removeString);
                        items = items.Remove(index, removeString.Length);
                        dtCategories.SetValue("Items", y, items);

                        if (items.Length == 0)
                        {
                            dtCategories.SetValue("Select", y, selectValue);
                        }
                    }

                    break;
                }

                recordset.MoveNext();
            }
        }

        public static void GetSelectedItemCategories(DataTable dtCategories, DataTable dtSelCats, string itemCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = $@"select ""U_ID"", ""U_Name"" 
                             from ""@CVA_OICT"" 
                            where ""U_ItemCode"" = '{itemCode}'";
            recordset.DoQuery(query);

            for (var i = 0; i < recordset.RecordCount; i++)
            {
                for (var y = 0; y < dtCategories.Rows.Count; y++)
                {
                    if (dtCategories.GetValue("ID", y).ToString() != recordset.Fields.Item("U_ID").Value.ToString()) continue;

                    dtCategories.SetValue("Select", y, "Y");
                    break;
                }

                dtSelCats.Rows.Add();

                dtSelCats.SetValue("Select", i, "Y");
                dtSelCats.SetValue("ID", i, recordset.Fields.Item("U_ID").Value.ToString());
                dtSelCats.SetValue("Name", i, recordset.Fields.Item("U_Name").Value.ToString());

                recordset.MoveNext();
            }
        }

        public static void RemoveSelectedItemCategories(string itemCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            var query = $@"delete from ""@CVA_OICT""  
                            where ""U_ItemCode"" like '{itemCode}'";
            recordset.DoQuery(query);
        }

        public static void SetItemToIntegrate(string itemCode)
        {
            var item = (SAPbobsCOM.Items)CommonController.Company.GetBusinessObject(BoObjectTypes.oItems);
            item.GetByKey(itemCode);
            item.UserFields.Fields.Item("U_CVA_Integrated").Value = "N";
            item.Update();
        }
    }
}
