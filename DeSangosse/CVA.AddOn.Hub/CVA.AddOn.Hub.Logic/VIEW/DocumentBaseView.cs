using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.UI;
using CVA.AddOn.Hub.Logic.BLL;
using CVA.AddOn.Hub.Logic.MODEL;
using CVA.Hub.BLL;
using CVA.Hub.HELPER;
using SAPbouiCOM;
using System;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class DocumentBaseView : BaseForm
    {
        private Form Form;
        public static string ErrorMessage { get; set; }
        public static bool BlockCancel { get; set; }
        public static bool IsCanceling { get; set; }
        public static bool BlockQuantity { get; set; }
        public static bool HasChangedQuantity { get; set; }

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            }

            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    SBOApp.Application.MenuEvent -= Application_MenuEvent;
                    SBOApp.Application.StatusBarEvent -= Application_StatusBarEvent;
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "11" && ItemEventInfo.CharPressed != 9)
                    {
                        return this.Quantidade_KeyDownBefore();
                    }
                    if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "13" && ItemEventInfo.CharPressed != 9)
                    {
                        HasChangedQuantity = true;
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                {
                    if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "13")
                    {
                        return this.QtdeEmbalagem_LostFocusBefore();
                    }
                }
                if(ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.FormTypeEx == "139" && ItemEventInfo.ItemUID == "10000329")
                    {
                        if (ItemEventInfo.PopUpIndicator == 1)
                        {
                            var ds = Form.DataSources.DBDataSources.Item("ORDR");
                            var bll = new PedidoVendaBLL();
                            return bll.ValidaLotes(ds.GetValue("DocEntry", 0));
                        }
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    ErrorMessage = String.Empty;
                    SBOApp.Application.MenuEvent += Application_MenuEvent;
                    SBOApp.Application.StatusBarEvent += Application_StatusBarEvent;
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == DocumentIDs.MainUsage)
                    {
                        this.UtilizacaoPrincipal_ComboSelectAfter();
                    }
                }
                if (ItemEventInfo.ItemUID == "38")
                {
                    if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                    {
                        if (ItemEventInfo.ColUID == ItemMatrixIDs.Usage) // Utilização
                        {
                            this.Utilizacao_ComboSelectAfter();
                        }
                    }
                    if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                    {
                        EventFiltersBLL.DisableEvents();
                        try
                        {
                            switch (ItemEventInfo.ColUID)
                            {
                                // Não colocar no lostfocus se não ele entra em looping
                                //case "1": // Código item
                                //case "3": // Descrição item
                                //    this.Item_LostFocusAfter();
                                //    break;
                                case "13": // Qtde. Embalagem
                                    this.QtdeEmbalagem_LostFocusAfter();
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            EventFiltersBLL.SetDefaultEvents();
                        }
                    }
                    if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                    {
                        EventFiltersBLL.DisableEvents();
                        try
                        {
                            switch (ItemEventInfo.ColUID)
                            {
                                case "1": // Código item
                                case "3": // Descrição item
                                    this.Item_LostFocusAfter();
                                    break;
                                    //case "13": // Qtde. Embalagem
                                    // Não colocar no validate se não ele recalcula a quantidade!
                                    //    this.QtdeEmbalagem_LostFocusAfter();
                                    //    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            EventFiltersBLL.SetDefaultEvents();
                        }
                    }
                }
            }

            return base.ItemEvent();
        }
        #endregion

        #region FormDataEvent
        public override bool FormDataEvent()
        {

            Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
            if (BusinessObjectInfo.BeforeAction)
            {
                if (!IsCanceling)
                {
                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                    {
                        return this.ValidateSaveAction();
                    }
                }
                IsCanceling = false;
            }
            else
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    this.SetObsNF();
                }
            }

            return true;
        }
        #endregion

        #region Item_LostFocusAfter
        internal virtual void Item_LostFocusAfter()
        {
            this.Form.Freeze(true);
            try
            {
                Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
                var itemCode = ((EditText)mt_Items.GetCellSpecific("1", ItemEventInfo.Row)).Value;

                if (!string.IsNullOrEmpty(itemCode))
                {
                    ItemBLL itemBLL = new ItemBLL();
                    if (itemBLL.ItemExiste(itemCode))
                    {
                        Column col_LinhaProduto = mt_Items.Columns.Item("2003");

                        if (!col_LinhaProduto.Visible)
                        {
                            SBOApp.Application.SetStatusBarMessage("Coluna 'Linha de Produtos' deve estar visível em tela");
                        }
                        if (!col_LinhaProduto.Editable)
                        {
                            SBOApp.Application.SetStatusBarMessage("Coluna 'Linha de Produtos' deve estar editável em tela");
                        }

                        if (col_LinhaProduto.Visible && col_LinhaProduto.Editable)
                        {
                            EditText et_LinhaProduto = mt_Items.GetCellSpecific("2003", ItemEventInfo.Row) as EditText;
                            DocumentItemModel itemModel = itemBLL.GetCentroCusto(itemCode);
                            if (!String.IsNullOrEmpty(itemModel.OcrCode2))
                            {
                                et_LinhaProduto.Value = itemModel.OcrCode2;
                            }
                        }

                        ComboBox cb_UtilizacaoPrincipal = this.Form.Items.Item("1720002171").Specific as ComboBox;
                        if (!String.IsNullOrEmpty(cb_UtilizacaoPrincipal.Value))
                        {
                            UtilizacaoBLL utilizacaoBLL = new UtilizacaoBLL();
                            EditText et_Deposito = mt_Items.GetCellSpecific("24", ItemEventInfo.Row) as EditText;
                            string deposito = utilizacaoBLL.GetDeposito(cb_UtilizacaoPrincipal.Value);
                            if (!String.IsNullOrEmpty(deposito))
                                et_Deposito.Value = deposito;
                        }

                        if (B1Forms.GetTipoDoc(this.Form.Type.ToString()) == CVA.Hub.MODEL.TipoDoc.Saida)
                        {
                            Column col_TextoLivre = mt_Items.Columns.Item("256");

                            if (!col_TextoLivre.Visible)
                            {
                                SBOApp.Application.SetStatusBarMessage("Coluna 'Detalhes do item' deve estar visível em tela");
                            }
                            if (!col_TextoLivre.Editable)
                            {
                                SBOApp.Application.SetStatusBarMessage("Coluna 'Detalhes do item' deve estar editável em tela");
                            }

                            if (col_TextoLivre.Visible && col_TextoLivre.Editable)
                            {
                                EditText et_TextoLivre = mt_Items.GetCellSpecific("256", ItemEventInfo.Row) as EditText;
                                string itemObs = itemBLL.ItemGetObs(itemCode);

                                et_TextoLivre.Value = itemObs;
                            }
                        }

                        this.CalculateQuantity(ItemEventInfo.Row);
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                this.Form.Freeze(false);
            }
        }
        #endregion

        #region Utilizacao
        internal virtual void Utilizacao_ComboSelectAfter()
        {
            this.Form.Freeze(true);
            try
            {
                Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
                var utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", ItemEventInfo.Row)).Value;

                Column col_Deposito = mt_Items.Columns.Item("24");

                if (col_Deposito.Visible && col_Deposito.Editable)
                {
                    EditText et_Deposito = mt_Items.GetCellSpecific("24", ItemEventInfo.Row) as EditText;
                    if (!String.IsNullOrEmpty(utilizacao))
                    {
                        UtilizacaoBLL utilizacaoBLL = new UtilizacaoBLL();
                        string deposito = utilizacaoBLL.GetDeposito(utilizacao);
                        if (!String.IsNullOrEmpty(deposito))
                            et_Deposito.Value = deposito;
                    }
                }
                else
                {
                    SBOApp.Application.SetStatusBarMessage("Coluna 'Depósito' deve estar visível em tela");
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                this.Form.Freeze(false);
            }
        }

        internal virtual void UtilizacaoPrincipal_ComboSelectAfter()
        {
            this.Form.Freeze(true);
            try
            {
                Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
                Column col_Deposito = mt_Items.Columns.Item("24");
                if (!col_Deposito.Visible)
                    throw new Exception("Coluna 'Depósito' deve estar visível em tela");

                for (var i = 1; i <= mt_Items.VisualRowCount; i++)
                {
                    var utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", i)).Value;
                    var et_Deposito = (EditText)mt_Items.GetCellSpecific("24", i);

                    if (!String.IsNullOrEmpty(utilizacao))
                    {
                        UtilizacaoBLL utilizacaoBLL = new UtilizacaoBLL();
                        string deposito = utilizacaoBLL.GetDeposito(utilizacao);
                        if (!String.IsNullOrEmpty(deposito))
                            et_Deposito.Value = deposito;
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                this.Form.Freeze(false);
            }
        }
        #endregion

        #region Quantidade
        internal virtual bool QtdeEmbalagem_LostFocusBefore()
        {
            if (BlockQuantity)
            {
                Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
                string qtdeEmbalagemStr = ((EditText)mt_Items.GetCellSpecific("13", ItemEventInfo.Row)).Value.Replace(".", ",");
                double qtdeEmbalagem;
                if (double.TryParse(qtdeEmbalagemStr, out qtdeEmbalagem))
                {
                    if (qtdeEmbalagem % 1 != 0)
                    {
                        SBOApp.Application.SetStatusBarMessage("Campo Qtde. (CXs)  deve ser um valor numérico inteiro");
                        return false;
                    }
                }
            }
            return true;
        }

        internal virtual void QtdeEmbalagem_LostFocusAfter()
        {
            if (HasChangedQuantity)
            {
                this.Form.Freeze(true);
                this.CalculateQuantity(ItemEventInfo.Row);
                this.Form.Freeze(false);
                HasChangedQuantity = false;
            }
        }

        private void CalculateQuantity(int row)
        {
            try
            {
                Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
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

                    EditText et_qtde = mt_Items.GetCellSpecific("11", row) as EditText;
                    if (qtdeEmbalagem == 0)
                    {
                        try
                        {
                            et_qtde.Value = "1";
                        }
                        catch { }
                    }
                    else
                    {
                        string itemCode = ((EditText)mt_Items.GetCellSpecific("1", row)).Value;
                        ItemBLL itemBLL = new ItemBLL();
                        double qtdePorEmbalagem = itemBLL.GetQtdePorEmbalagem(itemCode, B1Forms.GetTipoDoc(this.Form.Type.ToString()));
                        et_qtde.Value = (qtdeEmbalagem * qtdePorEmbalagem).ToString().Replace(",", ".");
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
        }

        internal virtual bool Quantidade_KeyDownBefore()
        {
            Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
            string utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", ItemEventInfo.Row)).Value;
            if (!String.IsNullOrEmpty(utilizacao))
            {
                UtilizacaoBLL utilizacaoBLL = new UtilizacaoBLL();
                BlockQuantity = utilizacaoBLL.GetBloqueioQtde(utilizacao) == "S";
                if (BlockQuantity)
                {
                    SBOApp.Application.SetStatusBarMessage("Campo Qtde. (KG,L) desabilitado para edição");
                    return false;
                }
            }
            else
            {
                SBOApp.Application.SetStatusBarMessage("Campo Qtde. (KG,L) desabilitado para edição. Por favor, informe a utilização");
                return false;
            }
            return true;
        }

        private void Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.MenuUID == "771" || pVal.MenuUID == "773" || pVal.MenuUID == "774" || pVal.MenuUID == "8801") // Recortar|Colar|Eliminar|Repetir(Colar)|
            {
                try
                {
                    Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;

                    CellPosition position = mt_Items.GetCellFocus();
                    if (position != null)
                    {
                        if (mt_Items.Columns.Item(position.ColumnIndex).DataBind.Alias == "Quantity")
                        {
                            string utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", position.rowIndex)).Value;
                            if (!String.IsNullOrEmpty(utilizacao))
                            {
                                UtilizacaoBLL utilizacaoBLL = new UtilizacaoBLL();
                                if (utilizacaoBLL.GetBloqueioQtde(utilizacao) == "S")
                                {
                                    SBOApp.Application.SetStatusBarMessage("Campo Qtde. (KG,L) desabilitado para edição");
                                    BubbleEvent = false;
                                }
                            }
                            else
                            {
                                SBOApp.Application.SetStatusBarMessage("Campo Qtde. (KG,L) desabilitado para edição. Por favor, informe a utilização");
                                BubbleEvent = false;
                            }
                        }
                    }
                }
                catch { }
            }
        }
        #endregion

        #region ValidateSaveAction
        private bool ValidateSaveAction()
        {
            this.Form.Freeze(true);
            try
            {
                EventFiltersBLL.DisableEvents();

                Matrix mt_Items = this.Form.Items.Item("38").Specific as Matrix;
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
                        ItemBLL itemBLL = new ItemBLL();
                        DocumentItemModel itemModel = itemBLL.GetCentroCusto(edtItem.Value.ToString());
                        if (itemModel.Validacao != "S")
                        {
                            if (!String.IsNullOrEmpty(itemModel.OcrCode2) && itemModel.OcrCode2 != edtCentroCusto.Value.Trim())
                                throw new Exception($"Centro de Custo {edtCentroCusto.Value} inválido para o item {edtItem.Value} na linha {i}.");
                        }
                    }
                }

                Column col_Deposito = mt_Items.Columns.Item("24");
                if (!col_Deposito.Visible)
                    throw new Exception("Coluna 'Depósito' deve estar visível em tela");
                if (!col_Deposito.Editable)
                    throw new Exception("Coluna 'Depósito' deve estar editável em tela");

                for (var i = 1; i <= mt_Items.VisualRowCount; i++)
                {
                    var item = ((EditText)mt_Items.GetCellSpecific("1", i)).Value;
                    string qtdeEmbalagemStr = ((EditText)mt_Items.GetCellSpecific("13", i)).Value.Replace(".", ",");

                    double qtdeEmbalagem;
                    double.TryParse(qtdeEmbalagemStr, out qtdeEmbalagem);
                    if (qtdeEmbalagem == 0 && !String.IsNullOrEmpty(item))
                    {
                        throw new Exception($"Linha {i}: Qtde. (CXs) deve ser informada!");
                    }

                    var utilizacao = ((ComboBox)mt_Items.GetCellSpecific("2011", i)).Value;
                    var et_Deposito = (EditText)mt_Items.GetCellSpecific("24", i);

                    if (!String.IsNullOrEmpty(utilizacao))
                    {
                        UtilizacaoBLL utilizacaoBLL = new UtilizacaoBLL();
                        string deposito = utilizacaoBLL.GetDeposito(utilizacao);
                        if (!String.IsNullOrEmpty(deposito) && deposito != et_Deposito.Value.ToString())
                            throw new Exception($"Linha {i}: Depósito {et_Deposito.Value} inválido para a utilização {utilizacao}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                EventFiltersBLL.SetDefaultEvents();
                this.Form.Freeze(false);
            }
        }
        #endregion

        #region SetObsNF
        private void SetObsNF()
        {
            DBDataSource dtsDoc = this.Form.DataSources.DBDataSources.Item(0);
            if (!String.IsNullOrEmpty(dtsDoc.GetValue("DocEntry", dtsDoc.Offset)))
            {
                int docEntry = Convert.ToInt32(dtsDoc.GetValue("DocEntry", dtsDoc.Offset));
                int objType = Convert.ToInt32(dtsDoc.GetValue("ObjType", dtsDoc.Offset));
                string cancelado = dtsDoc.GetValue("CANCELED", dtsDoc.Offset);
                if (cancelado != "Y")
                {
                    try
                    {
                        DocumentoBLL documentoBLL = new DocumentoBLL();
                        documentoBLL.UpdateObs(docEntry, objType);
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.MessageBox(ex.Message);
                    }
                }
            }
        }
        #endregion

        private void Application_StatusBarEvent(string Text, BoStatusBarMessageType messageType)
        {
            if (Text.Contains("UI_API -7780") && !String.IsNullOrEmpty(ErrorMessage))
            {
                SBOApp.Application.SetStatusBarMessage(ErrorMessage);
            }
        }
    }
}
