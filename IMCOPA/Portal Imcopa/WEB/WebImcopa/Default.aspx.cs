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
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               // try
               // {
               //     RfcDestinationManager.RegisterDestinationConfiguration(new
               //         util.MyBackendConfig());//1
               // }
               // catch (RfcInvalidStateException ex)
               // {

               // }
                configuraRepresentante();
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
    }
}
