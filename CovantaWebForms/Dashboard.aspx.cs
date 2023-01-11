using System;

namespace CovantaWebForms
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FormsUser"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                lblUserName.Text = Session["FormsUser"].ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["FormsUser"] = null;
            Response.Redirect("login.aspx");
        }
    }
}