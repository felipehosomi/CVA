using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.Alessi.DAO.Resources;
using CVA.Core.Alessi.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.Core.Alessi.VIEW
{
    public class f134 : BaseForm
    {
        private Form Form;
        private Form Form_Item;

        public static ButtonCombo BtCombo;
        //public static string cardcode;
        //public static string cardname;
        //public static string groupcode;
        //public static string cardType;

      

        #region Constructor

        public f134()
        {
            FormCount++;
        }

        public f134(ItemEvent itemEvent)
        {

            this.ItemEventInfo = itemEvent;
        }

        public f134(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f134(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {

                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);

                    var item = Form.Items.Add("btn_ResFi", BoFormItemTypes.it_BUTTON_COMBO);
                    item.Left = 145;
                    item.Top = 553;
                    item.Width = 126;
                    item.Height = 20;
                    item.DisplayDesc = true;

                    BtCombo = (ButtonCombo)item.Specific;
                    BtCombo.Caption = "Res Financeiro";
                    BtCombo.ValidValues.Add("1", "Grupo Economico");
                    BtCombo.ValidValues.Add("2", "Cliente / Fornecedor");
                    return true;

                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "btn_ResFi")
                {                    
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);

                    SBOApp.Application.SetStatusBarMessage("Carregando Informações Aguarde....", BoMessageTime.bmt_Short, false);
                    BtCombo = (ButtonCombo)Form.Items.Item("btn_ResFi").Specific;

                    DBDataSource ds_GC = Form.DataSources.DBDataSources.Item("OCRD");
                    string groupcode = ds_GC.GetValue("U_CVA_GRUPOECON", ds_GC.Offset);
                    

                    DBDataSource ds_PN = Form.DataSources.DBDataSources.Item("OCRD");
                    string cardcode = ds_PN.GetValue("CardCode", ds_PN.Offset);
                    string cardname = ds_PN.GetValue("CardName", ds_PN.Offset);
                    string cardType = ds_PN.GetValue("CardType", ds_PN.Offset);

                    if (cardType == "C")
                    {
                        
                        if (BtCombo.Selected.Value == "1" && cardType == "C")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1004").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1004").Activate();

                            //Pega o formulario Ativo e grava as Informaçoes do Grupo Economico
                            Form_Item = SBOApp.Application.Forms.ActiveForm;
                            Form_Item.Visible = false;
                            Form_Item.Title = "Resumo Financeiro Parceiro de Negócio (Grupo Econômico)";
                            Form_Item.Items.Item("Lbl_PN").Visible = false;
                            Form_Item.Items.Item("Lbl_Grupo").Visible = true;
                            Form_Item.Items.Item("Item_112").Visible = false;
                            Form_Item.Items.Item("btn_AtvPN").Visible = false;


                            #region Dados Grupo

                            var code = (EditText)Form_Item.Items.Item("tx_code").Specific;
                            var name = (EditText)Form_Item.Items.Item("tx_name").Specific;

                            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oRecordSet.DoQuery(String.Format(Query.PegaDescricaoGrupo, groupcode));

                            if (!oRecordSet.EoF)
                            {
                                code.Value = oRecordSet.Fields.Item(0).Value.ToString();
                                name.Value = oRecordSet.Fields.Item(1).Value.ToString();

                            }

                            #endregion  

                            #region Coluna Limite Grupo

                            ColunaLimteGrupo(groupcode);

                            #endregion

                            #region Dados da Maior Compra

                            MaiorCompraGrupo(groupcode);

                            #endregion

                            #region Dados da Ultima Compra

                            UltimaCompraGrupo(groupcode);

                            #endregion

                            #region Coluna Aberto

                            ColunaAbertoGrupo(groupcode);

                            #endregion
                            
                            #region Acumulo

                            GetAcumuloGrupo(groupcode);

                            #endregion

                            #region Coluna Atrasados

                            GetAtrasadosGrupo(groupcode);
                            #endregion

                            #region Coluna liquidados

                            ColunaLiquidadosGrupo(groupcode);

                            #endregion

                            #region Juros

                            GetJurosGrupo();
                            #endregion

                            #region CadastroPN

                            GetDataCadastroGrupo(groupcode);

                            #endregion

                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1004").Enabled = false;
                            Form_Item.Visible = true;
                        }
                        if (BtCombo.Selected.Value == "2" && cardType == "C")
                        {
                            //Ativa o Menu para abrir a Tela
                            SBOApp.Application.Menus.Item("M1004").Enabled = true;

                            //Abre a tela
                            SBOApp.Application.Menus.Item("M1004").Activate();

                            //Pega o formulario Ativo e grava as Informaçoes do PN
                            Form_Item = SBOApp.Application.Forms.ActiveForm;
                            Form_Item.Visible = false;
                            Form_Item.Title = "Resumo Financeiro Parceiro de Negócio (Cliente)";

                            #region Dados do PN
                            Form_Item.Items.Item("Lbl_Grupo").Visible = false;
                            Form_Item.Items.Item("Lbl_PN").Visible = true;                                                      

                            var code = (EditText)Form_Item.Items.Item("tx_code").Specific;
                            var name = (EditText)Form_Item.Items.Item("tx_name").Specific;

                            code.Value = cardcode;
                            name.Value = cardname;

                            #endregion

                            #region Dados da Ultima Compra

                            UltimaCompra(cardcode, cardType);

                            #endregion

                            #region Dados da Maior Compra

                            MaiorCompra(cardcode, cardType);

                            #endregion

                            #region Coluna Limite

                            ColunaLimte(cardcode, cardType);

                            #endregion

                            #region Coluna Aberto

                            ColunaAberto(cardcode, cardType);

                            #endregion

                            #region Coluna liquidados

                            ColunaLiquidados(cardcode, cardType);

                            #endregion

                            #region Juros

                            GetJuros();
                            #endregion

                            #region CadastroPN

                            GetDataCadastroPN(cardcode);

                            #endregion

                            #region Acumulo

                            GetAcumulo(cardcode,cardType);

                            #endregion

                            #region Coluna Atrasados

                            GetAtrasados(cardcode,cardType);
                            #endregion

                            // Desabilita o menu, para não fica visivel.
                            SBOApp.Application.Menus.Item("M1004").Enabled = false;
                            Form_Item.Visible = true;

                        }
                        BtCombo.Caption = "Res Financeiro";                       
                    }
                    else
                    {                        
                        SBOApp.Application.SetStatusBarMessage("Funcionalidade apenas para clientes...", BoMessageTime.bmt_Short, true);
                    }                    
                }
            }            
            return true;
       }

        #region Metodos PN
        
        private void MaiorCompra(string cardcode, string cardType)
        {
            SAPbobsCOM.Recordset rst = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rst.DoQuery(String.Format(Query.PegaDadosMaiorCompra, cardcode, cardType));

            var ListaMaiorCompra = new List<MaiorCompraModel>();

            if (!rst.EoF)
            {
                var model = new MaiorCompraModel();

                var aux1 = Convert.ToDateTime(rst.Fields.Item(0).Value);
                model.Valor = string.Format("{0:0,0.00}", rst.Fields.Item(1).Value);
                model.Nota_Fiscal = rst.Fields.Item(2).Value.ToString();


                String dia, mes;

                if (aux1.Day < 10)
                    dia = "0" + aux1.Day;
                else
                    dia = aux1.Day.ToString();

                if (aux1.Month < 10)
                    mes = "0" + aux1.Month;
                else
                    mes = aux1.Month.ToString();


                var data = dia + "/" + mes + "/" + aux1.Year;


                model.Data = data;


                ListaMaiorCompra.Add(model);
                rst.MoveNext();
            }

            var NF = (EditText)Form_Item.Items.Item("tx_MC_nota").Specific;
            var valor = (EditText)Form_Item.Items.Item("tx_MC_vlr").Specific;
            var Dt = (EditText)Form_Item.Items.Item("tx_MC_data").Specific;


            foreach (var item in ListaMaiorCompra)
            {
                Dt.Value = item.Data.ToString();
                valor.Value = item.Valor.ToString();
                NF.Value = item.Nota_Fiscal.ToString();
            }
        }

        private void UltimaCompra(string cardcode, string cardType)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.PegaDadosUltimaCompra, cardcode, cardType));

            var ListaUltimaCompra = new List<UltimaCompraModel>();

            if (!oRecordSet.EoF)
            {
                var model = new UltimaCompraModel();

                model.Nota_Fiscal = oRecordSet.Fields.Item(0).Value.ToString();
                model.Valor = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);
                var aux = Convert.ToDateTime(oRecordSet.Fields.Item(2).Value);

                String dia, mes;

                if (aux.Day < 10)
                    dia = "0" + aux.Day;
                else
                    dia = aux.Day.ToString();

                if (aux.Month < 10)
                    mes = "0" + aux.Month;
                else
                    mes = aux.Month.ToString();


                var data = dia + "/" + mes + "/" + aux.Year;


                model.Data = data;
                var ts = DateTime.Today - aux;
                model.Dias_Sem_Mov = ts.Days.ToString();

                ListaUltimaCompra.Add(model);
                oRecordSet.MoveNext();
            }

            var Nota_fiscal = (EditText)Form_Item.Items.Item("tx_UCNF").Specific;
            var Valor = (EditText)Form_Item.Items.Item("tx_UCval").Specific;
            var Data = (EditText)Form_Item.Items.Item("tx_UCData").Specific;
            var Dias_Sem_Mov = (EditText)Form_Item.Items.Item("tx_UCDia").Specific;

            foreach (var item in ListaUltimaCompra)
            {
                Data.Value = item.Data.ToString();
                Valor.Value = item.Valor.ToString();
                Nota_fiscal.Value = item.Nota_Fiscal.ToString();
                Dias_Sem_Mov.Value = item.Dias_Sem_Mov.ToString();
            }
        }

        private void ColunaLimte(string cardcode, string cardType)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.PegaDadosColunaLimite, cardcode, cardType));

            var ListaColunaLimite = new List<ColunaLimiteModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaLimiteModel();

                model.Limite = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(0).Value);
                model.Disponivel = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);
                model.Credito = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(3).Value);
                var aux = Convert.ToDateTime(oRecordSet.Fields.Item(2).Value);

                String dia, mes;

                if (aux.Day < 10)
                    dia = "0" + aux.Day;
                else
                    dia = aux.Day.ToString();

                if (aux.Month < 10)
                    mes = "0" + aux.Month;
                else
                    mes = aux.Month.ToString();


                var data = dia + "/" + mes + "/" + aux.Year;


                model.Validade = data;



                ListaColunaLimite.Add(model);
                oRecordSet.MoveNext();
            }

            var Limite = (EditText)Form_Item.Items.Item("tx_limite").Specific;
            var Disponivel = (EditText)Form_Item.Items.Item("tx_disp").Specific;
            var Credito = (EditText)Form_Item.Items.Item("tx_cred").Specific;
            var Validade = (EditText)Form_Item.Items.Item("tx_val").Specific;


            foreach (var item in ListaColunaLimite)
            {
                Limite.Value = item.Limite.ToString();
                Disponivel.Value = item.Disponivel.ToString();
                Credito.Value = item.Credito.ToString();
                Validade.Value = item.Validade.ToString();

            }
        }

        private void ColunaAberto(string cardcode, string cardType)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.PegaDadosColunaAberto, cardcode, cardType));

            var ListaColunaAberto = new List<ColunaAbertoModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaAbertoModel();

                model.QtdeAberto = Convert.ToInt32(oRecordSet.Fields.Item(0).Value);
                model.ValorAberto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);

                model.QtdeEmCarteira = Convert.ToInt32(oRecordSet.Fields.Item(2).Value);
                model.ValorEmCarteira = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(3).Value);

                model.QtdeBoleto = Convert.ToInt32(oRecordSet.Fields.Item(4).Value);
                model.ValorBoleto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(5).Value);

                model.QtdeCheque = Convert.ToInt32(oRecordSet.Fields.Item(6).Value);
                model.ValorCheque = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(7).Value);

                model.QtdeAtrasado = Convert.ToInt32(oRecordSet.Fields.Item(8).Value);
                model.ValorAtrasado = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(9).Value);

                model.QtdeTotal = Convert.ToInt32(oRecordSet.Fields.Item(10).Value);
                model.ValorTotal = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(11).Value);

                


                ListaColunaAberto.Add(model);
                oRecordSet.MoveNext();
            }

            var QtdeAberto = (EditText)Form_Item.Items.Item("txA_qtdA").Specific;
            var ValorAberto = (EditText)Form_Item.Items.Item("txA_vlA").Specific;

            var QtdeEmCarteira = (EditText)Form_Item.Items.Item("txA_qtdC").Specific;
            var ValorEmCarteira = (EditText)Form_Item.Items.Item("txA_vlC").Specific;

            var QtdeBoleto = (EditText)Form_Item.Items.Item("txA_qtdB").Specific;
            var ValorBoleto = (EditText)Form_Item.Items.Item("txA_vlB").Specific;

            var QtdeCheque = (EditText)Form_Item.Items.Item("txA_qtdCQ").Specific;
            var ValorCheque = (EditText)Form_Item.Items.Item("txA_vlCQ").Specific;

            var QtdeTotal = (EditText)Form_Item.Items.Item("txA_qtdT").Specific;
            var ValorTotal = (EditText)Form_Item.Items.Item("txA_vlT").Specific;

            var QtdeAtrasado = (EditText)Form_Item.Items.Item("txA_qtdAt").Specific;
            var ValorAtrasado = (EditText)Form_Item.Items.Item("txA_vlAt").Specific;

            foreach (var item in ListaColunaAberto)
            {
                QtdeAberto.Value = item.QtdeAberto.ToString();
                ValorAberto.Value = item.ValorAberto.ToString();

                QtdeEmCarteira.Value = item.QtdeEmCarteira.ToString();
                ValorEmCarteira.Value = item.ValorEmCarteira.ToString();


                QtdeBoleto.Value = item.QtdeBoleto.ToString();
                ValorBoleto.Value = item.ValorBoleto.ToString();

                QtdeCheque.Value = item.QtdeCheque.ToString();
                ValorCheque.Value = item.ValorCheque.ToString();

                QtdeAtrasado.Value = item.QtdeAtrasado.ToString();
                ValorAtrasado.Value = item.ValorAtrasado.ToString();

                QtdeTotal.Value = item.QtdeTotal.ToString();
                ValorTotal.Value = item.ValorTotal.ToString();              

            }
        }

        private void ColunaLiquidados(string cardcode, string cardType)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.PegaDadosColunaLiquidados, cardcode, cardType));

            var ListaColunaLiquidados = new List<ColunaLuquidadosModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaLuquidadosModel();

                model.QtdeAberto = Convert.ToInt32(oRecordSet.Fields.Item(0).Value);
                model.ValorAberto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);

                model.QtdeEmCarteira = Convert.ToInt32(oRecordSet.Fields.Item(2).Value);
                model.ValorEmCarteira = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(3).Value);

                model.QtdeBoleto = Convert.ToInt32(oRecordSet.Fields.Item(4).Value);
                model.ValorBoleto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(5).Value);

                model.QtdeCheque = Convert.ToInt32(oRecordSet.Fields.Item(6).Value);
                model.ValorCheque = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(7).Value);

                model.QtdeTotal = Convert.ToInt32(oRecordSet.Fields.Item(8).Value);
                model.ValorTotal = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(9).Value);

                model.QtdeAtrasado = Convert.ToInt32(oRecordSet.Fields.Item(10).Value);
                model.ValorAtrasado = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(11).Value);


                ListaColunaLiquidados.Add(model);
                oRecordSet.MoveNext();
            }

            var QtdeAberto = (EditText)Form_Item.Items.Item("txL_qtdA").Specific;
            var ValorAberto = (EditText)Form_Item.Items.Item("tx_L_vl_A").Specific;

            var QtdeEmCarteira = (EditText)Form_Item.Items.Item("txL_qtdC").Specific;
            var ValorEmCarteira = (EditText)Form_Item.Items.Item("tx_L_vl_C").Specific;

            var QtdeBoleto = (EditText)Form_Item.Items.Item("txL_qtdB").Specific;
            var ValorBoleto = (EditText)Form_Item.Items.Item("tx_L_vl_B").Specific;

            var QtdeCheque = (EditText)Form_Item.Items.Item("txL_qtdCq").Specific;
            var ValorCheque = (EditText)Form_Item.Items.Item("txL_vlCQ").Specific;

            var QtdeTotal = (EditText)Form_Item.Items.Item("txL_qtdT").Specific;
            var ValorTotal = (EditText)Form_Item.Items.Item("txL_vlT").Specific;

            var QtdeAtrasado = (EditText)Form_Item.Items.Item("txL_qtdAt").Specific;
            var ValorAtrasado = (EditText)Form_Item.Items.Item("txL_vlAt").Specific;



            foreach (var item in ListaColunaLiquidados)
            {
                QtdeAberto.Value = item.QtdeAberto.ToString();
                ValorAberto.Value = item.ValorAberto.ToString();

                QtdeEmCarteira.Value = item.QtdeEmCarteira.ToString();
                ValorEmCarteira.Value = item.ValorEmCarteira.ToString();


                QtdeBoleto.Value = item.QtdeBoleto.ToString();
                ValorBoleto.Value = item.ValorBoleto.ToString();

                QtdeCheque.Value = item.QtdeCheque.ToString();
                ValorCheque.Value = item.ValorCheque.ToString();

                QtdeTotal.Value = item.QtdeTotal.ToString();
                ValorTotal.Value = item.ValorTotal.ToString();

                QtdeAtrasado.Value = item.QtdeAtrasado.ToString();
                ValorAtrasado.Value = item.ValorAtrasado.ToString();

            }
        }

        private void GetDataCadastroPN(string cardcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetDataCadastroPN, cardcode));
            var data = "";

            if (!oRecordSet.EoF)
            {
                var aux1 = Convert.ToDateTime(oRecordSet.Fields.Item(0).Value);
                var DataCadastro = (EditText)Form_Item.Items.Item("tx_DtCad").Specific;

                String dia, mes;

                if (aux1.Day < 10)
                    dia = "0" + aux1.Day;
                else
                    dia = aux1.Day.ToString();

                if (aux1.Month < 10)
                    mes = "0" + aux1.Month;
                else
                    mes = aux1.Month.ToString();


                 data = dia + "/" + mes + "/" + aux1.Year;

                DataCadastro.Value = data;
                oRecordSet.MoveNext();

            }
        }
        
        private void GetJuros()
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetJuros));

            var Juros = (EditText)Form_Item.Items.Item("tx_juros").Specific;

            if (!oRecordSet.EoF)
            {
                Juros.Value = oRecordSet.Fields.Item(0).Value.ToString() + "%";
                oRecordSet.MoveNext();
            }
        }

        private void GetAcumulo( string cardcode,string cardtype)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetAcumulo, cardcode,cardtype));

            var Acumulo = (EditText)Form_Item.Items.Item("tx_Ac").Specific;

            if (!oRecordSet.EoF)
            {
                Acumulo.Value = string.Format("{0:0,0.00}",oRecordSet.Fields.Item(0).Value);   
                oRecordSet.MoveNext();
            }
        }

        private void GetAtrasados(string cardcode, string cardtype)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.PegaDadosAtrasados, cardcode, cardtype));

            var ListaColunaAtrasados = new List<ColunaAtrasadosModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaAtrasadosModel();

                model.Media = oRecordSet.Fields.Item(0).Value.ToString();

                model.Atual = oRecordSet.Fields.Item(1).Value.ToString();

                model.Maior = oRecordSet.Fields.Item(2).Value.ToString();

                model.Atrasados =  oRecordSet.Fields.Item(3).Value.ToString();

                ListaColunaAtrasados.Add(model);
                oRecordSet.MoveNext();
            }

            var Media = (EditText)Form_Item.Items.Item("tx_AT_med").Specific;
            var Atual = (EditText)Form_Item.Items.Item("tx_AT_M").Specific;
            var Maior = (EditText)Form_Item.Items.Item("txAT_Atual").Specific;
            var Atrasados = (EditText)Form_Item.Items.Item("tx_AT_MP").Specific;



            foreach (var item in ListaColunaAtrasados)
            {
                Media.Value = item.Media.ToString();
                Atual.Value = item.Atual.ToString();
                Maior.Value = item.Maior.ToString();
                Atrasados.Value = item.Atrasados.ToString();                
            }
        }

        //private void GetAtividadesPN (string cardcode)
        //{
        //    SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //    oRecordSet.DoQuery(String.Format(Query.GetAtividadesPN, cardcode));

        //    var AtividadesPN = (EditText)Form_Item.Items.Item("tx_Ac").Specific;

        //    if (!oRecordSet.EoF)
        //    {
        //        Acumulo.Value = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(0).Value);
        //        oRecordSet.MoveNext();
        //    }
        //}

        #endregion

        #region Metodos Grupo

        private void ColunaLimteGrupo(string groupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetLimiteGrupo, groupcode));

            var ListaColunaLimite = new List<ColunaLimiteModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaLimiteModel();

                model.Limite = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(0).Value);
                model.Disponivel = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);
                model.Credito = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(3).Value);
                var aux = Convert.ToDateTime(oRecordSet.Fields.Item(2).Value);

                String dia, mes;

                if (aux.Day < 10)
                    dia = "0" + aux.Day;
                else
                    dia = aux.Day.ToString();

                if (aux.Month < 10)
                    mes = "0" + aux.Month;
                else
                    mes = aux.Month.ToString();


                var data = dia + "/" + mes + "/" + aux.Year;


                model.Validade = data;



                ListaColunaLimite.Add(model);
                oRecordSet.MoveNext();
            }

            var Limite = (EditText)Form_Item.Items.Item("tx_limite").Specific;
            var Disponivel = (EditText)Form_Item.Items.Item("tx_disp").Specific;
            var Credito = (EditText)Form_Item.Items.Item("tx_cred").Specific;
            var Validade = (EditText)Form_Item.Items.Item("tx_val").Specific;


            foreach (var item in ListaColunaLimite)
            {
                Limite.Value = item.Limite.ToString();
                Disponivel.Value = item.Disponivel.ToString();
                Credito.Value = item.Credito.ToString();
                Validade.Value = item.Validade.ToString();

            }
        }

        private void MaiorCompraGrupo(string groupcode)
        {
            SAPbobsCOM.Recordset rst = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            rst.DoQuery(String.Format(Query.GetMaiorCompra, groupcode));

            var ListaMaiorCompra = new List<MaiorCompraModel>();

            if (!rst.EoF)
            {
                var model = new MaiorCompraModel();

                var aux1 = Convert.ToDateTime(rst.Fields.Item(0).Value);
                model.Valor = string.Format("{0:0,0.00}", rst.Fields.Item(1).Value);
                model.Nota_Fiscal = rst.Fields.Item(2).Value.ToString();


                String dia, mes;

                if (aux1.Day < 10)
                    dia = "0" + aux1.Day;
                else
                    dia = aux1.Day.ToString();

                if (aux1.Month < 10)
                    mes = "0" + aux1.Month;
                else
                    mes = aux1.Month.ToString();


                var data = dia + "/" + mes + "/" + aux1.Year;


                model.Data = data;


                ListaMaiorCompra.Add(model);
                rst.MoveNext();
            }

            var NF = (EditText)Form_Item.Items.Item("tx_MC_nota").Specific;
            var valor = (EditText)Form_Item.Items.Item("tx_MC_vlr").Specific;
            var Dt = (EditText)Form_Item.Items.Item("tx_MC_data").Specific;


            foreach (var item in ListaMaiorCompra)
            {
                Dt.Value = item.Data.ToString();
                valor.Value = item.Valor.ToString();
                NF.Value = item.Nota_Fiscal.ToString();
            }
        }

        private void UltimaCompraGrupo(string groupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetUltimaCompraGrupo, groupcode));

            var ListaUltimaCompra = new List<UltimaCompraModel>();

            if (!oRecordSet.EoF)
            {
                var model = new UltimaCompraModel();

                model.Nota_Fiscal = oRecordSet.Fields.Item(0).Value.ToString();
                model.Valor = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);
                var aux = Convert.ToDateTime(oRecordSet.Fields.Item(2).Value);

                String dia, mes;

                if (aux.Day < 10)
                    dia = "0" + aux.Day;
                else
                    dia = aux.Day.ToString();

                if (aux.Month < 10)
                    mes = "0" + aux.Month;
                else
                    mes = aux.Month.ToString();


                var data = dia + "/" + mes + "/" + aux.Year;


                model.Data = data;
                var ts = DateTime.Today - aux;
                model.Dias_Sem_Mov = ts.Days.ToString();

                ListaUltimaCompra.Add(model);
                oRecordSet.MoveNext();
            }

            var Nota_fiscal = (EditText)Form_Item.Items.Item("tx_UCNF").Specific;
            var Valor = (EditText)Form_Item.Items.Item("tx_UCval").Specific;
            var Data = (EditText)Form_Item.Items.Item("tx_UCData").Specific;
            var Dias_Sem_Mov = (EditText)Form_Item.Items.Item("tx_UCDia").Specific;

            foreach (var item in ListaUltimaCompra)
            {
                Data.Value = item.Data.ToString();
                Valor.Value = item.Valor.ToString();
                Nota_fiscal.Value = item.Nota_Fiscal.ToString();
                Dias_Sem_Mov.Value = item.Dias_Sem_Mov.ToString();
            }
        }

        private void ColunaAbertoGrupo(string groupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetAbertoGrupo, groupcode));

            var ListaColunaAberto = new List<ColunaAbertoModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaAbertoModel();

                model.QtdeAberto = Convert.ToInt32(oRecordSet.Fields.Item(0).Value);
                model.ValorAberto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);

                model.QtdeEmCarteira = Convert.ToInt32(oRecordSet.Fields.Item(2).Value);
                model.ValorEmCarteira = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(3).Value);

                model.QtdeBoleto = Convert.ToInt32(oRecordSet.Fields.Item(4).Value);
                model.ValorBoleto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(5).Value);

                model.QtdeCheque = Convert.ToInt32(oRecordSet.Fields.Item(6).Value);
                model.ValorCheque = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(7).Value);

                model.QtdeAtrasado = Convert.ToInt32(oRecordSet.Fields.Item(8).Value);
                model.ValorAtrasado = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(9).Value);

                model.QtdeTotal = Convert.ToInt32(oRecordSet.Fields.Item(10).Value);
                model.ValorTotal = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(11).Value);




                ListaColunaAberto.Add(model);
                oRecordSet.MoveNext();
            }

            var QtdeAberto = (EditText)Form_Item.Items.Item("txA_qtdA").Specific;
            var ValorAberto = (EditText)Form_Item.Items.Item("txA_vlA").Specific;

            var QtdeEmCarteira = (EditText)Form_Item.Items.Item("txA_qtdC").Specific;
            var ValorEmCarteira = (EditText)Form_Item.Items.Item("txA_vlC").Specific;

            var QtdeBoleto = (EditText)Form_Item.Items.Item("txA_qtdB").Specific;
            var ValorBoleto = (EditText)Form_Item.Items.Item("txA_vlB").Specific;

            var QtdeCheque = (EditText)Form_Item.Items.Item("txA_qtdCQ").Specific;
            var ValorCheque = (EditText)Form_Item.Items.Item("txA_vlCQ").Specific;

            var QtdeTotal = (EditText)Form_Item.Items.Item("txA_qtdT").Specific;
            var ValorTotal = (EditText)Form_Item.Items.Item("txA_vlT").Specific;

            var QtdeAtrasado = (EditText)Form_Item.Items.Item("txA_qtdAt").Specific;
            var ValorAtrasado = (EditText)Form_Item.Items.Item("txA_vlAt").Specific;

            foreach (var item in ListaColunaAberto)
            {
                QtdeAberto.Value = item.QtdeAberto.ToString();
                ValorAberto.Value = item.ValorAberto.ToString();

                QtdeEmCarteira.Value = item.QtdeEmCarteira.ToString();
                ValorEmCarteira.Value = item.ValorEmCarteira.ToString();


                QtdeBoleto.Value = item.QtdeBoleto.ToString();
                ValorBoleto.Value = item.ValorBoleto.ToString();

                QtdeCheque.Value = item.QtdeCheque.ToString();
                ValorCheque.Value = item.ValorCheque.ToString();

                QtdeAtrasado.Value = item.QtdeAtrasado.ToString();
                ValorAtrasado.Value = item.ValorAtrasado.ToString();

                QtdeTotal.Value = item.QtdeTotal.ToString();
                ValorTotal.Value = item.ValorTotal.ToString();

            }
        }

        private void GetAcumuloGrupo(string groupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetAcumuloGrupo, groupcode));

            var Acumulo = (EditText)Form_Item.Items.Item("tx_Ac").Specific;

            if (!oRecordSet.EoF)
            {
                Acumulo.Value = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(0).Value);
                oRecordSet.MoveNext();
            }
        }

        private void GetAtrasadosGrupo(string grupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetAtrasadosGrupo, grupcode));

            var ListaColunaAtrasados = new List<ColunaAtrasadosModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaAtrasadosModel();

                model.Media = oRecordSet.Fields.Item(0).Value.ToString();

                model.Atual = oRecordSet.Fields.Item(1).Value.ToString();

                model.Maior = oRecordSet.Fields.Item(2).Value.ToString();

                model.Atrasados = oRecordSet.Fields.Item(3).Value.ToString();

                ListaColunaAtrasados.Add(model);
                oRecordSet.MoveNext();
            }

            var Media = (EditText)Form_Item.Items.Item("tx_AT_med").Specific;
            var Atual = (EditText)Form_Item.Items.Item("tx_AT_M").Specific;
            var Maior = (EditText)Form_Item.Items.Item("txAT_Atual").Specific;
            var Atrasados = (EditText)Form_Item.Items.Item("tx_AT_MP").Specific;



            foreach (var item in ListaColunaAtrasados)
            {
                Media.Value = item.Media.ToString();
                Atual.Value = item.Atual.ToString();
                Maior.Value = item.Maior.ToString();
                Atrasados.Value = item.Atrasados.ToString();
            }
        }

        private void ColunaLiquidadosGrupo(string grupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetLiquidadosGrupo, grupcode));

            var ListaColunaLiquidados = new List<ColunaLuquidadosModel>();

            if (!oRecordSet.EoF)
            {
                var model = new ColunaLuquidadosModel();

                model.QtdeAberto = Convert.ToInt32(oRecordSet.Fields.Item(0).Value);
                model.ValorAberto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(1).Value);

                model.QtdeEmCarteira = Convert.ToInt32(oRecordSet.Fields.Item(2).Value);
                model.ValorEmCarteira = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(3).Value);

                model.QtdeBoleto = Convert.ToInt32(oRecordSet.Fields.Item(4).Value);
                model.ValorBoleto = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(5).Value);

                model.QtdeCheque = Convert.ToInt32(oRecordSet.Fields.Item(6).Value);
                model.ValorCheque = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(7).Value);

                model.QtdeTotal = Convert.ToInt32(oRecordSet.Fields.Item(8).Value);
                model.ValorTotal = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(9).Value);

                model.QtdeAtrasado = Convert.ToInt32(oRecordSet.Fields.Item(10).Value);
                model.ValorAtrasado = string.Format("{0:0,0.00}", oRecordSet.Fields.Item(11).Value);


                ListaColunaLiquidados.Add(model);
                oRecordSet.MoveNext();
            }

            var QtdeAberto = (EditText)Form_Item.Items.Item("txL_qtdA").Specific;
            var ValorAberto = (EditText)Form_Item.Items.Item("tx_L_vl_A").Specific;

            var QtdeEmCarteira = (EditText)Form_Item.Items.Item("txL_qtdC").Specific;
            var ValorEmCarteira = (EditText)Form_Item.Items.Item("tx_L_vl_C").Specific;

            var QtdeBoleto = (EditText)Form_Item.Items.Item("txL_qtdB").Specific;
            var ValorBoleto = (EditText)Form_Item.Items.Item("tx_L_vl_B").Specific;

            var QtdeCheque = (EditText)Form_Item.Items.Item("txL_qtdCq").Specific;
            var ValorCheque = (EditText)Form_Item.Items.Item("txL_vlCQ").Specific;

            var QtdeTotal = (EditText)Form_Item.Items.Item("txL_qtdT").Specific;
            var ValorTotal = (EditText)Form_Item.Items.Item("txL_vlT").Specific;

            var QtdeAtrasado = (EditText)Form_Item.Items.Item("txL_qtdAt").Specific;
            var ValorAtrasado = (EditText)Form_Item.Items.Item("txL_vlAt").Specific;



            foreach (var item in ListaColunaLiquidados)
            {
                QtdeAberto.Value = item.QtdeAberto.ToString();
                ValorAberto.Value = item.ValorAberto.ToString();

                QtdeEmCarteira.Value = item.QtdeEmCarteira.ToString();
                ValorEmCarteira.Value = item.ValorEmCarteira.ToString();


                QtdeBoleto.Value = item.QtdeBoleto.ToString();
                ValorBoleto.Value = item.ValorBoleto.ToString();

                QtdeCheque.Value = item.QtdeCheque.ToString();
                ValorCheque.Value = item.ValorCheque.ToString();

                QtdeTotal.Value = item.QtdeTotal.ToString();
                ValorTotal.Value = item.ValorTotal.ToString();

                QtdeAtrasado.Value = item.QtdeAtrasado.ToString();
                ValorAtrasado.Value = item.ValorAtrasado.ToString();

            }
        }

        private void GetDataCadastroGrupo(string grupcode)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetDataCadastroGrupo, grupcode));
            var data = "";

            if (!oRecordSet.EoF)
            {
                var aux1 = Convert.ToDateTime(oRecordSet.Fields.Item(0).Value);
                var DataCadastro = (EditText)Form_Item.Items.Item("tx_DtCad").Specific;

                String dia, mes;

                if (aux1.Day < 10)
                    dia = "0" + aux1.Day;
                else
                    dia = aux1.Day.ToString();

                if (aux1.Month < 10)
                    mes = "0" + aux1.Month;
                else
                    mes = aux1.Month.ToString();


                data = dia + "/" + mes + "/" + aux1.Year;

                DataCadastro.Value = data;
                oRecordSet.MoveNext();

            }
        }

        private void GetJurosGrupo()
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetJuros));

            var Juros = (EditText)Form_Item.Items.Item("tx_juros").Specific;

            if (!oRecordSet.EoF)
            {
                Juros.Value = oRecordSet.Fields.Item(0).Value.ToString() + "%";
                oRecordSet.MoveNext();
            }
        }

        

        #endregion

    }
}
