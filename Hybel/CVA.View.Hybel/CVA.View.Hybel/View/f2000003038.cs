using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Simulador Importação
    /// </summary>
    public class f2000003038 : BaseForm
    {
        Form Form;
        public static bool Changed = false;

        #region Constructor
        public f2000003038()
        {
            FormCount++;
        }

        public f2000003038(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003038(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003038(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public object Show(List<ItemModel> itemList)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            try
            {
                string impostoWhere = String.Empty;
                foreach (var itemModel in itemList)
                {
                    impostoWhere += $", '{itemModel.Imposto}'";
                }

                string sqlImposto = String.Format(SQL.Imposto_GetTaxas, impostoWhere.Substring(2));

                List<ImpostoModel> impostoList = new CrudController().FillModelListAccordingToSql<ImpostoModel>(sqlImposto);
                DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                dt_Item.Rows.Add(itemList.Count + 1);

                ImpostoModel impostoModel;

                int i = 0;
                double totalRS = 0;
                foreach (var itemModel in itemList)
                {
                    dt_Item.SetValue("Linha", i, itemModel.Linha);
                    dt_Item.SetValue("Item", i, itemModel.ItemCode);
                    dt_Item.SetValue("Qtde", i, itemModel.Quantidade);
                    dt_Item.SetValue("Peso_Unit", i, itemModel.Peso);
                    dt_Item.SetValue("Peso", i, itemModel.Quantidade * itemModel.Peso);
                    dt_Item.SetValue("Preco_US", i, itemModel.PrecoUnitarioUSD);
                    dt_Item.SetValue("Preco_RS", i, itemModel.PrecoUnitarioUSD * itemModel.TaxaMoeda);
                    dt_Item.SetValue("Taxa", i, itemModel.TaxaMoeda);
                    dt_Item.SetValue("Imposto", i, itemModel.Imposto);

                    impostoModel = impostoList.FirstOrDefault(m => m.CodigoImposto == itemModel.Imposto && m.TipoImposto == "II");
                    if (impostoModel != null)
                    {
                        dt_Item.SetValue("P_II", i, impostoModel.Taxa / 100);
                    }
                    impostoModel = impostoList.FirstOrDefault(m => m.CodigoImposto == itemModel.Imposto && m.TipoImposto == "PIS-IMP");
                    if (impostoModel != null)
                    {
                        dt_Item.SetValue("P_PIS", i, impostoModel.Taxa / 100);
                    }
                    impostoModel = impostoList.FirstOrDefault(m => m.CodigoImposto == itemModel.Imposto && m.TipoImposto == "COFINS-IMP");
                    if (impostoModel != null)
                    {
                        dt_Item.SetValue("P_COFINS", i, impostoModel.Taxa / 100);
                    }
                    impostoModel = impostoList.FirstOrDefault(m => m.CodigoImposto == itemModel.Imposto && m.TipoImposto == "IPI-IMP");
                    if (impostoModel != null)
                    {
                        dt_Item.SetValue("P_IPI", i, impostoModel.Taxa / 100);
                    }
                    impostoModel = impostoList.FirstOrDefault(m => m.CodigoImposto == itemModel.Imposto && m.TipoImposto == "ICMS-IMP");
                    if (impostoModel != null)
                    {
                        if (impostoModel.Taxa != 0)
                        {
                            dt_Item.SetValue("P_ICMS", i, impostoModel.Taxa / 100);
                        }
                        else
                        {
                            dt_Item.SetValue("P_ICMS", i, 1);
                        }
                        dt_Item.SetValue("P_BC_ICMS", i, impostoModel.BaseCalculo / 100);
                    }
                    else
                    {
                        dt_Item.SetValue("P_ICMS", i, 1);
                        dt_Item.SetValue("P_BC_ICMS", i, 1);
                    }

                    totalRS += itemModel.PrecoUnitarioUSD * itemModel.TaxaMoeda;
                    i++;
                }

                dt_Item.SetValue("Item", itemList.Count, "Total");
                dt_Item.SetValue("Qtde", itemList.Count, itemList.Sum(m => m.Quantidade));
                dt_Item.SetValue("Peso", itemList.Count, itemList.Sum(m => m.Peso * m.Quantidade));
                dt_Item.SetValue("Preco_US", itemList.Count, itemList.Sum(m => m.PrecoUnitarioUSD));
                dt_Item.SetValue("Preco_RS", itemList.Count, totalRS);

                Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                gr_Item.Columns.Item("Linha").TitleObject.Caption = "#";
                gr_Item.Columns.Item("Peso_Unit").TitleObject.Caption = "Peso Unit.";
                gr_Item.Columns.Item("Peso").TitleObject.Caption = "Peso Total";
                gr_Item.Columns.Item("Preco_US").TitleObject.Caption = "Preço USD";
                gr_Item.Columns.Item("Preco_RS").TitleObject.Caption = "Preço R$";
                gr_Item.Columns.Item("Frete_Int").TitleObject.Caption = "Frete Intern.";
                gr_Item.Columns.Item("Desp_Adic").TitleObject.Caption = "Desp. Adic.";
                gr_Item.Columns.Item("Frete_Nac").TitleObject.Caption = "Frete Nac.";
                gr_Item.Columns.Item("BC_ICMS").TitleObject.Caption = "BC. ICMS";
                gr_Item.Columns.Item("Tot_Imp").TitleObject.Caption = "Total c/ Imp. (-) IPI";
                gr_Item.Columns.Item("Unit_Imp").TitleObject.Caption = "Custo Unit. c/ Impostos";
                gr_Item.Columns.Item("Unit_S_Imp").TitleObject.Caption = "Custo Unit. s/ Impostos";

                gr_Item.Columns.Item("P_II").Visible = false;
                gr_Item.Columns.Item("P_PIS").Visible = false;
                gr_Item.Columns.Item("P_COFINS").Visible = false;
                gr_Item.Columns.Item("P_IPI").Visible = false;
                gr_Item.Columns.Item("P_ICMS").Visible = false;
                gr_Item.Columns.Item("P_BC_ICMS").Visible = false;
                if (!itemList.Any(m => m.Peso > 0))
                {
                    gr_Item.Columns.Item("Peso_Unit").Visible = false;
                    gr_Item.Columns.Item("Peso").Visible = false;
                }

                gr_Item.Columns.Item("Linha").Editable = false;

                gr_Item.AutoResizeColumns();
                gr_Item.RowHeaders.Width = 0;
                //this.Calculate();
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
            return Form;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            try
            {
                if (!ItemEventInfo.BeforeAction)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                    {
                        if (ItemEventInfo.ItemUID == "gr_Item" && ItemEventInfo.ColUID == "Imposto")
                        {
                            Form.Freeze(true);
                            this.ChooseFromList();
                        }
                    }
                    if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE && ItemEventInfo.ItemUID == "gr_Item")
                    {
                        Form.Freeze(true);
                        EventFilterBLL.DisableEvents();
                        if (!Changed)
                        {
                            return true;
                        }
                        Changed = false;
                        DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                        Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
                        
                        switch (ItemEventInfo.ColUID)
                        {
                            case "Taxa":
                            case "Preco_US":
                                this.CalculaPrecoUS();
                                double taxa = (double)dt_Item.GetValue("Taxa", ItemEventInfo.Row);
                                double precoUS = (double)dt_Item.GetValue("Preco_US", ItemEventInfo.Row);
                                dt_Item.SetValue("Preco_RS", ItemEventInfo.Row, taxa * precoUS);
                                // Chama antes das despesas para calcular o CIF
                                this.CalculaValores();
                                this.CalculaPorPesoOuCIF("Desp_Adic");
                                this.CalculaPorPesoOuCIF("Frete_Nac");
                                this.CalculaPorPesoOuCIF("Armazem");
                                // Chama novamente pois as despesas foram recalculadas
                                this.CalculaValores();
                                break;
                            case "Capazia":
                            case "Frete_Int":
                                if (ItemEventInfo.Row + 1 == dt_Item.Rows.Count)
                                {
                                    this.CalculaPorPesoOuQtde(ItemEventInfo.ColUID);
                                    this.CalculaValores();
                                }
                                break;
                            case "Desp_Adic":
                            case "Frete_Nac":
                            case "Armazem":
                                if (ItemEventInfo.Row + 1 == dt_Item.Rows.Count)
                                {
                                    this.CalculaPorPesoOuCIF(ItemEventInfo.ColUID);
                                    this.CalculaValores();
                                }
                                break;
                            case "Qtde":
                            case "Peso_Unit":
                                if (ItemEventInfo.Row + 1 < dt_Item.Rows.Count)
                                {
                                    if (ItemEventInfo.ColUID == "Qtde")
                                    {
                                        double total = 0;
                                        for (int i = 0; i < dt_Item.Rows.Count - 1; i++)
                                        {
                                            total += (double)dt_Item.GetValue(ItemEventInfo.ColUID, i);
                                        }
                                        dt_Item.SetValue(ItemEventInfo.ColUID, dt_Item.Rows.Count - 1, total);
                                    }
                                    dt_Item.SetValue("Peso", ItemEventInfo.Row, (double)dt_Item.GetValue("Qtde", ItemEventInfo.Row) * (double)dt_Item.GetValue("Peso_Unit", ItemEventInfo.Row));

                                    double pesoTotal = 0;
                                    for (int i = 0; i < dt_Item.Rows.Count - 1; i++)
                                    {
                                        pesoTotal += (double)dt_Item.GetValue("Peso", i);
                                    }
                                    dt_Item.SetValue("Peso", dt_Item.Rows.Count - 1, pesoTotal);

                                    this.CalculaPorPesoOuQtde("Capazia");
                                    this.CalculaPorPesoOuQtde("Frete_Int");
                                    this.CalculaPorPesoOuCIF("Desp_Adic");
                                    this.CalculaPorPesoOuCIF("Frete_Nac");
                                    this.CalculaPorPesoOuCIF("Armazem");
                                    this.CalculaValores();
                                }
                                break;
                        }
                    }
                    if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT && ItemEventInfo.ItemUID == "cb_Tipo")
                    {
                        Form.Freeze(true);
                        this.CalculaValores();
                    }
                }
                else
                {
                    if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                    {
                        int result = SBOApp.Application.MessageBox("Dados não serão salvos, deseja continuar?", 2, "Sim", "Não");
                        if (result == 2)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN && ItemEventInfo.ItemUID == "gr_Item")
                    {
                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                        Form.Freeze(true);
                        DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                        switch (ItemEventInfo.ColUID)
                        {
                            case "Capazia":
                            case "Frete_Int":
                            case "Desp_Adic":
                            case "Frete_Nac":
                            case "Armazem":
                                if (ItemEventInfo.Row + 1 < dt_Item.Rows.Count)
                                {
                                    return false;
                                }
                                else if (ItemEventInfo.CharPressed != 9)
                                {
                                    Changed = true;
                                }
                                break;
                            case "Qtde":
                            case "Peso_Unit":
                            case "Preco_US":
                            case "Taxa":
                            case "CIF":
                            case "Item":
                            case "Imposto":
                                if (ItemEventInfo.Row + 1 == dt_Item.Rows.Count)
                                {
                                    return false;
                                }
                                else if (ItemEventInfo.CharPressed != 9)
                                {
                                    Changed = true;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                if (Form != null)
                {
                    Form.Freeze(false);
                    //SBOApp.Application.SetStatusBarMessage($"Evento:  {ItemEventInfo.EventType.ToString()} - Campo: {ItemEventInfo.ColUID}");
                }
                EventFilterBLL.EnableEvents();
            }
            return true;
        }
        #endregion

        private void CalculaPrecoUS()
        {
            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            double preco = 0;
            
            for (int i = 0; i < dt_Item.Rows.Count - 1; i++)
            {
                preco += (double)dt_Item.GetValue("Preco_US", i);
            }
            dt_Item.SetValue("Preco_US", dt_Item.Rows.Count - 1, preco);
        }

        private void CalculaPorPesoOuCIF(string coluna)
        {
            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
            string colunaBase;
            if (gr_Item.Columns.Item("Peso").Visible)
            {
                colunaBase = "Peso";
            }
            else
            {
                colunaBase = "CIF";
            }

            double cifTotal = (double)dt_Item.GetValue(colunaBase, dt_Item.Rows.Count - 1);
            double valor = (double)dt_Item.GetValue(coluna, dt_Item.Rows.Count - 1);

            if (cifTotal > 0)
            {
                for (int i = 0; i < dt_Item.Rows.Count - 1; i++)
                {
                    double total = (double)dt_Item.GetValue(colunaBase, i);
                    dt_Item.SetValue(coluna, i, (total / cifTotal) * valor);
                }
            }
        }

        #region CalculaPorPesoOuQtde
        private void CalculaPorPesoOuQtde(string coluna)
        {
            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;
            string colunaBase;
            if (gr_Item.Columns.Item("Peso").Visible)
            {
                colunaBase = "Peso";
            }
            else
            {
                colunaBase = "Qtde";
            }
            double qtdeTotal = (double)dt_Item.GetValue(colunaBase, dt_Item.Rows.Count - 1);
            double valor = (double)dt_Item.GetValue(coluna, dt_Item.Rows.Count - 1);

            if (qtdeTotal > 0)
            {
                for (int i = 0; i < dt_Item.Rows.Count - 1; i++)
                {
                    double qtde = (double)dt_Item.GetValue(colunaBase, i);
                    dt_Item.SetValue(coluna, i, (qtde / qtdeTotal) * valor);
                }
            }
        }
        #endregion

        #region CalculaValores
        private void CalculaValores()
        {
            string tipo = Form.DataSources.UserDataSources.Item("ud_Tipo").Value;
            if (String.IsNullOrEmpty(tipo.Trim()))
            {
                SBOApp.Application.SetStatusBarMessage("Informe o tipo");
                return;
            }

            DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
            double sumCIF = 0;
            double sumII = 0;
            double sumPIS = 0;
            double sumCOFINS = 0;
            double sumBC = 0;
            double sumICMS = 0;
            double sumSemIPI = 0;
            double sumIPI = 0;

            Grid gr_Item = Form.Items.Item("gr_Item").Specific as Grid;

            for (int i = 0; i < dt_Item.Rows.Count - 1; i++)
            {
                double qtde = (double)dt_Item.GetValue("Qtde", i);
                double precoRS = (double)dt_Item.GetValue("Preco_RS", i);
                double capazia = (double)dt_Item.GetValue("Capazia", i);
                double freteInt = (double)dt_Item.GetValue("Frete_Int", i);
                double cif = (qtde * precoRS) + capazia + freteInt;
                double baseCalculoICMS;
                double totalSemIPI;
                double ii = Math.Round(cif * (double)dt_Item.GetValue("P_II", i), 2);
                double pis = Math.Round(cif * (double)dt_Item.GetValue("P_PIS", i), 2);
                double cofins = Math.Round(cif * (double)dt_Item.GetValue("P_COFINS", i), 2);
                double unitSemImposto;
                double peso = (double)dt_Item.GetValue("Peso", i);
                double ipi;
                double icms;

                dt_Item.SetValue("CIF", i, cif);
                dt_Item.SetValue("II", i, ii);
                dt_Item.SetValue("PIS", i, pis);
                dt_Item.SetValue("COFINS", i, cofins);

                sumCIF += cif;
                sumII += ii;
                sumPIS += pis;
                sumCOFINS += cofins;

                ipi = (cif + ((double)dt_Item.GetValue("II", i))) * (double)dt_Item.GetValue("P_IPI", i);
                dt_Item.SetValue("IPI", i, ipi);
                sumIPI += ipi;

                baseCalculoICMS = cif + ii + pis + cofins + ipi + (double)dt_Item.GetValue("Desp_Adic", i) + (double)dt_Item.GetValue("Frete_Nac", i) + (double)dt_Item.GetValue("Armazem", i);
                baseCalculoICMS = baseCalculoICMS / (double)dt_Item.GetValue("P_BC_ICMS", i);
                icms = baseCalculoICMS * (double)dt_Item.GetValue("P_ICMS", i);
                dt_Item.SetValue("BC_ICMS", i, baseCalculoICMS);

                sumBC += baseCalculoICMS;

                dt_Item.SetValue("ICMS", i, icms);
                sumICMS += icms;

                totalSemIPI = cif + ii + pis + cofins + (double)dt_Item.GetValue("Desp_Adic", i) + (double)dt_Item.GetValue("Frete_Nac", i) + (double)dt_Item.GetValue("Armazem", i) + (double)dt_Item.GetValue("ICMS", i);
                dt_Item.SetValue("Tot_Imp", i, totalSemIPI);
                sumSemIPI += totalSemIPI;

                if (gr_Item.Columns.Item("Peso").Visible)
                {
                    dt_Item.SetValue("Unit_Imp", i, (double)dt_Item.GetValue("Tot_Imp", i) / (double)dt_Item.GetValue("Peso", i) / (double)dt_Item.GetValue("Qtde", i));

                    unitSemImposto = (double)dt_Item.GetValue("Tot_Imp", i) - (double)dt_Item.GetValue("ICMS", i) - pis - cofins;
                    unitSemImposto = unitSemImposto / (double)dt_Item.GetValue("Peso", i) / (double)dt_Item.GetValue("Qtde", i);
                    dt_Item.SetValue("Unit_S_Imp", i, unitSemImposto);
                }
                else
                {
                    dt_Item.SetValue("Unit_Imp", i, (double)dt_Item.GetValue("Tot_Imp", i) / (double)dt_Item.GetValue("Qtde", i));

                    unitSemImposto = (double)dt_Item.GetValue("Tot_Imp", i) - (double)dt_Item.GetValue("ICMS", i) - (double)dt_Item.GetValue("PIS", i) - (double)dt_Item.GetValue("COFINS", i);
                    unitSemImposto = unitSemImposto / (double)dt_Item.GetValue("Qtde", i);
                    dt_Item.SetValue("Unit_S_Imp", i, unitSemImposto);
                }
            }
            dt_Item.SetValue("CIF", dt_Item.Rows.Count - 1, sumCIF);
            dt_Item.SetValue("II", dt_Item.Rows.Count - 1, sumII);
            dt_Item.SetValue("PIS", dt_Item.Rows.Count - 1, sumPIS);
            dt_Item.SetValue("COFINS", dt_Item.Rows.Count - 1, sumCOFINS);
            dt_Item.SetValue("BC_ICMS", dt_Item.Rows.Count - 1, sumBC);
            dt_Item.SetValue("ICMS", dt_Item.Rows.Count - 1, sumICMS);
            dt_Item.SetValue("Tot_Imp", dt_Item.Rows.Count - 1, sumSemIPI);
            dt_Item.SetValue("IPI", dt_Item.Rows.Count - 1, sumIPI);
        }
        #endregion

        #region ChooseFromList
        private void ChooseFromList()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0)
                {
                    DataTable dt_Item = Form.DataSources.DataTables.Item("dt_Item");
                    if (ItemEventInfo.Row + 1 < dt_Item.Rows.Count)
                    {
                        dt_Item.SetValue("Imposto", ItemEventInfo.Row, oDataTable.GetValue("Code", 0).ToString());
                        string sqlImposto = String.Format(SQL.Imposto_GetTaxas, $"'{oDataTable.GetValue("Code", 0).ToString()}'");
                        List<ImpostoModel> impostoList = new CrudController().FillModelListAccordingToSql<ImpostoModel>(sqlImposto);
                        ImpostoModel impostoModel = impostoList.FirstOrDefault(m => m.TipoImposto == "II");
                        if (impostoModel != null)
                        {
                            dt_Item.SetValue("P_II", ItemEventInfo.Row, impostoModel.Taxa / 100);
                        }
                        else
                        {
                            dt_Item.SetValue("P_II", ItemEventInfo.Row, 0);
                        }

                        impostoModel = impostoList.FirstOrDefault(m => m.TipoImposto == "PIS-IMP");
                        if (impostoModel != null)
                        {
                            dt_Item.SetValue("P_PIS", ItemEventInfo.Row, impostoModel.Taxa / 100);
                        }
                        else
                        {
                            dt_Item.SetValue("P_PIS", ItemEventInfo.Row, 0);
                        }

                        impostoModel = impostoList.FirstOrDefault(m => m.TipoImposto == "COFINS-IMP");
                        if (impostoModel != null)
                        {
                            dt_Item.SetValue("P_COFINS", ItemEventInfo.Row, impostoModel.Taxa / 100);
                        }
                        else
                        {
                            dt_Item.SetValue("P_COFINS", ItemEventInfo.Row, 0);
                        }

                        impostoModel = impostoList.FirstOrDefault(m => m.TipoImposto == "IPI-IMP");
                        if (impostoModel != null)
                        {
                            dt_Item.SetValue("P_IPI", ItemEventInfo.Row, impostoModel.Taxa / 100);
                        }
                        else
                        {
                            dt_Item.SetValue("P_IPI", ItemEventInfo.Row, 0);
                        }

                        impostoModel = impostoList.FirstOrDefault(m => m.TipoImposto == "ICMS-IMP");
                        if (impostoModel != null)
                        {
                            if (impostoModel.Taxa != 0)
                            {
                                dt_Item.SetValue("P_ICMS", ItemEventInfo.Row, impostoModel.Taxa / 100);
                            }
                            else
                            {
                                dt_Item.SetValue("P_ICMS", ItemEventInfo.Row, 1);
                            }
                            dt_Item.SetValue("P_BC_ICMS", ItemEventInfo.Row, impostoModel.BaseCalculo / 100);
                        }
                        else
                        {
                            dt_Item.SetValue("P_ICMS", ItemEventInfo.Row, 1);
                            dt_Item.SetValue("P_BC_ICMS", ItemEventInfo.Row, 1);
                        }
                        this.CalculaValores();
                    }
                }
            }
        }
        #endregion
    }
}
