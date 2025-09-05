
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ApplicationServices
{
    public static class FormExtension
    {
        public static Item GetItem(this Form vForm, string ItemUID)
        {
            Item vRet = null;
            
            try
            {
                vRet = vForm.Items.Item(ItemUID);
            }
            catch
            {
                vRet = null;
            }

            return vRet;
        }

        public static UserDataSource GetUserDataSource(this Form vForm, string UniqueID)
        {
            UserDataSource vRet = null;

            try
            {
                vRet = vForm.DataSources.UserDataSources.Item(UniqueID);
            }
            catch
            {
                vRet = null;
            }

            return vRet;
        }

        public static DBDataSource GetDBDataSource(this Form vForm, string UniqueID)
        {
            DBDataSource vRet = null;

            try
            {
                vRet = vForm.DataSources.DBDataSources.Item(UniqueID);
            }
            catch
            {
                vRet = null;
            }

            return vRet;
        }
    }
}
