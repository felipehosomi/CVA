using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.DIME.BLL;
using CVA.View.DIME.DAO.Resources;
using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace CVA.View.DIME.VIEW
{
    /// <summary>
    /// Quadro 09
    /// </summary>
    public class f2000005011 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000005011()
        {
            FormCount++;
        }

        public f2000005011(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000005011(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000005011(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            ComboBox cb_Branch = (ComboBox)Form.Items.Item("cb_Filial").Specific;
            cb_Branch.AddValuesFromQuery(SQL.Branch_Get);

            Form.Items.Item("bt_ApImp").Visible = Form.Mode == BoFormMode.fm_ADD_MODE;

            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_ApImp")
                    {
                        var dt_DIME = Form.DataSources.DBDataSources.Item("@CVA_DIME_09");

                        var filial = dt_DIME.GetValue("U_Filial", dt_DIME.Offset);
                        var period = dt_DIME.GetValue("U_Periodo", dt_DIME.Offset).Trim();
                        if (!DateTime.TryParseExact($"01/{period.Trim()}", "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out var date))
                        {
                            SBOApp.Application.MessageBox("Período deve estar no formada MM/AAAA");
                            return false;
                        }

                        try
                        {
                            Form.Freeze(true);
                            var rst = (Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                            rst.DoQuery($"SP_CVA_DIME_APURAR_IMPOSTOS '{date.ToString("yyyy-MM")}'");

                            var v010 = "0";
                            var v040 = "0";
                            var v050 = "0";
                            var v075 = "0";
                            var v080 = "0";
                            var v120 = "0";
                            var v999 = "0";

                            if (rst.RecordCount > 0)
                            {
                                v010 = rst.Fields.Item("010").Value.ToString().Replace(",", ".");
                                v040 = rst.Fields.Item("040").Value.ToString().Replace(",", ".");
                                v050 = rst.Fields.Item("050").Value.ToString().Replace(",", ".");
                                v075 = rst.Fields.Item("075").Value.ToString().Replace(",", ".");
                                v080 = rst.Fields.Item("080").Value.ToString().Replace(",", ".");
                                v120 = rst.Fields.Item("120").Value.ToString().Replace(",", ".");
                                v999 = rst.Fields.Item("999").Value.ToString().Replace(",", ".");
                            }

                            Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                            mt_Item.Clear();
                            mt_Item.FlushToDataSource();

                            DBDataSource ds_Item = Form.DataSources.DBDataSources.Item("@CVA_DIME_09_ITEM");
                            int rCount = 1;

                            mt_Item.AddRow(7);

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "010";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v010;

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "040";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v040;

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "050";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v050;

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "075";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v075;

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "080";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v080;

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "120";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v120;

                            ((EditText)mt_Item.Columns.Item("cl_Item").Cells.Item(rCount).Specific).Value = "999";
                            ((EditText)mt_Item.Columns.Item("cl_Valor").Cells.Item(rCount++).Specific).Value = v999;
                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.MessageBox("Erro ao Apurar Imposto" + ex.Message);
                        }
                        finally
                        {
                            Form.Freeze(false);
                        }
                    }
                }

                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "et_Period")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        if (mt_Item.RowCount == 0)
                        {
                            this.AddRow();
                        }
                    }

                    if (ItemEventInfo.ItemUID == "mt_Item")
                    {
                        Matrix mt_Item = Form.Items.Item("mt_Item").Specific as Matrix;
                        if (ItemEventInfo.Row == mt_Item.RowCount)
                        {
                            if (!String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("cl_Item", ItemEventInfo.Row)).Value))
                            {
                                this.AddRow();
                            }
                        }
                    }
                }
            }

            return true;
        }

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                    DBDataSource dt_DIME = Form.DataSources.DBDataSources.Item("@CVA_DIME_09");

                    string filial = dt_DIME.GetValue("U_Filial", dt_DIME.Offset);
                    string period = dt_DIME.GetValue("U_Periodo", dt_DIME.Offset).Trim();
                    DateTime date;
                    if (!DateTime.TryParseExact($"01/{period.Trim()}", "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out date))
                    {
                        SBOApp.Application.MessageBox("Período deve estar no formada MM/AAAA");
                        return false;
                    }

                    if (!String.IsNullOrEmpty(filial) && !String.IsNullOrEmpty(period))
                    {
                        if (CrudController.ExecuteScalar(String.Format(SQL.DimeQuadro09_Get, period, filial)) != null)
                        {
                            SBOApp.Application.MessageBox("Configurações já cadastradas para filial e periodo informados!");
                            return false;
                        }
                    }

                    dt_DIME.SetValue("Code", dt_DIME.Offset, CrudController.GetNextCode("@CVA_DIME_09").PadLeft(4, '0'));
                }
            }

            return true;
        }

        private void AddRow()
        {
            DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("@CVA_DIME_09_ITEM");
            dt_Item.Clear();

            Matrix mt_Item = (Matrix)Form.Items.Item("mt_Item").Specific;
            mt_Item.AddRow();
        }
    }
}
