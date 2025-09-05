namespace TaxDeterminationLoader
{
    using SAPbobsCOM;
    using SAPbouiCOM;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using TaxDeterminationLoader.Resources;


    internal static class EventHandler
    {
        private static SAPbobsCOM.Company company;
        private static int keyFieldsCounter;
        private static frmOKSFTCD ofrmOKSFTCD = new frmOKSFTCD();
        private static readonly Support Sup = new Support();

        private static void AppEvent(BoAppEventTypes eventType)
        {
            if ((((eventType == BoAppEventTypes.aet_CompanyChanged) || (eventType == BoAppEventTypes.aet_LanguageChanged)) || (eventType == BoAppEventTypes.aet_ServerTerminition)) || (eventType == BoAppEventTypes.aet_ShutDown))
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        internal static void EventHandlerStart()
        {
            try
            {
                Sup.SetApplication();
                Support.StatusbarMessagesPrefix = "TaxDeterminationLoader";
                //List<Support.EventFilterType> eventFilters = new List<Support.EventFilterType>();
                //Support.EventFilterType item = new Support.EventFilterType {
                //    _eventType = BoEventTypes.et_FORM_LOAD,
                //    _formUniqueId = new string[] { "80402", "OKSFTCD" }
                //};
                //eventFilters.Add(item);
                //item._eventType = BoEventTypes.et_CLICK;
                //item._formUniqueId = new string[] { "80402", "OKSFTCD" };
                //eventFilters.Add(item);
                //item._eventType = BoEventTypes.et_FORM_DATA_ADD;
                //item._formUniqueId = new string[] { "80402", "OKSFTCD" };
                //eventFilters.Add(item);
                //Support.SetEventHandlingFilters(eventFilters);
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format(TaxDeterminationLoader.Resources.Resources.BusinessOneNotFound, Environment.NewLine, Environment.NewLine, exception.Message));
                Environment.Exit(0);
            }
            Support.pSbo.AppEvent += new _IApplicationEvents_AppEventEventHandler(TaxDeterminationLoader.EventHandler.AppEvent);
            Support.pSbo.ItemEvent += new _IApplicationEvents_ItemEventEventHandler(TaxDeterminationLoader.EventHandler.ItemEvent);
            Support.ShowMessageInStatusBar("Addon carregado", BoStatusBarMessageType.smt_Success);
        }

        private static void ItemEvent(string formUid, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            try
            {
                Recordset recordset;
                Exception exception;

                if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    string keyField1 = "";
                    string keyField2 = "";
                    string keyField3 = "";
                    string keyField4 = "";
                    if ((pVal.FormTypeEx == "80401") && pVal.BeforeAction)
                    {
                        SAPbouiCOM.Form form = Support._sboApp.Forms.Item(formUid);
                        try
                        {
                            Item item = form.Items.Add("bt_DelAll", BoFormItemTypes.it_BUTTON);
                            int top = form.Items.Item("2025").Top;
                            int height = form.Items.Item("2025").Height;
                            int left = form.Items.Item("2025").Left + 120;

                            item.Left = left;
                            item.Top = top;
                            item.Width = 100;
                            item.FromPane = form.Items.Item("2025").FromPane;
                            item.ToPane = form.Items.Item("2025").ToPane;
                            SAPbouiCOM.Button specific = (SAPbouiCOM.Button)item.Specific;
                            specific.Caption = "Remover tudo";
                            form.Refresh();
                        }
                        catch (Exception)
                        {
                            Item item = form.Items.Item("bt_DelAll");
                            item.Visible = true;
                            item.Enabled = true;
                        }
                    }

                    if ((pVal.FormTypeEx == "80402") && pVal.BeforeAction)
                    {
                        try
                        {
                            SAPbouiCOM.Form form = Support._sboApp.Forms.Item(formUid);
                            try
                            {
                                Item item = form.Items.Add("f80402btX", BoFormItemTypes.it_BUTTON);
                                int top = form.Items.Item("2002").Top;
                                int height = form.Items.Item("2002").Height;
                                int left = form.Items.Item("2002").Left;
                                //item.Left = left - 0x3f;
                                item.Left = 391;
                                //item.Top = top;
                                item.Top = 253;
                                item.Width = 60;
                                item.Height = height;
                                item.FromPane = form.Items.Item("2002").FromPane;
                                item.ToPane = form.Items.Item("2002").ToPane;
                                SAPbouiCOM.Button specific = (SAPbouiCOM.Button)item.Specific;
                                specific.Caption = "Importar";
                                form.Refresh();
                            }
                            catch (Exception)
                            {
                                Item item = form.Items.Item("f80402btX");
                                item.Visible = true;
                                item.Enabled = true;
                            }

                            try
                            {
                                Item item = form.Items.Add("f80402btY", BoFormItemTypes.it_BUTTON);
                                int top = form.Items.Item("2002").Top;
                                int height = form.Items.Item("2002").Height;
                                int left = form.Items.Item("2002").Left;
                                //item.Left = (left - 0x71) - 0x3f;
                                item.Left = 292;
                                //item.Top = top;
                                item.Top = 253;
                                item.Width = 110;
                                item.Height = height;
                                item.FromPane = form.Items.Item("2002").FromPane;
                                item.ToPane = form.Items.Item("2002").ToPane;
                                SAPbouiCOM.Button specific = (SAPbouiCOM.Button)item.Specific;
                                specific.Caption = "Limpar todos campos";
                            }
                            catch (Exception)
                            {
                                Item item = form.Items.Item("f80402btY");
                                item.Visible = true;
                                item.Enabled = true;
                                form.Refresh();
                            }

                            Support.SetoFormByFormTypeEx("80402");
                            Support.SetoMatrix("2003");
                            keyFieldsCounter = 0;
                            for (int i = 0; i < Support.oMatrix.Columns.Count; i++)
                            {
                                if (Support.oMatrix.Columns.Item(i).Editable)
                                {
                                    keyFieldsCounter++;
                                }
                            }

                            //recordset = Sup.getRecordSet(TaxDeterminationLoader.Resources.Resources.SelectVerifyKeyFields, " WHERE TcdId = 1 AND Priority = '" + Support.GetMatrixItemFromCurrentRowAsString("2000", Support.oMatrix.GetNextSelectedRow()) + "'");
                            //keyFieldsCounter = Convert.ToInt32(Sup.getRecordSetResult(recordset, 3));
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            Log.writeLog("Erro na criação do botão;" + exception.Message);
                            Console.WriteLine(exception.Message);
                        }
                    }
                }
                if (pVal.EventType == BoEventTypes.et_CLICK)
                {
                    int rowCount;
                    if (pVal.FormTypeEx == "80401")
                    {
                        if ((pVal.ItemUID == "bt_DelAll") && pVal.BeforeAction)
                        {
                            SAPbouiCOM.Form form = Support._sboApp.Forms.Item(formUid);
                            DeleteAll(form);
                        }
                    }

                    if (pVal.FormTypeEx == "80402")
                    {
                        if ((pVal.ItemUID == "f80402btX") && pVal.BeforeAction)
                        {
                            try
                            {
                                Sup.LoadFormSafely("OKSFTCD", TaxDeterminationLoader.Resources.Resources.DetermCodImposto_formImport, false, true);
                                SAPbouiCOM.Form oForm = Support._sboApp.Forms.Item("OKSFTCD");
                                ofrmOKSFTCD.adjustForm(oForm);
                            }
                            catch (Exception exception2)
                            {
                                exception = exception2;
                                Log.writeLog("Erro ao abrir o form de importaçao;" + exception.Message);
                                Console.WriteLine(exception.Message);
                            }
                        }

                        if ((pVal.ItemUID == "f80402btY") && pVal.BeforeAction)
                        {
                            Support.SetoFormByFormTypeEx("80402");
                            Support.SetoMatrix("2003");
                            rowCount = Support.oMatrix.RowCount;
                            while (rowCount > 0)
                            {
                                if (rowCount != 1)
                                {
                                    Support.oMatrix.Columns.Item("2000").Cells.Item(rowCount - 1).Click(BoCellClickType.ct_Right, 0);
                                }
                                rowCount--;
                            }
                        }

                    }
                    if ((formUid == "OKSFTCD") && ((pVal.ItemUID == "OKSFTCDBIM") && pVal.BeforeAction))
                    {
                        StreamReader reader = null;
                        try
                        {
                            string separator = Sup.getUDFEditValue("OKSFTCD", "OKSFTCDECS");
                            string path = Sup.getUDFEditValue("OKSFTCD", "OKSFTCDECA");
                            if (separator == "")
                            {
                                if (Support.showMessageBox(ProjectMessages.BlankSeparatorChar, 2, "Sim", "Não", "") == 1)
                                {
                                    separator = " ";
                                }
                                else
                                {
                                    return;
                                }
                            }

                            if (path != "")
                            {
                                bool flag2 = false;
                                try
                                {
                                    reader = new StreamReader(path, System.Text.Encoding.Default, true);
                                    flag2 = true;
                                }
                                catch (Exception)
                                {
                                    Support.ShowMessageInStatusBar(ProjectMessages.FileProblems1, BoStatusBarMessageType.smt_Error);
                                    Support.showMessageBox(ProjectMessages.FileNotExist, 2, "", "", "");
                                    return;
                                }
                                if (flag2)
                                {
                                    Support._sboApp.Forms.Item("OKSFTCD").Close();
                                    string[] strArray = null;
                                    int length = 0; ;

                                    long lineNum = 0;
                                    long errorNum = 0;
                                    string line = reader.ReadLine(); // Primeira linha cabeçalho
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        Log.writeLog(line);
                                        lineNum += 1L;
                                        long num8 = 0L;
                                        if (String.IsNullOrEmpty(line.Trim()) || !line.Contains(separator))
                                        {
                                            continue;
                                        }
                                        strArray = line.Split(new char[] { separator[0] });
                                        length = strArray.Length;
                                        string column1 = "";
                                        string column2 = "";
                                        string column3 = "";
                                        string column4 = "";
                                        string column5 = "";
                                        string dateFrom = strArray[keyFieldsCounter];
                                        string dateTo = strArray[keyFieldsCounter + 1];

                                        switch (keyFieldsCounter)
                                        {
                                            case 1:
                                                column1 = strArray[0];
                                                break;
                                            case 2:
                                                column1 = strArray[0];
                                                column2 = strArray[1];
                                                break;
                                            case 3:
                                                column1 = strArray[0];
                                                column2 = strArray[1];
                                                column3 = strArray[2];
                                                break;
                                            case 4:
                                                column1 = strArray[0];
                                                column2 = strArray[1];
                                                column3 = strArray[2];
                                                column4 = strArray[3];
                                                break;
                                            case 5:
                                                column1 = strArray[0];
                                                column2 = strArray[1];
                                                column3 = strArray[2];
                                                column4 = strArray[3];
                                                column5 = strArray[4];
                                                break;
                                        }
                                        if (dateFrom == "")
                                        {
                                            num8 += 1L;
                                            errorNum += 1L;
                                            Log.writeLog(string.Concat(new object[] { "Linha ", lineNum, ";", ProjectMessages.EfectFromMandatory }));
                                        }
                                        else
                                        {
                                            try
                                            {
                                                DateTime time = Convert.ToDateTime(dateFrom);
                                                if (dateTo != "")
                                                {
                                                    DateTime time2 = Convert.ToDateTime(dateTo);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                num8 += 1L;
                                                errorNum += 1L;
                                                Log.writeLog(string.Concat(new object[] { "Linha ", lineNum, ";", ProjectMessages.InvalidDate, ";", dateFrom, ";", dateTo }));
                                            }
                                        }
                                        if (num8 != 0L)
                                        {
                                            continue;
                                        }
                                        Console.WriteLine("Valor 1: " + column1);
                                        Console.WriteLine("Valor 2: " + column2);
                                        Console.WriteLine("Valor 3: " + column3);
                                        Console.WriteLine("Valor 4: " + column4);
                                        Console.WriteLine("Valor 5: " + column5);
                                        Console.WriteLine("Efetivo desde: " + dateFrom);
                                        Console.WriteLine("Efetivo até: " + dateTo);
                                        try
                                        {
                                            Support.SetoFormByFormTypeEx("80402");
                                            Support.FreezeForm(true);

                                            Support.SetoMatrix("2003");
                                            List<string> columns = new List<string>();

                                            for (int i = 1; i < Support.oMatrix.Columns.Count; i++)
                                            {
                                                if (Support.oMatrix.Columns.Item(i).Editable)
                                                {
                                                    string columnName = "V_" + (i - 1);
                                                    columns.Add(columnName);
                                                }
                                            }

                                            switch (keyFieldsCounter)
                                            {
                                                case 1:
                                                    Support.SetMatrixContents(columns[0], -1, Support.oMatrix.RowCount, column1);
                                                    break;

                                                case 2:
                                                    Support.SetMatrixContents(columns[0], -1, Support.oMatrix.RowCount, column1);
                                                    Support.SetMatrixContents(columns[1], -1, Support.oMatrix.RowCount, column2);
                                                    break;

                                                case 3:
                                                    Support.SetMatrixContents(columns[0], -1, Support.oMatrix.RowCount, column1);
                                                    Support.SetMatrixContents(columns[1], -1, Support.oMatrix.RowCount, column2);
                                                    Support.SetMatrixContents(columns[2], -1, Support.oMatrix.RowCount, column3);
                                                    break;

                                                case 4:
                                                    Support.SetMatrixContents(columns[0], -1, Support.oMatrix.RowCount, column1);
                                                    Support.SetMatrixContents(columns[1], -1, Support.oMatrix.RowCount, column2);
                                                    Support.SetMatrixContents(columns[2], -1, Support.oMatrix.RowCount, column3);
                                                    Support.SetMatrixContents(columns[3], -1, Support.oMatrix.RowCount, column4);
                                                    break;

                                                case 5:
                                                    Support.SetMatrixContents(columns[0], -1, Support.oMatrix.RowCount, column1);
                                                    Support.SetMatrixContents(columns[1], -1, Support.oMatrix.RowCount, column2);
                                                    Support.SetMatrixContents(columns[2], -1, Support.oMatrix.RowCount, column3);
                                                    Support.SetMatrixContents(columns[3], -1, Support.oMatrix.RowCount, column4);
                                                    Support.SetMatrixContents(columns[4], -1, Support.oMatrix.RowCount, column5);
                                                    break;
                                            }
                                        }
                                        catch (Exception exception5)
                                        {
                                            exception = exception5;
                                            num8 += 1L;
                                            errorNum += 1L;
                                            Support.ShowMessageInStatusBar(ProjectMessages.FileProblems3, BoStatusBarMessageType.smt_Error);
                                            Log.writeLog(string.Concat(new object[] { "Linha ", lineNum, ";", ProjectMessages.FileProblems3, ";", line, ";", exception.Message }));
                                            Support._sboApp.Forms.ActiveForm.Close();
                                        }
                                        finally
                                        {
                                            Support.FreezeForm(false);
                                        }

                                        if (num8 == 0L)
                                        {
                                            Support.oMatrix.Columns.Item("2000").Cells.Item(Support.oMatrix.RowCount).Click(BoCellClickType.ct_Double, 0);
                                            Support.SetoFormByFormTypeEx("80403");
                                            Support.FreezeForm(true);
                                            Support.SetoMatrix("2003");
                                            Support.SetMatrixContents("2001", -1, Support.oMatrix.RowCount, dateFrom);
                                            Support.SetMatrixContents("2002", -1, Support.oMatrix.RowCount, dateTo);
                                            Support.FreezeForm(false);
                                            Support.oMatrix.Columns.Item("2000").Cells.Item(Support.oMatrix.RowCount).Click(BoCellClickType.ct_Double, 0);
                                            int rowIndex = 0;

                                            Support.SetoFormByFormTypeEx("80404");
                                            Support.SetoMatrix("2002");
                                            Support.FreezeForm(true);

                                            var t = Support.oMatrix.VisualRowCount;

                                            for (rowCount = keyFieldsCounter + 2; rowCount < length; rowCount++)
                                            {
                                                string str18;
                                                rowIndex++;

                                                if (rowIndex <= t)
                                                {
                                                    string taxCode = strArray[rowCount].ToString();
                                                    if (taxCode != "")
                                                    {
                                                        recordset = Sup.getRecordSet(TaxDeterminationLoader.Resources.Resources.SelectVerifyTaxCode, " WHERE \"Code\" = '" + taxCode + "'");
                                                        str18 = Sup.getRecordSetResult(recordset, 0);
                                                    }
                                                    else
                                                    {
                                                        str18 = "1";
                                                    }
                                                    if (Convert.ToInt32(str18) != 1)
                                                    {
                                                        num8 += 1L;
                                                        errorNum += 1L;
                                                        Log.writeLog(string.Concat(new object[] { "Linha ", lineNum, ";", ProjectMessages.InvalidTaxCode, ";", taxCode }));
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(string.Concat(new object[] { "Cod. Imposto ", rowIndex, ": ", taxCode }));
                                                        Support.SetMatrixContents("256000005", -1, rowIndex, taxCode);
                                                        Support.SetMatrixContents("140002005", -1, rowIndex, taxCode);
                                                        Support.SetMatrixContents("2002", -1, rowIndex, taxCode);
                                                    }
                                                }

                                            }
                                            Support.FreezeForm(false);
                                            SAPbouiCOM.Form activeForm = Support._sboApp.Forms.ActiveForm;
                                            activeForm.Items.Item("2000").Click(BoCellClickType.ct_Regular);
                                            if (activeForm.Type == 80404)
                                            {
                                                activeForm.Items.Item("2000").Click(BoCellClickType.ct_Regular);
                                            }
                                            activeForm = Support._sboApp.Forms.ActiveForm;
                                            activeForm.Items.Item("2000").Click(BoCellClickType.ct_Regular);
                                            if (activeForm.Type == 80403)
                                            {
                                                activeForm.Items.Item("2000").Click(BoCellClickType.ct_Regular);
                                            }
                                            Support._sboApp.Forms.ActiveForm.Items.Item("2000").Click(BoCellClickType.ct_Regular);
                                        }
                                        if (num8 == 0L)
                                        {
                                            Log.writeLog(string.Concat(new object[] { "Linha ", lineNum, ";", ProjectMessages.SucessImport }));
                                        }
                                    }
                                    if (errorNum == 0L)
                                    {
                                        Support.ShowMessageInStatusBar(ProjectMessages.SucessImport, BoStatusBarMessageType.smt_Success);
                                    }
                                    else
                                    {
                                        Support.ShowMessageInStatusBar(ProjectMessages.FailedImport, BoStatusBarMessageType.smt_Error);
                                    }
                                }
                            }
                            else
                            {
                                Support.ShowMessageInStatusBar(ProjectMessages.FileProblems2, BoStatusBarMessageType.smt_Error);
                                Support.showMessageBox(ProjectMessages.BlankFilePath, 2, "", "", "");
                            }
                        }
                        catch (Exception exception6)
                        {
                            exception = exception6;
                            Log.writeLog(ProjectMessages.Error1 + ";" + exception.Message);
                            Console.WriteLine(exception.Message);
                        }
                        finally
                        {
                            try
                            {
                                if (reader != null)
                                {
                                    reader.Close();
                                }
                            }
                            catch { }
                            try
                            {
                                Support.FreezeForm(false);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Support.ShowMessageInStatusBar(ex.Message, BoStatusBarMessageType.smt_Error);
            }
        }

        private static void DeleteAll(SAPbouiCOM.Form form)
        {
            form.Freeze(true);
            Matrix mt_Main = (Matrix)form.Items.Item("2003").Specific;
            //for (int i = 0; i < mt_Main.RowCount; i++)
            //{
            //    try
            //    {
            //        bool enabled = ((SAPbouiCOM.ComboBox)mt_Main.GetCellSpecific("2001", i)).Item.Enabled;
            //        mt_Main.Columns.Item("2000").Cells.Item(i).Click(BoCellClickType.ct_RightNoBlocking);

            //        Support.pSbo.ActivateMenuItem("1283");
            //        //i--;
            //    }
            //    catch { }
            //}
            while (mt_Main.RowCount > 0)
            {
                mt_Main.Columns.Item("2000").Cells.Item(1).Click(BoCellClickType.ct_Double);
                try
                {
                    //Support.pSbo.ActivateMenuItem("5889");
                    Support.SetoFormByFormTypeEx("80402");
                    Support.FreezeForm(true);
                    Support.SetoMatrix("2003");
                    while (Support.oMatrix.RowCount > 0)
                    {
                        Support.oMatrix.Columns.Item("2000").Cells.Item(1).Click();
                        try
                        {
                            Support.pSbo.ActivateMenuItem("1283");
                        }
                        catch
                        {
                            break;
                        }
                    }
                    Support.FreezeForm(false);
                    SAPbouiCOM.Form activeForm = Support._sboApp.Forms.ActiveForm;
                    if (activeForm.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        activeForm.Items.Item("2000").Click();
                    }
                    activeForm.Items.Item("2000").Click();
                    Support.pSbo.ActivateMenuItem("1283");
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            form.Freeze(false);
            form.Items.Item("2000").Click();
        }
    }
}

