using Dover.Framework.Form;
using System;
using System.Collections.Generic;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using CVA.Core.ControleDesgasteFerramenta.HELPER;
using CVA.View.ControleQualidade.BLL;
using CVA.View.ControleQualidade.MODEL;
using System.Linq;

namespace CVA.View.ControleQualidade.VIEW
{
    public partial class EntradaProdutoAcabadoView
    {
        public Matrix mt_Principal { get; set; }
        public EditText et_DocNum { get; set; }
        public ComboBox cb_Tipo { get; set; }

        private ApontamentoQualidadeView _apontamentoView { get; set; }

        private ApontamentoBLL _apontamentoBLL { get; set; }
        private PlanoInspecaoBLL _planoInspecaoBLL { get; set; }
        private ItemBLL _itemBLL { get; set; }
        private SAPbouiCOM.Application _application { get; set; }
        private SAPbobsCOM.Company _company { get; set; }

        private static string ErrorMessage { get; set; }
    }

    [FormAttribute(B1Forms.EntradaProdutoAcabado, "CVA.View.ControleQualidade.Resources.Form.EntradaProdutoAcabadoFormPartial.srf")]
    public partial class EntradaProdutoAcabadoView : DoverSystemFormBase
    {
        public EntradaProdutoAcabadoView(ItemBLL itemBLL, ApontamentoBLL apontamentoBLL, PlanoInspecaoBLL planoInspecaoBLL, SAPbouiCOM.Application application, SAPbobsCOM.Company company)
        {
            _application = application;
            _company = company;
            _itemBLL = itemBLL;
            _apontamentoBLL = apontamentoBLL;
            _planoInspecaoBLL = planoInspecaoBLL;

            _application.StatusBarEvent += _application_StatusBarEvent;

            // Não remover, lista utilizada em telas diferentes, então precisa ser reiniciada
            StaticKeys.ItemList = new List<MODEL.Item>();
        }

        public override void OnInitializeComponent()
        {
            mt_Principal = this.GetItem("13").Specific as Matrix;
            et_DocNum = this.GetItem("7").Specific as EditText;
            cb_Tipo = this.GetItem("cb_Tipo").Specific as ComboBox;
        }

        public override void OnInitializeFormEvents()
        {
            base.OnInitializeFormEvents();
        }

        private void _application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                _application.StatusBar.SetText(ErrorMessage);
            }
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            _application.StatusBarEvent -= _application_StatusBarEvent;
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            UIAPIRawForm.Title = "Entrada de Produto Acabado";
        }

        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            try
            {
                if (String.IsNullOrEmpty(cb_Tipo.Value))
                {
                    cb_Tipo.Select("1");
                }

                // Se não for operador, não faz a validação
                if (cb_Tipo.Value != "1")
                {
                    BubbleEvent = true;
                    return;
                }

                if (StaticKeys.ItemList == null)
                {
                    StaticKeys.ItemList = new List<MODEL.Item>();
                }

                int existingDocNum = _apontamentoBLL.VerificaApontamentosRestantes(et_DocNum.Value);
                if (existingDocNum != 0)
                {
                    _apontamentoView = this.CreateForm<ApontamentoQualidadeView>();
                    _apontamentoView.TipoInspecao = TipoInspecaoEnum.Operador;
                    _apontamentoView.AlreadyExists = existingDocNum > 0;
                    _apontamentoView.DocNumEnt = et_DocNum.Value;
                    
                    if (!_apontamentoView.AlreadyExists)
                    {
                        for (int i = 0; i < mt_Principal.RowCount; i++)
                        {
                            var itemColumn = mt_Principal.Columns.Item(5);
                            var et_itemCode = itemColumn.Cells.Item(i + 1).Specific as EditText;
                            if (!String.IsNullOrEmpty(et_itemCode.Value.Trim()))
                            {
                                MODEL.Item planoItem = _itemBLL.GetInspecaoPorItem(et_itemCode.Value);
                                if (!String.IsNullOrEmpty(planoItem.PlanoInspecao))
                                {
                                    PlanoInspecao planoModel = _planoInspecaoBLL.GetHeader(planoItem.PlanoInspecao);
                                    if (planoModel == null)
                                    {
                                        ErrorMessage = String.Format("Plano de inspeção {0} não encontrado!", planoItem.PlanoInspecao);
                                        BubbleEvent = false;
                                        return;
                                    }

                                    if (planoItem.TipoInspecao != "1")
                                    {
                                        continue;
                                    }

                                    if (planoModel.Aprovado != "1")
                                    {
                                        ErrorMessage = String.Format("Plano de inspeção {0} não está aprovado!", planoModel.Code);
                                        BubbleEvent = false;
                                        return;
                                    }
                                    else
                                    {
                                        ErrorMessage = "Apontamento de Qualidade deve ser efetuado!";
                                    }

                                    MODEL.Item item = StaticKeys.ItemList.FirstOrDefault(il => il.ItemCode == et_itemCode.Value);
                                    if (item == null)
                                    {
                                        item = new MODEL.Item();
                                        item.ItemCode = et_itemCode.Value ;
                                        StaticKeys.ItemList.Add(item);
                                    }
                                    
                                    var ordemProducao = mt_Principal.Columns.Item(1);
                                    var numeroOrdem = ordemProducao.Cells.Item(i + 1).Specific as EditText;
                                    _apontamentoView.OrdemProducao = numeroOrdem.Value;
                                }
                            }
                        }
                    }

                    _apontamentoView.Proccess();
                    _apontamentoView.Show();
                    BubbleEvent = false;
                }
                else
                {
                    BubbleEvent = true;
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage("CVA: " + ex.Message, BoMessageTime.bmt_Short);
                BubbleEvent = true;
            }
        }
    }
}
