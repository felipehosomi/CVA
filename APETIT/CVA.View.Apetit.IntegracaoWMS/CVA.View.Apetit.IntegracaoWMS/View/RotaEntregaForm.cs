using CVA.View.Apetit.IntegracaoWMS.Helpers;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.IntegracaoWMS.View
{
    public class RotaEntregaForm : BaseForm
    {
        //Campos da tabela
        private const string __TableDescription = "Rotas de Entrega";

        public const string TB_Descricao = "CVA_DESCRICAO";
        public const string TB_FilialPrincipal = "CVA_FILIAL_PRINCIPAL";
        public const string TB_FilialPrincipalNome = "CVA_FILIAL_P_NOME";

        public const string CH_FilialDestino = "CVA_FILIAL_DESTINO";
        public const string CH_NomeFilialDestino = "CVA_FILIAL_D_NOME";

        public RotaEntregaForm()
        {//WhsCode
            MatrixItens = "mtxGrps";
            Type = "INTCROTA";
            TableName = "CVA_ROTAENTREGA";
            ChildName = "CVA_LN_ROTAENTREGA";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            IdToEvaluateGridEmpty = "edt_l_FD";

            ConfigureNavigationProperties("edtCode", false, true, false, false, false, false);
        }

        public override void CreateUserFields()
        {
            var userFields = new UserFields();

            UserTables.CreateIfNotExist(TableName, $"[CVA] {__TableDescription}", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            userFields.CreateIfNotExist("@" + TableName, TB_Descricao, "Descrição da Rota", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_FilialPrincipal, "Filial Principal", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + TableName, TB_FilialPrincipalNome, "Nome Filial Principal", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);

            UserTables.CreateIfNotExist(ChildName, $"[CVA] Lin. {__TableDescription}", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
            userFields.CreateIfNotExist("@" + ChildName, CH_FilialDestino, "Filial Destino", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, CH_NomeFilialDestino, "Nome Filial Destino", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES);
            userFields.CreateIfNotExist("@" + ChildName, "Calendar", "Calendário", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO);
            userFields.CreateIfNotExist("@" + ChildName, "TranspDays", "Dias de transporte", 10, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO);

            #region UDOs

            if (!UserObjects.Exists(Type))
            {
                userFields.CreateUserObject(Type, $"[CVA] {__TableDescription}", TableName, SAPbobsCOM.BoUDOObjType.boud_MasterData);
                userFields.AddChildTableToUserObject(Type, ChildName);
            }

            #endregion
        }

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);
            //var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            //CreateChooseFromList(f);
            //oForm.Freeze(false);
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            try
            {
                var openMenu = OpenMenu(MenuItem, FilePath, pVal);

                if (!string.IsNullOrEmpty(openMenu))
                {
                    ret = false;
                    Application.SetStatusBarMessage(openMenu);
                }
            }
            catch (Exception ex)
            {
                Application.MessageBox(ex.Message);
            }

            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.FormTypeEx.Equals(Type))
                {
                    if (!pVal.BeforeAction)
                    {
                        #region Filial Principal CHOOSE

                        if (pVal.ItemUID.Equals("edtFPrinc") && pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                        {
                            var oForm = Application.Forms.Item(pVal.FormUID);

                            var udDflVend = oForm.DataSources.UserDataSources.Item("udDflVend");
                            var iChoose = (ChooseFromListEvent)pVal;
                            DataTable dataTable = iChoose.SelectedObjects;

                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                oForm.DataSources.DBDataSources.Item("@" + TableName).SetValue($"U_{TB_FilialPrincipalNome}", 0, dataTable.GetValue("BPLName", 0).ToString());
                                udDflVend.Value = dataTable.GetValue("DflVendor", 0).ToString();
                            }
                        }



                        //if (pVal.ItemUID.Equals("edtFPrinc") && pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                        //{
                        //    var oForm = Application.Forms.Item(pVal.FormUID);
                        //    string idFilialPrinc = ((EditText)oForm.Items.Item("edtFPrinc").Specific).Value.ToString();
                        //    var udDflVend = oForm.DataSources.UserDataSources.Item("udDflVend");

                        //    if (!string.IsNullOrEmpty(idFilialPrinc))
                        //    {
                        //        var OBLP = ObterFilialJson(idFilialPrinc);
                        //        if (!string.IsNullOrEmpty(OBLP))
                        //        {
                        //            var obj = JsonConvert.DeserializeObject<JObject>(OBLP);
                        //            ((IEditText)oForm.Items.Item("edtFPrincN").Specific).Value = obj["Name"].ToString();

                        //            udDflVend.Value = obj["DefaultVendor"].ToString();
                        //        }
                        //    }
                        //}

                        #endregion

                        #region Filial Linha CHOOSE
                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("edt_l_FD"))
                        {
                            if (pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                            {
                                var oForm = Application.Forms.Item(pVal.FormUID);
                                var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                                var iChoose = (ChooseFromListEvent)pVal;
                                DataTable dataTable = iChoose.SelectedObjects;

                                var dtBranchData = oForm.DataSources.DataTables.Item("dtBranchData");

                                if (dataTable != null && dataTable.Rows.Count > 0)
                                {
                                    dtBranchData.Rows.Add();
                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue($"U_{CH_FilialDestino}", pVal.Row - 1, dataTable.GetValue("BPLId", 0).ToString());
                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue($"U_{CH_NomeFilialDestino}", pVal.Row - 1, dataTable.GetValue("BPLName", 0).ToString());
                                    dtBranchData.SetValue("DflCust", pVal.Row - 1, dataTable.GetValue("DflCust", 0).ToString());
                                }

                                mtx.LoadFromDataSourceEx();
                            }

                            //if (pVal.EventType.Equals(BoEventTypes.et_VALIDATE))
                            //{
                            //    var oForm = Application.Forms.Item(pVal.FormUID);
                            //    var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                            //    var idFilialLinha = ((IEditText)mtx.Columns.Item("edt_l_FD").Cells.Item(pVal.Row).Specific).Value;

                            //    if (!string.IsNullOrEmpty(idFilialLinha))
                            //    {
                            //        var OBPL = ObterFilialJson(idFilialLinha);
                            //        if (!string.IsNullOrEmpty(OBPL))
                            //        {
                            //            var obj = JsonConvert.DeserializeObject<JObject>(OBPL);
                            //            ((IEditText)mtx.Columns.Item("edt_l_FDN").Cells.Item(pVal.Row).Specific).Value = obj["Name"].ToString();
                            //        }
                            //    }
                            //}
                        }

                        if (pVal.ItemUID.Equals(MatrixItens) && pVal.ColUID.Equals("Calendar"))
                        {
                            if (pVal.EventType.Equals(BoEventTypes.et_CHOOSE_FROM_LIST))
                            {
                                var oForm = Application.Forms.Item(pVal.FormUID);
                                var mtx = ((IMatrix)oForm.Items.Item(MatrixItens).Specific);
                                var iChoose = (ChooseFromListEvent)pVal;
                                DataTable dataTable = iChoose.SelectedObjects;

                                if (dataTable != null && dataTable.Rows.Count > 0)
                                {
                                    oForm.DataSources.DBDataSources.Item("@" + ChildName).SetValue("U_Calendar", pVal.Row - 1, dataTable.GetValue("Code", 0).ToString());
                                }

                                mtx.LoadFromDataSourceEx();
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        /// <summary>
        /// Obtem Código e nome Filial
        /// </summary>
        /// <param name="filialId">Id da Filial</param>
        /// <returns>Code, Name ou null</returns>
        private string ObterFilialJson(string filialId)
        {
            SAPbobsCOM.Recordset rec = B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rec.DoQuery($@"
                                        SELECT 
                                         {"BPLId".Aspas()}
                                         ,{"BPLName".Aspas()}
                                         ,{"DflVendor".Aspas()}
                                        FROM {"OBPL".Aspas()}
                                        WHERE {"BPLId".Aspas()} = '{filialId}'");

            if (rec.RecordCount > 0)
            {
                return JsonConvert.SerializeObject(new
                {
                    Code = rec.Fields.Item("BPLId").Value.ToString(),
                    Name = rec.Fields.Item("BPLName").Value.ToString(),
                    DefaultVendor = rec.Fields.Item("DflCust").Value.ToString()
                });
            }

            return null;
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
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("INTEGWMS", MenuItem, "Cadastro de Rotas de Entrega", 0, BoMenuType.mt_STRING);
        }

        //public void CreateChooseFromList(Form oForm)
        //{
        //    var oCFLs = oForm.ChooseFromLists;
        //    var oCFLCreationParams = B1Connection.Instance.Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);
        //    oCFLCreationParams.MultiSelection = false;
        //    oCFLCreationParams.ObjectType = "64";
        //    oCFLCreationParams.UniqueID = "OWHS";

        //    var oCFL = oCFLs.Add(oCFLCreationParams);
        //    var edt = (IEditText)oForm.Items.Item("edtFPrinc");
        //    edt.ChooseFromListUID = "OWHS";
        //    edt.ChooseFromListAlias = "WhsCode";

        //    var oGrid = ((IGrid)oForm.Items.Item(MatrixItens).Specific);
        //    var edtCol = (EditTextColumn)oGrid.Columns.Item("Filial de Destino");
        //    edtCol.LinkedObjectType = "2";
        //    edtCol.ChooseFromListUID = "OCRD";
        //    edtCol.ChooseFromListAlias = "CardCode";

        //    //oCon.Alias = "SellItem";
        //    //oCon.Operation = BoConditionOperation.co_EQUAL;
        //    //oCon.CondVal = "Y";
        //}
    }
}