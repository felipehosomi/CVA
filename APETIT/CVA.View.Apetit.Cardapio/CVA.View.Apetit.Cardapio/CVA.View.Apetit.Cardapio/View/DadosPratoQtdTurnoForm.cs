using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVA.View.Apetit.Cardapio.View
{
    public class DadosPratoQtdTurnoForm : BaseForm
    {
        //Campos da tabela
        public const string TB_IdLinhaPlan = "CVA_ID_LN_PLAN";
        public const string TB_IdPlan = "CVA_PLAN_ID";
        public const string TB_IdTurno = "CVA_ID_TURNO";
        public const string TB_DesTurno = "CVA_DES_TURNO";
        public const string TB_Qtd = "CVA_QTD";

        public List<QtdTurnoModel> QtdTurnoModels = new List<QtdTurnoModel>();
        Dictionary<string, List<LineItemData>> MatrixItemList;
        string COLUID;
        int Row;

        //1//public DadosPratoQtdTurnoForm(List<QtdTurnoModel> qtdTurnoModel)
        public DadosPratoQtdTurnoForm(Dictionary<string, List<LineItemData>> matrixItemList, string coluid, int row)
        {
            if(matrixItemList != null) QtdTurnoModels = matrixItemList[coluid][row].QtdTurnos;
            COLUID = coluid;
            Row = row;
            MatrixItemList = matrixItemList;
            Type = "CARDITTR";
            TableName = "CVA_TURNO_QTD";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();

            UserTables.CreateIfNotExist(TableName, "[CVA] Dados Prato Qtd. Turno", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
            userFields.CreateIfNotExist("@" + TableName, TB_IdPlan, "ID Plan.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_IdLinhaPlan, "ID Linha Plan.", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_IdTurno, "ID Turno", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_DesTurno, "Descr. Turno", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None);
            userFields.CreateIfNotExist("@" + TableName, TB_Qtd, "Quantidade Turno", 12, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price);
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        internal override void LoadDefault(Form oForm)
        {
            var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;

            Filters.Add(f.TypeEx, BoEventTypes.et_VALIDATE);
            Filters.Add(f.TypeEx, BoEventTypes.et_LOST_FOCUS);
            Filters.Add(f.TypeEx, BoEventTypes.et_FORM_DATA_ADD);
            Filters.Add(f.TypeEx, BoEventTypes.et_FORM_DATA_UPDATE);
            Filters.Add(f.TypeEx, BoEventTypes.et_FORM_DATA_LOAD);
            Filters.Add(f.TypeEx, BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(f.TypeEx, BoEventTypes.et_FORM_CLOSE);
            Filters.Add(f.TypeEx, BoEventTypes.et_FORM_DEACTIVATE);

            try
            {
                var labelTurnoRef = oForm.Items.Item("Item_0");
                var labelQtdRef = oForm.Items.Item("Item_1");

                var top = labelTurnoRef.Top + 25;

                for (int i = 0; i < QtdTurnoModels.Count; i++)
                {
                    top += 20;
                    var item = QtdTurnoModels[i];

                    var staticTurno = oForm.Items.Add("s_" + i.ToString(), BoFormItemTypes.it_STATIC);
                    var edtQtd = oForm.Items.Add("e_" + i.ToString(), BoFormItemTypes.it_EDIT);

                    staticTurno.Top = top;
                    edtQtd.Top = top;
                    staticTurno.Left = labelTurnoRef.Left;
                    edtQtd.Left = labelQtdRef.Left;

                    ((StaticText)staticTurno.Specific).Caption = item.Turno;
                    ((EditText)edtQtd.Specific).Value = item.QTD.ToString();
                    ((EditText)edtQtd.Specific).ValidateAfter += DadosPratoQtdTurnoForm_KeyDownAfter;
                }
                //for (var item in QtdTurnoModel)
                //{
                //    var staticTurno = oForm.Items.Add()
                //}
            }
            catch (Exception ex)
            {
            }
            //oForm.Freeze(true);
            //CreateChooseFromList(f);
            //TODO: Usar QtdTurnoModel para montar a tela
            //oForm.Freeze(false);
        }

        private void DadosPratoQtdTurnoForm_KeyDownAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var oForm = B1Connection.Instance.Application.Forms.ActiveForm;
                if (!oForm.TypeEx.Equals(TYPEEX)) return;

                var edtQtd = (EditText)oForm.Items.Item(pVal.ItemUID).Specific;
                var parsed = 0d;

                var turnoUID = pVal.ItemUID.Replace("e_", "s_");
                var turnoStatic = (StaticText)oForm.Items.Item(turnoUID).Specific;

                //1//var turno = QtdTurnoModels.FirstOrDefault(x => x.Turno.Equals(turnoStatic.Caption));
                foreach (var item in MatrixItemList)
                {
                    var turno = item.Value[Row].QtdTurnos.FirstOrDefault(x => x.Turno.Equals(turnoStatic.Caption));

                    if (double.TryParse(edtQtd.Value, out parsed))
                        turno.QTD = parsed;
                    else
                        edtQtd.Value = turno.QTD.ToString();
                }
            }
            catch
            {
            }
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            //var openMenu = OpenMenu(MenuItem, FilePath, pVal);

            //if (!string.IsNullOrEmpty(openMenu))
            //{
            //    ret = false;
            //    Application.SetStatusBarMessage(openMenu);
            //}

            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            if (pVal.FormTypeEx.Equals(TYPEEX))
            {
                try
                {
                    if (!pVal.BeforeAction)
                    {
                        if(pVal.ItemUID.Equals("btnConfirm") && pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                        {
                            if(DIHelper.HasForm(Application, FormUID))
                            {
                                var oForm = Application.Forms.Item(pVal.FormUID);
                                oForm.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var oForm = Application.Forms.GetForm(pVal.FormUID);
                    if (oForm != null) oForm.Freeze(false);

                    Application.SetStatusBarMessage(ex.Message);
                    ret = false;
                }
            }

            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);

            Filters.Add(Type, BoEventTypes.et_COMBO_SELECT);
            Filters.Add(Type, BoEventTypes.et_CHOOSE_FROM_LIST);
            Filters.Add(Type, BoEventTypes.et_PICKER_CLICKED);
            Filters.Add(Type, BoEventTypes.et_VALIDATE);
            Filters.Add(Type, BoEventTypes.et_LOST_FOCUS);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_ADD);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_UPDATE);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_LOAD);
            Filters.Add(Type, BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(Type, BoEventTypes.et_MATRIX_LINK_PRESSED);
            Filters.Add(Type, BoEventTypes.et_FORM_CLOSE);
            Filters.Add(Type, BoEventTypes.et_FORM_DEACTIVATE);
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (BusinessObjectInfo.FormTypeEx.Equals(TYPEEX))
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD && !BusinessObjectInfo.BeforeAction)
                    {
                        //var oForm = Application.Forms.ActiveForm;
                        //oForm.Freeze(true);

                        //LerLinhasAlterarTotaisComensais(oForm);

                        //oForm.Freeze(false);
                    }
                }
            }
            catch (Exception ex) { }

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            //Helpers.Menus.Add("CVAPDADOSC", MenuItem, "Composição de Quantidade", 6, BoMenuType.mt_STRING);
        }
    }

    public class QtdTurnoModel
    {
        public string Turno { get; set; }
        public double QTD { get; set; }
        public int IdPlanejamento { get; set; }
        public int IdLinhaPlan { get; set; }

        public QtdTurnoModel(string turno, double qtd, int idPlanejamento, int idLinhaPlan)
        {
            Turno = turno;
            QTD = qtd;
            IdPlanejamento = idPlanejamento;
            IdLinhaPlan = idLinhaPlan;
        }
    }
}