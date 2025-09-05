using CVA.Core.ControleDesgasteFerramenta.HELPER;
using CVA.View.ControleQualidade.MODEL;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Linq;

namespace CVA.View.ControleQualidade.VIEW
{
    [FormAttribute(B1Forms.Lote, "CVA.View.ControleQualidade.Resources.Form.EmptyFormPartial.srf")]
    public class LoteView : DoverSystemFormBase
    {
        public Matrix mt_Principal { get; set; }
        public Matrix mt_Item { get; set; }
        public Button bt_Adicionar { get; set; }
        
        public LoteView()
        {

        }

        public override void OnInitializeComponent()
        {
            mt_Item = this.GetItem("35").Specific as Matrix;
            mt_Principal = this.GetItem("3").Specific as Matrix;
            bt_Adicionar = this.GetItem("1").Specific as Button;

            OnCustomInitializeCustomEvents();
        }
        
        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }        

        private void OnCustomInitializeCustomEvents()
        {
            bt_Adicionar.ClickBefore += Bt_Adicionar_ClickBefore;
        }

        protected internal virtual void Bt_Adicionar_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                string itemCode = (mt_Item.Columns.Item("5").Cells.Item(1).Specific as EditText).Value;

                if (StaticKeys.ItemList == null)
                {
                    StaticKeys.ItemList = new System.Collections.Generic.List<MODEL.Item>();
                }

                MODEL.Item item = StaticKeys.ItemList.FirstOrDefault(i => i.ItemCode == itemCode);
                if (item == null)
                {
                    item = new MODEL.Item();
                    item.ItemCode = itemCode;
                    StaticKeys.ItemList.Add(item);
                }

                var columnLote = mt_Principal.Columns.Item("2");
                item.LoteList = new System.Collections.Generic.List<ItemLote>();
                for (int i = 1; i <= mt_Principal.RowCount; i++)
                {
                    var lote = columnLote.Cells.Item(1).Specific as EditText;
                    item.LoteList.Add(new ItemLote() { Lote = lote.Value });
                }

                BubbleEvent = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
