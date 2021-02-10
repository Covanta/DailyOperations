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
        <div>
    <table cellspacing="0" rules="all" border="1" id="ExceptionsReportData_CommentsFacilityTypeData" style="border-collapse: collapse;">
        <tbody>
            <tr>
                <td class="TypeRow"></td>
                <td class="TypeRow" colspan="2">
                    <div>
                        <table cellspacing="0" rules="all" border="1" id="ExceptionsReportData_CommentsFacilityTypeData_ctl02_CommentsData" style="border-collapse: collapse;">
                            <tbody>
                                <tr>
                                    <td class="FacilityRow" style="width: 40%;"></td>
                                    <td class="FacilityRow" style="width: 60%;">
                                        <b style="text-decoration: underline;">Daily Global OPS Exceptions Report Power BI: <a href="https://app.powerbi.com/groups/me/apps/b9d85c40-4eef-4409-847a-14f442dc488d/reports/9cae9272-f970-4a82-8df5-35dad29ccc96/ReportSection7e645a9074a2b3253660?ctid=4aabd90b-7a09-4a17-b04d-52f3f6a8fab0">click Here</a></b>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
	</form>
</body>
</html>
