using CVA.Core.DSP.Controle.BLL;
using CVA.Core.DSP.Controle.HELPER;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.Core.DSP.Controle.VIEW
{
    public partial class ProdutoAcabadoView
    {
        private FerramentasBLL _ferramentaBLL { get; set; }
        private ItemBLL _itemBLL { get; set; }
        private SAPbouiCOM.Application _application { get; set; }
        public Matrix grid_View { get; set; }
        public Button bt_Adicionar { get; set; }
    }
  //  [FormAttribute(B1Forms.EntradaProdutoAcabado, "CVA.Core.DSP.Controle.Resources.Forms.RecursoFormPartial.xml")]
    public partial class ProdutoAcabadoView : DoverSystemFormBase
    {
        public ProdutoAcabadoView(FerramentasBLL ferramentaBLL, ItemBLL itemBLL, SAPbouiCOM.Application application)
        {
            _ferramentaBLL = ferramentaBLL;
            _itemBLL = itemBLL;
            _application = application;
        }

        public override void OnInitializeComponent()
        {
            grid_View = this.GetItem("13").Specific as Matrix;
            bt_Adicionar = this.GetItem("1").Specific as Button;
        }

        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            for (int i = 0; i < grid_View.RowCount; i++)
            {
                BusinessOneLog.Add("Iniciando processo de atualização do contador");
                var itemColumn = grid_View.Columns.Item(5);
                var quantityColumn = grid_View.Columns.Item(15);
                var et_itemCode = itemColumn.Cells.Item(i + 1).Specific as EditText;
                var et_quantity = quantityColumn.Cells.Item(i + 1).Specific as EditText;

                var itemCode = et_itemCode.Value;

                if (!string.IsNullOrEmpty(itemCode))
                {
                    var oitm = _itemBLL.GetCounter(itemCode);
                    int quantity = 0;
                    int.TryParse(et_quantity.Value.Split('.')[0], out quantity);
                    if (quantity > 0)
                    {
                        if (_ferramentaBLL.Update(oitm.Tool, quantity * oitm.Quantity))
                            _application.SetStatusBarMessage("CVA: Ferramenta atualizada com sucesso!", BoMessageTime.bmt_Medium, false);
                        else
                            _application.SetStatusBarMessage("CVA: Erro ao atualizar ferramenta", BoMessageTime.bmt_Medium);
                    }
                }

            }
        }
    }
}