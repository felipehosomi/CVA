using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebImcopa.control;
using System.Data;
using System.Configuration;
using System.IO;

namespace WebImcopa
{
    public partial class ConsultaHistoricoVendas : System.Web.UI.Page
    {
        private SAPService _ctrl = new SAPService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlAvisos.Visible = false;                
                configuraRepresentante();
                btnOk.Enabled = true;
                extModal.Show();
            }
        }

        private DataTable CarregarDadosGrid()
        {
            try
            {
                string codigoCliente = !string.IsNullOrEmpty(txtCodigoCliente.Text) ? txtCodigoCliente.Text.PadLeft(10, '0') : string.Empty;
                string dtInicio = !string.IsNullOrEmpty(txtDataInicio.Text) ? String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtDataInicio.Text)) : string.Empty;
                string dtFim = !string.IsNullOrEmpty(txtDataFim.Text) ? String.Format("{0:yyyyMMdd}", Convert.ToDateTime(txtDataFim.Text)) : string.Empty;
                string corretor = Session["USUARIO"] != null ? Session["USUARIO"].ToString() : string.Empty;

                var dtRetorno = _ctrl.Sales_Get_History(string.Empty, 0, codigoCliente, string.Empty, 0, string.Empty, string.Empty, dtInicio, dtFim, corretor);
                Session["GRIDHISTORICO"] = dtRetorno;

                return dtRetorno;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void configuraRepresentante()
        {
            try
            {
                SAPService ctrl_ = new SAPService();
                object[] objRetorno = new object[3];
                DataTable dtVendas = new DataTable();

                if (Session["USUARIO"] == null)
                    Page.Response.Redirect("~/Login.aspx", false);

                string codrep = Session["USUARIO"].ToString();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                lblcdRepresentante.Text = codrep;                

                objRetorno = ctrl_.Quotations_Fornecedor(codrep, 1);
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

                    //ddlVendas.DataSource = dtVendas_aux;
                    //ddlVendas.DataBind();
                    //ddlVendas.SelectedIndex = 0;
                }
            }
            catch
            {
                lblVendedor.Text = "Fornecedor não encontrado!";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtCodigoCliente);
            pnlAvisos.Visible = false;
            btnOk.Enabled = true;
            extModal.Show();
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            string fileName = "HistoricoVendas_" + DateTime.Now.ToString("dd_MM-yyyy");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            var dt = Session["GRIDHISTORICO"] != null ? (DataTable)Session["GRIDHISTORICO"] : CarregarDadosGrid();
            
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Table table = new Table();
                    
                    if (gridViewHistoricoVenda.HeaderRow != null)
                    {
                        var row = new TableRow();
                        
                        row.Cells.Add(new TableCell { Text = "Número da OV" });
                        row.Cells.Add(new TableCell { Text = "Fatura", HorizontalAlign = HorizontalAlign.Right });
                        row.Cells.Add(new TableCell { Text = "Quantidade", HorizontalAlign = HorizontalAlign.Right });
                        row.Cells.Add(new TableCell { Text = "Valor", HorizontalAlign = HorizontalAlign.Right });
                        row.Cells.Add(new TableCell { Text = "Cidade", HorizontalAlign = HorizontalAlign.Left });
                        row.Cells.Add(new TableCell { Text = "Código Cliente", HorizontalAlign = HorizontalAlign.Right });
                        row.Cells.Add(new TableCell { Text = "Cliente", HorizontalAlign = HorizontalAlign.Left });

                        table.Rows.Add(row);                        
                    }

                    foreach (GridViewRow row in gridViewHistoricoVenda.Rows)
                    {
                        PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    if (gridViewHistoricoVenda.FooterRow != null)
                    {
                        PrepareControlForExport(gridViewHistoricoVenda.FooterRow);
                        table.Rows.Add(gridViewHistoricoVenda.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

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
        
        protected void btnOK_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = false;

            DataTable dt_List = CarregarDadosGrid();

            

            if (dt_List.Rows.Count == 0)
            {
                string errorMsg = "Nenhum registro encontrado";
                if (string.IsNullOrEmpty(txtDataInicio.Text) || string.IsNullOrEmpty(txtDataFim.Text))
                    errorMsg = "Nenhum registro encontrado. Necessário informar Data Início e Fim para realizar a consulta.";
                
                Avisos(errorMsg, "0");
                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewHistoricoVenda', 'HeaderDiv');</script>");
            }
            else
            {
                lblQuant.Text = calculatotal(dt_List, 2);
                lblValor.Text = calculatotal(dt_List, 3);

                gridViewHistoricoVenda.DataSource = dt_List;
                gridViewHistoricoVenda.DataBind();

                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewHistoricoVenda', 'HeaderDiv');</script>");
            }
        }
        private string calculatotal(DataTable dt_List, int col)
        {
            double total = 0;
            foreach (DataRow row in dt_List.Rows)
            {
                total += Convert.ToDouble(row[col].ToString());
            }
            return total.ToString();
        }
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
        
        protected void gridViewHistoricoVenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor; this.style.backgroundColor='Orange';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[2].Text = String.Format(@"{0:N0}", Convert.ToDouble(lblQuant.Text));
                e.Row.Cells[3].Text = String.Format(@"{0:N2}", Convert.ToDouble(lblValor.Text));
            }
        }

        #region [ Ordenação ]

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
                Session["SORTHISTORICO"] = dataView.Sort;
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }
        
        protected void gridViewHistoricoVenda_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtHistorico = ((DataTable)Session["GRIDHISTORICO"] != null ? (DataTable)Session["GRIDHISTORICO"] : CarregarDadosGrid());
            gvOrdenaExpressao = e.SortExpression;
            int pageIndex = gridViewHistoricoVenda.PageIndex;
            gridViewHistoricoVenda.DataSource = ordenaDataTable(dtHistorico, false);
            gridViewHistoricoVenda.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewHistoricoVenda', 'HeaderDiv');</script>");
            gridViewHistoricoVenda.PageIndex = pageIndex;
        }

        #endregion

        protected void gridViewHistoricoVenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtHistorico = ((DataTable)Session["GRIDHISTORICO"] != null ? (DataTable)Session["GRIDHISTORICO"] : CarregarDadosGrid());
            gridViewHistoricoVenda.DataSource = ordenaDataTable(dtHistorico, true);
            gridViewHistoricoVenda.PageIndex = e.NewPageIndex;
            gridViewHistoricoVenda.DataBind();            
        }

    }
}