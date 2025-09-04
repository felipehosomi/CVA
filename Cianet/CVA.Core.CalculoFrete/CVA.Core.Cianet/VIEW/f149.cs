using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.Core.Cianet.BLL;
using SAPbouiCOM;
using System;

namespace CVA.Core.Cianet.VIEW
{
    public class f149 : BaseForm
    {
        //Cotação de venda
        private Form form;
        private RegraFreteBLL _bll;

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

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_VALIDATE)
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
                                SetUnitPrice(ItemEventInfo.Row);
                                break;
                        }
                    }
                }
            }

            if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_ITEM_PRESSED && ItemEventInfo.ItemUID == "1" && (ItemEventInfo.FormMode == 3 || ItemEventInfo.FormMode == 2))
            {
                form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                var ds = form.DataSources.DBDataSources.Item("OQUT");

                var user = SBOApp.Application.Company.UserName;

                var result = 2;

                if (_bll.Check_UserPermission(user) >= 1)
                {
                    result = SBOApp.Application.MessageBox("O frete será calculado automaticamente. Deseja zerar o valor do mesmo?", 1, "Sim", "Não");
                }

                if (result == 2)
                {
                    var tipoEnvio = _bll.Check_TipoEnvio(ds.GetValue("TrnspCode", 0));

                    if (tipoEnvio == "CIF")
                    {
                        var cardcode = ds.GetValue("CardCode", 0);
                        var matriz = (Matrix)form.Items.Item("38").Specific;
                        var totalFrete = Calcula_Frete(matriz, cardcode);
                        var idDespesa = _bll.Get_IdDespesa();

                        if (totalFrete > 0)
                        {
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
                                oForm.Items.Item("1").Click();
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
                        oForm.Items.Item("1").Click();
                    }
                    catch { }
                }
            }
            return true;
        }

        private void SetUnitPrice(int row)
        {
            try
            {
                form.Freeze(true);
                EventFilterController.DisableEvents();
                DBDataSource dt_QUT1 = form.DataSources.DBDataSources.Item("QUT1");
                Matrix mtxItems = (Matrix)form.Items.Item("38").Specific;
                string priceIPIstr = dt_QUT1.GetValue("U_CVA_Preco_IPI", row - 1);
                //double priceIPI;
                //double.TryParse(priceIPIstr.Replace(".", ","), out priceIPI);

                var priceIPI = Convert.ToDouble(priceIPIstr); 

                if (priceIPI > 0)
                {
                    string taxCode = dt_QUT1.GetValue("TaxCode", row - 1);
                    EditText etxPrice = (EditText)mtxItems.GetCellSpecific("14", row);

                    if (!String.IsNullOrEmpty(taxCode))
                    {
                        QuotationBLL quotationBLL = new QuotationBLL();
                        etxPrice.Value = quotationBLL.CalculateUnitPrice(priceIPI, taxCode).ToString();
                    }
                    else
                    {
                        etxPrice.Value = priceIPI.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                FormEventsBLL.SetEvents();
                form.Freeze(false);
            }
        }

        public override bool FormDataEvent()
        {
            //if (!BusinessObjectInfo.BeforeAction)
            //{
            //    //Calcula valor do frete baseado nas regras
            //    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
            //    {
            //        _bll = new RegraFreteBLL();
            //        //Pega os dados para pesquisas
            //        var numDoc = Convert.ToInt32(((EditText)Form.Items.Item("8").Specific).Value);
            //        var codCli = ((EditText)Form.Items.Item("4").Specific).Value;
            //        var matriz = (Matrix)Form.Items.Item("38").Specific;

            //        //Calcula o o valor total do frete de todos os itens
            //        var totalFrete = Calcula_Frete(matriz, codCli);

            //        //Busca o ID do tipo de despesa 'Frete'
            //        var idDespesa = _bll.Get_IdDespesa();

            //        //Verifica se o documento possuí o tipo de despesa 'Frete'
            //        if (totalFrete > 0)
            //        {
            //            if (_bll.Check_DespesaFrete("C", idDespesa, numDoc))
            //                _bll.Update_ValorFrete("C", totalFrete, idDespesa, numDoc);
            //            else
            //                _bll.Insert_DespesaFrete("C", totalFrete, idDespesa, numDoc);
            //        }
            //    }
            //}

            return true;
        }

        private double Calcula_Frete(Matrix matriz, string cliente)
        {
            var valorFrete = 0.0;
            for (int i = 1; i < matriz.VisualRowCount; i++)
            {
                var coluna1 = ((EditText)matriz.GetCellSpecific("1", i)).Value.ToString();
                var coluna2 = ((EditText)matriz.GetCellSpecific("11", i)).Value.ToString();
                var coluna3 = ((EditText)matriz.GetCellSpecific("21", i)).Value.ToString();

                var produto = coluna1;
                var quantidade = coluna2;
                var valorProduto = coluna3;

                var valorLinha = _bll.CalculaFrete(cliente, produto, quantidade, valorProduto);
                valorFrete = valorFrete + valorLinha;
            }

            return valorFrete;
        }
    }
}
