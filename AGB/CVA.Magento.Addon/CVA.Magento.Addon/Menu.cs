using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbobsCOM;

namespace CVA.Magento.Addon
{
    class Menu
    {
        private static SAPbouiCOM.Application sboApp = Application.SBO_Application;
        private static Company oCompany = (Company)sboApp.Company.GetDICompany();

        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "CVA.Magento.Addon";
            oCreationPackage.String = "CVA Integração Magento";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("CVA.Magento.Addon");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "CVA.Magento.Addon.FormConfig";
                oCreationPackage.String = "Configuração";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                //Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;            

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "CVA.Magento.Addon.FormConfig")
                {
                    FormConfig activeForm = new FormConfig();
                    activeForm.Show();
                }
                if (pVal.MenuUID == "1292" && pVal.BeforeAction)
                {
                    SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.ActiveForm;

                    if (oForm.UniqueID == "FormConfig")
                    {
                        oForm.Freeze(true);

                        switch (FormConfig.ActiveItem) //oForm.ActiveItem
                        {
                            case "mtxDep":
                                {
                                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxDep").Specific;
                                    oMatrix.FlushToDataSource();

                                    var oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG1");
                                    oDB.InsertRecord(oDB.Size);

                                    oMatrix.LoadFromDataSourceEx(false);
                                    oMatrix.AutoResizeColumns();
                                    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;

                                    oForm.Freeze(false);
                                    break;
                                }
                            case "mtxCond":
                                {
                                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxCond").Specific;
                                    oMatrix.FlushToDataSource();

                                    var oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG2");
                                    oDB.InsertRecord(oDB.Size);

                                    oMatrix.LoadFromDataSourceEx(false);
                                    oMatrix.AutoResizeColumns();
                                    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;

                                    oForm.Freeze(false);
                                    break;
                                }
                            case "mtxFormas":
                                {
                                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxFormas").Specific;
                                    oMatrix.FlushToDataSource();

                                    var oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG3");
                                    oDB.InsertRecord(oDB.Size);

                                    oMatrix.LoadFromDataSourceEx(false);
                                    oMatrix.AutoResizeColumns();
                                    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;

                                    oForm.Freeze(false);
                                    break;
                                }
                            case "mtxDatas":
                                {
                                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxDatas").Specific;
                                    oMatrix.FlushToDataSource();

                                    var oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG4");
                                    oDB.InsertRecord(oDB.Size);

                                    oMatrix.LoadFromDataSourceEx(false);
                                    oMatrix.AutoResizeColumns();
                                    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;

                                    oForm.Freeze(false);
                                    break;
                                }
                            case "mtxFrete":
                                {
                                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxFrete").Specific;
                                    oMatrix.FlushToDataSource();

                                    var oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG5");
                                    oDB.InsertRecord(oDB.Size);

                                    oMatrix.LoadFromDataSourceEx(false);
                                    oMatrix.AutoResizeColumns();
                                    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;

                                    oForm.Freeze(false);
                                    break;
                                }
                        }

                        oForm.Freeze(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
