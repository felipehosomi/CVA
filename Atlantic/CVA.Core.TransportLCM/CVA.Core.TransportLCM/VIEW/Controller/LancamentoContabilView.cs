using CVA.Core.TransportLCM.BLL;
using CVA.Core.TransportLCM.BLL.BaseConciliadora;
using CVA.Core.TransportLCM.HELPER;
using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;

namespace CVA.Core.TransportLCM.VIEW.Controller
{
    [Form(B1Forms.LancamentoContabil, "CVA.Core.TransportLCM.VIEW.Form.LancamentoContabilFormPartial.srf")]
    public class LancamentoContabilView : DoverSystemFormBase
    {
        private SAPbouiCOM.Application _application { get; set; }
        private bool ResetStatus { get; set; } = false;

        Button bt_Rep { get; set; }
        StaticText st_RepEmp { get; set; }
        StaticText st_RepId { get; set; }

        ComboBox cb_RepEmp { get; set; }
        EditText et_RepId { get; set; }
        static Item it_DocDate { get; set; }
        static ComboBox cb_CVAStu { get; set; }

        DBDataSource dt_LCM { get; set; }
        DBDataSource dt_LCM1 { get; set; }
        LancamentoContabilBLL _lancamentoContabilBLL { get; set; }
        BaseBLL _baseBLL { get; set; }

        private static List<BaseDePara> _filialList { get; set; }

        /* Eduardo Gonçalves */
        Matrix mt_Linhas { get; set; }

        public LancamentoContabilView(SAPbouiCOM.Application application, LancamentoContabilBLL lancamentoContabilBLL, BaseBLL baseBLL)
        {
            _application = application;
            _lancamentoContabilBLL = lancamentoContabilBLL;
            _baseBLL = baseBLL;
        }

        public override void OnInitializeComponent()
        {
            it_DocDate = this.GetItem("6");
            bt_Rep = this.GetItem("bt_Rep").Specific as Button;
            cb_RepEmp = this.GetItem("cb_RepEmp").Specific as ComboBox;
            et_RepId = this.GetItem("et_RepId").Specific as EditText;
            cb_CVAStu = this.GetItem("cb_CVAStu").Specific as ComboBox;

            st_RepEmp = this.GetItem("st_RepEmp").Specific as StaticText;
            st_RepId = this.GetItem("st_RepId").Specific as StaticText;

            cb_RepEmp.DataBind.SetBound(true, "OJDT", "U_CVA_EmpDestino");
            et_RepId.DataBind.SetBound(true, "OJDT", "U_CVA_IdDestino");
            cb_CVAStu.DataBind.SetBound(true, "OJDT", "U_CVA_STATUS");

            dt_LCM = UIAPIRawForm.DataSources.DBDataSources.Item("OJDT");
            dt_LCM1 = UIAPIRawForm.DataSources.DBDataSources.Item("JDT1");

            Matrix mtx;
            try
            {
                _filialList = _baseBLL.GetFilialList();
                foreach (var item in _filialList)
                {
                    try
                    {
                        cb_RepEmp.ValidValues.Add(item.CnpjFilialDe, item.Nome);
                    }
                    catch { }
                }
            }
            catch (Exception e)
            {
                _application.SetStatusBarMessage("CVA - Erro ao buscar lista de filiais, por favor verifique a tabela [@CVA_CONFIG_DB]");
                bt_Rep.Item.Enabled = false;
            }

            /* Eduardo Gonçalves */
            mt_Linhas = this.GetItem("76").Specific as Matrix;
            AddChooseFromLists();
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListUID = "CFL1";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAlias = "AcctCode";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter -= ContaControle_ChooseFromListAfter;
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter += ContaControle_ChooseFromListAfter;
            OnInitializeCustomEvents();
        }

        public void OnInitializeCustomEvents()
        {
            bt_Rep.ClickAfter += Bt_Rep_ClickAfter;
            /* Eduardo Gonçalves */
            AddChooseFromLists();
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListUID = "CFL1";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAlias = "AcctCode";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter -= ContaControle_ChooseFromListAfter;
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter += ContaControle_ChooseFromListAfter;
        }

        /* Eduardo Gonçalves */
        internal virtual void ContaControle_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            var cfl_Evento = (ISBOChooseFromListEventArg)pVal;
            var cfl_Id = cfl_Evento.ChooseFromListUID;
            var form = _application.Forms.Item(pVal.FormUID);
            var cfl = form.ChooseFromLists.Item(cfl_Id);
            var dt_SelectedObjects = cfl_Evento.SelectedObjects;
            string value = null;

            if (dt_SelectedObjects != null)
            {
                value = Convert.ToString(dt_SelectedObjects.GetValue(0, 0));

                try
                {
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Item.Click();
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Value = value;
                }
                catch { }
                finally
                {
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Item.Click();
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Value = value;
                }

                try
                {
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Item.Click();
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Value = value;
                }
                catch { }
                finally
                {
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Item.Click();
                    ((EditText)mt_Linhas.GetCellSpecific("U_CVA_ContaControle", pVal.Row)).Value = value;
                }
            }
        }

