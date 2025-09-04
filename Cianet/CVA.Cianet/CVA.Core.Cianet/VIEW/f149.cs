using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.Core.Cianet.BLL;
using SAPbouiCOM;
using System;

namespace CVA.Core.Cianet.VIEW
{
    public class f149 : DefaultForm
    {
        //Cotação de venda
        private static bool X = true;
        private static bool Y = true;

        #region Constructor
        public f149()
        {
            FormCount++;
            this._bll = new RegraFreteBLL();
        }

        public f149(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
            this._bll = new RegraFreteBLL();
        }

        public f149(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
            this._bll = new RegraFreteBLL();
        }

        public f149(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
            this._bll = new RegraFreteBLL();
        }
        #endregion

        //Consulta formatada para automatizar a transportadora

        //NOME: Transportadora Padrão - Cotação de Venda
        //IF($[OQUT.TrnspCode] = '2')
        //      SELECT CARDCODE FROM OCRD WHERE CARDCODE = 'F000003'
        //ELSE
        //      SELECT ''

        #region ItemEvent

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_VALIDATE || ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == "38")
                    {
                        form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                        switch (ItemEventInfo.ColUID)
                        {
                            case "U_CVA_Preco_IPI":
                            case "1": // Cód. Item
                            case "3": // Descrição. Item
                            case "14": // Preço
                            case "160": // Imposto
                            case "2011": // Utilização
                                SetUnitPrice(ItemEventInfo.Row);
                                break;
                        }
                    }
                }
            }

            if (ItemEventInfo.BeforeAction && (ItemEventInfo.EventType == BoEventTypes.et_CLICK) && ItemEventInfo.ItemUID == "1" && (ItemEventInfo.FormMode == 3 || ItemEventInfo.FormMode == 2))
            {
                
                if (X)
                {
                    form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    var ds = form.DataSources.DBDataSources.Item("OQUT");
                    var ds12 = form.DataSources.DBDataSources.Item("QUT12");

                    var user = SBOApp.Application.Company.UserName;

                    var result = 2;

                    var tipoEnvio = _bll.Check_TipoEnvio(ds.GetValue("TrnspCode", 0));
                    if (tipoEnvio == "CIF")
                    {
                        if (_bll.Check_UserPermission(user) >= 1)
                        {
                            result = SBOApp.Application.MessageBox("O frete será calculado automaticamente. Deseja zerar o valor do mesmo?", 1, "Zerar Frete", "Calcular Frete", "Mater Valor");
                        }

                        Form.Items.Item("16").Click(); // Para Atualizar o Valor MC no Matriz.
                        if (result == 2)
                        {
                            var cardcode = ds.GetValue("CardCode", 0);
                            var matriz = (Matrix)form.Items.Item("38").Specific;
                            var transportadora = "";
                            var totalFrete = Calcula_Frete(matriz, cardcode, out transportadora);
                            var idDespesa = _bll.Get_IdDespesa();

                            if (totalFrete > 0)
                            {

                                try
                                {
                                    form.Items.Item("2013").Click();
                                    if (!string.IsNullOrEmpty(transportadora)) ((EditText)form.Items.Item("2022").Specific).Value = transportadora;
                                }
                                catch
                                {
                                    ds12.SetValue("Carrier", 0, transportadora);
                                }

                                form.Items.Item("91").Click(BoCellClickType.ct_Linked);
                                var oForm = SBOApp.Application.Forms.ActiveForm;
                                var oMatrix = (Matrix)oForm.Items.Item("3").Specific;
                                for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                                {
                                    var expnsCode = (EditText)oMatrix.GetCellSpecific("1", i);

                                    if (expnsCode.Value.ToString() == idDespesa.ToString())
                                    {
                                        try
                                        {
                                            var teste = ((EditText)oMatrix.GetCellSpecific("3", i)).Value;
                                            ((EditText)oMatrix.GetCellSpecific("3", i)).Value = totalFrete.ToString();
                                        }
                                        catch { }

                                        try
                                        {
                                            ((ComboBox)oMatrix.GetCellSpecific("10", i)).Select("T", BoSearchKey.psk_ByValue);
                                        }
                                        catch { }
                                        break;
                                    }
                                }

                                oForm.Items.Item("1").Click();
                                try
                                {
                                    X = false;
                                    oForm.Items.Item("1").Click();
                                    form.Items.Item("1").Click();
                                    X = true;
                                }
                                catch { }
                            }
                        }

                    }
                    if (result == 1)
                    {
                        var totalFrete = 0.0;
                        var idDespesa = _bll.Get_IdDespesa();

                        form.Items.Item("91").Click(BoCellClickType.ct_Linked);
                        var oForm = SBOApp.Application.Forms.ActiveForm;
                        var oMatrix = (Matrix)oForm.Items.Item("3").Specific;
                        for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                        {
                            var expnsCode = (EditText)oMatrix.GetCellSpecific("1", i);

                            if (expnsCode.Value.ToString() == idDespesa.ToString())
                            {
                                try
                                {
                                    ((EditText)oMatrix.GetCellSpecific("3", i)).Value = totalFrete.ToString();
                                }
                                catch { }

                                try
                                {
                                    ((ComboBox)oMatrix.GetCellSpecific("10", i)).Select("T", BoSearchKey.psk_ByValue);
                                }
                                catch { }
                                break;
                            }
                        }
                        oForm.Items.Item("1").Click();
                        try
                        {
                            X = false;
                            oForm.Items.Item("1").Click();
                            form.Items.Item("1").Click();
                            X = true;
                        }
                        catch { }
                    }
                    if (result == 3)
                    {
                        X = false;
                        form.Items.Item("1").Click();
                        X = true;
                    }

                }
            }

            return true;
        }
        #endregion

        #region Preço Unitário

        private void SetUnitPrice(int row)
        {
            if (Y)
            {
                Y = false;
                try
                {
                    this.form.Freeze(true);
                    DBDataSource source = this.form.DataSources.DBDataSources.Item("QUT1");
                    Matrix matrixx = (Matrix)this.form.Items.Item("38").Specific;
                    string str = source.GetValue("U_CVA_Preco_IPI", row - 1);
                    double priceIPI = Convert.ToDouble(str.Substring(0, str.Length - 2).Replace(".", ","));
                    if (priceIPI > 0.0)
                    {
                        string str3 = source.GetValue("TaxCode", row - 1);
                        EditText cellSpecific = (EditText)matrixx.GetCellSpecific("14", row);
                        if (!string.IsNullOrEmpty(str3))
                        {
                            QuotationBLL _bll = new QuotationBLL();
                            cellSpecific.Value = (_bll.CalculateUnitPrice(priceIPI, str3));
                        }
                        else
                        {
                            cellSpecific.Value = (priceIPI.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Y = true;
                    this.form.Freeze(false);
                }
            }
        }

        #endregion
        

    }
}
