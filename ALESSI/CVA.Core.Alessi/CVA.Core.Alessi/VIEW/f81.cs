using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.Alessi.BLL;
using SAPbouiCOM;
using System;

namespace CVA.Core.Alessi.VIEW
{
    public class f81 : BaseForm
    {

        #region Constructor
        public f81()
        {
            FormCount++;
        }

        public f81(ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f81(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f81(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion



        public override bool ItemEvent()
        {
            base.ItemEvent();

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    var item = Form.Items.Add("btCalc", BoFormItemTypes.it_BUTTON);

                    item.Top = 16;
                    item.Left = 322;
                    item.Width = 65;
                    item.Height = 20;

                    Button btn = (Button)Form.Items.Item("btCalc").Specific;
                    btn.Caption = "Calcular";

                    var mtx = (Matrix)Form.Items.Item("10").Specific;

                    mtx.Columns.Item("3").Editable = true;
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "btCalc")
                    {
                        this.calcular();
                    }

                }
            }
            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                {

                }
            }
            return true;
        }

        private void calcular()
        {
            SBOApp.Application.SetStatusBarMessage("Processando Informações Aguarde...", BoMessageTime.bmt_Short, false);
            var oMatrix = (Matrix)Form.Items.Item("10").Specific;
            var btnCalcular = (Button)Form.Items.Item("btCalc").Specific;
            var edTexto = (EditText)Form.Items.Item("10000024").Specific;
            Button btn = (Button)Form.Items.Item("10000021").Specific;
            string docNum = "";
            try
            {
                var bt = VerificaBtnExpandir();
                if (bt)
                {
                    btn.Item.Click();
                }

                Form.Freeze(true);

                for (int i = 1; i <= oMatrix.VisualRowCount; i++)
                {
                    if (!bt)
                    {
                        SBOApp.Application.SetStatusBarMessage("Processando linha: " + i + " de: " + oMatrix.VisualRowCount, BoMessageTime.bmt_Short, false);

                        docNum = ((EditText)oMatrix.GetCellSpecific("11", i)).Value.ToString();
                        string itemCode = ((EditText)oMatrix.GetCellSpecific("8", i)).Value.ToString();
                        string qtdSaida = ((EditText)oMatrix.GetCellSpecific("4", i)).Value.ToString();
                        double ValorSaida = Convert.ToDouble(qtdSaida.ToString());
                        string qtdLiberar = ((EditText)oMatrix.GetCellSpecific("2", i)).Value.ToString();
                        double ValorLiberar = Convert.ToDouble(qtdLiberar.ToString());
                        string regra = PedidoVendaBLL.GetRegra(docNum);
                        string qtdAbrir = ((EditText)oMatrix.GetCellSpecific("4", i)).Value.ToString();
                        double ValorAbrir = Convert.ToDouble(qtdAbrir);

                        if (regra == "2")
                        {
                            int emb = ProdutoBLL.GetQtdEmbalagem(itemCode);

                            if (emb > ValorLiberar)
                            {
                                ((EditText)oMatrix.GetCellSpecific("3", i)).Value = "0";
                            }
                            else

                            if (ValorAbrir < emb)
                            {
                                ((EditText)oMatrix.GetCellSpecific("3", i)).Value = "0";
                            }
                            else
                            {
                                if (ValorSaida < ValorLiberar)
                                {
                                    double qnt = Math.Floor(ValorSaida / emb);
                                    double valorFinal = (qnt * emb);
                                    if (valorFinal == ValorLiberar)
                                    {
                                        double total = valorFinal - 1;
                                        ((EditText)oMatrix.GetCellSpecific("3", i)).Value = total.ToString();
                                    }
                                    else
                                    {
                                        ((EditText)oMatrix.GetCellSpecific("3", i)).Value = valorFinal.ToString();
                                    }

                                }
                            }
                        }

                    }
                    else
                    {
                        string linha = ((EditText)oMatrix.GetCellSpecific("12", i)).Value.ToString();

                        if (string.IsNullOrEmpty(linha))
                        {
                            docNum = ((EditText)oMatrix.GetCellSpecific("11", i)).Value.ToString();
                            continue;
                        }
                        SBOApp.Application.SetStatusBarMessage("Processando linha: " + i + " de: " + oMatrix.VisualRowCount, BoMessageTime.bmt_Short, false);

                        string itemCode = ((EditText)oMatrix.GetCellSpecific("8", i)).Value.ToString();
                        string qtdSaida = ((EditText)oMatrix.GetCellSpecific("4", i)).Value.ToString();
                        double ValorSaida = Convert.ToDouble(qtdSaida.ToString());
                        string qtdLiberar = ((EditText)oMatrix.GetCellSpecific("2", i)).Value.ToString();
                        double ValorLiberar = Convert.ToDouble(qtdLiberar.ToString());
                        string regra = PedidoVendaBLL.GetRegra(docNum);
                        string qtdAbrir = ((EditText)oMatrix.GetCellSpecific("4", i)).Value.ToString();
                        double ValorAbrir = Convert.ToDouble(qtdAbrir);

                        if (regra == "2")
                        {
                            int emb = ProdutoBLL.GetQtdEmbalagem(itemCode);

                            if (emb > ValorLiberar)
                            {
                                ((EditText)oMatrix.GetCellSpecific("3", i)).Value = "0";
                            }
                            else
                            if (ValorAbrir < emb)
                            {
                                ((EditText)oMatrix.GetCellSpecific("3", i)).Value = "0";
                            }
                            else
                            {
                                if (ValorSaida < ValorLiberar)
                                {
                                    double qnt = Math.Floor(ValorSaida / emb);
                                    double valorFinal = (qnt * emb);
                                    if (valorFinal == ValorLiberar)
                                    {
                                        double total = valorFinal - 1;
                                        ((EditText)oMatrix.GetCellSpecific("3", i)).Value = total.ToString();
                                    }
                                    else
                                    {
                                        ((EditText)oMatrix.GetCellSpecific("3", i)).Value = valorFinal.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                SBOApp.Application.SetStatusBarMessage("Operação Concluida com Sucesso !!!", BoMessageTime.bmt_Short, false);
                edTexto.Item.Click(BoCellClickType.ct_Regular);
                btnCalcular.Item.Enabled = false;
                oMatrix.Columns.Item("3").Editable = false;
            }
            catch (Exception ex)
            {
                Form.Freeze(false);
                SBOApp.Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        private bool VerificaBtnExpandir()
        {
            Button btn = (Button)Form.Items.Item("10000021").Specific;

            if (btn.Item.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
