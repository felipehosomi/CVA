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

namespace WebImcopa.menu
{
    public partial class cmenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string usuario = string.Empty;
                if (Session["USUARIO"] == null) 
                    Page.Response.Redirect("~/Login.aspx", false);
                else
                    usuario = (string)Session["USUARIO"];

                MnuPrincipal.DataSource = Server.MapPath("menu\\menu.xml");
                MnuPrincipal.UserRoles.Add("R1");
                
                //if (usuario == "802687")
                //{
                //    MnuPrincipal.UserRoles.Add("R3");
                //}
                //else
                //{
                //    MnuPrincipal.UserRoles.Add("R2");
                //}

                MnuPrincipal.UserRoles.Add("R2");                
                MnuPrincipal.UserRoles.Add("R4");
                MnuPrincipal.UserRoles.Add("R5");
                if (usuario == ConfigurationManager.AppSettings["administrador"].ToString())
                {
                    MnuPrincipal.UserRoles.Add("R6");
                }
                MnuPrincipal.DataBind();
            }
        }
    }
}