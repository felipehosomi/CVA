using CVA.Core.ControleDesgasteFerramenta.HELPER;
using CVA.View.ControleQualidade.BLL;
using CVA.View.ControleQualidade.MODEL;
using Dover.Framework.Attribute;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.VIEW
{
    public partial class OrdemProducaoView
    {
        private ApontamentoQualidadeView _apontamentoQualidadeView { get; set; }
        private ApontamentoInspetorView _apontamentoInspetorView { get; set; }
        private ApontamentoBLL _apontamentoBLL { get; set; }
        private ItemBLL _itemBLL { get; set; }

        private SAPbouiCOM.Application _application { get; set; }
        private SAPbobsCOM.Company _company { get; set; }
        public DBDataSource ds_Main { get; set; }
    }

    [FormAttribute(B1Forms.OrdemProducao, "CVA.View.ControleQualidade.Resources.Form.EmptyFormPartial.srf")]
    public partial class OrdemProducaoView : DoverSystemFormBase
    {
        #region Initialize
        public OrdemProducaoView(SAPbouiCOM.Application application, SAPbobsCOM.Company company, ApontamentoBLL apontamentoBLL, ItemBLL itemBLL, ApontamentoQualidadeView apontamentoQualidadeView, ApontamentoInspetorView apontamentoInspetor)
        {
            _application = application;
            _company = company;
            _apontamentoBLL = apontamentoBLL;
            _itemBLL = itemBLL;
            _apontamentoQualidadeView = apontamentoQualidadeView;
            _apontamentoInspetorView = apontamentoInspetor;
        }


        public override void OnInitializeComponent()
        {
            ds_Main = UIAPIRawForm.DataSources.DBDataSources.Item("OWOR");
        }

        public override void OnInitializeFormEvents()
        {
            _application.MenuEvent += _application_MenuEvent;
        }
        #endregion

        #region Events
        protected internal virtual void _application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "frmQualidade":
                        _apontamentoQualidadeView = this.CreateForm<ApontamentoQualidadeView>();

                        _apontamentoQualidadeView.OrdemProducao = ds_Main.GetValue("DocNum", ds_Main.Offset);
                        _apontamentoQualidadeView.TipoInspecao = TipoInspecaoEnum.Qualidade;

                        StaticKeys.ItemList = new List<MODEL.Item>();
                        StaticKeys.ItemList.Add(new MODEL.Item() { ItemCode = ds_Main.GetValue("ItemCode", ds_Main.Offset) });

                        _apontamentoQualidadeView.AlreadyExists = _apontamentoBLL.ExistsQualidade(ds_Main.GetValue("DocNum", ds_Main.Offset));

                        _apontamentoQualidadeView.Proccess();
                        _apontamentoQualidadeView.Show();
                        break;
                    case "frmInspetor":
                        _apontamentoInspetorView = this.CreateForm<ApontamentoInspetorView>();

                        _apontamentoInspetorView.OrdemProducao = ds_Main.GetValue("DocNum", ds_Main.Offset);
                        _apontamentoInspetorView.ItemCode = ds_Main.GetValue("ItemCode", ds_Main.Offset);

                        _apontamentoInspetorView.Proccess();
                        _apontamentoInspetorView.Show();
                        break;
                }
            }
        }

        protected override void OnFormRightClickBefore(ref ContextMenuInfo pVal, out bool BubbleEvent)
        {
            try
            {
                if (ds_Main.GetValue("Status", ds_Main.Offset) == "R")
                {
                    MODEL.Item item = _itemBLL.GetInspecaoPorItem(ds_Main.GetValue("ItemCode", ds_Main.Offset));
                    if (!String.IsNullOrEmpty(item.PlanoInspecao))
                    {
                        MenuItem oMenuItem = null;
                        Menus oMenus = null;
                        MenuCreationParams oCreationPackage = null;
                        oCreationPackage = ((MenuCreationParams)(_application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams)));

                        // 1280 - Menu do botão direito
                        oMenuItem = _application.Menus.Item("1280");
                        oMenus = oMenuItem.SubMenus;

                        oCreationPackage.Type = BoMenuType.mt_STRING;
                        oCreationPackage.Enabled = true;

                        if (item.TipoInspecao == "1")
                        {
                            // Criação do menu Inspetor
                            oCreationPackage.UniqueID = "frmInspetor";
                            oCreationPackage.String = "Plano de Inspeção - Inspetor";
                            oMenus.AddEx(oCreationPackage);
                        }
                        else if (item.TipoInspecao == "2")
                        {
                            // Criação do menu Qualidade
                            oCreationPackage.UniqueID = "frmQualidade";
                            oCreationPackage.String = "Plano de Inspeção - Qualidade";
                            oMenus.AddEx(oCreationPackage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_application.SetStatusBarMessage("CVA: " + ex.Message);
            }
            BubbleEvent = true;
        }

        protected override void OnFormRightClickAfter(ref ContextMenuInfo pVal)
        {

            if (ds_Main.GetValue("Status", ds_Main.Offset) == "R")
            {
                MenuItem oMenuItem = null;
                Menus oMenus = null;

                oMenuItem = _application.Menus.Item("1280");
                oMenus = oMenuItem.SubMenus;
                try
                {
                    oMenus.RemoveEx("frmQualidade");
                }
                catch { }
                try
                {
                    oMenus.RemoveEx("frmInspetor");
                }
                catch { }
            }

        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            _application.MenuEvent -= _application_MenuEvent;
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            UIAPIRawForm.Title = "Ordem de Produção";
        }
        #endregion
    }
}
