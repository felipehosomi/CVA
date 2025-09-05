using CVA.View.EmissorEtiqueta.BLL;
using CVA.View.EmissorEtiqueta.CONTROLLER;
using CVA.View.EmissorEtiqueta.MODEL;
using Dover.Framework.Attribute;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;

namespace CVA.View.EmissorEtiqueta.VIEW
{
    public partial class PrincipalFormView
    {
        #region View Components
        public EditText et_Item { get; set; }
        public EditText et_ItemDescription { get; set; }
        public ComboBox cb_Lote { get; set; }
        public EditText et_Quantity { get; set; }
        public ComboBox cb_LabelType { get; set; }
        private Button bt_Print { get; set; }
        private Button bt_Cancel { get; set; }
        private Button bt_Help { get; set; }
        private EditText tb_Fabrication { get; set; }
        private EditText tb_Validate { get; set; }
        #endregion

        #region Private Instances
        private OitmBLL _oitmBLL { get; set; }
        private ObtnBLL _obtnBLL { get; set; }
        private SAPbouiCOM.Application _application { get; set; }
        private HelpFormController _helpFormController { get; set; }
        private PrintBLL _printBLL { get; set; }
        #endregion
    }

    [FormAttribute("CVAInitForm", "CVA.View.EmissorEtiqueta.FORM.CVAFormPrincipal.srf")]
    [MenuEvent(UniqueUID = "CVAFormPrincipal")]
    public partial class PrincipalFormView : DoverUserFormBase
    {
        /// <summary>
        /// Contructor Method
        /// Application dependency injection
        /// </summary>
        /// <param name="oitmBLL"></param>
        /// <param name="obtnBLL"></param>
        /// <param name="application"></param>
        /// <param name="helpFormController"></param>
        public PrincipalFormView(OitmBLL oitmBLL, ObtnBLL obtnBLL, SAPbouiCOM.Application application, HelpFormController helpFormController, PrintBLL printBLL)
        {
            _oitmBLL            = oitmBLL;
            _obtnBLL            = obtnBLL;
            _application        = application;
            _helpFormController = helpFormController;
            _printBLL           = printBLL;
            
            OnInitializeCustomEvents();
        }

        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }

        /// <summary>
        /// Initialize Form components and realize bind to define components in partial to xml form.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.UIAPIRawForm.Freeze(true);

            et_Item             = this.GetItem("tbItem").Specific as EditText;
            et_ItemDescription  = this.GetItem("tbDesc").Specific as EditText;
            cb_Lote             = this.GetItem("cbLote").Specific as ComboBox;
            et_Quantity         = this.GetItem("tbQuant").Specific as EditText;
            cb_LabelType        = this.GetItem("cbMod").Specific as ComboBox;
            bt_Print            = this.GetItem("btImp").Specific as Button;
            bt_Help             = this.GetItem("btHelp").Specific as Button;
            bt_Cancel           = this.GetItem("btCan").Specific as Button;
            tb_Fabrication      = this.GetItem("tbFab").Specific as EditText;
            tb_Validate         = this.GetItem("tbVal").Specific as EditText;

            LoadLabelTypes();
            this.UIAPIRawForm.Freeze(false);
        }


        /// <summary>
        /// Initialize events from components defineds in partial
        /// </summary>
        public void OnInitializeCustomEvents()
        {
            et_Item.ChooseFromListAfter += new _IEditTextEvents_ChooseFromListAfterEventHandler(Et_Item_PressedAfter);
            bt_Cancel.ClickAfter        += Bt_Cancel_ClickAfter;
            bt_Help.ClickAfter          += Bt_Help_ClickAfter;
            bt_Print.ClickAfter         += Bt_Print_ClickAfter;
        }

        #region Events
        protected internal virtual void Bt_Print_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            VIEWPARAMETER view_parameter = new VIEWPARAMETER();
            view_parameter.ItemCode     = et_Item.Value;
            view_parameter.Lote         = cb_Lote.Selected.Description;
            view_parameter.Modelo       = cb_LabelType.Selected.Value;
            view_parameter.Quantidade   = et_Quantity.Value;
            view_parameter.Validade     = tb_Validate.Value;
            view_parameter.Fabricacao   = tb_Fabrication.Value;

            _printBLL.CreateFile(view_parameter);
        }

        protected internal virtual void Bt_Cancel_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            _application.SetStatusBarMessage("CVA: Encerrando addin de impressão de etiqueta", BoMessageTime.bmt_Short, false);
            this.UIAPIRawForm.Close();
        }

        protected internal virtual void Bt_Help_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            _helpFormController.Show();
        }

        protected internal virtual void Et_Item_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);

            SBOChooseFromListEventArg chooseFromListEvent = ((SAPbouiCOM.SBOChooseFromListEventArg)(pVal));
            UserDataSource dataSource = this.UIAPIRawForm.DataSources.UserDataSources.Item("udsItem");
            
            if (chooseFromListEvent.SelectedObjects != null)
                dataSource.ValueEx = chooseFromListEvent.SelectedObjects.GetValue(0, 0).ToString();

            this.et_ItemDescription.Value = _oitmBLL.GetItemDescription(dataSource.ValueEx);
            LoadLoteCombo(dataSource.ValueEx);
            this.UIAPIRawForm.Freeze(false);
        }
        #endregion

        #region Aux Methods
        private void LoadLoteCombo(string itemCode)
        {
            var lotes = _obtnBLL.GetLoteByItem(itemCode);
            if (lotes.Count > 0)
            {
                EnableFields();
                foreach (var lote in lotes)
                    cb_Lote.ValidValues.Add(lote.DistNumber, lote.DistNumber);
            }
            else
            {
                EnableFields(false);
                _application.SetStatusBarMessage("CVA: Atenção, não existem lotes cadastrados para este item!", BoMessageTime.bmt_Short, true);
            }       
        }

        private void EnableFields(bool enable = true)
        {
            cb_Lote.Item.Enabled = enable;
            bt_Print.Item.Enabled = enable;

            if (enable)
            {
                while (cb_Lote.ValidValues.Count > 0)
                {
                    cb_Lote.ValidValues.Remove(0, BoSearchKey.psk_Index);
                }
            }
        }

        private void LoadLabelTypes()
        {
            try
            {
                cb_LabelType.ValidValues.Add(1.ToString(), "Componentes");
                cb_LabelType.ValidValues.Add(2.ToString(), "Implantes");
                cb_LabelType.ValidValues.Add(3.ToString(), "Interno");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}