using CVA.Core.DSP.Controle.BLL;
using Dover.Framework.Attribute;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.MODEL;
using CVA.Core.DSP.Controle.HELPER;

namespace CVA.Core.DSP.Controle.VIEW
{
    [FormAttribute("CVA_CAD_MOTIVO", "CVA.Core.DSP.Controle.Resources.Forms.CadastroMotivos.xml")]
    [MenuEvent(UniqueUID = "CVA_CAD_MOTIVO")]

    public class MotivoView : DoverUserFormBase
    {
        #region Atributos
        public MotivoBLL _motivoBLL { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public SAPbobsCOM.Company _company { get; set; }        
        //public SAPbouiCOM.EditText et_nome { get; set; }
        //public SAPbouiCOM.EditText et_desc { get; set; }
        //public SAPbouiCOM.Button bt_add { get; set; }

        #endregion

        #region Construtor
        public MotivoView()
        {

        }
        #endregion


        public override void OnInitializeComponent()
        {
            //et_nome = this.GetItem("et_nome").Specific as EditText;
            //et_desc = this.GetItem("et_desc").Specific as EditText;
            //bt_add = this.GetItem("bt_add").Specific as Button;

            OnCustomInitializeEvents();

        }
        
        public override void OnInitializeFormEvents()
        {

        }

        private void OnCustomInitializeEvents()
        {
            //bt_add.ClickAfter += Bt_add_ClickAfter;

        }

        protected internal virtual void Bt_add_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            
            //var Code = et_nome.Value;
            //var Name = et_desc.Value;
            //_motivoBLL.Cadastrar(Code, Name);
            //et_nome.Value = " ";
            //et_desc.Value = " ";  


        }
    }
}
