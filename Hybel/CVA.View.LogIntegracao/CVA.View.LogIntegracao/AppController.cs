using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;
using SAPbobsCOM;
using System.Xml;

namespace CVA.View.LogIntegracao
{
    public class AppController
    {
        private Application _application { get; set; }
        private Company _company { get; set; }
        private EventFilters _filters { get; set; }

        public AppController()
        {
            // Conecta ao SAP Business One em execução
            Connect();

            // Cria menus
            AddMenu("43520", "CVA", "CVA - Monitoramento de integração", -1, BoMenuType.mt_POPUP, $"{AppDomain.CurrentDomain.BaseDirectory}\\if_sync_126579.jpg");
            AddMenu("CVA", "FRM", "Log de integração", 0, BoMenuType.mt_STRING);

            // Registra os filtros
            AddFilter("FRM", BoEventTypes.et_MENU_CLICK);
            AddFilter("CVAFRM", BoEventTypes.et_ITEM_PRESSED);

            // Registra os eventos
            _application.AppEvent += AppEvent;
            _application.ItemEvent += ItemEvent;
            _application.MenuEvent += MenuEvent;
            
        }

        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                if(pVal.MenuUID.Equals("FRM") && !pVal.BeforeAction)
                {
                    var oForm = LoadForm($"{AppDomain.CurrentDomain.BaseDirectory}\\{FormLog.FileName}");
                    oForm.Freeze(true);
                    LoadForm(oForm);
                    oForm.Visible = true;
                    oForm.PaneLevel = 1;
                    oForm.Items.Item(FormLog.FolderImportacao).Click();
                    EnableMenus(oForm, false);
                    oForm.Freeze(false);
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }

        private void ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            var ret = true;
            Form oForm;

            try
            {
                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_FORM_LOAD) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.ButtonAtualizar) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioExportacaoAguardando) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioExportacaoComErro) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioExportacaoImportado) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioExportacaoTodos) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioImportacaoAguardando) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioImportacaoComErro) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioImportacaoImportado) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }

                if (pVal.FormTypeEx.Equals(FormLog.Type) && pVal.EventType.Equals(BoEventTypes.et_ITEM_PRESSED) && pVal.ItemUID.Equals(FormLog.RadioImportacaoTodos) && !pVal.BeforeAction)
                {
                    oForm = _application.Forms.ActiveForm;
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }

        private void AppEvent(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    if (_application.Menus.Exists("FRM")) _application.Menus.RemoveEx("FRM");
                    if (_application.Menus.Exists("CVA")) _application.Menus.RemoveEx("CVA");
                    Environment.Exit(-1);
                    break;
            }
        }

        private void Connect()
        {
            SetApplication();
            _company = (Company)_application.Company.GetDICompany();
            _filters = new EventFilters();
        }

        private void SetApplication()
        {
            var sboGuiApi = new SboGuiApi();
            var str = Environment.GetCommandLineArgs().GetValue(1).ToString();
            sboGuiApi.Connect(str);
            _application = sboGuiApi.GetApplication();
        }

        private void AddFilter(string containerUID, BoEventTypes eventType)
        {
            int pos;
            if(!FilterExists(eventType, out pos))
            {
                var filter = _filters.Add(eventType);
                filter.AddEx(containerUID);
                _application.SetFilter(_filters);
            }
            else
            {
                _filters.Item(pos).AddEx(containerUID);
                _application.SetFilter(_filters);
            }
        }

        private bool FilterExists(BoEventTypes eventType, out int pos)
        {
            var ret = false;
            pos = -1;

            try
            {
                for(var i = 0; i < _filters.Count; i++)
                {
                    var f = _filters.Item(i);
                    if (eventType.Equals(f.EventType))
                    {
                        ret = true;
                        pos = i;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

        private void AddMenu(string parentID, string menuID, string description, int position, BoMenuType menuType, string imagePath = null)
        {
            var oCreationPackage = (MenuCreationParams)_application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            var oMenuItem = _application.Menus.Item(parentID);
            var oMenus = oMenuItem.SubMenus;
            var exist = (oMenus != null) && oMenuItem.SubMenus.Exists(menuID);

            if (exist)
            {
                oMenuItem.SubMenus.RemoveEx(menuID);
                exist = false;
            }

            if (!exist)
            {
                oCreationPackage.Type = menuType;
                oCreationPackage.UniqueID = menuID;
                oCreationPackage.String = description;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = position;
                oCreationPackage.Image = imagePath;

                if(oMenus == null)
                {
                    oMenuItem.SubMenus.Add(menuID, description, menuType, position);
                    oMenus = oMenuItem.SubMenus;
                }

                oMenus.AddEx(oCreationPackage);
            }
        }

        private Form LoadForm(string formPath)
        {
            var oXmlDoc = new XmlDocument();
            var oCreationPackage = (FormCreationParams)_application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            oCreationPackage.UniqueID = $"{oCreationPackage.UniqueID}{Guid.NewGuid().ToString().Substring(2, 10)}";

            oXmlDoc.Load(formPath);
            oCreationPackage.XmlData = oXmlDoc.InnerXml;
            return _application.Forms.AddEx(oCreationPackage);
        }

        private void LoadForm(Form oForm)
        {
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoImportado).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoImportado).Specific).ValOff = "N";

            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoAguardando).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoAguardando).Specific).ValOff = "N";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoAguardando).Specific).GroupWith(FormLog.RadioExportacaoImportado);

            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoComErro).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoComErro).Specific).ValOff = "N";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoComErro).Specific).GroupWith(FormLog.RadioExportacaoAguardando);

            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoTodos).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoTodos).Specific).ValOff = "N";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioExportacaoTodos).Specific).GroupWith(FormLog.RadioExportacaoComErro);

            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoImportado).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoImportado).Specific).ValOff = "N";

            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoAguardando).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoAguardando).Specific).ValOff = "N";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoAguardando).Specific).GroupWith(FormLog.RadioImportacaoImportado);

            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoComErro).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoComErro).Specific).ValOff = "N";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoComErro).Specific).GroupWith(FormLog.RadioImportacaoAguardando);

            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoTodos).Specific).ValOn = "Y";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoTodos).Specific).ValOff = "N";
            ((OptionBtn)oForm.Items.Item(FormLog.RadioImportacaoTodos).Specific).GroupWith(FormLog.RadioImportacaoComErro);

        }

        private void EnableMenus(Form oForm, bool enable)
        {
            oForm.EnableMenu("1281", enable); //Find Record
            oForm.EnableMenu("1282", enable); //Add New Record
            oForm.EnableMenu("1288", enable); //Next Record
            oForm.EnableMenu("1289", enable); //Previous Record
            oForm.EnableMenu("1290", enable); //Fist Record
            oForm.EnableMenu("1291", enable); //Last Record
        }


    }
}