        /* Eduardo Gonçalves */
        private void AddChooseFromLists()
        {
            ChooseFromListCollection cfl_Collection;
            ChooseFromListCreationParams cfl_CreationParams;
            ChooseFromList cfl;
            Conditions conditions;
            Condition condition;

            try
            {
                cfl_Collection = UIAPIRawForm.ChooseFromLists;
                cfl_CreationParams = (ChooseFromListCreationParams)_application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);
                cfl_CreationParams.MultiSelection = false;
                cfl_CreationParams.ObjectType = "1";
                cfl_CreationParams.UniqueID = "CFL1";

                cfl = cfl_Collection.Add(cfl_CreationParams);
                conditions = cfl.GetConditions();
                condition = conditions.Add();
                condition.Alias = "LocManTran";
                condition.Operation = BoConditionOperation.co_EQUAL;
                condition.CondVal = "Y";
                cfl.SetConditions(conditions);
            }
            catch { }
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            UIAPIRawForm.Title = "Lançamento contábil manual";

            AddChooseFromLists();
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListUID = "CFL1";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAlias = "AcctCode";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter -= ContaControle_ChooseFromListAfter;
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter += ContaControle_ChooseFromListAfter;
        }

        protected override void OnFormDataLoadAfter(ref BusinessObjectInfo pVal)
        {
            if (!String.IsNullOrEmpty(dt_LCM.GetValue("U_CVA_IdOrigem", dt_LCM.Offset)))
            {
                st_RepEmp.Caption = "Empresa origem";
                st_RepId.Caption = "Nº transação origem";

                cb_RepEmp.DataBind.SetBound(true, "OJDT", "U_CVA_EmpOrigem");
                et_RepId.DataBind.SetBound(true, "OJDT", "U_CVA_IdOrigem");
            }
            else
            {
                st_RepEmp.Caption = "Empresa destino";
                st_RepId.Caption = "Nº transação destino";

                cb_RepEmp.DataBind.SetBound(true, "OJDT", "U_CVA_EmpDestino");
                et_RepId.DataBind.SetBound(true, "OJDT", "U_CVA_IdDestino");
            }

            if (!String.IsNullOrEmpty(cb_RepEmp.Value))
            {
                cb_RepEmp.Item.Enabled = false;
            }

            AddChooseFromLists();
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListUID = "CFL1";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAlias = "AcctCode";
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter -= ContaControle_ChooseFromListAfter;
            mt_Linhas.Columns.Item("U_CVA_ContaControle").ChooseFromListAfter += ContaControle_ChooseFromListAfter;
        }

        internal virtual void Bt_Rep_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            switch (UIAPIRawForm.Mode)
            {
                case BoFormMode.fm_PRINT_MODE:
                case BoFormMode.fm_OK_MODE:
                case BoFormMode.fm_VIEW_MODE:
                    if (!String.IsNullOrEmpty(et_RepId.Value))
                    {
                        _application.SetStatusBarMessage("CVA - LCM já replicado!");
                        return;
                    }

                    if (!String.IsNullOrEmpty(cb_RepEmp.Value))
                    {
                        var contaDestino = dt_LCM1.GetValue("U_CVA_ContaDestino", dt_LCM.Offset);
                        var contaControle = dt_LCM1.GetValue("U_CVA_ContaControle", dt_LCM.Offset);
                        if (contaDestino.Contains(".") && contaControle.Contains("."))
                        {
                            _application.SetStatusBarMessage("CVA - Não é possível selecionar uma Conta Destino e uma Conta Controle na mesma linha.");
                            return;
                        }

                        int journalEntry = Convert.ToInt32(dt_LCM.GetValue("TransId", dt_LCM.Offset));
                        string idDestino;
                        string msg = _lancamentoContabilBLL.Replicate(journalEntry, out idDestino);
                        if (!String.IsNullOrEmpty(msg))
                        {
                            _application.SetStatusBarMessage(msg);
                        }
                        else
                        {
                            et_RepId.Value = idDestino;
                            _application.SetStatusBarMessage("CVA - LCM replicado com sucesso!", BoMessageTime.bmt_Medium, false);
                        }
                    }
                    else
                    {
                        _application.SetStatusBarMessage("CVA - Empresa destino deve ser informada!");
                    }
                    break;
                case BoFormMode.fm_UPDATE_MODE:
                case BoFormMode.fm_ADD_MODE:
                case BoFormMode.fm_EDIT_MODE:
                    _application.SetStatusBarMessage("CVA - Salve o registro antes de continuar");
                    break;
                default:
                    break;
            }
        }


        public static void OnCancelOrDuplicate()
        {
            cb_CVAStu.Item.Enabled = true;
            cb_CVAStu.Select("0", BoSearchKey.psk_ByValue);
            it_DocDate.Click();
            cb_CVAStu.Item.Enabled = false;
        }
    }
}
