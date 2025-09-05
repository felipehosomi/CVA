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
using System.Drawing;
using ImcopaWEB;
using WebImcopa.control;
using System.Text;
using System.Collections.Specialized;

namespace WebImcopa
{
    public partial class ImprimeLista : System.Web.UI.Page
    {
        SAPService rfcs = new SAPService();
        string codrep = String.Empty;
        NameValueCollection collectionValues;

        protected void Page_Load(object sender, EventArgs e)
        {
            collectionValues = HttpContext.Current.Request.QueryString;
            if (!Page.IsPostBack)
            {
                if (collectionValues.Count > 0)
                {
                    string strSession = collectionValues.Get("GRID");
                    this.carregaDados(strSession);
                }
            }
        }

        private void carregaDados(string strSession)
        {

            try
            {
                DataTable dt_List = new DataTable();

                dt_List = (DataTable)Session[strSession];

                if (dt_List != null)
                {
                    if (strSession == "DATATABLECLIENTES")
                    {
                        lblTitulo.Text = "Relatório de Clientes";
                        gridViewClientes.Visible = true;
                        gridViewClientes.DataSource = ordenaDataTable(dt_List, "SORTCLIENTES");
                        gridViewClientes.DataBind();
                    }
                    else if (strSession == "DATATABLECOTACOES")
                    {
                        lblTitulo.Text = "Relatório de Cotações";
                        grvConsulta.Visible = true;
                        grvConsulta.DataSource = ordenaDataTable(dt_List, "SORTCOTACOES");
                        grvConsulta.DataBind();
                    }
                    else if (strSession == "DATATABLETITULOS")
                    {
                        lblTitulo.Text = "Relatório de Títulos Abertos";
                        grvTitulos.Visible = true;
                        lblTotal.Text = calculatotal(dt_List);
                        grvTitulos.DataSource = ordenaDataTable(dt_List, "SORTTITULOS");
                        grvTitulos.DataBind();
                    }
                    else if (strSession == "DATATABLEFATURAMENTOS")
                    {
                        lblTitulo.Text = "Relatório de Faturamentos";
                        gridViewFaturamentos.Visible = true;
                        lblQuant.Text = calculatotal(dt_List, 10);
                        lblValor.Text = calculatotal(dt_List, 11);
                        gridViewFaturamentos.DataSource = ordenaDataTable(dt_List, "SORTFATURAS");
                        gridViewFaturamentos.DataBind();
                    }
                    else if (strSession == "GRIDHISTORICO")
                    {
                        lblTitulo.Text = "Relatório de Histórico de Vendas";
                        gridViewHistoricoVenda.Visible = true;
                        gridViewHistoricoVenda.DataSource = ordenaDataTable(dt_List, "SORTHISTORICO");
                        gridViewHistoricoVenda.DataBind();
                    }
                    else if (strSession == "DATATABLEMETAS")
                    {
                        lblTitulo.Text = "Relatório de Metas por Produto";
                        gridViewMetas.Visible = true;
                        gridViewMetas.DataSource = ordenaDataTable(dt_List, "SORTMETAS");
                        gridViewMetas.DataBind();
                    }
                }
            }
            catch
            {
                throw;
            }

        }
        protected void gridViewClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[9].Text = e.Row.Cells[9].Text.Replace("&nbsp;", String.Empty);
                if (e.Row.Cells[9].Text != String.Empty)
                {
                    e.Row.Font.Bold = true;
                    e.Row.ForeColor = Color.Red;
                }
            }
        }
        protected void grvTitulos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[4].Text = String.Format(@"{0:N0}", Convert.ToDouble(lblTotal.Text));
            }
        }
        protected void gridViewFaturamentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[10].Text = String.Format(@"{0:N0}", Convert.ToDouble(lblQuant.Text));
                e.Row.Cells[11].Text = String.Format(@"{0:N2}", Convert.ToDouble(lblValor.Text));
            }
        }
        protected void gridViewHistoricoVenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
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
        private string calculatotal(DataTable dt_List, int col)
        {
            double total = 0;
            foreach (DataRow row in dt_List.Rows)
            {
                total += Convert.ToDouble(row[col].ToString());
            }
            return total.ToString();
        }
        /// <summary>
        /// Completa com zeros a esquerda da string para realizar consultas no SAP
        /// </summary>
        /// <param name="valor">string</param>
        /// <param name="tamanho">string</param>
        /// <returns>string</returns>
        private string espaco_dir(string valor, int tamanho)
        {
            int qtd = (tamanho - valor.Length);
            StringBuilder sb = new StringBuilder();
            sb.Append(valor.ToUpper());
            if (qtd > 0)
            {
                for (int i = 0; i < qtd; i++)
                {
                    sb.Append("&nbsp;");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Funcoes de paginacao e ordenacao de GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Ordenação e Paginação grvConsulta

        private DataView ordenaDataTable(DataTable dataTable, string strSort)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (Session[strSort] != null)
                {
                    dataView.Sort = Session[strSort].ToString();
                    return dataView;
                }
                else
                {
                    return dataView;
                }
            }
            else
            {
                return new DataView();
            }
        }
        #endregion Ordenação e Paginação
    }
}
