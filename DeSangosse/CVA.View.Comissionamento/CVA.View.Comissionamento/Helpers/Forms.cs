using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using System.Xml;

namespace CVA.View.Comissionamento.Helpers
{
    public class Forms
    {
        public static Form LoadForm(string formPath)
        {
            var oXmlDoc = new XmlDocument();
            var oCreationPackage = (FormCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            oCreationPackage.UniqueID = $"{oCreationPackage.UniqueID}{Guid.NewGuid().ToString().Substring(2, 10)}";

            oXmlDoc.Load(formPath);
            oCreationPackage.XmlData = oXmlDoc.InnerXml;
            return B1Connection.Instance.Application.Forms.AddEx(oCreationPackage);
        }

        public static void LoadPricForm(Form oForm)
        {
            oForm.Freeze(true);

            var oButtonUp = (Button)oForm.Items.Item(View.PriorizacaoCriteriosForm.ButtonUp).Specific;
            oButtonUp.Image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\ButtonUp.bmp";
            
            var oButtonDown = (Button)oForm.Items.Item(View.PriorizacaoCriteriosForm.ButtonDown).Specific;
            oButtonDown.Image = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\ButtonDown.bmp";

            var oRecordset = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("SELECT Code, Name, CASE U_ATIVO WHEN 'Y' THEN 0 WHEN 'N' THEN 1 END AS U_ATIVO, ISNULL(U_POS, CAST(Code AS INT)) AS U_POS FROM [@CVA_CRIT_COMISSAO] ORDER BY 4");
            var recordCount = oRecordset.RecordCount;

            var oMatrix = (Matrix)oForm.Items.Item(View.PriorizacaoCriteriosForm.MatrixItens).Specific;
            oMatrix.Columns.Item(View.PriorizacaoCriteriosForm.ColumnCode).Visible = false;
            oMatrix.Columns.Item(View.PriorizacaoCriteriosForm.ColumnPos).Visible = false;
            oMatrix.Columns.Item(View.PriorizacaoCriteriosForm.ColumnCheckAtivo).ValOn = "Y";
            oMatrix.Columns.Item(View.PriorizacaoCriteriosForm.ColumnCheckAtivo).ValOff = "N";


            oMatrix.Clear();

            var ud0 = oForm.DataSources.UserDataSources.Item(View.PriorizacaoCriteriosForm.UserDataSourceUD_0);
            var ud1 = oForm.DataSources.UserDataSources.Item(View.PriorizacaoCriteriosForm.UserDataSourceUD_1);
            var ud2 = oForm.DataSources.UserDataSources.Item(View.PriorizacaoCriteriosForm.UserDataSourceUD_2);
            var ud3 = oForm.DataSources.UserDataSources.Item(View.PriorizacaoCriteriosForm.UserDataSourceUD_3);
            var ud4 = oForm.DataSources.UserDataSources.Item(View.PriorizacaoCriteriosForm.UserDataSourceUD_4);

            ud0.Value = null;
            ud1.Value = null;
            ud2.Value = null;
            ud3.Value = null;
            ud4.Value = null;

            oMatrix.Clear();
            oMatrix.LoadFromDataSourceEx(true);

            for(int i = 0; i < recordCount; i++)
            {
                oMatrix.AddRow();
                oMatrix.ClearRowData(i);
            }

            int j = 1;

            while (!oRecordset.EoF)
            {
                oMatrix.ClearRowData(j);

                ud0.ValueEx = oRecordset.Fields.Item("U_POS").Value.ToString();
                ud1.ValueEx = oRecordset.Fields.Item("Name").Value.ToString();                

                var ativo = oRecordset.Fields.Item("U_ATIVO").Value.ToString();

                ud2.ValueEx = ativo.Equals("0") ? "Y" : "N";

                ud3.ValueEx = oRecordset.Fields.Item("Code").Value.ToString();
                ud4.ValueEx = oRecordset.Fields.Item("U_POS").Value.ToString();
                
                oMatrix.SetLineData(j);
                j++;
                oRecordset.MoveNext();
            }

            oMatrix.CommonSetting.SetRowEditable(1, false);

            for (int i = 1; i <= oMatrix.RowCount; i++)
            {
                var code = (EditText)oMatrix.GetCellSpecific("Col_2", i);
                var pos = (EditText)oMatrix.GetCellSpecific("#", i);

                var sCmp = B1Connection.Instance.Company.GetCompanyService();
                var oGeneralService = sCmp.GetGeneralService("UDOCRIT");
                var oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", code.Value.ToString());

                var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("U_POS", pos.Value.ToString());
                oGeneralService.Update(oGeneralData);
            }            

            oForm.Freeze(false);
        }

        public static void LoadPgtoForm(Form oForm)
        {
            oForm.Freeze(true);

            Helpers.UserQueries.AssignFormattedSearch("PGTOVENDEDOR", Properties.Resources.REGRVENDEDOR, View.PagarComissaoForm.Type, View.PagarComissaoForm.EditVendedor);

            oForm.Freeze(false);
        }

        public static void LoadCalcForm(Form oForm)
        {
            oForm.Freeze(true);

            var oItem = oForm.Items.Item(View.CalculoComissaoForm.OptionPagas);
            var oOption = (OptionBtn)oItem.Specific;
            oOption.GroupWith(View.CalculoComissaoForm.OptionTodas);

            oItem = oForm.Items.Item(View.CalculoComissaoForm.OptionNaoPagas);
            oOption = (OptionBtn)oItem.Specific;
            oOption.GroupWith(View.CalculoComissaoForm.OptionPagas);

            oForm.Freeze(false);
        }

        public static void LoadDelForm(Form oForm)
        {
            oForm.Freeze(true);            

            oForm.Freeze(false);
        }

        public static void LoadRegrForm(Form oForm)
        {
            oForm.Freeze(true);

            Helpers.UserQueries.AssignFormattedSearch("REGRCOMISSIONADO", Properties.Resources.REGRCOMISSIONADO, View.RegraComissaoForm.Type, View.RegraComissaoForm.EditComissionado);
            Helpers.UserQueries.AssignFormattedSearch("REGRVENDEDOR", Properties.Resources.REGRVENDEDOR, View.RegraComissaoForm.Type, View.RegraComissaoForm.EditVendedor);
            Helpers.UserQueries.AssignFormattedSearch("REGRGRUPOITEM", Properties.Resources.REGRGRUPOITEM, View.RegraComissaoForm.Type, View.RegraComissaoForm.EditGrupoItem);
            Helpers.UserQueries.AssignFormattedSearch("REGRCIDADE", Properties.Resources.REGRCIDADE, View.RegraComissaoForm.Type, View.RegraComissaoForm.EditCidade);

            var oCombo = (ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboTipoComissao).Specific;

            if (oCombo.ValidValues.Count.Equals(0))
            {
                var oRecordset = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordset.DoQuery("SELECT Code, Name FROM [@CVA_TIPO_COMISSAO]");
                while (!oRecordset.EoF)
                {
                    oCombo.ValidValues.Add(oRecordset.Fields.Item("Code").Value.ToString(), oRecordset.Fields.Item("Name").Value.ToString());
                    oRecordset.MoveNext();
                }                
            }

            oCombo = (ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboEstado).Specific;

            if (oCombo.ValidValues.Count.Equals(0))
            {
                var oRecordset = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordset.DoQuery("SELECT DISTINCT Code, Name FROM OCST");
                while (!oRecordset.EoF)
                {
                    oCombo.ValidValues.Add(oRecordset.Fields.Item("Code").Value.ToString(), oRecordset.Fields.Item("Name").Value.ToString());
                    oRecordset.MoveNext();
                }
            }

            oCombo = (ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboFabricante).Specific;

            if (oCombo.ValidValues.Count.Equals(0))
            {
                var oRecordset = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordset.DoQuery("SELECT DISTINCT FirmCode, FirmName FROM OMRC");
                while (!oRecordset.EoF)
                {
                    oCombo.ValidValues.Add(oRecordset.Fields.Item("FirmCode").Value.ToString(), oRecordset.Fields.Item("FirmName").Value.ToString());
                    oRecordset.MoveNext();
                }
            }

            oCombo = (ComboBox)oForm.Items.Item(View.RegraComissaoForm.ComboSetor).Specific;

            if (oCombo.ValidValues.Count.Equals(0))
            {
                var oRecordset = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordset.DoQuery("SELECT DISTINCT IndCode, IndName FROM OOND");
                while (!oRecordset.EoF)
                {
                    oCombo.ValidValues.Add(oRecordset.Fields.Item("IndCode").Value.ToString(), oRecordset.Fields.Item("IndName").Value.ToString());
                    oRecordset.MoveNext();
                }
            }

            var oCFLs = oForm.ChooseFromLists;
            var oCFL = oCFLs.Item(View.RegraComissaoForm.ChooseFromListOCRD);
            var oCons = oCFL.GetConditions();

            var oCon = oCons.Add();
            oCon.Alias = "CardType";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "C";
            oCFL.SetConditions(oCons);

            var oCheck = (CheckBox)oForm.Items.Item(View.RegraComissaoForm.CheckAtivo).Specific;
            oCheck.ValOff = "N";
            oCheck.ValOn = "Y";

            oForm.Freeze(false);
        }

        public static Form LoadParamForm()
        {
            #region Form
            var oCreationParams = (FormCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
            oCreationParams.BorderStyle = BoFormBorderStyle.fbs_Fixed;
            oCreationParams.UniqueID = $"{Guid.NewGuid().ToString().Substring(2, 10)}";
            oCreationParams.FormType = "PARAM";

            var oForm = B1Connection.Instance.Application.Forms.AddEx(oCreationParams);

            oForm.Title = "Monitoramento de Comissões - critérios de seleção";
            oForm.Left = 446;
            oForm.Top = 63;
            oForm.ClientHeight = 202;
            oForm.ClientWidth = 478;
            #endregion

            #region Datasources
            oForm.DataSources.UserDataSources.Add("chk1", BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("chk2", BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("chk3", BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("opt1", BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("opt2", BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("opt3", BoDataType.dt_SHORT_TEXT, 1);            
            #endregion

            #region Buttons
            var oItem = oForm.Items.Add("1", BoFormItemTypes.it_BUTTON);
            oItem.Left = 6;
            oItem.Width = 65;
            oItem.Top = 126;
            oItem.Height = 20;            

            var oButton = (Button)oItem.Specific;
            oButton.Caption = "Ok";            

            oItem = oForm.Items.Add("2", BoFormItemTypes.it_BUTTON);
            oItem.Left = 76;
            oItem.Width = 65;
            oItem.Top = 126;
            oItem.Height = 20;

            oButton = (Button)oItem.Specific;
            oButton.Caption = "Cancelar";
            #endregion

            #region Labels
            oItem = oForm.Items.Add("Item_2", BoFormItemTypes.it_STATIC);
            oItem.Left = 6;
            oItem.Top = 6;
            oItem.Height = 14;
            oItem.Width = 80;

            var oStatic = (StaticText)oItem.Specific;
            oStatic.Caption = "Comissionado";

            oItem = oForm.Items.Add("Item_3", BoFormItemTypes.it_STATIC);
            oItem.Left = 6;
            oItem.Top = 36;
            oItem.Height = 14;
            oItem.Width = 80;

            oStatic = (StaticText)oItem.Specific;
            oStatic.Caption = "Vendedor";

            oItem = oForm.Items.Add("Item_4", BoFormItemTypes.it_STATIC);
            oItem.Left = 6;
            oItem.Top = 66;
            oItem.Height = 14;
            oItem.Width = 80;

            oStatic = (StaticText)oItem.Specific;
            oStatic.Caption = "Cliente";

            oItem = oForm.Items.Add("Item_5", BoFormItemTypes.it_STATIC);
            oItem.Left = 6;
            oItem.Top = 96;
            oItem.Height = 14;
            oItem.Width = 80;

            oStatic = (StaticText)oItem.Specific;
            oStatic.Caption = "Comissões";
            #endregion
                        
            #region CheckBoxes
            oItem = oForm.Items.Add("Item_6", BoFormItemTypes.it_CHECK_BOX);
            oItem.Left = 91;
            oItem.Top = 6;
            oItem.Height = 14;
            oItem.Width = 16;

            var oCheck = (CheckBox)oItem.Specific;
            oCheck.DataBind.SetBound(true, "", "chk1");
            oCheck.Caption = string.Empty;
            oCheck.Checked = false;
            oCheck.ValOff = "N";
            oCheck.ValOn = "Y";

            oItem = oForm.Items.Add("Item_7", BoFormItemTypes.it_CHECK_BOX);
            oItem.Left = 91;
            oItem.Top = 36;
            oItem.Height = 14;
            oItem.Width = 16;

            oCheck = (CheckBox)oItem.Specific;
            oCheck.DataBind.SetBound(true, "", "chk2");
            oCheck.Caption = string.Empty;
            oCheck.Checked = false;
            oCheck.ValOff = "N";
            oCheck.ValOn = "Y";

            oItem = oForm.Items.Add("Item_8", BoFormItemTypes.it_CHECK_BOX);
            oItem.Left = 91;
            oItem.Top = 66;
            oItem.Height = 14;
            oItem.Width = 16;

            oCheck = (CheckBox)oItem.Specific;
            oCheck.DataBind.SetBound(true, "", "chk3");
            oCheck.Caption = string.Empty;
            oCheck.Checked = false;
            oCheck.ValOff = "N";
            oCheck.ValOn = "Y";
            #endregion

            #region RadioButtons
            oItem = oForm.Items.Add("Item_9", BoFormItemTypes.it_OPTION_BUTTON);
            oItem.Left = 91;
            oItem.Top = 96;
            oItem.Height = 14;
            oItem.Width = 53;

            var radio = (OptionBtn)oItem.Specific;
            radio.Caption = "Pagas";
            radio.DataBind.SetBound(true, "", "opt1");
            radio.ValOff = "N";
            radio.ValOn = "Y";

            oItem = oForm.Items.Add("Item_10", BoFormItemTypes.it_OPTION_BUTTON);
            oItem.Left = 147;
            oItem.Top = 96;
            oItem.Height = 14;
            oItem.Width = 53;
            oItem.LinkTo = "Item_9";

            radio = (OptionBtn)oItem.Specific;
            radio.Caption = "Não pagas";
            radio.DataBind.SetBound(true, "", "opt2");
            radio.ValOff = "N";
            radio.ValOn = "Y";

            oItem = oForm.Items.Add("Item_11", BoFormItemTypes.it_OPTION_BUTTON);
            oItem.Left = 238;
            oItem.Top = 96;
            oItem.Height = 14;
            oItem.Width = 53;
            oItem.LinkTo = "Item_10";

            radio = (OptionBtn)oItem.Specific;
            radio.Caption = "Todas";
            radio.DataBind.SetBound(true, "", "opt3");
            radio.ValOff = "N";
            radio.ValOn = "Y";
            #endregion

            #region ChooseFromLists
            var oCFLs = oForm.ChooseFromLists;
            var oCFLCreationParams = (ChooseFromListCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);
            oCFLCreationParams.MultiSelection = true;            
            oCFLCreationParams.ObjectType = "53";
            oCFLCreationParams.UniqueID = "CFL1";

            var oCFL = oCFLs.Add(oCFLCreationParams);

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = (ChooseFromListCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);
            oCFLCreationParams.MultiSelection = true;
            oCFLCreationParams.ObjectType = "53";
            oCFLCreationParams.UniqueID = "CFL2";

            oCFL = oCFLs.Add(oCFLCreationParams);

            oCFLs = oForm.ChooseFromLists;
             oCFLCreationParams = (ChooseFromListCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);
            oCFLCreationParams.MultiSelection = true;
            oCFLCreationParams.ObjectType = "2";
            oCFLCreationParams.UniqueID = "CFL3";

            oCFL = oCFLs.Add(oCFLCreationParams);
            var oCons = oCFL.GetConditions();
            var oCon = oCons.Add();
            oCon.Alias = "CardType";
            oCon.Operation = BoConditionOperation.co_EQUAL;
            oCon.CondVal = "C";
            oCFL.SetConditions(oCons);
            #endregion

            #region Option Buttons
            oItem = oForm.Items.Add("Item_12", BoFormItemTypes.it_BUTTON);
            oItem.Left = 113;
            oItem.Top = 6;
            oItem.Height = 14;
            oItem.Width = 23;

            oButton = (Button)oItem.Specific;
            oButton.Caption = "...";
            oButton.ChooseFromListUID = "CFL1";

            oItem = oForm.Items.Add("Item_13", BoFormItemTypes.it_BUTTON);
            oItem.Left = 113;
            oItem.Top = 36;
            oItem.Height = 14;
            oItem.Width = 23;

            oButton = (Button)oItem.Specific;
            oButton.Caption = "...";
            oButton.ChooseFromListUID = "CFL2";

            oItem = oForm.Items.Add("Item_14", BoFormItemTypes.it_BUTTON);
            oItem.Left = 113;
            oItem.Top = 66;
            oItem.Height = 14;
            oItem.Width = 23;

            oButton = (Button)oItem.Specific;
            oButton.Caption = "...";
            oButton.ChooseFromListUID = "CFL3";
            #endregion

            return oForm;
        }
    }    
}
