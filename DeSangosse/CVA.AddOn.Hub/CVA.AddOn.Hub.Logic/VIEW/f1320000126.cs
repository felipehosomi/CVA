using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Hub.Logic.BLL;
using CVA.AddOn.Hub.Logic.DAO.Lote;
using CVA.AddOn.Hub.Logic.MODEL;
using SAPbouiCOM;
using System.Collections.Generic;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f1320000126 : BaseForm
    {
        private Form form = null;
        public List<LotesModel> ListaItemLote { get; set; }
       


        #region Constructor
        public f1320000126()
        {
            FormCount++;
        }

        public f1320000126(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f1320000126(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1320000126(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        
        
        #endregion

        #region Show()

        public object Show()
        {
            return null;
        }

        public object Show(string[] args)
        {
            form = SBOApp.Application.OpenForm(BoFormObjectEnum.fo_ItemBatchNumbers, "", args[0]);

            
            return form;

        }
        #endregion

        public override bool ItemEvent()
        {
            this.IsSystemForm = true;
            base.ItemEvent();

            var button = (Button)Form.Items.Item("1").Specific;
            var BtnCancelar = (Button)Form.Items.Item("2").Specific;

            if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK && (ItemEventInfo.ItemUID == "1"|| ItemEventInfo.ItemUID == "2"))
            {
                var oMatrixItem = (Matrix)Form.Items.Item("3").Specific;
                var oMatrixLote = (Matrix)Form.Items.Item("5").Specific;
                
                string erro = "";

                ListaItemLote = new List<LotesModel>();
                if (button.Caption == "OK" && BtnCancelar.Caption == "Cancelar" )
                {

                    for (int i = 1; i <= oMatrixItem.VisualRowCount; i++)
                    {
                        if (!oMatrixItem.IsRowSelected(i) && i < oMatrixItem.RowCount)
                        {
                            oMatrixItem.Columns.Item("0").Cells.Item(i).Click(BoCellClickType.ct_Regular);
                        }



                        for (int j = 1; j <= oMatrixLote.VisualRowCount; j++)
                        {
                            var loteModel = new LotesModel();
                            loteModel.Item = ((EditText)oMatrixItem.GetCellSpecific("1", i)).Value.ToString();
                            loteModel.Lote = ((EditText)oMatrixLote.GetCellSpecific("1", j)).Value.ToString();
                            ListaItemLote.Add(loteModel);
                        }
                        
                        if (oMatrixItem.IsRowSelected(i) && i < oMatrixItem.RowCount)
                        {

                            oMatrixItem.Columns.Item("0").Cells.Item(i + 1).Click(BoCellClickType.ct_Regular);
                        }                        
                    }
                    var _lotesPedidoVendaBll = new LotesPedidoVendaBLL();
                    _lotesPedidoVendaBll.InsertLote(ListaItemLote);
                }
            }
         return true;
        }
    }
}
