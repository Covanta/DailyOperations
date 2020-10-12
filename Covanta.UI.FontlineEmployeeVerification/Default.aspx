<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Covanta.UI.FontlineEmployeeVerification._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="Images/CovantaEnergySmall.jpg"  alt="" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
    <img src="Images/cscLogo.jpg" alt="" />
     <hr />
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Label ID="lbl_Frontline" runat="server" Text="FrontLine" ForeColor="#6600FF" 
            Font-Size="X-Large"></asp:Label>
     <hr />
    </div>
    <div id="main">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <br />
    &nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="txtBox_LastName" runat="server" Width="242px" Height="26px" 
            Font-Size="Large"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;
     <asp:Button ID="btn_GetRecords" runat="server" Text="Get Records" 
            Font-Size="Medium" ForeColor="#0066FF" Height="41px" 
            onclick="btn_GetRecords_Click" Width="114px" />
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtBox_newPassword" runat="server" Width="122px" Height="26px" 
            Font-Size="Large" ReadOnly="True"></asp:TextBox>
    &nbsp;&nbsp;&nbsp;
     <asp:Button ID="btn_generatePassword" runat="server" Text="Generate new password" 
            Font-Size="Medium" ForeColor="#0066FF" Height="41px" 
            onclick="btn_generatePassword_Click" Width="199px" />
             <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Label ID="lbl_LastName" runat="server" Text="Enter Last Name" ForeColor="Red"></asp:Label>
     &nbsp;&nbsp;&nbsp;&nbsp;
             <br />
              <br />
    </div>
    <div style="margin-left: 360px">
        <asp:GridView ID="GridView1" runat="server" Width="412px">
        </asp:GridView>
    </div>
     <br />
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Label ID="lbl_Message1" runat="server" ForeColor="#006699"></asp:Label>
     &nbsp;&nbsp;&nbsp;&nbsp;
      <br />   
     <asp:XmlDataSource DataFile="ApplicationInfo.xml" runat="server" ID="xdsApplicationInfo" XPath="applications/application" />
                                <h3>Application Links:</h3>
                <asp:datalist runat="server" ID="dstApplicationInfo" DataSourceID="xdsApplicationInfo">
                    <ItemTemplate>
                        <a href='<%# Eval("url") %>' target="_blank" class="applink"><%# Eval("name") %></a>
                    </ItemTemplate>
                </asp:datalist>
    </form>
</body>
</html>
