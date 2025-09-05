using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebImcopa.control;

namespace WebImcopa
{
    public partial class ConsultaMetas : Page
    {
        private SAPService ctrl = new SAPService();

        private string GvOrdenaSentido
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string GvOrdenaExpressao
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        public string Cut(object descricao, int def)
        {
            var tamanho = descricao.ToString().Length;
            var cutSize = def;

            if (tamanho < cutSize)
            {
                cutSize = tamanho;
            }

            var retorno = descricao.ToString().Substring(0, cutSize);

            if (cutSize == def)
            {
                retorno += "...";
            }

            return retorno;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ConfiguraRepresentante();
                pnlAvisos.Visible = false;
                this.AtualizaDados(true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            pnlAvisos.Visible = false;
            this.AtualizaDados(true);
        }

        protected void Avisos(string tipo, string doc)
        {
            pnlAvisos.Visible = true;

            switch (tipo)
            {
                case "1":
                    this.imgAvisos.ImageUrl = "~/Imagens/Ok16.gif";
                    this.lblAvisos.Text = doc;
                    break;
                case "0":
                    this.imgAvisos.ImageUrl = "~/Imagens/Exclamation16.gif";
                    this.lblAvisos.Text = " Erro: " + doc;
                    break;
            }
        }

        protected void gridViewMetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var dtMetas = (DataTable)this.Session["DATATABLEMETAS"] ?? this.ctrl.Get_Products_Goal_Realized(lblcdRepresentante.Text);
            gridViewMetas.DataSource = this.OrdenaDataTable(dtMetas, true);
            gridViewMetas.PageIndex = e.NewPageIndex;
            gridViewMetas.DataBind();
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewMetas', 'HeaderDiv');</script>");
        }

        protected void gridViewMetas_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dtMetas = (DataTable)this.Session["DATATABLEMETAS"] ?? this.ctrl.Get_Products_Goal_Realized(lblcdRepresentante.Text);
            this.GvOrdenaExpressao = e.SortExpression;
            var pageIndex = gridViewMetas.PageIndex;
            gridViewMetas.DataSource = this.OrdenaDataTable(dtMetas, false);
            gridViewMetas.DataBind();
            gridViewMetas.PageIndex = pageIndex;
            ClientScript.RegisterStartupScript(this.GetType(), "CreateGridHeader", "<script>CreateGridHeader('DataDiv', 'gridViewMetas', 'HeaderDiv');</script>");
        }

        protected void gridViewMetas_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }

        private void ConfiguraRepresentante()
        {
            try
            {
                this.ctrl = new SAPService();
                var objRetorno = new object[4];

                if (Session["USUARIO"] == null)
                {
                    Page.Response.Redirect("~/Login.aspx", false);
                }

                var codrep = Session["USUARIO"].ToString();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != string.Empty)
                {
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);
                }

                lblcdRepresentante.Text = codrep;
                objRetorno = this.ctrl.Quotations_Fornecedor(codrep, 1);
                lblVendedor.Text = objRetorno[1].ToString();
            }
            catch
            {
                lblVendedor.Text = "Fornecedor não encontrado!";
            }
        }

        private void AtualizaDados(bool atualizar)
        {
            if (atualizar)
            {
                Session["DATATABLEMETAS"] = null;
                Session["DATATABLEMETAS"] = this.ctrl.Get_Products_Goal_Realized(lblcdRepresentante.Text);
            }

            var dtList = (DataTable)this.Session["DATATABLEMETAS"] ?? this.ctrl.Get_Products_Goal_Realized(lblcdRepresentante.Text);

            if (dtList.Rows.Count == 0)
            {
                if (IsPostBack)
                {
                    Avisos("0", "Nenhum registro encontrado.");
                }
            }

            gridViewMetas.DataSource = dtList;
            gridViewMetas.DataBind();

            ClientScript.RegisterStartupScript(
                this.GetType(),
                "CreateGridHeader",
                "<script>CreateGridHeader('DataDiv', 'gridViewMetas', 'HeaderDiv');</script>");

        }

        private string GetOrdenaSentido()
        {
            switch (this.GvOrdenaSentido)
            {
                case "ASC":
                    this.GvOrdenaSentido = "DESC";
                    break;
                case "DESC":
                    this.GvOrdenaSentido = "ASC";
                    break;
            }

            return this.GvOrdenaSentido;
        }

        private DataView OrdenaDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable == null)
            {
                return new DataView();
            }

            var dataView = new DataView(dataTable);

            if (this.GvOrdenaExpressao != string.Empty)
            {
                dataView.Sort = string.Format("{0} {1}", this.GvOrdenaExpressao, isPageIndexChanging ? this.GvOrdenaSentido : this.GetOrdenaSentido());
            }

            this.Session["SORTMETAS"] = dataView.Sort;
            return dataView;
        }
    }
}