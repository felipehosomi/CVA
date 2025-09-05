using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace Picking.Producao.Addon.View
{
    [CVA.AddOn.Common.Attributes.Form(1101)]
    public class FrmPesquisasSatisfacao : BaseForm
    {
        Form oForm;

        #region Constructor
        public FrmPesquisasSatisfacao()
        {
            FormCount++;
        }

        public FrmPesquisasSatisfacao(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public FrmPesquisasSatisfacao(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public FrmPesquisasSatisfacao(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show(string srfPath)
        {
            var obj = base.Show(srfPath);
            LoadGrid();
            return obj;
        }

        private void LoadGrid()
        {
            var grid = (Grid)this.Form.Items.Item("gr_Item").Specific;
            grid.DataTable.ExecuteQuery("SELECT \"Code\", \"U_Desc\", \"U_Ativa\" FROM \"@CVA_PESQUISA\"");
            grid.Columns.Item("Code").Visible = false;
            grid.Columns.Item("U_Desc").Editable = false;
            grid.Columns.Item("U_Desc").TitleObject.Caption = "Pesquisa";
            grid.Columns.Item("U_Ativa").TitleObject.Caption = "Ativa";
            grid.Columns.Item("U_Ativa").Type = BoGridColumnType.gct_CheckBox;
            grid.AutoResizeColumns();
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Save")
                    {
                        Save();
                    }
                    else if (ItemEventInfo.ItemUID == "bt_Close")
                    {
                        oForm = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
                        oForm.Close();
                    }
                }
            }
            return true;
        }

        private void Save()
        {
            try
            {
                oForm = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
                Recordset rs = SBOApp.Application.Company.GetDICompany().GetBusinessObject(BoObjectTypes.BoRecordset);
                var grid = (Grid)oForm.Items.Item("gr_Item").Specific;

                for (int i = 0; i < grid.DataTable.Rows.Count; i++)
                {
                    string currentCode = grid.DataTable.GetValue("Code", i);
                    string ativa = grid.DataTable.GetValue("U_Ativa", i);
                    rs.DoQuery($"UPDATE \"@CVA_PESQUISA\" SET \"U_Ativa\" = '{ativa}' WHERE \"Code\" = '{currentCode}'");
                }

                SBOApp.Application.StatusBar.SetText("Pesquisas atualizadas com sucesso", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBOApp.Application.StatusBar.SetText("Erro ao salvar: " + ex.ToString(), BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
