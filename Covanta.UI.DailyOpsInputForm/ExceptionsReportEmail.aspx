<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExceptionsReportEmail.aspx.cs"
	Inherits="Covanta.UI.DailyOpsInputForm.ExceptionsReportEmail" %>

<%@ Register Src="ExceptionsReportData.ascx" TagName="ExceptionsReportData" TagPrefix="cov" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<style type="text/css">
		h1
		{
			font-size: 14pt;
			font-weight: bold;
			text-align: center;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h1>
			<a href="http://opsreport.cov.corp/ExceptionsReport.aspx">Covanta WtE Operations Exceptions Report for
				<%= DateTime.Now.AddDays(-1).ToShortDateString() %></a></h1>
		<cov:ExceptionsReportData ID="ExceptionsReportData" runat="server" />
	</div>
	</form>
</body>
</html>
