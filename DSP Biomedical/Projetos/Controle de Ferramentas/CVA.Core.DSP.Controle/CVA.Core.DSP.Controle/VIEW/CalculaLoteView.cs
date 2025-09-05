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
    [FormAttribute(B1Forms.OrdemProducao, "CVA.Core.DSP.Controle.Resources.Forms.CalculaLote.xml")]
    public class CalculaLoteView : DoverSystemFormBase
    {
        #region Atributos

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
