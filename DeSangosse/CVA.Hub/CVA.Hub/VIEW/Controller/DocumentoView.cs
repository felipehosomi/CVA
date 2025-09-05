using Dover.Framework.Form;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;

namespace CVA.Hub.VIEW.Controller
{
    public class DocumentoView : DoverSystemFormBase
    {
        private Matrix mt_Items { get; set; }
        private ComboBox cb_UtilizacaoPrincipal { get; set; }
        protected SAPbouiCOM.Application _application { get; set; }
        private ItemBLL _ItemBLL { get; set; }
        private UtilizacaoBLL _UtilizacaoBLL { get; set; }
        private DocumentoBLL _DocumentoBLL { get; set; }
        private string ErrorMessage { get; set; }
        private bool DesabilitaValidacaoQtde { get; set; } = false;

        public DocumentoView(SAPbouiCOM.Application application, ItemBLL itemBLL, UtilizacaoBLL utilizacaoBLL, DocumentoBLL documentoBLL)
        {
            _application = application;
            _ItemBLL = itemBLL;
            _UtilizacaoBLL = utilizacaoBLL;
            _DocumentoBLL = documentoBLL;
        }

        #region Initialize
        public override void OnInitializeComponent()
        {
            mt_Items = this.GetItem("38").Specific as Matrix;
            cb_UtilizacaoPrincipal = this.GetItem("1720002171").Specific as ComboBox;
            OnCustomInitializeEvents();
        }

        public void OnCustomInitializeEvents()
        {
            _application.MenuEvent += _application_MenuEvent;
            _application.StatusBarEvent += _application_StatusBarEvent;

            cb_UtilizacaoPrincipal.ComboSelectAfter += UtilizacaoPrincipal_ComboSelectAfter;

            // IMPORTANTE - Necessário setar os eventos na matrix
            // Se setar na coluna, às vezes pára de funcionar
            mt_Items.LostFocusAfter += Mt_Items_LostFocusAfter;
            mt_Items.ComboSelectAfter += Mt_Items_ComboSelectAfter;
            mt_Items.KeyDownBefore += Mt_Items_KeyDownBefore;
        }
        #endregion

