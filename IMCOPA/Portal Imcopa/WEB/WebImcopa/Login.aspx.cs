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
using WebImcopa.control;
using System.DirectoryServices;
using System.Text;

namespace WebImcopa
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Abandon();
                ((ScriptManager)Page.FindControl("MasterScript")).SetFocus(UserName);
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string codrep = String.Empty;
            try
            {
                bool existe = ADHelper.IsUserValid(UserName.Text.Trim(), Password.Text.Trim());
                if (existe)
                {
                    Session["USUARIO"] = UserName.Text.Trim();

                    if (Decrypt(ConfigurationManager.AppSettings["ADDefaultPassword"]) == Password.Text.Trim())
                    {
                        Page.Response.Redirect("~/Senha.aspx", false);
                    }
                    else
                    {
                        Page.Response.Redirect("~/Default.aspx", false);
                    }
                }
                else
                {
                    Avisos("0", "USUARIO E/OU SENHA INVALIDO");
                }
            }
            catch (Exception ex)
            {
                Avisos("0", "ERRO: " + ex.InnerException.Message);
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

        protected void lkbNovaSenha_Click(object sender, EventArgs e)
        {
            var ex = new Exception();
            if (UserName.Text.Trim() == "")
            {
                Avisos("0", "Para solicitar uma nova senha favor informar o usuário.");
            }
            else
            {
                SAPService ctrl_ = new SAPService();

                string codrep = UserName.Text.Trim();

                if (Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]) != String.Empty)
                    codrep = Convert.ToString(ConfigurationManager.AppSettings["AUTHO"]);

                var mensagem = String.Format("Sua nova senha de acesso ao Portal de Vendas é <b>{0}</b>. Favor realizar a alteração dessa senha após acessar o portal.", Decrypt(ConfigurationManager.AppSettings["ADDefaultPassword"]));
                var emailRepresentante = string.Empty;
                var fornecedor = ctrl_.Quotations_Fornecedor(codrep, 0);

                if (fornecedor[2] != null)
                    emailRepresentante = fornecedor[2].ToString();

                if (!string.IsNullOrEmpty(emailRepresentante))
                {
                    bool senhaAlterada = ADHelper.ResetPassword(UserName.Text.Trim(), out ex);

                    if (senhaAlterada)
                    {
                        if (ctrl_.EnviarEmail(emailRepresentante, "Nova Senha de Acesso", mensagem))
                        {
                            Avisos("1", "Email enviado com sucesso");
                        }
                        else
                        {
                            Avisos("0", "Ocorreu um erro ao enviar o Email...");
                        }
                    }
                    else
                    {
                        Avisos("0", "Ocorreu um erro ao redefinir a senha...");
                    }
                }
                else
                {
                    Avisos("0", "E-mail não encontrado...");
                }
                
            }
        }

        public static string Decrypt(string strSenha)
        {
            try
            {
                Byte[] b = Convert.FromBase64String(strSenha);
                string strDecrypted = ASCIIEncoding.UTF8.GetString(b);
                return strDecrypted;
            }
            catch
            {
                throw;
            }
        }
    }
}
