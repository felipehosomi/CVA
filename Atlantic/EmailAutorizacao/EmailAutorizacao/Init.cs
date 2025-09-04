using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailAutorizacao.BLL;
using EmailAutorizacao.VIEW.Controller;
using EmailAutorizacao.HELPER;
using SAPbouiCOM;

namespace EmailAutorizacao
{
    //[AddIn(Name = "CVA.View.EmailAutorizacao", Description = "CVA – Envio de e-mail para autorização de documentos", Namespace = "CVA Consultoria", InitMethod = "Initialize")]
    public class Init
    {
        private Application _application { get; }
        private readonly B1Connection _conn;
        private EventFilters _filters;

        public Init()
        {
            _conn = B1Connection.Instance;
            _application = _conn.Application;
            _filters = new EventFilters();
            Initialize();
            AddFilters();
            _application.AppEvent += AppEvents;
            _application.FormDataEvent += FormDataEvents;
            _application.ItemEvent += ItemEvents;
            _application.StatusBar.SetText("Add-on EmailAutorizacao liberado para uso.", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
        }
        
        private void Initialize()
        {
            try
            {
                StaticKeys.ConfigModel = ConfigBLL.GetConfig(3);
                if (StaticKeys.ConfigModel == null || String.IsNullOrEmpty(StaticKeys.ConfigModel.Banco))
                {
                    _application.MessageBox("Configurações não encontradas para base do Portal. Verifique a tabela [@CVA_CONFIG_DB]");
                }
            }
            catch (Exception)
            {
                _application.MessageBox("CVA - Tabela [@CVA_CONFIG_DB] não encontrada!");
                return;
            }
        }

        private void AddFilters()
        {
            UIHelper.AddFilter(ref _filters, "142", BoEventTypes.et_FORM_DATA_ADD);
            UIHelper.AddFilter(ref _filters, "142", BoEventTypes.et_FORM_ACTIVATE);
        }

        private void FormDataEvents(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                var pVal = BusinessObjectInfo;

                if(pVal.FormTypeEx == "142" && pVal.EventType == BoEventTypes.et_FORM_DATA_ADD && !pVal.BeforeAction)
                {
                    var oForm = _application.Forms.ActiveForm;
                    PedidoCompraView.OnFormDataAddAfter(oForm);
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }

        private void AppEvents(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    Environment.Exit(0);
                    break;
            }
        }

        private void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                if(pVal.FormTypeEx == "142" && pVal.EventType == BoEventTypes.et_FORM_ACTIVATE && !pVal.BeforeAction)
                {
                    var oForm = _application.Forms.ActiveForm;
                    PedidoCompraView.OnFormActivateAfter(oForm);
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            BubbleEvent = ret;
        }
    }
}
