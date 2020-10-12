<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExceptionsReport.aspx.cs"
	Inherits="Covanta.UI.DailyOpsInputForm.ExceptionsReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="ExceptionsReportData.ascx" tagname="ExceptionsReportData" tagprefix="cov" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/opsscripts.js" type="text/javascript"></script>
	<title>Covanta WtE Operations Exceptions Report</title>
	<style type="text/css">
		*
		{
			font-family: Arial;
			font-size: 10pt;
		}
		#NavigationButtonDiv
		{
			margin-left: auto;
			margin-right: auto;
			text-align:center;
			margin-bottom:20px;
		}
		table
		{
			margin-left: auto;
			margin-right: auto;
			border-style: none;
			width: 100%;
		}
		td
		{
			vertical-align: top;
			border-style: none;
		}
		th
		{
			border-style: none;
			text-align: left;
			padding-bottom: 5px;
		}
		h1
		{
			font-size: 14pt;
			font-weight: bold;
			text-align: center;
		}
		h2
		{
			font-size: 12pt;
			font-weight: bold;
			text-decoration: underline;
			text-align: center;
		}
		#PleaseWait
		{
			font-variant: small-caps;
			font-size: 20pt;
			font-weight: bold;
			text-align: center;
		}
		#region-facility-date-table
		{
			margin-bottom: 10px;
			text-align: center;
		}
		#region-facility-date-table td
		{
			width: 33%;
		}
		#SummaryData
		{
			width: 50%;
		}
		.TypeRow
		{
			padding-top: 5px;
			padding-bottom: 5px;
		}
		.FacilityRow
		{
			padding-bottom: 5px;
			padding-top: 5px;
		}
		.SubFacilityRow
		{
			padding-bottom: 5px;
		}
	</style>
    
</head>
<body>
	<form id="form1" runat="server">
	<asp:HiddenField ID="LastReportDate" runat="server" Visible="False" />
	<asp:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
	</asp:ToolkitScriptManager>
	<div>
		<h1>
			Covanta WtE Operations Morning Report - Detail Page
		</h1>
		<div id="NavigationButtonDiv">
			<asp:Button ID="InputFormButton" runat="server" Text="Fill Out Daily Operations Report"
				OnClick="InputFormButton_Click" />
			<asp:Button ID="SummaryReportButton" runat="server" Text="View Summary Report" 
				OnClick="SummaryReportButton_Click" />
		</div>
		<asp:UpdatePanel ID="UpdatePanel" runat="server">
			<ContentTemplate>
                <asp:Timer ID="RefreshTimer" runat="server" EnableViewState="false" Interval="300000" OnTick="RefreshTimer_Tick">
                </asp:Timer>
				<table id="region-facility-date-table">
					<tr>
						<td>
							Region:&nbsp;
							<asp:DropDownList ID="RegionList" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="RegionList_SelectedIndexChanged"
								AutoPostBack="True">
								<asp:ListItem Selected="True" Value="">All Regions</asp:ListItem>
							</asp:DropDownList>
						</td>
						<td>
							Facility:&nbsp;
							<asp:DropDownList ID="FacilityList" runat="server" AppendDataBoundItems="False" AutoPostBack="True"
								OnSelectedIndexChanged="FacilityList_SelectedIndexChanged">
							</asp:DropDownList>
						</td>
						<td>
							Report Date:&nbsp;
							<asp:TextBox ID="ReportDate" runat="server" OnTextChanged="ReportDate_TextChanged"
								AutoPostBack="True" AutoCompleteType="Disabled"></asp:TextBox>
							<asp:CalendarExtender ID="ReportDateCalendar" runat="server" TargetControlID="ReportDate">
							</asp:CalendarExtender>
						</td>
					</tr>
				</table>
				<hr />
				<asp:UpdateProgress ID="UpdateProgress" runat="server">
					<ProgressTemplate>
						<div id="PleaseWait">
							Please Wait...
						</div>
					</ProgressTemplate>
				</asp:UpdateProgress>
				<cov:ExceptionsReportData ID="ExceptionsReportData" runat="server" />   
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	</form>
</body>
</html>
