using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Boleto.BLL;
using CVA.View.Boleto.MODEL;
using SAPbouiCOM;
using System;
using System.Text.RegularExpressions;

namespace CVA.View.Boleto.VIEW
{
    /// <summary>
    /// Contas a pagar - Meio de pagamento
    /// </summary>
    public class f196 : BaseForm
    {
        private Form Form;
        private static FormaPagamentoModel FormaPagamentoModel;
        public static string Boleto { get; set; }

        #region Constructor
        public f196()
        {
            FormCount++;
        }

        public f196(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f196(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f196(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            }

            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_ITEM_PRESSED && ItemEventInfo.ItemUID == "1")
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    if (Form.PaneLevel == 5)
                    {
                        ((EditText)Form.Items.Item("118").Specific).Value = BoletoBLL.GetNextCode().ToString();
                        this.ValidaLinhaDigitavel();
                        bool isOk = this.ValidaValores();
                        return isOk;
                    }
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN && ItemEventInfo.ItemUID == "2009")
                {
                    string codigoBanco = ((ComboBox)Form.Items.Item("148").Specific).Value;
                    if (String.IsNullOrEmpty(codigoBanco.Trim()))
                    {
                        SBOApp.Application.SetStatusBarMessage("Necessário informar o banco antes de preencher o código de barras");
                        return false;
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    this.ValidaLinhaDigitavel();
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT && ItemEventInfo.ItemUID == "148")
                {
                    this.ValidaLinhaDigitavel();
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS && ItemEventInfo.ItemUID == "2009")
                {
                    this.ValidaLinhaDigitavel();
                }
            }

            return true;
        }

        private void ValidaLinhaDigitavel()
        {
            if (Form.PaneLevel == 5)
            {
                string linhaDigitavel = ((EditText)Form.Items.Item("2009").Specific).Value.Trim();
                if (linhaDigitavel.Length != 44 && linhaDigitavel.Length != 47)
                {
                    return;
                }
                string codigoBanco = ((ComboBox)Form.Items.Item("148").Specific).Value;
                if (String.IsNullOrEmpty(codigoBanco.Trim()))
                {
                    SBOApp.Application.SetStatusBarMessage("Necessário informar o banco antes de preencher o código de barras");
                    ((EditText)Form.Items.Item("2009").Specific).Value = String.Empty;
                    return;
                }

                FormaPagamentoModel = FormaPagamentoBLL.GetCode(linhaDigitavel, codigoBanco);
                if (FormaPagamentoModel == null || FormaPagamentoModel.Code == null)
                {
                    return;
                }

                if (FormaPagamentoModel.ValidaLinha == "Y")
                {
                    Form.Freeze(true);
                    EventFilterBLL.DisableEvents();
                    try
                    {
                        ((ComboBox)Form.Items.Item("119").Specific).Select(FormaPagamentoModel.Code, BoSearchKey.psk_ByValue);
                        DateTime dataBase = new DateTime(1997, 10, 4);
                        int qtdeDias;
                        if (linhaDigitavel.Length == 47)
                        {
                            qtdeDias = Convert.ToInt32(linhaDigitavel.Substring(33, 4));
                        }
                        else
                        {
                            qtdeDias = Convert.ToInt32(linhaDigitavel.Substring(30, 4));
                        }
                        double valorTotal;
                        if (linhaDigitavel.Length == 47)
                        {
                            valorTotal = Convert.ToDouble(linhaDigitavel.Substring(37, 8) + "," + linhaDigitavel.Substring(45, 2));
                        }
                        else
                        {
                            valorTotal = Convert.ToDouble(linhaDigitavel.Substring(34, 8) + "," + linhaDigitavel.Substring(42, 2));
                        }

                        DateTime dataBoleto = dataBase.AddDays(qtdeDias);
                        //((EditText)Form.Items.Item("126").Specific).Value = dataBoleto.ToString("yyyyMMdd");
                        ((EditText)Form.Items.Item("126").Specific).Value = DateTime.Today.ToString("yyyyMMdd");
                        ((EditText)Form.Items.Item("123").Specific).Value = valorTotal.ToString();
                    }
                    catch (Exception ex)
                    {
                        //SBOApp.Application.SetStatusBarMessage("Erro geral ao validar linha: " + ex.Message);
                    }
                    finally
                    {
                        EventFilterBLL.SetDefaultEvents();
                        Form.Freeze(false);
                    }
                }
            }
        }

        private bool ValidaValores()
        {
            if (FormaPagamentoModel.ValidaLinha == "Y")
            {
                Form.Freeze(true);
                try
                {
                    string linhaDigitavel = ((EditText)Form.Items.Item("2009").Specific).Value;
                    if (linhaDigitavel.Length != 44 && linhaDigitavel.Length != 47)
                    {
                        SBOApp.Application.MessageBox("Tamanho do código de barras inválido");
                    }

                    int count = 0;
                    for (int i = 0; i < SBOApp.Application.Forms.Count; i++)
                    {
                        Form fr_ContasAPagar = SBOApp.Application.Forms.Item(i);
                        if (fr_ContasAPagar.Type == 426)
                        {
                            count++;
                        }
                        if (count > 1)
                        {
                            SBOApp.Application.MessageBox("Existe mais de uma tela de Contas a Pagar em aberto. Apenas uma tela deve estar em aberto para a validação do código de barras");
                        }
                    }
                    string valor = ((EditText)Form.Items.Item("22").Specific).Value;

                    double valorBoleto = double.Parse(Regex.Replace(valor, "[^0-9.]", ""));
                    DateTime dataBoleto = DateTime.ParseExact(((EditText)Form.Items.Item("126").Specific).Value, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                    for (int i = 0; i < SBOApp.Application.Forms.Count; i++)
                    {
                        Form fr_ContasAPagar = SBOApp.Application.Forms.Item(i);
                        if (fr_ContasAPagar.Type == 426)
                        {
                            valor = ((EditText)fr_ContasAPagar.Items.Item("12").Specific).Value;
                            if (valor == "")
                            {
                                valor = "0";
                            }

                            double valorTotal = double.Parse(Regex.Replace(valor, "[^0-9.]", ""));

                            if (valorTotal != valorBoleto)
                            {
                                SBOApp.Application.MessageBox("Valor do boleto não confere com o total dos documentos");
                            }

                            Matrix mt_Docs = (Matrix)fr_ContasAPagar.Items.Item("20").Specific;
                            for (int j = 0; j < mt_Docs.RowCount; j++)
                            {
                                if (((CheckBox)mt_Docs.GetCellSpecific("10000127", j)).Checked)
                                {
                                    DateTime dataVencimento = DateTime.ParseExact(((EditText)mt_Docs.GetCellSpecific("21", j)).Value, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                    //if (dataVencimento != dataBoleto)
                                    //{
                                    //    SBOApp.Application.MessageBox("Data de vencimento do boleto não confere com a data do documento");
                                    //}
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SBOApp.Application.MessageBox("Erro geral ao validar valores: " + ex.Message);
                }
                finally
                {
                    Form.Freeze(false);
                }
            }
            return true;
        }
    }
}
