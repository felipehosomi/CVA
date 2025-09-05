using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;
using WebImcopa.control;

namespace WebImcopa
{
    public partial class Senha : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                configuraRepresentante();
             //   UserName.Text = Session["USUARIO"].ToString();
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
                UserName.Text = codrep;
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

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            var _ex = new Exception();
            try
            {
                if (ADHelper.IsUserValid(UserName.Text.Trim(), Password.Text.Trim()))
                {
                    if (PasswordNew.Text.Trim().Equals(PasswordNewRep.Text.Trim()))
                    {
                        ADHelper.SetUserPassword(UserName.Text.Trim(), Password.Text.Trim(), PasswordNew.Text.Trim(), out _ex);

                        if (_ex == null)
                            Avisos("1", "Senha alterada com sucesso!");
                        else
                            Avisos("0", _ex.Message);
                    }
                    else
                    {
                        Avisos("0", "As entradas de sua nova senha não combinam!");
                    }
                }
                else
                {
                    Avisos("0", "Login e/ou senha inválido!");
                }
            }
            catch (Exception ex)
            {
                Avisos("0", ex.Message);
            }
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
    }
}
