using CVA.Core.DSP.Controle.BLL;
using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.HELPER;
using CVA.Core.DSP.Controle.MODEL;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.VIEW
{
    public partial class OrdemProducaoView
    {
        private SAPbouiCOM.Application _application { get; set; }
        private RecursoBLL _RecursoBLL { get; set; }
        private EditText et_Item { get; set; }
        private EditText et_Quantity { get; set; }
        private Matrix mt_Items { get; set; }
        public string ItemCode { get; set; }
        private double Quantity { get; set; }
        
    }

  //  [FormAttribute("65211", "CVA.Core.DSP.Controle.Resources.Forms.EmptyFormPartial.xml")]
    public partial class OrdemProducaoView : DoverSystemFormBase
    {
        public OrdemProducaoView(SAPbouiCOM.Application application, RecursoBLL recursoBLL)
        {
            _application = application;
            _RecursoBLL = recursoBLL;

        }

        public override void OnInitializeComponent()
        {

            this.UIAPIRawForm.Title = "Cadastro de Recursos e Ferramentas";
            et_Item = this.GetItem("6").Specific as EditText;
            et_Quantity = this.GetItem("12").Specific as EditText;
            mt_Items = this.GetItem("37").Specific as Matrix;
            OnCustomInitializeEvents();
        }

        public void OnCustomInitializeEvents()
        {
            et_Item.GotFocusAfter += Et_Item_GotFocusAfter;
            et_Item.LostFocusAfter += Et_Item_LostFocusAfter;

            et_Quantity.GotFocusAfter += Et_Quantity_GotFocusAfter;
            et_Quantity.LostFocusAfter += Et_Quantity_LostFocusAfter;


        }

        protected internal virtual void Et_Item_GotFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            ItemCode = et_Item.Value.Trim();
        }

        protected internal virtual void Et_Item_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (ItemCode != et_Item.Value.Trim())
            {
                this.SetFixedValue();
            }
        }

        protected internal virtual void Et_Quantity_GotFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            Quantity = double.Parse(et_Quantity.Value.Replace(".", ","));
        }

        protected internal virtual void Et_Quantity_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (Quantity != double.Parse(et_Quantity.Value.Replace(".", ",")))
            {
                this.SetFixedValue();
            }
        }

        private void SetFixedValue()
        {
            if (!String.IsNullOrEmpty(et_Item.Value.Trim()))
            {
                UIAPIRawForm.Freeze(true);
                try
                {
                    Quantity = double.Parse(et_Quantity.Value.Replace(".", ","));
                    List<Recurso> list = _RecursoBLL.GetRecursoFixo(et_Item.Value);
                    if (list.Count > 0)
                    {
                        for (int i = 1; i <= mt_Items.RowCount; i++)
                        {
                            ComboBox cb_Tipo = mt_Items.GetCellSpecific("1880000002", i) as ComboBox;
                            if (cb_Tipo.Value == "290")
                            {
                                EditText et_Recurso = mt_Items.GetCellSpecific("4", i) as EditText;
                                Recurso recurso = list.FirstOrDefault(r => r.Code == et_Recurso.Value.Trim());
                                if (recurso != null)
                                {
                                    EditText et_PlannedQty = mt_Items.GetCellSpecific("14", i) as EditText;
                                    et_PlannedQty.Value = recurso.Quantity.ToString().Replace(",", ".");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _application.SetStatusBarMessage(ex.Message);
                }
                finally
                {
                    UIAPIRawForm.Freeze(false);
                }
            }
        }

       
    }
}
