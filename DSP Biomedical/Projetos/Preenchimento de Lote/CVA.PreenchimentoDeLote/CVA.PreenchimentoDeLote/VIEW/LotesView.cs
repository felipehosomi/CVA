using CVA.PreenchimentoDeLote.Auxiliar;
using CVA.PreenchimentoDeLote.DAO;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace CVA.PreenchimentoDeLote.VIEW
{
    [FormAttribute("41", "CVA.PreenchimentoDeLote.RESOURCES.form.Lote_Definicao.xml")]
    public class LotesView : DoverSystemFormBase
    {
        #region Atributos



        public SAPbouiCOM.Application application { get; set; }
        public SAPbouiCOM.ItemEvent pVal { get; set; }
        public SAPbobsCOM.Company _company { get; set; }
        public SAPbouiCOM.IForm _form { get; set; }

        public SAPbouiCOM.Matrix matrix { get; set; }



        public CalculaLoteDAO _calculaLoteDAO { get; set; }
        public LotesView _lotesView { get; set; }


        #endregion

        #region Construtor

        public LotesView()
        {
            _calculaLoteDAO = new CalculaLoteDAO();

        }
        #endregion


        public override void OnInitializeComponent()
        {
            _form = ((DoverSystemFormBase)(this)).UIAPIRawForm;

            matrix = (Matrix)this.GetItem("3").Specific;


            OnCustomInitializeEvents();
        }

        public override void OnInitializeFormEvents()
        {

        }

        private void OnCustomInitializeEvents()
        {
            LoadNewLote();
        }

        public void LoadNewLote()
        {
            var writer = new Writer();
            var read = writer.Read().ToString();

            ((SAPbouiCOM.EditText)matrix.Columns.Item(2).Cells.Item(1).Specific).Value = read;


        }
    }
}
