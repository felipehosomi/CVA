using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using WebImcopa.control;
using System.Collections.Specialized;

namespace WebImcopa
{
    public partial class Detalhe : System.Web.UI.Page
    {
        SAPService _ctrl = new SAPService();
        NameValueCollection collectionValues;
        string codrep = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            collectionValues = HttpContext.Current.Request.QueryString;
            if (!Page.IsPostBack)
            {
                if (collectionValues.Count > 0)
                {
                    configuraRepresentante();
                    string cotacao = collectionValues.Get("cod");
                    this.consultaCotacoes(codrep, cotacao);
                }
            }
        }

        private void configuraRepresentante()
        {
            if (Session["USUARIO"] == null)
                Page.Response.Redirect("~/Login.aspx", false);
            else
                codrep = Session["USUARIO"].ToString();
            if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);
        }

        private void consultaCotacoes(string strRepres, string strCotacao)
        {
            DataTable ldt_tracking = new DataTable();

            if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                strRepres = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

            string data_criacao = "";
            string embal = "0";
            string total = "0";
            string quant = "0";
            string unitrs = "0";
            decimal total_itens = 0;
            string stotal_itens = "";
            int imatnr = 0;

            DataTable ldt_cab = new DataTable();
            DataTable ldt_datagrid = new DataTable();
            ldt_datagrid.Columns.Add(new DataColumn("Matnr", typeof(string)));
            ldt_datagrid.Columns.Add(new DataColumn("Descr", typeof(string)));
            ldt_datagrid.Columns.Add(new DataColumn("Embal", typeof(decimal)));
            ldt_datagrid.Columns.Add(new DataColumn("Total", typeof(string)));
            ldt_datagrid.Columns.Add(new DataColumn("Quant", typeof(string)));
            ldt_datagrid.Columns.Add(new DataColumn("Unitrs", typeof(string)));

            DataTable ldt_item = new DataTable();

            object[] objRetorno = new object[3];
            objRetorno = _ctrl.Quotations_Tracking_Detail(strRepres, strCotacao);
            ldt_cab = (DataTable)objRetorno[0];
            ldt_item = (DataTable)objRetorno[1];

            if (ldt_cab.Rows.Count > 0)
            {

                foreach (DataRow row in ldt_cab.Rows)
                {
                    this.Lbcliente.Text = row["Name1"].ToString() + " - " + row["Ort01"].ToString() + " " + row["Regio"].ToString();
                    this.Lbcdcliente.Text = row["Kunnr"].ToString();
                    this.Lbcnpj.Text = row["Stcd1"].ToString();
                    this.Lbcondpg.Text = objRetorno[2].ToString();
                    this.Lbnumcot.Text = strCotacao;
                }

                foreach (DataRow row in ldt_item.Rows)
                {
                    DataRow dr = ldt_datagrid.NewRow();

                    imatnr = Convert.ToInt32(row["Matnr"]);
                    dr["Matnr"] = Convert.ToString(imatnr);
                    dr["Descr"] = row["Descr"];

                    embal = Convert.ToString(row["Embal"]);
                    total = Convert.ToString(row["Total"]);
                    quant = Convert.ToString(row["Quant"]);
                    unitrs = Convert.ToString(row["Unitrs"]);

                    dr["Embal"] = embal;
                    total = "R$ " + total.Replace(".", ",");
                    dr["Total"] = total;
                    total_itens = total_itens + Convert.ToDecimal(row["Total"]);
                    if (quant.Length > 0)
                        quant = quant.Substring(0, quant.IndexOf(","));
                    //if(quant.Length > 0)
                    //       quant = quant.Substring(0, quant.IndexOf("."));
                    dr["Quant"] = quant;

                    if (unitrs.Length > 0)
                        unitrs = unitrs.Substring(0, unitrs.Length - 1);
                    unitrs = "R$ " + unitrs.Replace(".", ",");
                    dr["Unitrs"] = unitrs;

                    this.Lbnumcot.Text = row["Vbeln"].ToString();
                    data_criacao = row["Erdat"].ToString();
                    if (!data_criacao.Equals(""))
                    {
                        data_criacao = data_criacao.Substring(6, 2) + "/" + data_criacao.Substring(4, 2) + "/" + data_criacao.Substring(0, 4);
                        this.Lbdtcriacao.Text = data_criacao;
                    }

                    ldt_datagrid.Rows.Add(dr);
                }
                this.DataGridDetail.DataSource = ldt_datagrid;
                this.DataGridDetail.DataBind();
                this.DataGridDetail.Visible = true;

                stotal_itens = Convert.ToString(total_itens);
                stotal_itens = "R$ " + stotal_itens.Replace(".", ",");

                this.Lbtotal.Text = stotal_itens;
                this.Lbcondpg.Text = objRetorno[2].ToString();
            }
        }
    }
}
