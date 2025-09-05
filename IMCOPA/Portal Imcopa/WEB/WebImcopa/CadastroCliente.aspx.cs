using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using ImcopaWEB;
using WebImcopa.control;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;

namespace WebImcopa
{
    public partial class CadastroCliente : Page
    {
        #region Atributos
        SAPService _sap = new SAPService();
        string strVkbur = String.Empty;
        string strVkgrp = String.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                configuraRepresentante();
                pnlAvisos.Visible = false;
                ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtCNPJ);
            }
        }

        //Nesta Parte do código e desta maneira inserimos a referência a DLL que já está registrada.

        [DllImport("DllInscE32.dll")]
        //Método static extern ConsisteInscricaoEstadual que retornará a validação da IE
        public static extern int ConsisteInscricaoEstadual(string vInsc, string vUF);


        private void configuraRepresentante()
        {
            try
            {
                if (Session["USUARIO"] == null)
                    Page.Response.Redirect("~/Login.aspx", false);

                string codrep = Session["USUARIO"].ToString();
                object[] objRetorno = new object[3];
                DataTable dtVendas = new DataTable();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                // busca o representante de acordo com o código informado
                lblcdRepresentante.Text = codrep;
                objRetorno = _sap.Quotations_Fornecedor(codrep, 1);
                lblVendedor.Text = objRetorno[1].ToString();
                dtVendas = (DataTable)objRetorno[2];

                if (dtVendas.Rows.Count > 0)
                {
                    string[] lista = new string[dtVendas.Rows.Count];
                    for (int i = 0; i < dtVendas.Rows.Count; i++)
                    {
                        lista[i] = dtVendas.Rows[i]["VKBUR"].ToString() + " / " + dtVendas.Rows[i]["VKGRP"].ToString() + " / " + dtVendas.Rows[i]["BEZEI"].ToString();
                    }

                    lstVendas.DataSource = lista;
                    lstVendas.DataBind();
                    lstVendas.SelectedIndex = 0;
                }
                ddlAreaNielsen.DataSource = (DataTable)objRetorno[3];
                ddlAreaNielsen.DataBind();
                ddlAreaNielsen.Items.Insert(0, new ListItem("<Selecione>", ""));

                ddlCanalVenda.DataSource = (DataTable)objRetorno[5];
                ddlCanalVenda.DataBind();
                ddlCanalVenda.Items.Insert(0, new ListItem("<Selecione>", ""));

                ddlTipoCliente.DataSource = (DataTable)objRetorno[4];
                ddlTipoCliente.DataBind();
                ddlTipoCliente.Items.Insert(0, new ListItem("<Selecione>", ""));
            }
            catch
            {
                lblVendedor.Text = "Fornecedor não encontrado!";
            }
        }

        protected void Salvar_Cliente(object sender, EventArgs e)
        {
            if (ValidaCliente())
            {
                var model = new ZSDE027()
                {
                    Redes = txtRede.Text,
                    House_No = txtNumero.Text,
                    Stcd1_Cobr = RemoveMascara(txtCNPJCobranca.Text),
                    Zdbekr = txtLimite.Text,
                    Vtweg = ddlCanalVenda.Text,
                    Namev = txtContato.Text.ToUpper(),
                    Kukla = ddlTipoCliente.Text,
                    Land1 = "BR",
                    Name1 = txtNome.Text.ToUpper(),
                    Ort01 = txtCidade.Text.ToUpper(),
                    Ort02 = txtBairro.Text.ToUpper(),
                    Pstlz = txtCEP.Text,
                    Regio = txtUF.Text.ToUpper(),
                    Stras = txtEndereco.Text.ToUpper(),
                    Stcd1 = RemoveMascara(txtCNPJ.Text),
                    Stcd3 = RemoveMascara(txtInscricaoEstadual.Text),
                    Vkbur = lstVendas.SelectedValue.Split('/')[0].Trim().ToString(),
                    Vkgrp = lstVendas.SelectedValue.Split('/')[1].Trim().ToString(),
                    Email = txtEmail.Text.ToUpper(),
                    Sort1 = txtNomeFantasia.Text.ToUpper(),
                    Tel_Number = RemoveMascara(txtComercial.Text),
                    Mob_Number = RemoveMascara(txtCelular.Text),
                    Fax_Number = RemoveMascara(txtFax.Text),
                    Niels = ddlAreaNielsen.Text,
                    Mao_Obra = cbMaoObra.SelectedValue,
                    Tipo_Veiculo = cbTipoVeiculo.SelectedValue,
                    Cont_Fin_Tel = txtFoneFinan.Text,
                    Cont_Fin_Email = txtEmailFinan.Text,
                    Cont_Log_Tel = txtFoneLog.Text,
                    Cont_Log_Email = txtEmailLog.Text,
                    Cont_Comp_Tel = txtFoneComp.Text,
                    Cont_Comp_Email = txtEmailComp.Text,
                    Local_Entrega = txtLocalEntrega.Text,
                    Tipo_Carga = cbTipoCarga.SelectedValue,
                    Cad_Chep = cbCHEP.SelectedValue,
                    Cli_Agend = cbAgendamento.SelectedValue,
                    Tipo_Estab = cbTipoEstabelecimento.SelectedValue,
                    Zterm = cbCondPgto.SelectedValue,
                    Rest_Horario1 = txtDe.Text,
                    Rest_Horario2 = txtAte.Text,
                    Ped_Compra_Cli = cbPedido.SelectedValue,
                    Cont_Fin_Nome = txtContatoFinan.Text,
                    Cont_Log_Nome = txtContatoLog.Text,
                    Obs = txtObservacoes.Text,
                    Rest_Hor_Sem1 = cbDeDia.SelectedValue,
                    Rest_Hor_Sem2 = cbAteDia.SelectedValue,
                    Rest_Hor_Dia1 = txtDeMes.Text,
                    Rest_Hor_Dia2 = txtAteMes.Text,
                    Data_Aniver = txtAniversario.Text,
                    Zwels = cbFormaPgto.SelectedValue,
                    Compl_Num = txtComplemento.Text
                };

                var result = _sap.Salvar_Cliente(model);
                LimpaTela();
                Avisos(result[0], result[1]);
            }
            else
                return;
        }



        public bool ValidaCliente()
        {
            #region Dados Cadastrais
            if (RemoveMascara(txtCNPJ.Text) == null || RemoveMascara(txtCNPJ.Text) == String.Empty)
            {
                Avisos("0", "Campo 'CNPJ' é obrigatório.");
                return false;
            }
            else if (!_sap.ValidaCnpj(txtCNPJ.Text))
            {
                Avisos("0", "'CNPJ' inválido.");
                return false;
            }
            else if (txtNome.Text == null || txtNome.Text.Equals(""))
            {
                Avisos("0", "Campo 'Razão Social' é obrigatório.");
                return false;
            }
            else if (txtNomeFantasia.Text == null || txtNomeFantasia.Text.Equals(""))
            {
                Avisos("0", "Campo 'Nome Fantasia' é obrigatório.");
                return false;
            }
            else if (txtEndereco.Text == null || txtEndereco.Text.Equals(""))
            {
                Avisos("0", "Campo 'Endereço' é obrigatório.");
                return false;
            }
            else if (txtBairro.Text == null || txtBairro.Text.Equals(""))
            {
                Avisos("0", "Campo 'Bairro' é obrigatório.");
                return false;
            }
            else if (txtCidade.Text == null || txtCidade.Text.Equals(""))
            {
                Avisos("0", "Campo 'Cidade' é obrigatório.");
                return false;
            }
            else if (RemoveMascara(txtComercial.Text) == null || RemoveMascara(txtComercial.Text) == String.Empty)
            {
                Avisos("0", "Campo 'Telefone Comercial' é obrigatório.");
                return false;
            }
            else if (txtInscricaoEstadual.Text == null || txtInscricaoEstadual.Text.Equals(""))
            {
                Avisos("0", "Campo 'Inscrição Estadual' é obrigatório.");
                return false;
            }
            else if (ConsisteInscricaoEstadual(RemoveMascara(txtInscricaoEstadual.Text), txtUF.Text) > 0)
            {
                Avisos("0", "'Inscrição Estadual' inválida.");
                return false;
            }
            else if (txtRede.Text == null || txtRede.Text.Equals(""))
            {
                Avisos("0", "Campo 'Rede' é obrigatório.");
                return false;
            }
            else if (txtNumero.Text == null || txtNumero.Text.Equals(""))
            {
                Avisos("0", "Campo 'N°' é obrigatório.");
                return false;
            }
            else if (txtCEP.Text == null || txtCEP.Text.Equals(""))
            {
                Avisos("0", "Campo 'CEP' é obrigatório.");
                return false;
            }
            else if (txtUF.Text == null || txtUF.Text.Equals(""))
            {
                Avisos("0", "Campo 'UF' é obrigatório.");
                return false;
            }
            else if (txtEmail.Text == null || txtEmail.Text.Equals(""))
            {
                Avisos("0", "Campo 'Email NF-e' é obrigatório.");
                return false;
            }
            #endregion

            #region Dados Financeiros
            else if (txtFoneFinan.Text == null || txtFoneFinan.Text.Equals(""))
            {
                Avisos("0", "Campo 'Telefone' é obrigatório nos Dados Financeiros.");
                return false;
            }
            else if (txtEmailFinan.Text == null || txtEmailFinan.Text.Equals(""))
            {
                Avisos("0", "Campo 'Email' é obrigatório nos Dados Financeiros.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbCondPgto.SelectedValue))
            {
                Avisos("0", "Campo 'Condição de Pgto' é obrigatório nos Dados Financeiros.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbFormaPgto.SelectedValue))
            {
                Avisos("0", "Campo 'Forma de Pgto' é obrigatório nos Dados Financeiros.");
                return false;
            }
            #endregion

            #region Dados Logísticos
            else if (txtFoneLog.Text == null || txtFoneLog.Text.Equals(""))
            {
                Avisos("0", "Campo 'Telefone' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (txtEmailLog.Text == null || txtEmailLog.Text.Equals(""))
            {
                Avisos("0", "Campo 'Email' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbMaoObra.SelectedValue))
            {
                Avisos("0", "Campo 'Mão de Obra' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbTipoVeiculo.SelectedValue))
            {
                Avisos("0", "Campo 'Tipo de Veículo' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbTipoCarga.SelectedValue))
            {
                Avisos("0", "Campo 'Tipo de Carga' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbCHEP.SelectedValue))
            {
                Avisos("0", "Campo 'Cad. CHEP' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbTipoEstabelecimento.SelectedValue))
            {
                Avisos("0", "Campo 'Estabelecimento' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(cbAgendamento.SelectedValue))
            {
                Avisos("0", "Campo 'Agendamento' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (txtLocalEntrega.Text == null || txtLocalEntrega.Text.Equals(""))
            {
                Avisos("0", "Campo 'Local de Entrega' é obrigatório nos Dados Logísticos.");
                return false;
            }
            #endregion

            #region Dados do Comprador
            else if (txtContato.Text == null || txtContato.Text.Equals(""))
            {
                Avisos("0", "Campo 'Contato' é obrigatório nos Dados do Comprador.");
                return false;
            }
            else if (txtFoneComp.Text == null || txtFoneComp.Text.Equals(""))
            {
                Avisos("0", "Campo 'Telefone' é obrigatório nos Dados do Comprador.");
                return false;
            }
            else if (txtEmailComp.Text == null || txtEmailComp.Text.Equals(""))
            {
                Avisos("0", "Campo 'Email' é obrigatório nos Dados do Comprador.");
                return false;
            }
            #endregion

            else if (String.IsNullOrEmpty(cbPedido.SelectedValue))
            {
                Avisos("0", "Campo 'N° Pedido do Cliente' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(ddlAreaNielsen.SelectedValue))
            {
                Avisos("0", "Campo 'Área Nielsen' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(ddlCanalVenda.SelectedValue))
            {
                Avisos("0", "Campo 'Canal de Venda' é obrigatório nos Dados Logísticos.");
                return false;
            }
            else if (String.IsNullOrEmpty(ddlTipoCliente.SelectedValue))
            {
                Avisos("0", "Campo 'Tipo de Cliente' é obrigatório nos Dados Logísticos.");
                return false;
            }

            else
                return true;
        }

        protected void Busca_CNPJ(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(RemoveMascara(txtCNPJ.Text)))
            {
                txtCNPJCobranca.Text = txtCNPJ.Text;
                var model = new ZSDE027() { Stcd1 = RemoveMascara(txtCNPJ.Text) };
                var result = _sap.Pesquisar_Cliente(model);

                if (result != null)
                {
                    Avisos("0", result[1]);
                    btnOK.Visible = false;
                    return;
                }
                else
                {
                    Avisos("1", "Cliente não cadastrado");
                    btnOK.Visible = true;
                }

                Valida_CNPJ(RemoveMascara(txtCNPJ.Text));
        }
            else
                Avisos("0", "Preencha o campo 'CNPJ'");
    }

        private void Valida_CNPJ(string cnpj)
        {
            try
            {
                var federal = new Federal();
                var dadosReceita = federal.TesteUnitario_DELTA(RemoveMascara(txtCNPJ.Text));

                if (dadosReceita != null)
                {
                    if (dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/SituacaoCadastral").InnerText == "ATIVA")
                    {
                        txtNome.Text = dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/RazaoSocial").InnerText;
                        txtNomeFantasia.Text = dadosReceita.SelectSingleNode($@"Arquivo / Registro / Retorno /Dados/NomeFantasia").InnerText;
                        txtCEP.Text = RemoveMascara(dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/CEP").InnerText);
                        txtEndereco.Text = dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/Logradouro").InnerText;
                        txtBairro.Text = dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/Bairro").InnerText;
                        txtNumero.Text = dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/Numero").InnerText;
                        txtCidade.Text = dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/Municipio").InnerText;
                        txtUF.Text = dadosReceita.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/UF").InnerText;


                        var sintegra = new Sintegra();
                        var dadosSintegra = sintegra.TesteUnitario_DELTA_CNPJ(cnpj, txtUF.Text);

                        try
                        {
                            txtInscricaoEstadual.Text = dadosSintegra.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/InscricaoEstadual").InnerText;
                        }
                        catch (Exception ex)
                        {
                            txtInscricaoEstadual.Text = null;
                        }
                        Carrega_Combos();
                    }
                    else
                        Avisos("0", "Este CNPJ não está Ativo/Regular perante a receita");
                }
                else
                    Avisos("0", "Os dados para este CNPJ não foram encontrados");
            }
            catch (Exception ex)
            {
                Avisos("0", "Ocorreu um erro durante a consulta do CNPJ.");
                LimpaTela();
            }
        }


        protected void Busca_CEP(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(RemoveMascara(txtCEP.Text)))
                {
                    var ws = new WSCorreios.AtendeClienteClient();
                    var resposta = ws.consultaCEP(txtCEP.Text);
                    txtEndereco.Text = resposta.end;

                    txtBairro.Text = resposta.bairro;
                    txtCidade.Text = resposta.cidade;
                    txtUF.Text = resposta.uf;
                    pnlAvisos.Visible = false;
                }
                else
                    Avisos("0", "Preencha o campo 'CEP'");
            }
            catch (Exception ex)
            {
                Avisos("0", "CEP inválido");
            }
        }
        
        public void Carrega_Combos()
        {
            var model = new ZSDE027
            {
                Stcd1 = RemoveMascara(txtCNPJCobranca.Text)
            };
            var result = _sap.Buscar_CondicoesPgto(model);

            cbCondPgto.Items.Clear();

            foreach (var item in result)
            {
                cbCondPgto.Items.Insert(0, new ListItem(item.Code + " - " + item.Desc, item.Code));
            }
            cbCondPgto.Items.Insert(0, new ListItem("<Selecione>", ""));


            cbFormaPgto.Items.Clear();

            if (result.Count == 0 || String.IsNullOrEmpty(result[0].CodeFormaPgto))
            {
                cbFormaPgto.Items.Insert(0, new ListItem("Boleto - Banco do Brasil", "A"));
                cbFormaPgto.Items.Insert(0, new ListItem("Boleto - HSBC", "D"));
                cbFormaPgto.Items.Insert(0, new ListItem("Cobrança em Carteira", "I"));
                cbFormaPgto.Items.Insert(0, new ListItem("<Selecione>", ""));
            }
            else
            {
                cbFormaPgto.Items.Insert(0, new ListItem("Vínculada ao CNPJ raíz", "X"));
            }
        }

        protected void Avisos(string tipo, string mensagem)
        {
            pnlAvisos.Visible = true;
            if (tipo == "1")
            {
                imgAvisos.ImageUrl = "~/Imagens/Ok16.gif";
                lblAvisos.Text = mensagem;
            }
            if (tipo == "0")
            {
                imgAvisos.ImageUrl = "~/Imagens/Exclamation16.gif";
                lblAvisos.Text = " Erro: " + mensagem;
            }
        }

        private string RemoveMascara(string texto)
        {
            string retorno = texto.Replace('_', ' ')
                                  .Replace('/', ' ')
                                  .Replace('.', ' ')
                                  .Replace(',', ' ')
                                  .Replace('-', ' ')
                                  .Replace('(', ' ')
                                  .Replace(')', ' ')
                                  .Replace(" ", string.Empty)
                                  .Trim();
            return retorno;
        }

        private void LimpaTela()
        {
            txtRede.Text = "";
            txtNumero.Text = "";
            txtCNPJCobranca.Text = "";
            txtLimite.Text = "";
            ddlCanalVenda.Text = "";
            txtContato.Text = "";
            ddlTipoCliente.Text = "";
            txtNome.Text = "";
            txtCidade.Text = "";
            txtBairro.Text = "";
            txtCEP.Text = "";
            txtUF.Text = "";
            txtEndereco.Text = "";
            txtCNPJ.Text = "";
            txtInscricaoEstadual.Text = "";
            txtEmail.Text = "";
            txtNomeFantasia.Text = "";
            txtComercial.Text = "";
            txtCelular.Text = "";
            txtFax.Text = "";
            ddlAreaNielsen.Text = "";
            cbMaoObra.SelectedValue = "";
            cbTipoVeiculo.SelectedValue = "";
            txtFoneFinan.Text = "";
            txtEmailFinan.Text = "";
            txtFoneLog.Text = "";
            txtEmailLog.Text = "";
            txtFoneComp.Text = "";
            txtEmailComp.Text = "";
            txtLocalEntrega.Text = "";
            cbTipoCarga.SelectedValue = "";
            cbCHEP.SelectedValue = "";
            cbAgendamento.SelectedValue = "";
            cbTipoEstabelecimento.SelectedValue = "";
            cbCondPgto.SelectedValue = "";
            txtDe.Text = "";
            txtAte.Text = "";
            cbPedido.SelectedValue = "";
            txtContatoFinan.Text = "";
            txtContatoLog.Text = "";
            txtComplemento.Text = "";
        }
    }
}