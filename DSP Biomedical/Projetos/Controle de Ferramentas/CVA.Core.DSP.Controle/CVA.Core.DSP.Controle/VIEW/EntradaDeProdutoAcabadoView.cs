using CVA.Core.DSP.Controle.Auxiliar;
using CVA.Core.DSP.Controle.DAO;
using Dover.Framework.Form;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.VIEW
{
    [FormAttribute("65214", "CVA.Core.DSP.Controle.Resources.Forms.RecursoFormPartial.xml")]
    public class EntradaDeProdutoAcabadoView : DoverSystemFormBase
    {
        #region Atriburos

        public SAPbouiCOM.Application application { get; set; }
        public SAPbouiCOM.ItemEvent pVal { get; set; }
        public SAPbobsCOM.Company _company { get; set; }
        public SAPbouiCOM.IForm _form { get; set; }

        public CalculaLoteDAO _calculaLoteDAO { get; set; }
        public LotesView _lotesView { get; set; }

        public SAPbouiCOM.Matrix matrix { get; set; }

        #endregion

        #region Construtor

        public EntradaDeProdutoAcabadoView()
        {

        }

        #endregion


        public override void OnInitializeComponent()
        {
            _form = ((DoverSystemFormBase)(this)).UIAPIRawForm;

            matrix = this.GetItem("13").Specific as SAPbouiCOM.Matrix;

            OnCustomInitializeEvents();
        }

        public override void OnInitializeFormEvents()
        {

        }

        private void OnCustomInitializeEvents()
        {
            matrix.KeyDownBefore += Matrix_KeyDownBefore;
        }

        private void Matrix_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.CharPressed == 13)
            {
                var valor = (EditText)matrix.GetCellSpecific(1, 1);

                var doc = (ProductionOrders)_company.GetBusinessObject(BoObjectTypes.oProductionOrders);
                doc.GetByKey(Convert.ToInt32(valor.Value));

                var novoLote = valor.Value;

                var result = _calculaLoteDAO.GetNewLote(novoLote);

                var writer = new Writer();
                writer.Write(result.Rows[0]["novolote"].ToString());
            }
        }
    }
}
