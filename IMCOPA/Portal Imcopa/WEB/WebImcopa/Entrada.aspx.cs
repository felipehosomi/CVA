using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImcopaWEB;
using WebImcopa.control;

//Esta variável define qual sistema SAP voce esta chamando no web.config


namespace WebImcopa
{
    public partial class Entrada : System.Web.UI.Page
    {

        //public const string SAPR3_AS = "NCO_DEV";

        SAPService ctrl_ = new SAPService();
        public int qtdMinima = 0;
        public string utilizaPedido = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlAvisos.Visible = false;
                configuraRepresentante();
                //lblDtAtual.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDataRem.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                pnlAvisos.Visible = false;
                Timer1.Enabled = true;
                //((ScriptManager)Page.FindControl("MasterScript")).SetFocus(hplConsultarPedido);

                VerificarBloqueio();
            }
        }
        private void VerificarBloqueio()
        {
            if (Session["USUARIO"] == null)
                Page.Response.Redirect("~/Login.aspx", false);
            else
            {
                string codrep = Session["USUARIO"].ToString();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                object[] objRetornoBloqueio = new object[7];
                SAPService ctrl_ = new SAPService();
                objRetornoBloqueio = ctrl_.Quotations_Fornecedor(codrep, 0);

                if (objRetornoBloqueio != null && !string.IsNullOrEmpty(objRetornoBloqueio[3].ToString()) && objRetornoBloqueio[3].ToString().ToUpper().Equals("X"))
                {
                    pnlAvisos.Visible = true;
                    Avisos("Vendas suspensas!", "0");

                    txtCNPJ.Enabled =
                    txtCodCliente.Enabled =
                    txtAdicionais.Enabled =
                    txtDataRem.Enabled =
                    //   txtObservacoes.Enabled =
                    txtPedido.Enabled =
                    txtSubTotal.Enabled =
                    //   ddlCondPag.Enabled =
                    ddlFilEmb.Enabled =
                    //   ddlFormPagto.Enabled =
                    ddlFrete.Enabled =
                    btnCalendario.Enabled =
                    btnGravar.Enabled = false;
                }
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            //calcula(GridView1);
        }
        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            calcula(GridView1);
            ((ScriptManager)Page.FindControl("MasterScript")).SetFocus((TextBox)sender);
        }
        protected void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                string cnpj = txtCNPJ.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();

                if (txtTeste.Text == "S" && String.IsNullOrEmpty(txtPedido.Text))
                {
                    Avisos("Necessário informar número do pedido", "0");
                    return;
                }

                var minExigido = txtTeste2.Text.Replace(',', '.');
                if (Convert.ToDouble(txtTotQuantidade.Text) < Convert.ToDouble(minExigido))
                {
                    Avisos("Quantidade vendida é inferior ao mínimo exigido", "0");
                    return;
                }

                if (cnpj != string.Empty)
                {
                    //Preenchimento do cabecalho
                    ZSDE007 Cabecalho = new ZSDE007();
                    Cabecalho.Corretor = lblcdRepresentante.Text.Trim(); //*********
                    Cabecalho.Dtdes = Convert.ToDateTime(txtDataRem.Text).ToString("yyyyMMdd");
                    Cabecalho.Dtdsjrem = Convert.ToDateTime(txtDataRem.Text).ToString("yyyyMMdd");
                    Cabecalho.Incoterms1 = ddlFrete.SelectedValue;
                    Cabecalho.Incoterms2 = lblCidade.Text;
                    Cabecalho.Kunnr = lblCodCliente.Text.Trim();  //*********
                                                                  // Cabecalho.Pcond = ddlCondPag.SelectedValue;
                    Cabecalho.Werks = ddlFilEmb.SelectedValue; //*********
                                                               // Cabecalho.Zlsch = ddlFormPagto.SelectedValue;
                    Cabecalho.Bstkd = txtPedido.Text;

                    Cabecalho.Dtval = string.Empty;
                    Cabecalho.Testrun = string.Empty;
                    //Preenchimento dos itens
                    ZSDE008Table tItens = new ZSDE008Table();
                    ZSDE008 Item;

                    int posicao = 10;
                    foreach (GridViewRow gRow in GridView1.Rows)
                    {

                        if (((TextBox)gRow.Cells[4].FindControl("TextBox3")).Text != string.Empty)
                        {
                            Item = new ZSDE008();
                            Item.Matnr = gRow.Cells[0].Text;
                            Item.Posnr = posicao.ToString();
                            Item.Quant = Convert.ToDecimal(((TextBox)gRow.Cells[1].FindControl("TextBox1")).Text.Replace('_', ' ').Trim());
                            Item.Valor = Convert.ToDecimal(((TextBox)gRow.Cells[4].FindControl("TextBox2")).Text.Replace('_', ' ').Trim());
                            tItens.Add(Item);
                            posicao = posicao + 10;
                        }
                    }
                    //Preenchimento da Observacao e Inf. Adicionais
                    ZSDE009Table tObs = new ZSDE009Table();
                    ZSDE009 Obs;

                    //  Obs = new ZSDE009();
                    //  Obs.Texto = txtObservacoes.Text;
                    // tObs.Add(Obs);

                    Obs = new ZSDE009();
                    Obs.Texto = txtAdicionais.Text;
                    tObs.Add(Obs);

                    if (tItens.Count > 0)
                    {
                        string iDoc = string.Empty;
                        //  ddlCondPag.Enabled = false;
                        string[] retorno = new string[2];
                        lblAvisos.Text = null;
                        retorno = ctrl_.Quotations_Create(Cabecalho, tItens, tObs);
                        string tipo = retorno[0].ToString();
                        string mensagem = retorno[1].ToString();
                        Avisos(mensagem, tipo);
                        LimpaTela();
                        ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtCNPJ);
                    }
                    else
                    {
                        Avisos("Nenhum item selecionado!", "0");
                    }
                }
                else
                {
                    Avisos("'CNPJ' é obrigatório!", "0");
                }
            }
            catch (Exception ex)
            {
                Avisos(ex.Message, "0");
            }
        }
        protected void Avisos(string doc, string tipo)
        {
            pnlAvisos.Visible = true;
            if (tipo == "1")
            {
                LimpaTela();
                imgAvisos.ImageUrl = "~/Imagens/Ok16.gif";
                lblAvisos.Text = " Documento " + doc + " cadastrado com sucesso!";
            }
            else if (tipo == "0")
            {
                imgAvisos.ImageUrl = "~/Imagens/Exclamation16.gif";
                lblAvisos.Text = " Erro: " + doc;
            }
        }
        protected void txtCodCliente_TextChanged(object sender, EventArgs e)
        {
            if (pnlMateriais.Visible == false)
            {
                string texto = txtCodCliente.Text;
                texto = texto.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();
                if (texto != string.Empty)
                    buscaCliente();
                ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtPedido);
            }
        }
        protected void txtCNPJ_TextChanged(object sender, EventArgs e)
        {
            if (pnlMateriais.Visible == false)
            {
                string texto = txtCNPJ.Text;
                texto = texto.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();
                if (texto != string.Empty)
                    buscaCliente();
                ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtPedido);
            }
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtCNPJ);
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TextBox tb = (TextBox)e.Row.FindControl("TextBox1");
            if (tb != null)
            {
                tb.Attributes.Add("onkeypress", "return VerificaNumero(event)");
            }
            TextBox tb1 = (TextBox)e.Row.FindControl("TextBox2");
            if (tb1 != null)
            {
                tb1.Attributes.Add("onkeypress", "Formata(this,20,event,2)");
            }
        }
        private void configuraRepresentante()
        {
            try
            {
                SAPService ctrl_ = new SAPService();
                object[] objRetorno = new object[7];

                if (Session["USUARIO"] == null)
                    Page.Response.Redirect("~/Login.aspx", false);

                string codrep = Session["USUARIO"].ToString();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                // busca o representante de acordo com o código informado
                lblVendedor.Text = ctrl_.Quotations_Fornecedor(codrep, 0)[1].ToString();

                // busca o representante de acordo com o código informado
                lblcdRepresentante.Text = codrep;
                objRetorno = ctrl_.Quotations_Fornecedor(codrep, 2);
                lblVendedor.Text = objRetorno[1].ToString();
                Session["DATATABLEFORMASPAGTO"] = (DataTable)objRetorno[6];
            }
            catch
            {
                Avisos("Fornecedor não encontrado!", "0");
            }
        }
        private void configuraCondPag()
        {
            try
            {
                // busca as condições de pagamento
                DataTable dt_Condicoes = new DataTable();
                dt_Condicoes = selecionaItens(ctrl_.Quotations_Get_Pay_Cond(), "CPAGTO");
                if (dt_Condicoes.Rows.Count > 0)
                {
                    //ddlCondPag.DataSource = dt_Condicoes;
                    //ddlCondPag.DataBind();
                    //ddlCondPag.SelectedIndex = 1;
                    //ddlCondPag.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Avisos(ex.Message, "0");
            }
        }
        private void configuraGrupoMercadoria(string cliente)
        {
            try
            {
                // busca os matariais e grupos de materiais
                object[] objTabelas = new object[3];
                DataTable dt_GrMercadorias = new DataTable();
                DataTable dt_Materiais = new DataTable();
                objTabelas = ctrl_.Quotations_Get_Materials(cliente);
                dt_GrMercadorias = (DataTable)objTabelas[0];
                dt_Materiais = (DataTable)objTabelas[1];

                if (dt_GrMercadorias.Rows.Count > 0)
                {
                    foreach (DataRow rowA in dt_GrMercadorias.Rows)
                    {
                        //Label1.Text = rowA["Wgbez"].ToString();

                        DataTable dt_datagrid = new DataTable();

                        dt_datagrid.Columns.Add(new DataColumn("MATNR", typeof(string)));
                        dt_datagrid.Columns.Add(new DataColumn("QUANT", typeof(string)));
                        dt_datagrid.Columns.Add(new DataColumn("MAKTX", typeof(string)));
                        dt_datagrid.Columns.Add(new DataColumn("MAKTG", typeof(string)));
                        dt_datagrid.Columns.Add(new DataColumn("TOTMAT", typeof(string)));
                        dt_datagrid.Columns.Add(new DataColumn("TOTAL", typeof(string)));
                        //dt_datagrid.Columns.Add(new DataColumn("MATKL", typeof(string)));
                        //dt_datagrid.Columns.Add(new DataColumn("UNVEN", typeof(string)));
                        //dt_datagrid.Columns.Add(new DataColumn("UNITRS", typeof(string)));
                        //dt_datagrid.Columns.Add(new DataColumn("TBQUANT", typeof(string)));

                        if (dt_Materiais.Rows.Count > 0)
                        {
                            foreach (DataRow rowB in dt_Materiais.Rows)
                            {
                                if (rowA[0].ToString().Equals(rowB[0].ToString()))
                                {
                                    DataRow dr = dt_datagrid.NewRow();
                                    //dr["MATKL"] = rowB["MATKL"].ToString();
                                    dr["MATNR"] = rowB["MATNR"].ToString();
                                    dr["MAKTX"] = rowB["MAKTX"].ToString();
                                    //dr["MAKTG"] = rowB["MAKTG"].ToString();
                                    //dr["UNVEN"] = rowB["UNVEN"].ToString();
                                    //dr["QUANT"] = rowB["QUANT"].ToString();
                                    dt_datagrid.Rows.Add(dr);
                                }
                            }
                            GridView1.DataSource = dt_datagrid;
                            GridView1.DataBind();
                            pnlMateriais.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Avisos(ex.Message, "0");
            }
        }
        private void LimpaTela()
        {
            txtCodCliente.Text = null;
            txtCNPJ.Text = null;
            txtDataRem.Text = DateTime.Now.ToShortDateString();
            txtPedido.Text = null;
            txtAdicionais.Text = null;
            txtSubTotal.Text = null;
            lblNomeCliente.Text = null;
            lblCidade.Text = null;
            lblCodCliente.Text = null;
            lblUF.Text = null;
            ddlFrete.Items.Clear();
            ddlFilEmb.Items.Clear();
            pnlMateriais.Visible = false;
            txtTotQuantidade.Text = "0";
            txtSubTotal.Text = "0";
        }
        private void calcula(GridView Grid)
        {
            decimal Quant = 0;
            decimal Valor = 0;
            decimal Total = 0;
            decimal SubTotal = 0;
            decimal TotalQuantidade = 0;
            foreach (GridViewRow gRow in Grid.Rows)
            {
                Quant = 0;
                Valor = 0;
                Total = 0;
                string strQuant = ((TextBox)gRow.Cells[1].FindControl("TextBox1")).Text.Replace('_', ' ').Trim();
                string strValor = ((TextBox)gRow.Cells[3].FindControl("TextBox2")).Text.Replace('_', ' ').Trim();

                if (!strQuant.Equals(string.Empty))
                {
                    Quant = Convert.ToDecimal(strQuant);
                    if (!strValor.Equals(string.Empty))
                    {
                        Valor = Convert.ToDecimal(strValor);
                        Total = Math.Round(Quant * Valor, 2);
                        TotalQuantidade += Quant;
                    }
                }
                if (Total != 0)
                    ((TextBox)gRow.Cells[4].FindControl("TextBox3")).Text = mascara(Math.Round(Total, 2));
                else
                    ((TextBox)gRow.Cells[4].FindControl("TextBox3")).Text = string.Empty;
                SubTotal += Total;
            }
            txtSubTotal.Text = mascara(Math.Round(SubTotal, 2));
            txtTotQuantidade.Text = TotalQuantidade.ToString();
        }
        private void buscaCliente()
        {
            if (!ctrl_.ValidaCnpj(txtCNPJ.Text))
            {
                Avisos("'CNPJ' inválido.", "0");
            }

            if (txtCNPJ.Text != string.Empty || txtCodCliente.Text != string.Empty)
            {
                string CodCliente = txtCodCliente.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();
                string CNPJ = txtCNPJ.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();
                string Representante = lblcdRepresentante.Text.Trim();
                pnlAvisos.Visible = false;
                DataTable dt_Cliente = new DataTable();
                try
                {
                    dt_Cliente = ctrl_.Quotations_Get_Customer(CodCliente.Trim(), Representante.Trim(), CNPJ.Trim(), string.Empty);
                    if (dt_Cliente.Rows.Count > 0)
                    {
                        lblCodCliente.Text = dt_Cliente.Rows[0][0].ToString();
                        txtCodCliente.Text = dt_Cliente.Rows[0][0].ToString();
                        txtCNPJ.Text = dt_Cliente.Rows[0][1].ToString();
                        lblUF.Text = dt_Cliente.Rows[0][7].ToString();
                        //txtInco1.Text = dt_Cliente.Rows[0][13].ToString();
                        //txtInco2.Text = dt_Cliente.Rows[0][12].ToString();
                        lblNomeCliente.Text = dt_Cliente.Rows[0][3].ToString();
                        lblCidade.Text = dt_Cliente.Rows[0][5].ToString() + " - " + dt_Cliente.Rows[0][7].ToString();
                        configuraCondPag();
                        carregaCombos("FEMB");
                        carregaCombos("FRETE");
                        configuraGrupoMercadoria(lblCodCliente.Text);

                        txtTeste.Text = dt_Cliente.Rows[0][15].ToString();
                        txtTeste2.Text = dt_Cliente.Rows[0][14].ToString();
                        txtTotQuantidade.Text = "0";


                        formasPgto();
                        //ddlFormPagto.DataSource = (DataTable)Session["DATATABLEFORMASPAGTO"];
                        //ddlFormPagto.DataBind();
                    }
                    else
                    {
                        Avisos("Nenhum cliente encontrado!", "0");
                    }
                }
                catch (Exception ex)
                {
                    Avisos(ex.Message, "0");
                }
            }
        }
        private void carregaCombos(string chave)
        {
            //Carrega filiais de embarque
            string registros = ConfigurationManager.AppSettings[chave].ToString();
            string[] param = registros.Split('|');
            for (int i = 0; i < param.Length; i++)
            {
                string[] key = param[i].Split('-');
                if (chave == "FEMB")
                    ddlFilEmb.Items.Add(new ListItem(param[i].ToString(), key[0].ToString()));
                else if (chave == "FRETE")
                    ddlFrete.Items.Add(new ListItem(key[1].ToString(), key[0].ToString()));
            }
        }
        private DataTable selecionaItens(DataTable dtItens, string chave)
        {
            DataTable dtItens_aux = new DataTable();
            dtItens_aux.Columns.Add(new DataColumn("ID"));
            dtItens_aux.Columns.Add(new DataColumn("TEXT"));
            try
            {
                string registros = ConfigurationManager.AppSettings[chave].ToString();
                string[] param = registros.Split('|');
                foreach (DataRow row in dtItens.Rows)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        if (row[0].ToString() == param[i].ToString())
                        {
                            dtItens_aux.ImportRow(row);
                        }
                    }

                }
                return dtItens_aux;
            }
            catch
            {
                throw;
            }
        }
        private string mascara(decimal valor)
        {
            string retorno = string.Empty;
            try
            {
                retorno = String.Format("{0:C}", valor);
            }
            catch (Exception ex)
            {
                Avisos(ex.Message, "0");
            }
            return retorno;
        }
        void grvMateriais_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valor = string.Empty;
        }

        protected void ddlFilEmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            formasPgto();
        }

        private void formasPgto()
        {
            //ddlFormPagto.Items.Clear();
            //RfcDestination destination = RfcDestinationManager.
            //   GetDestination("QAS");//2

            //IRfcFunction function = null;
            //function = destination.Repository.CreateFunction("ZPWI_GET_FRM_PGTO");

            //function.SetValue("I_KUNNR", txtCodCliente.Text);
            //function.SetValue("I_CENTRO", ddlFilEmb.SelectedValue);

            //function.Invoke(destination);

            //IRfcTable E_OutTable = function.GetTable("T_ZSDE034");
            //foreach (IRfcStructure E_OutRow in E_OutTable)
            //{
            //    ListItem item = new ListItem(E_OutRow.GetString("TEXT1"), E_OutRow.GetString("ZLSCH"));

            //    ddlFormPagto.Items.Add(item);
            //    // drSaida = dtSaida.NewRow();
            //    // drSaida["Interface"] = E_OutRow.GetString("ATIVIDADE");
            //    // drSaida["Registros"] = E_OutRow.GetString("REGISTROS");
            //    // dtSaida.Rows.Add(drSaida);
            //}
            //ddlFormPagto.DataBind();

            /* 03/05/2016 - Código retirado de uma outra versão do projeto fornecido pela Imcopa. */
            string[] strArray = null;
            string[] strArray2 = null;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FORMAS_PAGAMENTO"].ToString().Trim()) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["FORMAS_PAGAMENTO_DESCRICAO"].ToString().Trim()))
            {
                strArray = ConfigurationManager.AppSettings["FORMAS_PAGAMENTO"].ToString().Split(new char[] { ';' });
                strArray2 = ConfigurationManager.AppSettings["FORMAS_PAGAMENTO_DESCRICAO"].ToString().Split(new char[] { ';' });
            }
            if (strArray.Length != strArray2.Length)
            {
                this.Avisos("Identificadores de formas de pagamentos e descri\x00e7\x00e3o de formas de pagamentos possuem tamanhos diferentes. Por favor revise o arquivo de configura\x00e7\x00f5es.", "0");
            }
            else
            {
                //  this.ddlFormPagto.Items.Clear();
                for (int i = 0; i < strArray.Length; i++)
                {
                    ListItem item = new ListItem(strArray2[i], strArray[i]);
                    //    this.ddlFormPagto.Items.Add(item);
                }
            }
        }
    }
}
