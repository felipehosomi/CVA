

using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.Core.Cianet.VIEW
{
    public class f25 : BaseForm
    {
        #region Constructor
        public f25()
        {
            FormCount++;
        }

        public f25(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f25(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f25(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13)
                    {
                        if (ItemEventInfo.ItemUID == "22")
                        {
                            return this.AddSerial();
                        }
                    }

                    EditText et_To = Form.Items.Item("et_To").Specific as EditText;
                    EditText et_From = Form.Items.Item("et_From").Specific as EditText;

                    if (ItemEventInfo.ItemUID == "et_To" && ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN && ItemEventInfo.CharPressed == 9)
                    {
                        if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
                        {
                            this.AddArray();
                        }

                    }
                    //if (ItemEventInfo.CharPressed == 13)
                    //{
                    //    if (ItemEventInfo.ItemUID == "et_From" || ItemEventInfo.ItemUID == "et_To")
                    //    {
                    //         et_From = Form.Items.Item("et_From").Specific as EditText;
                    //         et_To = Form.Items.Item("et_To").Specific as EditText;

                    //        if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
                    //        {
                    //            this.AddArray();
                    //        }
                    //        return false;
                    //    }
                    //}
                }
                else
                {
                    if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
                    {
                        if (ItemEventInfo.ItemUID == "bt_Array")
                        {
                            this.AddArray();
                        }
                    }
                }
            }
            return true;
        }

        private bool AddSerial()
        {
            try
            {
                Form.Freeze(true);
                Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
                Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
                EditText et_Serie = Form.Items.Item("22").Specific as EditText;

                string serie = et_Serie.Value.Trim();
                if (String.IsNullOrEmpty(serie))
                {
                    return false;
                }

                for (int i = 1; i <= mt_Serie.RowCount; i++)
                {
                    var teste = ((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim();
                    if (((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim() == serie)
                    {
                        var hashdui = ((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim();
                        if (((EditText)mt_Serie.GetCellSpecific("1320000027", i)).Value.Trim().ToLower() == "sim")
                        {
                            SBOApp.Application.SetStatusBarMessage("Número de série selecionado indisponível");
                            return false;
                        }

                        if (!mt_Serie.IsRowSelected(i))
                        {
                            mt_Serie.Columns.Item("0").Cells.Item(i).Click();
                        }
                        Form.Items.Item("8").Click();
                        et_Serie.Value = String.Empty;
                        int selectedItemRow = mt_Item.GetNextSelectedRow();
                        string openQty = ((EditText)mt_Item.GetCellSpecific("8", selectedItemRow)).Value.Replace(".", "");

                        if (Convert.ToInt32(openQty) == 0)
                        {
                            Form.Items.Item("1").Click();
                            selectedItemRow++;
                            // Seleciona próxima linha
                            for (int j = selectedItemRow; j <= mt_Item.RowCount; j++)
                            {
                                openQty = ((EditText)mt_Item.GetCellSpecific("8", j)).Value.Replace(".", "");
                                if (Convert.ToInt32(openQty) > 0)
                                {
                                    mt_Item.Columns.Item("0").Cells.Item(j).Click();
                                    return false;
                                }
                            }
                            // Se não achou, percorre tabela desde o início para verificar se possui alguma linha pendente
                            for (int j = 1; j <= mt_Item.RowCount; j++)
                            {
                                openQty = ((EditText)mt_Item.GetCellSpecific("8", j)).Value.Replace(".", "");
                                if (Convert.ToInt32(openQty) > 0)
                                {
                                    mt_Item.Columns.Item("0").Cells.Item(j).Click();
                                    return false;
                                }
                            }
                            // Se não possui liha pendente, fecha a tela
                            Form.Items.Item("1").Click();
                        }

                        try
                        {
                            if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
                            {
                                Form.Items.Item("1").Click();
                            }
                        }
                        catch { }
                        return false;
                    }
                }
                SBOApp.Application.SetStatusBarMessage("Número de série selecionado indisponível");
                return false;
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
                return false;
            }
            finally
            {
                try
                {
                    Form.Freeze(false);
                }
                catch { }
            }
        }

        //private void AddArray()
        //{
        //    try
        //    {
        //        Form.Freeze(true);
        //        Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
        //        Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
        //        EditText et_From = Form.Items.Item("et_From").Specific as EditText;
        //        EditText et_To = Form.Items.Item("et_To").Specific as EditText;

        //        if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
        //        {
        //            for (int i = 1; i < mt_Serie.RowCount - 1; i++)
        //            {
        //                string lineSerie = ((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim();
        //                if (et_From.Value.CompareTo(lineSerie) <= 0 && et_To.Value.CompareTo(lineSerie) >= 0)
        //                {
        //                    if (((EditText)mt_Serie.GetCellSpecific("1320000027", i)).Value.Trim().ToLower() == "sim")
        //                    {
        //                        SBOApp.Application.SetStatusBarMessage($"Nº série {lineSerie} já alocado");
        //                        continue;
        //                    }
        //                    if (!mt_Serie.IsRowSelected(i))
        //                    {
        //                        mt_Serie.Columns.Item("0").Cells.Item(i).Click(BoCellClickType.ct_Regular, (int)BoModifiersEnum.mt_CTRL);
        //                    }
        //                }
        //            }
        //            Form.Items.Item("8").Click();
        //            if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
        //            {
        //                Form.Items.Item("1").Click();
        //            }
        //        }
        //        else
        //        {
        //            SBOApp.Application.SetStatusBarMessage("Informe o valor 'De' e o valor 'Até'");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SBOApp.Application.SetStatusBarMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        Form.Freeze(false);
        //    }
        //}

        private void AddArray()
        {
            try
            {
                Form.Freeze(true);
                Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
                Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
                EditText et_From = Form.Items.Item("et_From").Specific as EditText;
                EditText et_To = Form.Items.Item("et_To").Specific as EditText;




                if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
                {
                    for (int i = 1; i < mt_Serie.RowCount + 1; i++)
                    {
                        var lineSerie = ((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim();
                        if (et_From.Value.CompareTo(lineSerie) <= 0 && (et_To.Value.CompareTo(lineSerie) > 0 || et_To.Value.CompareTo(lineSerie) == 0))
                        {
                            if (((EditText)mt_Serie.GetCellSpecific("1320000027", i)).Value.Trim().ToLower() == "sim")
                            {
                                SBOApp.Application.SetStatusBarMessage($"Nº série {lineSerie} já alocado");
                                continue;
                            }
                            if (!mt_Serie.IsRowSelected(i))
                            {
                                mt_Serie.Columns.Item("0").Cells.Item(i).Click(BoCellClickType.ct_Regular, (int)BoModifiersEnum.mt_CTRL);
                            }
                        }
                    }
                    Form.Items.Item("8").Click();
                    if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
                    {
                        Form.Items.Item("1").Click();
                    }
                }
                else
                {
                    SBOApp.Application.SetStatusBarMessage("Informe o valor 'De' e o valor 'Até'");
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
        }
    }




    //public override bool ItemEvent()
    //{
    //    base.ItemEvent();
    //    if (ItemEventInfo.BeforeAction)
    //    {
    //        if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_KEY_DOWN)
    //        {
    //            if (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13)
    //            {
    //                if (ItemEventInfo.ItemUID == "22")
    //                {
    //                    return this.AddSerial();
    //                }
    //            }

    //            if (ItemEventInfo.CharPressed == 13 || ItemEventInfo.CharPressed == 9)
    //            {
    //                if (ItemEventInfo.ItemUID == "et_From" || ItemEventInfo.ItemUID == "et_To")
    //                {
    //                    EditText et_From = Form.Items.Item("et_From").Specific as EditText;
    //                    EditText et_To = Form.Items.Item("et_To").Specific as EditText;

    //                    if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
    //                    {
    //                        this.AddArray();
    //                    }
    //                    return false;
    //                }
    //            }                 
    //        }
    //        else
    //        {
    //            if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
    //            {
    //                if (ItemEventInfo.ItemUID == "bt_Array")
    //                {
    //                    this.AddArray();
    //                }
    //            }
    //        }
    //    }
    //    return true;
    //}

    //private bool AddSerial()
    //{
    //    try
    //    {
    //        Form.Freeze(true);
    //        Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
    //        Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
    //        EditText et_Serie = Form.Items.Item("22").Specific as EditText;

    //        string serie = et_Serie.Value.Trim();
    //        if (String.IsNullOrEmpty(serie))
    //        {
    //            return false;
    //        }

    //        for (int i = 1; i <= mt_Serie.RowCount; i++)
    //        {
    //            var teste = ((EditText)mt_Serie.GetCellSpecific("19", i)).Value.Trim();
    //            if (((EditText)mt_Serie.GetCellSpecific("19", i)).Value.Trim() == serie)
    //            {
    //                var hashdui = ((EditText)mt_Serie.GetCellSpecific("19", i)).Value.Trim();
    //                if (((EditText)mt_Serie.GetCellSpecific("1320000027", i)).Value.Trim().ToLower() == "sim")
    //                {
    //                    SBOApp.Application.SetStatusBarMessage("Número de série selecionado indisponível");
    //                    return false;
    //                }

    //                if (!mt_Serie.IsRowSelected(i))
    //                {
    //                    mt_Serie.Columns.Item("0").Cells.Item(i).Click();
    //                }
    //                Form.Items.Item("8").Click();
    //                et_Serie.Value = String.Empty;
    //                int selectedItemRow = mt_Item.GetNextSelectedRow();
    //                string openQty = ((EditText)mt_Item.GetCellSpecific("8", selectedItemRow)).Value.Replace(".", "");

    //                if (Convert.ToInt32(openQty) == 0)
    //                {
    //                    Form.Items.Item("1").Click();
    //                    selectedItemRow++;
    //                    // Seleciona próxima linha
    //                    for (int j = selectedItemRow; j <= mt_Item.RowCount; j++)
    //                    {
    //                        openQty = ((EditText)mt_Item.GetCellSpecific("8", j)).Value.Replace(".", "");
    //                        if (Convert.ToInt32(openQty) > 0)
    //                        {
    //                            mt_Item.Columns.Item("0").Cells.Item(j).Click();
    //                            return false;
    //                        }
    //                    }
    //                    // Se não achou, percorre tabela desde o início para verificar se possui alguma linha pendente
    //                    for (int j = 1; j <= mt_Item.RowCount; j++)
    //                    {
    //                        openQty = ((EditText)mt_Item.GetCellSpecific("8", j)).Value.Replace(".", "");
    //                        if (Convert.ToInt32(openQty) > 0)
    //                        {
    //                            mt_Item.Columns.Item("0").Cells.Item(j).Click();
    //                            return false;
    //                        }
    //                    }
    //                    // Se não possui liha pendente, fecha a tela
    //                    Form.Items.Item("1").Click();
    //                }

    //                try
    //                {
    //                    if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
    //                    {
    //                        Form.Items.Item("1").Click();
    //                    }
    //                }
    //                catch { }
    //                return false;
    //            }
    //        }
    //        SBOApp.Application.SetStatusBarMessage("Número de série selecionado indisponível");
    //        return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        SBOApp.Application.SetStatusBarMessage(ex.Message);
    //        return false;
    //    }
    //    finally
    //    {
    //        try
    //        {
    //            Form.Freeze(false);
    //        }
    //        catch { }
    //    }
    //}

    //private void AddArray()
    //{
    //    try
    //    {
    //        Form.Freeze(true);
    //        Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
    //        Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
    //        EditText et_From = Form.Items.Item("et_From").Specific as EditText;
    //        EditText et_To = Form.Items.Item("et_To").Specific as EditText;




    //        if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
    //        {
    //            for (int i = 1; i < mt_Serie.RowCount+1; i++)
    //            {
    //                var lineSerie = ((EditText)mt_Serie.GetCellSpecific("19", i)).Value.Trim();
    //                if (et_From.Value.CompareTo(lineSerie) <= 0 && (et_To.Value.CompareTo(lineSerie) > 0 || et_To.Value.CompareTo(lineSerie) == 0))
    //                {
    //                    if (((EditText)mt_Serie.GetCellSpecific("1320000027", i)).Value.Trim().ToLower() == "sim")
    //                    {
    //                        SBOApp.Application.SetStatusBarMessage($"Nº série {lineSerie} já alocado");
    //                        continue;
    //                    }
    //                    if (!mt_Serie.IsRowSelected(i))
    //                    {
    //                        mt_Serie.Columns.Item("0").Cells.Item(i).Click(BoCellClickType.ct_Regular, (int)BoModifiersEnum.mt_CTRL);
    //                    }
    //                }
    //            }
    //            Form.Items.Item("8").Click();
    //            if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
    //            {
    //                Form.Items.Item("1").Click();
    //            }
    //        }
    //        else
    //        {
    //            SBOApp.Application.SetStatusBarMessage("Informe o valor 'De' e o valor 'Até'");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        SBOApp.Application.SetStatusBarMessage(ex.Message);
    //    }
    //    finally
    //    {
    //        Form.Freeze(false);
    //    }
    //}

}




