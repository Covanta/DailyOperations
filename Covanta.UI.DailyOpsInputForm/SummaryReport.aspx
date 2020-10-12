<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryReport.aspx.cs"
	Inherits="Covanta.UI.DailyOpsInputForm.SummaryReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Covanta WtE Operations Summary Report</title>
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
			border-top: 2px solid #000000;
			border-bottom: 2px solid #000000;
			padding-top: 5px;
			padding-bottom: 5px;
		}
		.FacilityRow
		{
			border-top: 1px solid #000000;
			border-bottom: 1px solid #000000;
			padding-bottom: 5px;
			padding-top: 5px;
		}
		.Bold
		{
			font-weight: bold;
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
			Covanta Energy WtE Operations Daily Report - Summary Page
		</h1>
		<div id="NavigationButtonDiv">
			<asp:Button ID="InputFormButton" runat="server" Text="Fill Out Daily Operations Report"
				OnClick="InputFormButton_Click" />
			<asp:Button ID="ExceptionsReportButton" runat="server" Text="View Exceptions Report"
				OnClick="ExceptionsReportButton_Click" />
		</div>
		<asp:UpdatePanel ID="UpdatePanel" runat="server">
			<ContentTemplate>
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
				<h2>
					Summary
				</h2>
				<asp:GridView ID="SummaryData" runat="server" AutoGenerateColumns="false">
					<Columns>
						<asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="50%" />
						<asp:BoundField HeaderText="Total" DataField="FacilitiesReportingExpected" />
						<asp:BoundField HeaderText="Reporting" DataField="FacilitiesReportingActual" />
					</Columns>
				</asp:GridView>
				<h2>
					Reporting Facilities
				</h2>
				<asp:GridView ID="FacilityReportingFacilityTypeData" runat="server" 
					AutoGenerateColumns="false" 
					onrowcreated="FacilityReportingFacilityTypeData_RowCreated">
					<Columns>
						<asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="25%" ItemStyle-CssClass="TypeRow Bold" />
						<asp:TemplateField HeaderText="Facility" HeaderStyle-Width="25%" ItemStyle-CssClass="TypeRow">
							<ItemTemplate>
								<asp:GridView ID="FacilityReportingData" runat="server" AutoGenerateColumns="false"
									ShowHeader="false" OnRowDataBound="FacilityReportingData_RowDataBound">
									<Columns>
										<asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="34%" ItemStyle-CssClass="FacilityRow" />
										<asp:TemplateField ItemStyle-Width="66%" ItemStyle-CssClass="FacilityRow"></asp:TemplateField>
									</Columns>
								</asp:GridView>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Operations Log Status" HeaderStyle-Width="50%"></asp:TemplateField>
					</Columns>
				</asp:GridView>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	</form>
</body>
</html>
