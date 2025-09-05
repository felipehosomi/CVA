using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Administração de picking
    /// </summary>
    public class f81 : BaseForm
    {
        #region Constructor
        public f81()
        {
            FormCount++;
        }

        public f81(SAPbouiCOM.ItemEvent itemEvent)
        { 
            this.ItemEventInfo = itemEvent;
        }

        public f81(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f81(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        public override bool ItemEvent()
        {
            base.ItemEvent();

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Item bt_Base = Form.Items.Item("11");
                    Item it_Complet = Form.Items.Add("bt_Complet", BoFormItemTypes.it_BUTTON);
                    
                    it_Complet.Height = bt_Base.Height;
                    it_Complet.Width = bt_Base.Width;
                    it_Complet.Top = bt_Base.Top;
                    it_Complet.Left = bt_Base.Left - bt_Base.Width - 5;
                    it_Complet.LinkTo = "11";
                    it_Complet.FromPane = 1;
                    it_Complet.ToPane = 1;

                    ((Button)Form.Items.Item("bt_Complet").Specific).Caption = "Somente pedidos completos";
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "bt_Complet")
                {
                    try
                    {
                        Form.Freeze(true);
                        Matrix mt_Picking = Form.Items.Item("10").Specific as Matrix;
                        // Ordena pelo número do documento para remover as linhas corretamente
                        mt_Picking.Columns.Item("11").TitleObject.Sort(BoGridSortType.gst_Ascending);

                        List<PickingModel> pickingList = new List<PickingModel>();

                        for (int i = 1; i <= mt_Picking.RowCount; i++)
                        {
                            PickingModel model = new PickingModel();
                            model.Linha = i;
                            int nrDoc;
                            double cumprimento;

                            if (Int32.TryParse(((EditText)mt_Picking.GetCellSpecific("11", i)).Value, out nrDoc))
                            {
                                model.NrDoc = nrDoc;
                            }
                            if (double.TryParse(((EditText)mt_Picking.GetCellSpecific("22", i)).Value, out cumprimento))
                            {
                                model.Cumprimento = cumprimento;
                            }
                            pickingList.Add(model);
                        }

                        IEnumerable<IGrouping<int, PickingModel>> groupedByDoc = pickingList.OrderByDescending(p => p.Linha).GroupBy(p => p.NrDoc);
                        foreach (var itemByDoc in groupedByDoc)
                        {
                            if (itemByDoc.Any(i => i.Cumprimento != 100))
                            {
                                foreach (var item in itemByDoc)
                                {
                                    mt_Picking.DeleteRow(item.Linha);
                                }
                            }
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

            return true;
        }
        #endregion
    }
}
