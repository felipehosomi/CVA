using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Seleção de número de série
    /// </summary>
    public class f25 : BaseForm
    {
        #region Constructor
        public f25()
        {
            FormCount++;
        }

        public f25(ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f25(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f25(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13)
                    {
                        if (ItemEventInfo.ItemUID == "22")
                        {
                            return this.AddSerial();
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

                string serie = et_Serie.Value.Trim().ToLower();
                if (String.IsNullOrEmpty(serie))
                {
                    return false;
                }

                for (int i = 1; i <= mt_Serie.RowCount; i++)
                {
                    string serieLinha = String.Empty;
                    try
                    {
                        serieLinha = ((EditText)mt_Serie.GetCellSpecific("19", i)).Value.Trim().ToLower();
                    }
                    catch { }
                    if (serieLinha == serie)
                    {
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
    }
}
