<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CovantaWebForms.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <title>Admin Login</title>
    <link rel="stylesheet" href="/css/auth.css">
</head>

<body>
    <div class="lowin lowin-blue">
        <div class="lowin-wrapper">
            <div class="lowin-box lowin-login">
                <div class="lowin-box-inner">
                    <form id="frmLogin" runat="server">
                        <p>Sign in to continue</p>

                        <div class="lowin-group">
                            <label>Email <a href="#" class="login-back-link">Sign in?</a></label>
                            <asp:TextBox ID="txtEmail" CssClass="lowin-input" runat="server" />
                            <asp:CustomValidator Display="None" ValidationGroup="valGroup" ID="CustomValidator1" runat="server" ErrorMessage="" ControlToValidate="txtEmail" ClientValidationFunction="ValidateTextBox" EnableClientScript="false" ValidateEmptyText="True" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                        </div>
                        <div class="lowin-group password-group">
                            <label>Password</label>
                            <asp:TextBox ID="txtPassword" CssClass="lowin-input" runat="server" TextMode="Password"/>
                        </div>
                        <asp:Button ID="btnSignIn" runat="server" Text="Sign In" CssClass="lowin-btn login-btn" OnClick="btnSignIn_Click" ValidationGroup="valGroup"/>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" Text="Email or Password is incorrect."/>
                    </form>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
