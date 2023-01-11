<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CovantaWebForms.Dashboard" %>

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
        }

        .topnav {
            overflow: hidden;
            background-color: #0081C6;
        }

            .topnav a {
                float: left;
                color: #f2f2f2;
                text-align: center;
                padding: 14px 16px;
                text-decoration: none;
                font-size: 17px;
            }

                .topnav a:hover {
                    background-color: #ddd;
                    color: black;
                }

                .topnav a.active {
                    background-color: #4CAF50;
                    color: white;
                }

        .topnav-right {
            float: right;
        }
    </style>
</head>
<body>
    <form id="frmDashboard" runat="server">
        <div class="topnav">
            <div class="topnav-right">
                <asp:Label ID="lblUserName" runat="server" />
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>
</body>
</html>
