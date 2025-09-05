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

namespace WebImcopa
{
    public partial class ConsultaTitulos : System.Web.UI.Page
    {
        SAPService ctrl_ = new SAPService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                configuraRepresentante();
                pnlAvisos.Visible = false;
                if (Session["DATATABLETITULOS"] == null)
                {
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
                SAPService ctrl_ = new SAPService();

                if (Session["USUARIO"] == null)
                    Page.Response.Redirect("~/Login.aspx", false);

                string codrep = Session["USUARIO"].ToString();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                // busca o representante de acordo com o código informado
                lblcdRepresentante.Text = codrep;
                lblVendedor.Text = ctrl_.Quotations_Fornecedor(codrep, 0)[1].ToString();
            }
            catch
            {
                lblVendedor.Text = "Fornecedor não encontrado!";
            }
        }
        private ZSDE028 filtro()
        {
            ZSDE028 i_header = new ZSDE028();

            i_header.Lifnr = zeroesc(lblcdRepresentante.Text, 10);
            //i_header.Kunnr = txtCodCliente.Text;
            //i_header.Name1 = txtNome.Text;

            //string CPF = txtCPF.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();
            string CNPJ = txtCNPJ.Text.Replace('_', ' ').Replace('/', ' ').Replace('.', ' ').Replace(',', ' ').Replace('-', ' ').Replace(" ", string.Empty).Trim();

            if (txtDataIni.Text != String.Empty)
            {
                DateTime dtInicial = Convert.ToDateTime(txtDataIni.Text);
                i_header.Dtlow = dtInicial.ToString("yyyyMMdd");
            }
            if (txtDataIni.Text != String.Empty && txtDataFin.Text != String.Empty)
            {
                DateTime dtFinal = Convert.ToDateTime(txtDataFin.Text);
                i_header.Dthigh = dtFinal.ToString("yyyyMMdd");
            }
            i_header.Stcd1 = CNPJ;
            //i_header.Stcd3 = CPF;

            return i_header;
        }
        private void atualizaDados(bool atualizar)
        {
            DataTable dt_List = new DataTable();

            if (atualizar)
            {
                //Session["DATATABLETITULOS"] = null;
                Session.Remove("DATATABLETITULOS");
                Session["DATATABLETITULOS"] = ctrl_.List_Customer(filtro(), 1);
            }

            dt_List = ((DataTable)Session["DATATABLETITULOS"] != null ? (DataTable)Session["DATATABLETITULOS"] : ctrl_.List_Customer(filtro(), 1));

            if (dt_List.Rows.Count == 0)
            {
                Avisos("0", "Nenhum registro encontrado.");
            }
            else
            {
                lblTotal.Text = calculatotal(dt_List).ToString();
                gridViewClientes.DataSource = dt_List;
                gridViewClientes.DataBind();
                ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewClientes', 'HeaderDiv');</script>");
            }
        }

        private string calculatotal(DataTable dt_List)
        {
            double total = 0;
            foreach (DataRow row in dt_List.Rows)
            {
                total += Convert.ToDouble(row["DMBTR"].ToString());
            }
            return total.ToString();
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            //btnImprimir.Enabled = true;
            //Session["DATATABLETITULOS"] = null;
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
                Session["SORTTITULOS"] = dataView.Sort;
                return dataView;
            }
            else
            {
                return new DataView();
            }
        }

        protected void gridViewClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtClientes = ((DataTable)Session["DATATABLETITULOS"] != null ? (DataTable)Session["DATATABLETITULOS"] : ctrl_.List_Customer(filtro(), 1));
            gridViewClientes.DataSource = ordenaDataTable(dtClientes, true);
            gridViewClientes.PageIndex = e.NewPageIndex;
            gridViewClientes.DataBind();
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewClientes', 'HeaderDiv');</script>");
        }

        protected void gridViewClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtClientes = ((DataTable)Session["DATATABLETITULOS"] != null ? (DataTable)Session["DATATABLETITULOS"] : ctrl_.List_Customer(filtro(), 1));
            gvOrdenaExpressao = e.SortExpression;
            int pageIndex = gridViewClientes.PageIndex;
            gridViewClientes.DataSource = ordenaDataTable(dtClientes, false);
            gridViewClientes.DataBind();
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
                e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor; this.style.backgroundColor='Orange';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[4].Text = String.Format(@"{0:N0}", Convert.ToDouble(lblTotal.Text));
            }
        }
        #endregion Ordenação e Paginação

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pnlAvisos.Visible = false;
            ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(txtCNPJ);
            extModal.Show();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            pnlAvisos.Visible = false;
            extModal.Show();
        }
    }
}