        #region ItemEvents
        #region Matrix
        internal virtual void Mt_Items_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "2011")
            {
                this.Utilizacao_ComboSelectAfter(sboObject, pVal);
            }

        }

        internal virtual void Mt_Items_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "11" && pVal.CharPressed != 9)
            {
                this.Quantidade_KeyDownBefore(sboObject, pVal, out BubbleEvent);
            }
        }

        internal virtual void Mt_Items_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "1" || pVal.ColUID == "3")
            {
                this.Item_LostFocusAfter(sboObject, pVal);
            }
            if (pVal.ColUID == "13")
            {
                this.QtdeEmbalagem_LostFocusAfter(sboObject, pVal);
            }
        }
        #endregion

        #region Item_LostFocusAfter
        internal virtual void Item_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                var itemCode = ((EditText)mt_Items.GetCellSpecific("1", pVal.Row)).Value;

                if (!string.IsNullOrEmpty(itemCode))
                {
                    if (_ItemBLL.ItemExiste(itemCode))
                    {
                        Column col_LinhaProduto = mt_Items.Columns.Item("2003");

                        if (!col_LinhaProduto.Visible)
                        {
                            _application.SetStatusBarMessage("Coluna 'Linha de Produtos' deve estar visível em tela");
                        }
                        if (!col_LinhaProduto.Editable)
                        {
                            _application.SetStatusBarMessage("Coluna 'Linha de Produtos' deve estar editável em tela");
                        }

                        if (col_LinhaProduto.Visible && col_LinhaProduto.Editable)
                        {
                            EditText et_LinhaProduto = mt_Items.GetCellSpecific("2003", pVal.Row) as EditText;
                            string centroCusto = _ItemBLL.GetCentroCusto(itemCode);
                            if (!String.IsNullOrEmpty(centroCusto))
                            {
                                et_LinhaProduto.Value = centroCusto;
                            }
                        }
                        
                        if (!String.IsNullOrEmpty(cb_UtilizacaoPrincipal.Value))
                        {
                            EditText et_Deposito = mt_Items.GetCellSpecific("24", pVal.Row) as EditText;
                            string deposito = _UtilizacaoBLL.GetDeposito(cb_UtilizacaoPrincipal.Value);
                            if (!String.IsNullOrEmpty(deposito))
                                et_Deposito.Value = deposito;
                        }

                        if (B1Forms.FormsSaida.Contains(this.UIAPIRawForm.Type.ToString()))
                        {
                            Column col_TextoLivre = mt_Items.Columns.Item("163");

                            if (!col_TextoLivre.Visible)
                            {
                                _application.SetStatusBarMessage("Coluna 'Texto Livre' deve estar visível em tela");
                            }
                            if (!col_TextoLivre.Editable)
                            {
                                _application.SetStatusBarMessage("Coluna 'Texto Livre' deve estar editável em tela");
                            }

                            if (col_TextoLivre.Visible && col_TextoLivre.Editable)
                            {
                                EditText et_TextoLivre = mt_Items.GetCellSpecific("163", pVal.Row) as EditText;
                                string itemObs = _ItemBLL.ItemGetObs(itemCode);
                                if (itemObs.Length > 100)
                                {
                                    itemObs = itemObs.Substring(0, 100);
                                }
                                et_TextoLivre.Value = itemObs;
                            }
                        }

                        this.CalculateQuantity(pVal.Row);
                    }
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region Quantidade
        internal virtual void QtdeEmbalagem_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);
            this.CalculateQuantity(pVal.Row);
            this.UIAPIRawForm.Freeze(false);
        }

        private void CalculateQuantity(int row)
        {
            try
            {
                DesabilitaValidacaoQtde = true;
                string qtdeEmbalagemStr = ((EditText)mt_Items.GetCellSpecific("13", row)).Value.Replace(".", ",");
                double qtdeEmbalagem;
                if (double.TryParse(qtdeEmbalagemStr, out qtdeEmbalagem))
                {
                    Column col_Qtde = mt_Items.Columns.Item("11");

                    if (!col_Qtde.Visible)
                    {
                        throw new Exception("Coluna 'Qtde. (KG,L)' deve estar visível em tela");
                    }
                    if (!col_Qtde.Editable)
                    {
                        throw new Exception("Coluna 'Qtde. (KG,L)' deve estar editável em tela");
                    }
                    string itemCode = ((EditText)mt_Items.GetCellSpecific("1", row)).Value;
                    EditText et_qtde = mt_Items.GetCellSpecific("11", row) as EditText;

                    double qtdePorEmbalagem = _ItemBLL.GetQtdePorEmbalagem(itemCode, B1Forms.GetTipoDoc(this.UIAPIRawForm.Type.ToString()));
                    et_qtde.Value = (qtdeEmbalagem * qtdePorEmbalagem).ToString();
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                DesabilitaValidacaoQtde = false;
            }
        }

        internal virtual void Quantidade_KeyDownBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", pVal.Row)).Value;
            if (!String.IsNullOrEmpty(utilizacao))
            {
                if (_UtilizacaoBLL.GetBloqueioQtde(utilizacao) == "S")
                {
                    _application.SetStatusBarMessage("Campo Qtde (KG,L) desabilitado para edição");
                    BubbleEvent = false;
                }
            }
            else
            {
                _application.SetStatusBarMessage("Campo Qtde (KG,L) desabilitado para edição. Por favor, informe a utilização");
                BubbleEvent = false;
            }
        }

        private void _application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.MenuUID == "771" || pVal.MenuUID == "773" || pVal.MenuUID == "774" || pVal.MenuUID == "8801") // Recortar|Colar|Eliminar|Repetir(Colar)|
            {
                CellPosition position = mt_Items.GetCellFocus();
                if (position != null)
                {
                    if (mt_Items.Columns.Item(position.ColumnIndex).DataBind.Alias == "Quantity")
                    {
                        string utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", position.rowIndex)).Value;
                        if (!String.IsNullOrEmpty(utilizacao))
                        {
                            if (_UtilizacaoBLL.GetBloqueioQtde(utilizacao) == "S")
                            {
                                _application.SetStatusBarMessage("Campo Qtde (KG,L) desabilitado para edição");
                                BubbleEvent = false;
                            }
                        }
                        else
                        {
                            _application.SetStatusBarMessage("Campo Qtde (KG,L) desabilitado para edição. Por favor, informe a utilização");
                            BubbleEvent = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Utilizacao
        internal virtual void Utilizacao_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                var utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", pVal.Row)).Value;

                Column col_Deposito = mt_Items.Columns.Item("24");

                if (col_Deposito.Visible && col_Deposito.Editable)
                {
                    EditText et_Deposito = mt_Items.GetCellSpecific("24", pVal.Row) as EditText;
                    if (!String.IsNullOrEmpty(utilizacao))
                    {
                        string deposito = _UtilizacaoBLL.GetDeposito(utilizacao);
                        if (!String.IsNullOrEmpty(deposito))
                            et_Deposito.Value = deposito;
                    }
                }
                else
                {
                    _application.SetStatusBarMessage("Coluna 'Depósito' deve estar visível em tela");
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        internal virtual void UtilizacaoPrincipal_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                Column col_Deposito = mt_Items.Columns.Item("24");
                if (!col_Deposito.Visible)
                    throw new Exception("Coluna 'Depósito' deve estar visível em tela");

                for (var i = 1; i <= mt_Items.VisualRowCount; i++)
                {
                    var utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", i)).Value;
                    var et_Deposito = (EditText)mt_Items.GetCellSpecific("24", i);

                    if (!String.IsNullOrEmpty(utilizacao))
                    {
                        string deposito = _UtilizacaoBLL.GetDeposito(utilizacao);
                        if (!String.IsNullOrEmpty(deposito))
                            et_Deposito.Value = deposito;
                    }
                }
            }
            catch (Exception ex)
            {
                _application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion
        #endregion

        #region FormEvents
        protected override void OnFormDataAddBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            this.ValidateSaveAction(out BubbleEvent);
        }

        protected override void OnFormDataUpdateBefore(ref BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            this.ValidateSaveAction(out BubbleEvent);
        }

        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            this.SetObsNF();
        }

        protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            this.SetObsNF();
        }

        protected override void OnFormCloseBefore(SBOItemEventArg pVal, out bool BubbleEvent)
        {
            _application.MenuEvent -= _application_MenuEvent;
            _application.StatusBarEvent -= _application_StatusBarEvent;
            BubbleEvent = true;
        }

        private void _application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                _application.SetStatusBarMessage(ErrorMessage);
            }
        }
        #endregion

        #region ValidateSaveAction
        private void ValidateSaveAction(out bool BubbleEvent)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                Column col_LinhaProduto = mt_Items.Columns.Item("2003");
                if (!col_LinhaProduto.Visible)
                    throw new Exception("Coluna 'Linha de Produtos' deve estar visível em tela");
                if (!col_LinhaProduto.Editable)
                    throw new Exception("Coluna 'Linha de Produtos' deve estar editável em tela");

                for (var i = 1; i <= mt_Items.VisualRowCount; i++)
                {
                    var edtCentroCusto = (EditText)mt_Items.GetCellSpecific("2003", i);
                    var edtItem = (EditText)mt_Items.GetCellSpecific("1", i);
                    if (!String.IsNullOrEmpty(edtItem.Value.Trim()))
                    {
                        if (_ItemBLL.GetCentroCusto(edtItem.Value.ToString()) != edtCentroCusto.Value.ToString())
                            throw new Exception($"Centro de Custo {edtCentroCusto.Value} inválido para o item {edtItem.Value} na linha {i}.");
                    }
                }

                Column col_Deposito = mt_Items.Columns.Item("24");
                if (!col_Deposito.Visible)
                    throw new Exception("Coluna 'Depósito' deve estar visível em tela");
                if (!col_Deposito.Editable)
                    throw new Exception("Coluna 'Depósito' deve estar editável em tela");

                for (var i = 1; i <= mt_Items.VisualRowCount; i++)
                {
                    var utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", i)).Value;
                    var et_Deposito = (EditText)mt_Items.GetCellSpecific("24", i);

                    if (!String.IsNullOrEmpty(utilizacao))
                    {
                        string deposito = _UtilizacaoBLL.GetDeposito(utilizacao);
                        if (!String.IsNullOrEmpty(deposito) && deposito != et_Deposito.Value.ToString())
                            throw new Exception($"Depósito {et_Deposito.Value} inválido para a utilização {utilizacao} na linha {i}.");
                    }
                }
                BubbleEvent = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                BubbleEvent = false;
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region SetObsNF
        private void SetObsNF()
        {
            DBDataSource dtsDoc = this.UIAPIRawForm.DataSources.DBDataSources.Item(0);
            if (!String.IsNullOrEmpty(dtsDoc.GetValue("DocEntry", dtsDoc.Offset)))
            {
                int docEntry = Convert.ToInt32(dtsDoc.GetValue("DocEntry", dtsDoc.Offset));
                int objType = Convert.ToInt32(dtsDoc.GetValue("ObjType", dtsDoc.Offset));
                try
                {
                    _DocumentoBLL.UpdateObs(docEntry, objType);
                }
                catch (Exception ex)
                {
                    _application.MessageBox(ex.Message);
                }
            }
        }
        #endregion
    }
}
