using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Demonstrativo de Produto
    /// </summary>
    public class f2000003040 : BaseForm
    {
        Form Form;
        public static bool Changed = false;

        #region Constructor
        public f2000003040()
        {
            FormCount++;
        }

        public f2000003040(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003040(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003040(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public object Show(ItemModel itemModel)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            try
            {
                Form.DataSources.UserDataSources.Item("ud_Item").Value = itemModel.ItemCode;
                Form.DataSources.UserDataSources.Item("ud_Desc").Value = itemModel.ItemName;
                Form.DataSources.UserDataSources.Item("ud_Fis").Value = itemModel.EstoqueFisico.ToString();
                Form.DataSources.UserDataSources.Item("ud_Enc").Value = itemModel.EstoqueEncomendado.ToString();
                Form.DataSources.UserDataSources.Item("ud_Res").Value = itemModel.EstoqueReservado.ToString();
                Form.DataSources.UserDataSources.Item("ud_Dis").Value = itemModel.EstoqueDisponivel.ToString();
                Form.DataSources.UserDataSources.Item("ud_Min").Value = itemModel.EstoqueMinimo.ToString();

                Form.DataSources.DataTables.Item("dt_Entrada").ExecuteQuery(SimuladorVendaBLL.GetEntradasSQL(itemModel.ItemCode));
                Form.DataSources.DataTables.Item("dt_Saida").ExecuteQuery(SimuladorVendaBLL.GetSaidasSQL(itemModel.ItemCode));

                Grid gr_Entrada = Form.Items.Item("gr_Entrada").Specific as Grid;
                gr_Entrada.RowHeaders.Width = 0;

                EditTextColumn cl_QtdeEntrada = (EditTextColumn)gr_Entrada.Columns.Item("Qtde");
                cl_QtdeEntrada.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

                Grid gr_Saida = Form.Items.Item("gr_Saida").Specific as Grid;
                gr_Saida.RowHeaders.Width = 0;

                EditTextColumn cl_QtdeSaida = (EditTextColumn)gr_Saida.Columns.Item("Qtde");
                cl_QtdeSaida.ColumnSetting.SumType = BoColumnSumType.bst_Auto;
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
            return Form;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_RESIZE)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    try
                    {

                        Form.Freeze(true);
                        Item gr_Entrada = Form.Items.Item("gr_Entrada");
                        gr_Entrada.Width = (int)(Form.Width * 0.46);
                        gr_Entrada.Height = (int)(Form.Height * 0.75);

                        Item gr_Saida = Form.Items.Item("gr_Saida");
                        gr_Saida.Left = Form.Width / 2;
                        gr_Saida.Width = (int)(Form.Width * 0.46);
                        gr_Saida.Height = (int)(Form.Height * 0.75);
                    }
                    catch { }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }
            }
            return true;
        }
    }
}
