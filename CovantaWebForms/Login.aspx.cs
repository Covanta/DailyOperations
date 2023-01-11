using CovantaWebForms.Classes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace CovantaWebForms
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string name;
            if (LoginHelper.VerifyUser(email, password, out name))
            {
                Session["FormsUser"] = name;
                Response.Redirect("dashboard.aspx");
            }
            else
            {
                lblError.Visible = true;
            }
        }
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool is_emailValid = txtEmail.Text != "" && new EmailAddressAttribute().IsValid(txtEmail.Text);
            string border = is_emailValid ? "none" : "2px solid red";
            txtEmail.Style.Add("border", border);

            bool is_passwordValid = txtPassword.Text != "";
            border = is_passwordValid ? "none" : "2px solid red";
            txtPassword.Style.Add("border", border);

            args.IsValid = is_emailValid && is_passwordValid;
        }
    }
}