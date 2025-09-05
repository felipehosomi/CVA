using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Globalization;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Custo de importação
    /// </summary>
    public class f2000003042 : BaseForm
    {
        Form Form;

        #region Constructor
        public f2000003042()
        {
            FormCount++;
        }

        public f2000003042(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003042(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003042(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show(string nfproduto)
        {
            Form = (Form)this.Show();
            Form.DataSources.UserDataSources.Item("ud_NF").Value = nfproduto;
            return Form;
        }

        public override object Show()
        {
            Form = (Form)base.Show();

            var cf_NF = Form.ChooseFromLists.Item("cf_NF");
            Conditions pConditions = cf_NF.GetConditions();
            cf_NF.SetConditions(pConditions);
            pConditions = cf_NF.GetConditions();
            Condition condition = pConditions.Add();
            condition.Alias = "CANCELED";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "N";
            cf_NF.SetConditions(pConditions);

            var cf_CTe = Form.ChooseFromLists.Item("cf_CTe");
            pConditions = cf_CTe.GetConditions();
            cf_CTe.SetConditions(pConditions);
            pConditions = cf_CTe.GetConditions();
            condition = pConditions.Add();
            condition.Alias = "CANCELED";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "N";
            cf_CTe.SetConditions(pConditions);

            var cf_Arm = Form.ChooseFromLists.Item("cf_Arm");
            pConditions = cf_Arm.GetConditions();
            cf_Arm.SetConditions(pConditions);
            pConditions = cf_Arm.GetConditions();
            condition = pConditions.Add();
            condition.Alias = "CANCELED";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "N";
            cf_Arm.SetConditions(pConditions);

            var cf_Ser = Form.ChooseFromLists.Item("cf_Ser");
            pConditions = cf_Ser.GetConditions();
            cf_Ser.SetConditions(pConditions);
            pConditions = cf_Ser.GetConditions();
            condition = pConditions.Add();
            condition.Alias = "CANCELED";
            condition.Operation = BoConditionOperation.co_EQUAL;
            condition.CondVal = "N";
            cf_Ser.SetConditions(pConditions);

            return Form;
        }

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Add")
                    {
                        try
                        {
                            #region validações

                            Form.Freeze(true);

                            //Produto
                            int docNumNF;
                            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_NF").Value, out docNumNF);

                            if (docNumNF == 0)
                            {
                                SBOApp.Application.MessageBox("Informe a Nota Fiscal do Produto!");
                                SBOApp.Application.SetStatusBarMessage("Informe a Nota Fiscal do Produto!");
                                return false;
                            }

                            Recordset recordSetNFE = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                            recordSetNFE.DoQuery(String.Format(SQL.VericarTipoNF, docNumNF));
                            var modeleloNFE = recordSetNFE.Fields.Item("Model").Value.ToString().Trim();

                            if (!modeleloNFE.Equals("39"))
                            {
                                SBOApp.Application.MessageBox($"Nota Fiscal do Produto:{docNumNF} tem que ser modelo NFe(55)");
                                SBOApp.Application.SetStatusBarMessage($"Nota Fiscal do Produto:{docNumNF} tem que ser modelo NFe(55)");
                                return false;
                            }

                            //Notas de despesas
                            int docNumCTe;
                            int docNumArmazenagem;
                            int docNumServicoImportacao;
                            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_CTe").Value, out docNumCTe);
                            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_NFAr").Value, out docNumArmazenagem);
                            Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_NFSe").Value, out docNumServicoImportacao);

                            if (docNumCTe == 0 && docNumArmazenagem == 0 && docNumServicoImportacao == 0)
                            {
                                SBOApp.Application.MessageBox("Informe pelo menos uma Nota Fiscal de Entrada de despesa!");
                                SBOApp.Application.SetStatusBarMessage("Informe pelo menos uma Nota Fiscal de Entrada de despesa!");
                                return false;
                            }

                            //NF de transporte
                            if (docNumCTe > 0)
                            {
                                recordSetNFE.DoQuery(String.Format(SQL.VericarTipoNF, docNumCTe));
                                var modeloCte = recordSetNFE.Fields.Item("Model").Value.ToString().Trim();

                                if (!modeloCte.Equals("44"))
                                {
                                    SBOApp.Application.MessageBox($"Nota Fiscal Transporte:{docNumCTe} tem que ser modelo CT-e");
                                    SBOApp.Application.SetStatusBarMessage($"Nota Fiscal Transporte:{docNumCTe} tem que ser modelo CT-e");
                                    return false;
                                }
                            }

                            //NF de Armazenagem
                            if (docNumArmazenagem > 0)
                            {
                                recordSetNFE.DoQuery(String.Format(SQL.VericarTipoNF, docNumArmazenagem));
                                var modeloArmazenagem = recordSetNFE.Fields.Item("Model").Value.ToString().Trim();

                                if (!modeloArmazenagem.Equals("46"))
                                {
                                    SBOApp.Application.MessageBox($"Nota Fiscal de Serviço de Armazenagem:{docNumArmazenagem} tem que ser modelo serviço NFS-e");
                                    SBOApp.Application.SetStatusBarMessage($"Nota Fiscal de Serviço de Armazenagem:{docNumArmazenagem} tem que ser modelo serviço NFS-e");
                                    return false;
                                }
                            }

                            //NF de Serviço Ass. de Importação
                            if (docNumServicoImportacao > 0)
                            {
                                recordSetNFE.DoQuery(String.Format(SQL.VericarTipoNF, docNumServicoImportacao));
                                var modeloServicoImportacao = recordSetNFE.Fields.Item("Model").Value.ToString().Trim();

                                if (!modeloServicoImportacao.Equals("46"))
                                {
                                    SBOApp.Application.MessageBox($"Nota Fiscal de Serviço de Ass. de Importação:{docNumServicoImportacao} tem que ser modelo serviço NFS-e");
                                    SBOApp.Application.SetStatusBarMessage($"Nota Fiscal de Serviço de Ass. de Importação:{docNumServicoImportacao} tem que ser modelo serviço NFS-e");
                                    return false;
                                }
                            }

                            //Data da despesa
                            DateTime dataDespesaImportacao;
                            if (!DateTime.TryParseExact(Form.DataSources.UserDataSources.Item("ud_Data").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataDespesaImportacao))
                            {
                                SBOApp.Application.SetStatusBarMessage("Informe a Data da Despesa!");
                                return false;
                            }

                            #endregion

                            var mensagemRetorno = DespesaImportacaoBLL.Adicionar(docNumNF, docNumCTe, docNumArmazenagem, docNumServicoImportacao, dataDespesaImportacao);

                            if (mensagemRetorno.erro)
                            {
                                SBOApp.Application.SetStatusBarMessage(mensagemRetorno.mensagemErro);
                                SBOApp.Application.MessageBox($"{mensagemRetorno.mensagemErro}");
                            }
                            else
                            {
                                SBOApp.Application.SetStatusBarMessage($"Despesa de Importação:{mensagemRetorno.DocNum} gerada com sucesso.");
                                SBOApp.Application.MessageBox($"Despesa de Importação: {mensagemRetorno.DocNum} gerada com sucesso!");
                                Form.DataSources.UserDataSources.Item("ud_NF").Value = String.Empty;
                                Form.DataSources.UserDataSources.Item("ud_CTe").Value = String.Empty;
                                Form.DataSources.UserDataSources.Item("ud_Data").Value = String.Empty;
                                Form.DataSources.UserDataSources.Item("ud_NFAr").Value = String.Empty;
                                Form.DataSources.UserDataSources.Item("ud_NFSe").Value = String.Empty;
                            }
                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            Form.Freeze(false);
                        }
                    }
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    IChooseFromListEvent oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
                    var oCFL = Form.ChooseFromLists.Item(oCFLEvento.ChooseFromListUID);
                    DataTable oDataTable = oCFLEvento.SelectedObjects;

                    if (oDataTable != null)
                    {
                        string udID = ItemEventInfo.ItemUID.Replace("et_", "ud_");
                        string docNum = oDataTable.GetValue("DocNum", 0).ToString();

                        if (oDataTable.Rows.Count > 0)
                        {
                            Form.DataSources.UserDataSources.Item(udID).Value = docNum;
                        }
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
