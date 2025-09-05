using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Busca Itens
    /// </summary>
    public class f2000003037 : BaseForm
    {
        private Form Form;
        private static Form DocumentForm = null;
        private static int DocumentRow = -1;
        private static int Filial = 0;

        #region Constructor
        public f2000003037()
        {
            FormCount++;
        }

        public f2000003037(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003037(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003037(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show(int filial, string hybel, ref Form form, int row)
        {
            Form = (Form)base.Show();

            Filial = filial;
            ((EditText)Form.Items.Item("et_Item").Specific).Value = hybel;

            DocumentForm = form;
            DocumentRow = row;
            
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
                    if (ItemEventInfo.ItemUID == "bt_Busca")
                    {
                        this.Busca();
                    }
                    if (ItemEventInfo.ItemUID == "bt_Selec")
                    {
                        this.SetDocumentValues();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    this.ChooseFromList();
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

        #region ChooseFromList
        private void ChooseFromList()
        {
            IChooseFromListEvent oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvento.ChooseFromListUID);
            DataTable oDataTable = oCFLEvento.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    string value = oDataTable.GetValue(0, 0).ToString();
                    EditText et_Choose = Form.Items.Item(ItemEventInfo.ItemUID).Specific as EditText;
                    try
                    {
                        et_Choose.Value = value;
                    }
                    catch { }
                }
            }
        }
        #endregion

        private void Busca()
        {
            ItemFiltroModel filtroModel = new ItemFiltroModel();
            filtroModel.Filial = Filial;
            filtroModel.ItemCode = Form.DataSources.UserDataSources.Item("ud_Item").Value;
            filtroModel.CodEngenharia = Form.DataSources.UserDataSources.Item("ud_Eng").Value;
            filtroModel.CodConcorrente = Form.DataSources.UserDataSources.Item("ud_Conc").Value;
            filtroModel.CodMontadora = Form.DataSources.UserDataSources.Item("ud_Mont").Value;
            filtroModel.ProdutoMontadora = Form.DataSources.UserDataSources.Item("ud_Prod").Value;
            filtroModel.Aplicacao = Form.DataSources.UserDataSources.Item("ud_Aplic").Value;
            filtroModel.TipoMaquina = Form.DataSources.UserDataSources.Item("ud_Maq").Value;
            filtroModel.Funcao = Form.DataSources.UserDataSources.Item("ud_Func").Value;

            string sql = ItemBLL.GetItemSQL(filtroModel);
            Form.DataSources.DataTables.Item("dt_Item").ExecuteQuery(sql);
        }

        private void SetDocumentValues()
        {
            if (DocumentForm != null)
            {
                //EventFilterBLL.DisableEvents();
                try
                {
                    Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                    Matrix mt_Doc = DocumentForm.Items.Item("38").Specific as Matrix;

                    if (gr_Item.Rows.SelectedRows != null && gr_Item.Rows.SelectedRows.Count > 0)
                    {
                        DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");

                        int row = gr_Item.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
                        string itemCode = dt_Item.GetValue("Cód. Item", row).ToString();

                        ((EditText)mt_Doc.GetCellSpecific("1", DocumentRow)).Value = itemCode;
                        // Quanto seta o item, ele apaga o concorrente
                        ((EditText)mt_Doc.GetCellSpecific("U_CVA_Hybel", DocumentRow)).Value = itemCode;
                        // Setando o item novamente pro foco ficar no campo
                        //((EditText)mt_Doc.GetCellSpecific("1", DocumentRow)).Value = itemCode;

                        DocumentBaseView.ItemHybel = itemCode;
                        DocumentBaseView.Line = DocumentRow;
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
                    //EventFilterBLL.EnableEvents();
                }
            }
        }
    }
}
