using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using WebImcopa.control;
using ImcopaWEB;
using System.Drawing;
using System.IO;

namespace WebImcopa
{
    public partial class ConsultaCliente : System.Web.UI.Page
    {
        SAPService _ctrl = new SAPService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                configuraRepresentante();
                pnlAvisos.Visible = false;
                if (Session["DATATABLECLIENTES"] == null)
                {
                    carregaCombos("BUKRS");
                    //btnImprimir.Enabled = false;
                    extModal.Show();
                }
                else
                {
                    //btnImprimir.Enabled = true;
                    atualizaDados(false);
                }
            }
        }

        private void configuraRepresentante()
        {
            try
            {
                SAPService _ctrl = new SAPService();
                object[] objRetorno = new object[3];
                DataTable dtVendas = new DataTable();

                if (Session["USUARIO"] == null)
                    Page.Response.Redirect("~/Login.aspx", false);

                string codrep = Session["USUARIO"].ToString();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                // busca o representante de acordo com o código informado
                lblcdRepresentante.Text = codrep;
                //lblVendedor.Text = ctrl_.Quotations_Fornecedor(codrep, 0)[1].ToString();
                objRetorno = _ctrl.Quotations_Fornecedor(codrep, 1);
                lblVendedor.Text = objRetorno[1].ToString();
                dtVendas = (DataTable)objRetorno[2];

                if (dtVendas.Rows.Count > 0)
                {
                    DataTable dtVendas_aux = new DataTable();
                    dtVendas_aux.Columns.Add(new DataColumn("VKGRP", typeof(string)));
                    dtVendas_aux.Columns.Add(new DataColumn("DESCR", typeof(string)));

                    DataRow drVendas = dtVendas_aux.NewRow();
                    drVendas["VKGRP"] = String.Empty;
                    drVendas["DESCR"] = String.Empty;
                    dtVendas_aux.Rows.Add(drVendas);

                    for (int i = 0; i < dtVendas.Rows.Count; i++)
                    {
                        drVendas = dtVendas_aux.NewRow();
                        drVendas["VKGRP"] = dtVendas.Rows[i]["VKGRP"].ToString();
                        drVendas["DESCR"] = dtVendas.Rows[i]["VKBUR"].ToString() + " / " + dtVendas.Rows[i]["VKGRP"].ToString() + " / " + dtVendas.Rows[i]["BEZEI"].ToString();
                        dtVendas_aux.Rows.Add(drVendas);
                    }

                    ddlVendas.DataSource = dtVendas_aux;
                    ddlVendas.DataBind();
                    ddlVendas.SelectedIndex = 0;
                }
            }
            catch
            {
                lblVendedor.Text = "Fornecedor não encontrado!";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtCodCliente);
            extModal.Show();
            pnlAvisos.Visible = false;
        }

        private ZSDE028 filtro()
        {
            ZSDE028 i_header = new ZSDE028();

            i_header.Lifnr = zeroesc(lblcdRepresentante.Text, 10);
            i_header.Kunnr = txtCodCliente.Text;
            //i_header.Name1 = txtNome.Text;

            //string CPF = txtCPF.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();
            string CNPJ = txtCNPJ.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();

            if (txtDataIni.Text != String.Empty)
            {
                DateTime dtInicial = Convert.ToDateTime(txtDataIni.Text);
                i_header.Dtlow = dtInicial.ToString("yyyyMMdd");
            }
            if (txtDataIni.Text != String.Empty)
            {
                DateTime dtFinal = Convert.ToDateTime(txtDataFin.Text);
                i_header.Dthigh = dtFinal.ToString("yyyyMMdd");
            }
            i_header.Stcd1 = CNPJ;
            //i_header.Stcd3 = CPF;

            if (ddlVendas.SelectedValue != String.Empty)
                i_header.Vkgrp = ddlVendas.SelectedValue;

            if (ddlEmpresa.SelectedValue != String.Empty)
                i_header.Bukrs = ddlEmpresa.SelectedValue;

            return i_header;
        }

        private void atualizaDados(bool atualizar)
        {
            DataTable dt_List = new DataTable();

            if (atualizar)
            {
                Session["DATATABLECLIENTES"] = null;
                Session["DATATABLECLIENTES"] = _ctrl.List_Customer(filtro(), 0);
            }

            dt_List = ((DataTable)Session["DATATABLECLIENTES"] != null ? (DataTable)Session["DATATABLECLIENTES"] : _ctrl.List_Customer(filtro(), 0));

            if (dt_List.Rows.Count == 0)
            {
                Avisos("0", "Nenhum registro encontrado.");
            }
            else
            {
                gridViewClientes.DataSource = dt_List;
                gridViewClientes.DataBind();

                //ClientScript.RegisterStartupScript(this.GetType(), "Key", "<script>MakeStaticHeader('" + gridViewClientes.ClientID + "', 400, 950 , 40 ,true); </script>", false);
                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewClientes', 'HeaderDiv');</script>");
            }
        }

        private void carregaCombos(string chave)
        {
            //Carrega empresa
            string registros = ConfigurationManager.AppSettings[chave].ToString();
            string[] param = registros.Split('|');
            for (int i = 0; i < param.Length; i++)
            {
                string[] key = param[i].Split('-');
                ddlEmpresa.Items.Add(new ListItem(param[i].ToString(), key[0].ToString()));
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //btnImprimir.Enabled = true;
            atualizaDados(true);
        }

        private string zeroesc(string valor, int tamanho)
        {
            int qtd = (tamanho - valor.Length);
            StringBuilder sb = new StringBuilder();
            if (qtd > 0)
            {
                for (int i = 0; i < qtd; i++)
                {
                    sb.Append("0");
                }
            }
            sb.Append(valor);
            return sb.ToString();
        }

        protected void Avisos(string tipo, string doc)
        {
            pnlAvisos.Visible = true;
            if (tipo == "1")
            {
                imgAvisos.ImageUrl = "~/Imagens/Ok16.gif";
                lblAvisos.Text = doc;
            }
            else if (tipo == "0")
            {
                imgAvisos.ImageUrl = "~/Imagens/Exclamation16.gif";
                lblAvisos.Text = " Erro: " + doc;
            }
        }

        public string cut(object descricao, int def)
        {
            int tamanho = descricao.ToString().Length;
            int cutSize = def;
            string retorno;
            if (tamanho < cutSize)
                cutSize = tamanho;
            retorno = descricao.ToString().Substring(0, cutSize).ToString();
            if (cutSize == def)
                retorno += "...";
            return retorno;
        }
        /// <summary>
        /// Funcoes de paginacao e ordenacao de GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Ordenação e Paginação gridViewClientes

        private string gvOrdenaSentido
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string gvOrdenaExpressao
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetOrdenaSentido()
        {
            switch (gvOrdenaSentido)
            {
                case "ASC":
                    gvOrdenaSentido = "DESC";
                    break;
                case "DESC":
                    gvOrdenaSentido = "ASC";
                    break;
            }
            return gvOrdenaSentido;
        }

        private DataView ordenaDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (gvOrdenaExpressao != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", gvOrdenaExpressao, gvOrdenaSentido);
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", gvOrdenaExpressao, GetOrdenaSentido());
                    }
                }
                Session["SORTCLIENTES"] = dataView.Sort;
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }

        protected void gridViewClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtClientes = ((DataTable)Session["DATATABLECLIENTES"] != null ? (DataTable)Session["DATATABLECLIENTES"] : _ctrl.List_Customer(filtro(), 0));
            gridViewClientes.DataSource = ordenaDataTable(dtClientes, true);
            gridViewClientes.PageIndex = e.NewPageIndex;
            gridViewClientes.DataBind();
            //ClientScript.RegisterStartupScript(this.GetType(), "Key", "<script>MakeStaticHeader('" + gridViewClientes.ClientID + "', 400, 950 , 40 ,true); </script>", false);
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewClientes', 'HeaderDiv');</script>");
        }

        protected void gridViewClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtClientes = ((DataTable)Session["DATATABLECLIENTES"] != null ? (DataTable)Session["DATATABLECLIENTES"] : _ctrl.List_Customer(filtro(), 0));
            gvOrdenaExpressao = e.SortExpression;
            int pageIndex = gridViewClientes.PageIndex;
            gridViewClientes.DataSource = ordenaDataTable(dtClientes, false);
            gridViewClientes.DataBind();
            //ClientScript.RegisterStartupScript(this.GetType(), "Key", "<script>MakeStaticHeader('" + gridViewClientes.ClientID + "', 400, 950 , 40 ,true); </script>", false);
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewClientes', 'HeaderDiv');</script>");
            gridViewClientes.PageIndex = pageIndex;
        }

        protected void gridViewClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[10].Text = e.Row.Cells[10].Text.Replace("&nbsp;", String.Empty);
                if (e.Row.Cells[10].Text != String.Empty)
                {
                    e.Row.Font.Bold = true;
                    e.Row.ForeColor = Color.Red;
                }
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor; this.style.backgroundColor='Orange';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }
        #endregion Ordenação e Paginação

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            string fileName = "ConsultaCliente_" + DateTime.Now.ToString("dd_MM-yyyy");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a form to contain the grid
                    Table table = new Table();

                    //  add the header row to the table
                    if (gridViewClientes.HeaderRow != null)
                    {
                        //    GridViewExportUtil.PrepareControlForExport(gridViewClientes.HeaderRow);
                        PrepareControlForExport(gridViewClientes.HeaderRow);
                        table.Rows.Add(gridViewClientes.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gridViewClientes.Rows)
                    {
                        //    GridViewExportUtil.PrepareControlForExport(row);
                        PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gridViewClientes.FooterRow != null)
                    {
                        //    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                        PrepareControlForExport(gridViewClientes.FooterRow);
                        table.Rows.Add(gridViewClientes.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                //if (current.HasControls())
                //{
                //    GridViewExportUtil.PrepareControlForExport(current);
                //}
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gridViewClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ClientesAtrelados"))
            {
                var codCliente = e.CommandArgument.ToString().Split('_')[0];
                string limiteCredito = e.CommandArgument.ToString().Split('_').Length > 0 ? e.CommandArgument.ToString().Split('_')[1] : string.Empty;

                var dtClintes = _ctrl.Get_Clients_Linked(codCliente);
                gridViewClientesAtrelados.DataSource = dtClintes;
                gridViewClientesAtrelados.DataBind();

                if (!string.IsNullOrEmpty(limiteCredito))
                    sLimiteCredito.InnerText = Convert.ToDecimal(limiteCredito).ToString("#,##");

                mpeClientesAtrelados.Show();
            }
        }
    }
}
