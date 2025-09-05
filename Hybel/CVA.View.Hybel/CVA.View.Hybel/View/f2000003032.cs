using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.DAO.Resources;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Busca Itens por engenharia (Obsoleto, utilizando consulta formatada)
    /// </summary>
    public class f2000003032 : BaseForm
    {
        private Form Form;
        private static Form DocumentForm = null;
        private static int DocumentRow = -1;

        #region Constructor
        public f2000003032()
        {
            FormCount++;
        }

        public f2000003032(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003032(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003032(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show(string codConcorrente, ref Form form, int row)
        {
            Form = (Form)base.Show();
            Form.Mode = BoFormMode.fm_FIND_MODE;

            ((EditText)Form.Items.Item("et_Conc").Specific).Value = codConcorrente;

            DocumentForm = form;
            DocumentRow = row;

            this.Search();

            return Form;
        }

        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            }
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_OK")
                    {
                        this.SetDocumentValues();
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_DOUBLE_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "gr_Item")
                    {
                        this.SetDocumentValues();
                        return false;
                    }
                }
            }

            return true;
        }

        private void SetDocumentValues()
        {
            if (DocumentForm != null)
            {
                EventFilterBLL.DisableEvents();
                try
                {
                    Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                    Matrix mt_Doc = DocumentForm.Items.Item("38").Specific as Matrix;

                    if (gr_Item.Rows.SelectedRows != null)
                    {
                        DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                        string concorrente = ((EditText)Form.Items.Item("et_Conc").Specific).Value;

                        int row = gr_Item.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
                        string itemCode = dt_Item.GetValue("Cód. Item", row).ToString();

                        ((EditText)mt_Doc.GetCellSpecific("1", DocumentRow)).Value = itemCode;
                        // Quanto seta o item, ele apaga o concorrente
                        ((EditText)mt_Doc.GetCellSpecific("U_CVA_Concorrente", DocumentRow)).Value = concorrente;
                        // Setando o item novamente pro foco ficar no campo
                        ((EditText)mt_Doc.GetCellSpecific("1", DocumentRow)).Value = itemCode;
                        
                        Form.Close();
                    }
                    else
                    {
                        SBOApp.Application.SetStatusBarMessage("Selecione a linha do item");
                    }
                }
                catch (Exception ex)
                {
                    SBOApp.Application.SetStatusBarMessage(ex.Message);
                }
                finally
                {
                    EventFilterBLL.EnableEvents();
                }
            }
        }

        private void Search()
        {
            string concorrente = ((EditText)Form.Items.Item("et_Conc").Specific).Value;
            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            dt_Item.ExecuteQuery(String.Format(SQL.Item_GetByConcorrente, concorrente));
        }
    }
}
