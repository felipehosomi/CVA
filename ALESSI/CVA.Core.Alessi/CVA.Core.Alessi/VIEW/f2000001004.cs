using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.Alessi.DAO.Resources;
using CVA.Core.Alessi.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class f2000001004 : BaseForm
    {
        private Form Form;
        public SAPbobsCOM.Documents oDocuments;


        #region Constructor

        public f2000001004()
        {
            FormCount++;
        }

        public f2000001004(ItemEvent itemEvent)
        {

            this.ItemEventInfo = itemEvent;
        }

        public f2000001004(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001004(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        #endregion

        public override bool ItemEvent()
        {
            if (ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_LOAD)
                {
                    

                    Form = SBOApp.Application.Forms.ActiveForm;

                    if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                    {
                        string cardcode = string.Empty;

                        // -> Coluna Ultima Compra
                        if (ItemEventInfo.ItemUID == "btn_UCNF")
                        {
                            var NfCompra = (EditText)Form.Items.Item("tx_UCNF").Specific;

                            var docentry = GetDocentry(NfCompra.Value);

                            SBOApp.Application.OpenForm(BoFormObjectEnum.fo_Invoice, "", docentry);
                        }
                        // -> Coluna Maoir Compra
                        if (ItemEventInfo.ItemUID == "btn_MCNT")
                        {
                            var NfCompra = (EditText)Form.Items.Item("tx_MC_nota").Specific;

                            var docentry = GetDocentry(NfCompra.Value);

                            SBOApp.Application.OpenForm(BoFormObjectEnum.fo_Invoice, "", docentry);
                        }

                        #region Aberto

                        // -> Coluna Em Aberto
                        if (ItemEventInfo.ItemUID == "btn_A_A")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1005").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1005").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Aberto / Aberto";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresAberto(cardcode);
                            }
                            else
                            {
                                GetValoresAbertoGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1005").Enabled = false;
                        }

                        // -> Coluna Em Carteira
                        if (ItemEventInfo.ItemUID == "btn_A_C")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1005").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1005").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Aberto / Em Carteira";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresEmCarteira(cardcode);
                            }
                            else
                            {
                                GetValoresEmCarteiraGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1005").Enabled = false;
                        }

                        // -> Coluna Boleto
                        if (ItemEventInfo.ItemUID == "btn_A_B")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1005").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1005").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Aberto / Boleto";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresBoleto(cardcode);
                            }
                            else
                            {
                                GetValoresBoletoGrupo(cardcode);
                            }

                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1005").Enabled = false;
                        }

                        // -> Coluna Cheque
                        if (ItemEventInfo.ItemUID == "btn_A_CQ")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1005").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1005").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Aberto / Cheque";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresCheque(cardcode);
                            }
                            else
                            {
                                GetValoresChequeGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1005").Enabled = false;
                        }

                        // -> Coluna Atrados
                        if (ItemEventInfo.ItemUID == "btn_A_At")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1005").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1005").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Aberto / Atrasados";


                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresAtrsados(cardcode);
                            }
                            else
                            {
                                GetValoresAtrasadosGrupo(cardcode);
                            }
                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1005").Enabled = false;
                        }

                        // -> Coluna Total
                        if (ItemEventInfo.ItemUID == "btn_A_T")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1005").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1005").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Aberto / Total";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresTotal(cardcode);
                            }
                            else
                            {
                                GetValoresTotalGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1005").Enabled = false;
                        }

                        #endregion

                        #region Liquidados

                        // -> Coluna Em Aberto
                        if (ItemEventInfo.ItemUID == "btn_L_A")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1006").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1006").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Em Liquidados / Em Aberto";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresLiquidadosAberto(cardcode);
                            }
                            else
                            {
                                GetValoresLiquidadosAbertoGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1006").Enabled = false;
                        }

                        // -> Coluna Em Carteira
                        if (ItemEventInfo.ItemUID == "btn_L_C")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1006").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1006").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Liquidados / Em Carteira";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresLiquidadosEmCarteira(cardcode);
                            }
                            else
                            {
                                GetValoresLiquidadosEmCarteiraGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1006").Enabled = false;
                        }

                        // -> Coluna Boleto
                        if (ItemEventInfo.ItemUID == "btn_L_B")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1006").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1006").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Liquidados / Boleto";
                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresLiquidadosBoleto(cardcode);
                            }
                            else
                            {
                                GetValoresLiquidadosBoletoGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1006").Enabled = false;
                        }

                        // -> Coluna Cheque
                        if (ItemEventInfo.ItemUID == "btn_L_CQ")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1006").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1006").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Liquidados / Cheque";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresLiquidadosCheque(cardcode);
                            }
                            else
                            {
                                GetValoresLiquidadosChequeGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1006").Enabled = false;
                        }

                        // -> Coluna Atrados
                        if (ItemEventInfo.ItemUID == "btn_L_At")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1006").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1006").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Liquidados / Atrasados";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresLiquidadosAtrasados(cardcode);
                            }
                            else
                            {
                                GetValoresLiquidadosAtrasadosGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1006").Enabled = false;
                        }

                        // -> Coluna Total
                        if (ItemEventInfo.ItemUID == "btn_L_T")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1006").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1006").Activate();

                             cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Coluna Liquidados / Total";

                            if (cardcode.StartsWith("C"))
                            {
                                GetValoresLiquidadosTotal(cardcode);
                            }
                            else
                            {
                                GetValoresLiquidadosTotalGrupo(cardcode);
                            }


                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1006").Enabled = false;
                        }

                        #endregion

                        #region Atividades PN
                        if (ItemEventInfo.ItemUID == "btn_AtvPN")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1007").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1007").Activate();

                            cardcode = ((EditText)Form.Items.Item("tx_code").Specific).Value;

                            Form = SBOApp.Application.Forms.ActiveForm;
                            Form.Title = "Relatório Atividades Parceiro de Negócio";

                            if (cardcode.StartsWith("C"))
                            {
                                GetAtividadesPN(cardcode);
                            }

                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1007").Enabled = false;
                        }
                        #endregion
                    }
                }
            }
            return true;
        }


        #region Relatório Analitico PN

        #region Aberto

        private string GetDocentry(string serial)
        {
            SAPbobsCOM.Recordset rst = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rst.DoQuery(String.Format(Query.GetDocEntry, serial));

            string docentry = "";

            if (!rst.EoF)
            {
                docentry = rst.Fields.Item(0).Value.ToString();
                rst.MoveNext();
            }

            return docentry;
        }

        private void GetValoresAberto(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValorAberto, cardcode));
        }

        private void GetValoresEmCarteira(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValorEmCarteira, cardcode));
        }

        private void GetValoresBoleto(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValorBoleto, cardcode));
        }

        private void GetValoresCheque(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValorCheque, cardcode));
        }

        private void GetValoresAtrsados(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValorAtrsado, cardcode));
        }

        private void GetValoresTotal(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValorTotal, cardcode));
        }

        #endregion

        #region Liquidados

        private void GetValoresLiquidadosAberto(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosAberto, cardcode));
        }

        private void GetValoresLiquidadosEmCarteira(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosEmCarteira, cardcode));
        }

        private void GetValoresLiquidadosBoleto(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosBoleto, cardcode));
        }

        private void GetValoresLiquidadosCheque(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosCheque, cardcode));
        }

        private void GetValoresLiquidadosAtrasados(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosAtrasados, cardcode));
        }

        private void GetValoresLiquidadosTotal(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosTotal, cardcode));
        }



        #endregion

        #endregion

        #region Relatório Analitico Grupo

        #region Aberto       

        private void GetValoresAbertoGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresAbertoGrupo, cardcode));
        }

        private void GetValoresEmCarteiraGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresEmcarteiraGrupo, cardcode));
        }

        private void GetValoresBoletoGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresBoletoGrupo, cardcode));
        }

        private void GetValoresChequeGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresChequeGrupo, cardcode));
        }

        private void GetValoresAtrasadosGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresAtrasadosGrupo, cardcode));
        }

        private void GetValoresTotalGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridAberto").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresTotalGrupo, cardcode));
        }

        #endregion

        #region Liquidados

        private void GetValoresLiquidadosAbertoGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosAbertoGrupo, cardcode));
        }

        private void GetValoresLiquidadosEmCarteiraGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosEmCarteiraGrupo, cardcode));
        }

        private void GetValoresLiquidadosBoletoGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosBoletoGrupo, cardcode));
        }

        private void GetValoresLiquidadosChequeGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosChequeGrupo, cardcode));
        }

        private void GetValoresLiquidadosAtrasadosGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosAtrasadosGrupo, cardcode));
        }

        private void GetValoresLiquidadosTotalGrupo(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridLiqu").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetValoresLiquidadosTotalGrupo, cardcode));
        }



        #endregion

        #endregion

        private void GetAtividadesPN(string cardcode)
        {
            var oGrid = (Grid)Form.Items.Item("gridATV").Specific;
            oGrid.DataTable.ExecuteQuery(String.Format(Query.GetAtividadesPN, cardcode));
        }
    }
}
