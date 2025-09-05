using CVA.PreenchimentoDeLote.BLL;
using CVA.PreenchimentoDeLote.DAO;
using CVA.PreenchimentoDeLote.MODEL;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace CVA.PreenchimentoDeLote.VIEW
{
    [FormAttribute("65211", "CVA.PreenchimentoDeLote.RESOURCES.form.CalculaLote.xml")]
    public class CalculaLoteView : DoverSystemFormBase
    {
        #region Atributos

        public ItemEvent ItemEventInfo { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public SAPbobsCOM.Company _company { get; set; }
        public IForm _form { get; set; }
        public SAPbouiCOM.ItemEvent itemEvent { get; set; }
        public SAPbouiCOM.BusinessObjectInfo businessObjectInfo { get; set; }
        public SAPbouiCOM.ContextMenuInfo contextMenuInfo { get; set; }


        public CalculaLoteBLL _CalculaLoteBLL { get; set; }
        public CalculaLoteDAO _CalculaLoteDAO { get; set; }

        public SAPbouiCOM.ComboBox cb_Callote { get; set; }

        public SAPbouiCOM.EditText et_numLote { get; set; }
        public SAPbouiCOM.EditText et_sublote { get; set; }

        public CalculaLoteView(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }


        #endregion

        #region Construtor
        public CalculaLoteView()
        {
            _CalculaLoteBLL = new CalculaLoteBLL();
            _CalculaLoteDAO = new CalculaLoteDAO();            
        }

        #endregion

        public override void OnInitializeComponent()
        {
            _form = ((DoverSystemFormBase)(this)).UIAPIRawForm;


            cb_Callote = this.GetItem("cb_Callote").Specific as SAPbouiCOM.ComboBox;
            et_numLote = this.GetItem("et_numLote").Specific as SAPbouiCOM.EditText;
            et_sublote = this.GetItem("et_sublote").Specific as SAPbouiCOM.EditText;

            cb_Callote.Item.DisplayDesc = true;
            cb_Callote.Select("Y", BoSearchKey.psk_ByValue);            
            et_numLote.Item.Enabled = false;
            et_numLote.Value = MontaProximoLote(_CalculaLoteBLL.GetNextLote()) + "-";
            OnCustomInitializeEvents();

            
        }

        public override void OnInitializeFormEvents()
        {
            
        }

        private void OnCustomInitializeEvents()
        {
            cb_Callote.ClickAfter += Cb_Callote_ClickAfter;

            
        }

        protected internal virtual void Cb_Callote_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (cb_Callote.Selected.Value == "Y")
            {
                et_numLote.Item.Enabled = false;
                et_numLote.Value = MontaProximoLote(_CalculaLoteBLL.GetNextLote()) + "-";
                
            }
            else
            {
                et_numLote.Item.Enabled = true;
                et_numLote.Value = "";
            }
        }

        public string MontaProximoLote(NextLote model)
        {

            int aux = model.code.IndexOf("-");
            
            var next = Convert.ToInt32(model.code.Substring(0, aux)) + 1;
            return next.ToString();


        }
        //public void LoadNewLote (string novolote)
        //{
        //    _CalculaLoteBLL.GetNewLote(novolote);

        //}
    





    }
}
