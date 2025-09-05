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
using ImcopaWEB;
using WebImcopa.control;

namespace WebImcopa
{
    public partial class Consulta : System.Web.UI.Page
    {
        SAPService _ctrl = new SAPService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlAvisos.Visible = false;
                lblFiltro.Visible = false;
                configuraRepresentante();
                btnOk.Enabled = true;
                extModal.Show();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = false;

            DataTable dt_List = carregaGrid();

            if (dt_List.Rows.Count == 0)
            {
                Avisos("Nenhum registro encontrado.", "0");
                grvConsulta.DataSource = dt_List;
                grvConsulta.DataBind();
                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'grvConsulta', 'HeaderDiv');</script>");
            }
            else
            {
                lblFiltro.Visible = true;
                grvConsulta.DataSource = dt_List;
                grvConsulta.DataBind();

                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'grvConsulta', 'HeaderDiv');</script>");
            }
        }

        private DataTable carregaGrid()
        {
            try
            {
                lblFiltro.Text = ddlVendas.SelectedItem.Text;
                string corretor = lblcdRepresentante.Text.Trim();
                string cotacao = txtPedido.Text.Trim();
                string grupo = string.Empty;

                if (txtDataIni.Text.Trim() == string.Empty)
                    txtDataIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
                if (txtDataFin.Text.Trim() == string.Empty)
                    txtDataFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                if (Convert.ToDateTime(txtDataIni.Text.Trim()) > Convert.ToDateTime(txtDataFin.Text.Trim()))
                    txtDataIni.Text = txtDataFin.Text;
                if (ddlVendas.SelectedValue != String.Empty)
                    grupo = ddlVendas.SelectedValue;

                // busca as pedidos cadastrados
                string dataini = Convert.ToDateTime(txtDataIni.Text.Trim()).ToString("yyyyMMdd");
                string datafin = Convert.ToDateTime(txtDataFin.Text.Trim()).ToString("yyyyMMdd");
                string cliente = txtCodCliente.Text.Trim();
                DataTable dt_Cotacoes = new DataTable();
                object[] objRetorno = new object[2];
                //dt_Cotacoes = ctrl_.Quotations_Tracking(corretor, cotacao, datafin, dataini, cliente);
                objRetorno = _ctrl.Quotations_Tracking(corretor, cotacao, datafin, dataini, cliente, grupo);
                Session["DATATABLECOTACOES"] = (DataTable)objRetorno[0];
                dt_Cotacoes = (DataTable)Session["DATATABLECOTACOES"];
                //grvConsulta.DataSource = dt_Cotacoes;
                //grvConsulta.DataBind();
                return dt_Cotacoes;
            }
            catch
            {
                throw;
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
            pnlAvisos.Visible = false;
            btnOk.Enabled = true;
            extModal.Show();
        }

        public string cut(object descricao, int tam)
        {
            int tamanho = descricao.ToString().Length;
            int cutSize = tam;
            string retorno;

            if (tamanho < cutSize)
                cutSize = tamanho;

            retorno = descricao.ToString().Substring(0, cutSize).ToString();

            if (cutSize == tam)
                retorno += "...";

            return retorno;
        }

        private string formataDateTime(string texto, bool eData)
        {
            string strSaida = string.Empty;
            if (eData)
                strSaida = texto.Substring(6, 2) + "/" + texto.Substring(4, 2) + "/" + texto.Substring(2, 2);
            else
                strSaida = texto.Substring(0, 2) + ":" + texto.Substring(2, 2) + ":" + texto.Substring(4, 2);
            return strSaida;
        }

        //protected void btnExcluir_Click(object sender, EventArgs e)
        //{
        //    pnlAvisos.Visible = false;
        //    try
        //    {
        //        ArrayList aryApagar = new ArrayList();
        //        string[] retorno = new string[2];
        //        foreach (GridViewRow grow in grvConsulta.Rows)
        //        {
        //            CheckBox ckb = (CheckBox)grow.Cells[0].FindControl("CheckBox1");
        //            if (ckb.Checked)
        //            {
        //                retorno = ctrl_.Quotations_Delete(grow.Cells[11].Text);
        //            }
        //        }

        //        btnExcluir.Enabled = false;
        //        if (retorno[0].ToString() == "0")
        //            Avisos(retorno[1].ToString(), retorno[0].ToString());
        //        else
        //            Avisos("Excluido(s) com sucesso!", retorno[0].ToString());
        //        carregaGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        Avisos(ex.Message, "0");
        //    }
        //}

        //protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool liga = false;
        //    foreach (GridViewRow grow in grvConsulta.Rows)
        //    {
        //        if (((CheckBox)grow.Cells[0].FindControl("CheckBox1")).Checked)
        //            liga = true;
        //    }
        //    //btnExcluir.Enabled = liga;
        //}

        protected void Avisos(string doc, string tipo)
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

        protected void btnImprimir_Click(object sender, EventArgs e)
        {

            //String url = "~/ImprimeLista.aspx?GRID=DATATABLECOTACOES";
            //Response.Redirect(url);
            //Response.Write(@"<script>window.open('~/ImprimeLista.aspx?GRID=DATATABLECOTACOES','_blank');</script>");
        }
        /// <summary>
        /// Funcoes de paginacao e ordenacao de GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Ordenação e Paginação grvConsulta

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
                Session["SORTCOTACOES"] = dataView.Sort;
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }

        protected void grvConsulta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtClientes = ((DataTable)Session["DATATABLECOTACOES"] != null ? (DataTable)Session["DATATABLECOTACOES"] : carregaGrid());
            grvConsulta.DataSource = ordenaDataTable(dtClientes, true);
            grvConsulta.PageIndex = e.NewPageIndex;
            grvConsulta.DataBind();
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'grvConsulta', 'HeaderDiv');</script>");
        }

        protected void grvConsulta_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtClientes = ((DataTable)Session["DATATABLECOTACOES"] != null ? (DataTable)Session["DATATABLECOTACOES"] : carregaGrid());
            gvOrdenaExpressao = e.SortExpression;
            int pageIndex = grvConsulta.PageIndex;
            grvConsulta.DataSource = ordenaDataTable(dtClientes, false);
            grvConsulta.DataBind();
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'grvConsulta', 'HeaderDiv');</script>");
            grvConsulta.PageIndex = pageIndex;
        }

        protected void grvConsulta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor; this.style.backgroundColor='Orange';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
        }
        #endregion Ordenação e Paginação
    }
}