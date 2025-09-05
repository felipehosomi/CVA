using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WebImcopa.control;

namespace WebImcopa
{
    public partial class Configuracao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                configuraRepresentante();
                carregaAD();
                carregaConexao();
                carregaInf();
                pnlAvisos.Visible = false;
            }
        }
        private void carregaConexao()
        {
            try
            {
                string Configuracao = ConfigurationManager.AppSettings["R3"].ToString();
                string[] confs = Configuracao.Split(' ');
                txtCliente.Text = confs[0].Split('=')[1].Trim();
                txtUsuario.Text = confs[1].Split('=')[1].Trim();
                txtSenha.Text = confs[2].Split('=')[1].Trim();
                txtLang.Text = confs[3].Split('=')[1].Trim();
                txtHost.Text = confs[4].Split('=')[1].Trim();
                txtSysnr.Text = confs[5].Split('=')[1].Trim();
            }
            catch
            {
                Avisos("0", "Erro ao carregar conexão SAP - Verificar WEB.CONFIG");
            }
        }
        private void carregaAD()
        {
            try
            {
                string ADuser = ConfigurationManager.AppSettings["ADAdminUser"].ToString();
                string ADpasswd = ConfigurationManager.AppSettings["ADAdminPassword"].ToString();
                string ADServer = ConfigurationManager.AppSettings["ADServer"].ToString();
                string ADPath = ConfigurationManager.AppSettings["ADPath"].ToString();
                txtAdministradorAD.Text = ADHelper.Decrypt(ADuser);
                txtSenhaAD.Text = ADHelper.Decrypt(ADpasswd);
                txtADServer.Text = ADServer;
                txtADPath.Text = ADPath;
            }
            catch
            {
                Avisos("0", "Erro ao carregar dados do AD - Verificar WEB.CONFIG");
            }
        }
        private void carregaInf()
        {
            try
            {
                string strAutho = ConfigurationManager.AppSettings["AUTHO"].ToString();
                string strFemb = ConfigurationManager.AppSettings["FEMB"].ToString();
                string strFrete = ConfigurationManager.AppSettings["FRETE"].ToString();
                string strCpagto = ConfigurationManager.AppSettings["CPAGTO"].ToString();
                txtAutho.Text = strAutho;
                txtFemb.Text = strFemb;
                txtFrete.Text = strFrete;
                txtPagto.Text = strCpagto;
            }
            catch
            {
                Avisos("0", "Erro ao carregar dados adicionais - Verificar WEB.CONFIG");
            }
        }

        private void configuraRepresentante()
        {
            try
            {
                SAPService _ctrl = new SAPService();

                if (Session["USUARIO"] == null)
                    Page.Response.Redirect("~/Login.aspx", false);

                string codrep = Session["USUARIO"].ToString();
                
                if (codrep != ConfigurationManager.AppSettings["administrador"].ToString())
                    Page.Response.Redirect("~/Login.aspx", false);

                // busca o representante de acordo com o código informado
                lblcdRepresentante.Text = codrep;
                lblVendedor.Text = _ctrl.Quotations_Fornecedor(codrep, 0)[1].ToString();
            }
            catch
            {
                lblVendedor.Text = "Fornecedor não encontrado!";
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

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            pnlAvisos.Visible = false;
        }

        protected void btnOK01a_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                string value = "CLIENT=" + txtCliente.Text.Trim() +
                              " USER=" + txtUsuario.Text.Trim() +
                              " PASSWD=" + txtSenha.Text.Trim() +
                              " LANG=" + txtLang.Text.Trim() +
                              " ASHOST=" + txtHost.Text.Trim() +
                              " SYSNR=" + txtSysnr.Text.Trim();


                config.AppSettings.Settings.Remove("R3");
                config.AppSettings.Settings.Add(new KeyValueConfigurationElement("R3", value));

                config.Save();
                Avisos("1", "Gravado com sucesso");
            }
            catch (Exception ex)
            {
                Avisos("0", "Falha ao gravar - " + ex.Message);
            }
        }

        protected void btnOK02_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

                if (txtAdministradorAD.Text != String.Empty && txtSenhaAD.Text != String.Empty)
                {
                    if (txtADServer.Text != String.Empty)
                    {
                        config.AppSettings.Settings.Remove("ADServer");
                        config.AppSettings.Settings.Add(new KeyValueConfigurationElement("ADServer", txtADServer.Text));
                    }
                    if (txtADPath.Text != String.Empty)
                    {
                        config.AppSettings.Settings.Remove("ADPath");
                        config.AppSettings.Settings.Add(new KeyValueConfigurationElement("ADPath", txtADPath.Text));
                    }
                    if (txtAdministradorAD.Text != String.Empty)
                    {
                        config.AppSettings.Settings.Remove("ADAdminUser");
                        config.AppSettings.Settings.Add(new KeyValueConfigurationElement("ADAdminUser", ADHelper.Encrypt(txtAdministradorAD.Text)));
                    }
                    if (txtSenhaAD.Text != String.Empty)
                    {
                        config.AppSettings.Settings.Remove("ADAdminPassword");
                        config.AppSettings.Settings.Add(new KeyValueConfigurationElement("ADAdminPassword", ADHelper.Encrypt(txtSenhaAD.Text)));
                    }
                }
                config.Save();
                Avisos("1", "Gravado com sucesso");
            }
            catch (Exception ex)
            {
                Avisos("0", "Falha ao gravar - " + ex.Message);
            }
        }

        protected void btnOK03_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

                config.AppSettings.Settings.Remove("AUTHO");
                config.AppSettings.Settings.Add(new KeyValueConfigurationElement("AUTHO", txtAutho.Text));

                if (txtFemb.Text != String.Empty)
                {
                    config.AppSettings.Settings.Remove("FEMB");
                    config.AppSettings.Settings.Add(new KeyValueConfigurationElement("FEMB", txtFemb.Text));
                }
                if (txtFrete.Text != String.Empty)
                {
                    config.AppSettings.Settings.Remove("FRETE");
                    config.AppSettings.Settings.Add(new KeyValueConfigurationElement("FRETE", txtFrete.Text));
                }
                if (txtPagto.Text != String.Empty)
                {
                    config.AppSettings.Settings.Remove("CPAGTO");
                    config.AppSettings.Settings.Add(new KeyValueConfigurationElement("CPAGTO", txtPagto.Text));
                }

                config.Save();
                Avisos("1", "Gravado com sucesso");
            }
            catch (Exception ex)
            {
                Avisos("0", "Falha ao gravar - " + ex.Message);
            }

        }
    }
}
