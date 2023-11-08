<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyOps.aspx.cs" Inherits="Covanta.UI.DailyOpsInputForm.DailyOps" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>Covanta WtE Operations Morning Report</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE10"/>
    <style type="text/css">
        * {
            font-family: Arial;
            font-size: 10pt;
        }

        #NavigationButtonDiv {
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }

        #Instructions {
            text-align: left;
        }

        h1 {
            font-size: 14pt;
            font-weight: bold;
            text-align: center;
        }

        h2 {
            font-size: 12pt;
            text-align: center;
        }

        h3 {
            margin-bottom: 2px;
            font-weight: bold;
        }

        h4 {
            margin-top: 0px;
            margin-bottom: 0px;
            font-weight: normal;
            text-decoration: underline;
        }

        ol {
            margin-top: 0px;
        }

        table {
            background-color: #FFFFFF;
        }

        #facility-date-table {
            width: 75%;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 10px;
        }

        #facility-cell {
            border-right: 1px solid #000000;
        }

        #facility-cell, #reporting-cell {
            width: 50%;
            text-align: center;
            vertical-align: middle;
            padding-bottom: 10px;
            border-bottom: 1px solid #000000;
        }

        #input-table {
            width: 100%;
        }

        .left-column, .right-column {
            vertical-align: middle;
            width: 50%;
            padding: 5px 2px 5px 2px;
        }

        .left-column {
            text-align: right;
        }

        .right-column {
            text-align: left;
        }

            .right-column textarea {
                width: 350px;
                height: 100px;
            }

        .table-subheading {
            font-size: 12pt;
            text-align: center;
            font-weight: bold;
            text-decoration: underline;
            margin-top: 5px;
            margin-bottom: 5px;
        }

        .Error {
            color: #ff0000;
            font-weight: bold;
        }

        .Warning {
            color: #0000ff;
            font-weight: bold;
        }

        #Errors, #Warnings {
            text-align: left;
            width: 50%;
            margin-left: auto;
            margin-right: auto;
            margin-top: 2px;
            margin-bottom: 2px;
        }

        #ErrorsHeader, #WarningsHeader {
            font-variant: small-caps;
            font-size: 14pt;
            text-decoration: underline;
        }

        #Buttons {
            text-align: center;
            margin-top: 5px;
        }

        #PleaseWait {
            font-variant: small-caps;
            font-size: 20pt;
            font-weight: bold;
            text-align: center;
        }

        .Prompt {
            text-align: center;
            width: 75%;
            margin-left: auto;
            margin-right: auto;
        }

        #UpdateButton {
            margin-right: 5px;
        }

        #ReplaceButton {
            margin-left: 5px;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (document.getElementById("InputRegion")) {
                showDowntimeField('Boiler1Status');
                showDowntimeField('Boiler2Status');
                showDowntimeField('Boiler3Status');
                showDowntimeField('Boiler4Status');
                showDowntimeField('Boiler5Status');
                showDowntimeField('Boiler6Status');
                showDowntimeField('Turbine1Status');
                showDowntimeField('Turbine2Status');
                showSystem('FerrousAvailable');
                showSystem('NonFerrousAvailable');
                showSystem('NonFerrousSmallsAvailable');
                showSystem('FrontEndFerrousAvailable');
                showSystem('EnhancedFerrousAvailable');
                showSystem('EnhancedNonFerrousAvailable');
                showEnvironmentalExplanation();
                showCemsExplanation();
                showOptionalField('FireSystemCheckBox');
                showOptionalField('CriticalAssetsCheckBox');
                //showOptionalField('FirstAidCheckBox');
                //showOptionalField('OshaRecordableCheckBox');
                showOptionalField('EmployeeSafetyIncidentsCheckBox');
                showOptionalField('NearMissCheckBox');
                showOptionalField('ContractorCheckBox');
                showOptionalField('CommentsCheckBox');
            }
        }
        function showDowntimeField(boilerStatusID) {
            var boilerStatus = document.getElementById(boilerStatusID);
            if (boilerStatus) {
                // Even though this is labeled as boiler, it is also used by the Turbine Generator fields
                var selection = boilerStatus.options[boilerStatus.selectedIndex].text;


                //Hours
                //Show or hide Downtime Hours and expected repair date based on Status Dropdown Box (Show for all except 'Operational')
                if (selection !== "" && selection !== "Operational" && selection !== "Decommissioned" && selection !== "Not Applicable")
                    //Show
                {
                    document.getElementById(boilerStatusID.replace("Status", "DowntimeRow")).style.display = "table-row";
                    document.getElementById(boilerStatusID.replace("Status", "ExpectedRepairDateRow")).style.display = "table-row";
                }
                    //Hide
                else {
                    document.getElementById(boilerStatusID.replace("Status", "DowntimeRow")).style.display = "none";
                    document.getElementById(boilerStatusID.replace("Status", "ExpectedRepairDateRow")).style.display = "none";
                }



                //Unscheduled Reason
                //Show or hide Downtime Reason Text based on Status Dropdown Box (Show for 'Unscheduled' or 'Stand-By' if Boiler Status,  Show for Unscheduled TG Outage or Other is Turbine)
                if ((selection === "Unscheduled Outage") || (selection === "Stand-By") || (selection === "Unscheduled TG Outage") || (selection === "Other"))
                    //Show
                    document.getElementById(boilerStatusID.replace("Status", "UnscheduledRow")).style.display = "table-row";
                else
                    //Hide
                    document.getElementById(boilerStatusID.replace("Status", "UnscheduledRow")).style.display = "none";



                //Scheduled Reason
                //Show or hide Scheduled Reason Dropdown based on Status Dropdown Box 
                if ((selection === "Scheduled TG Outage") || (selection === "Scheduled Outage"))
                //Show
                {
                    //document.getElementById(boilerStatusID.replace("Status", "ScheduledReasonRow")).style.display = "none";
                    document.getElementById(boilerStatusID.replace("Status", "ScheduledRow")).style.display = "table-row";
                }
                else
                //Hide
                {
                    //document.getElementById(boilerStatusID.replace("Status", "ScheduledReasonRow")).style.display = "none";
                    document.getElementById(boilerStatusID.replace("Status", "ScheduledRow")).style.display = "none";
                }
            }
        }


        function showSystem(availableID) {
            var available = document.getElementById(availableID + "_1");
            if (available) {
                var hoursField = document.getElementById(availableID.replace("Available", "HoursUnavailableRow"));
                var reasonField = document.getElementById(availableID.replace("Available", "ReasonRow"));
                var dateField = document.getElementById(availableID.replace("Available", "DateRow"));
                var processedField = document.getElementById(availableID.replace("Available", "ReprocessedRow"));

                if (available.checked) {
                    hoursField.style.display = "table-row";
                    reasonField.style.display = "table-row";
                    dateField.style.display = "table-row";
                    if (processedField)
                        processedField.style.display = "table-row";
                }
                else {
                    hoursField.style.display = "none";
                    reasonField.style.display = "none";
                    dateField.style.display = "none";
                    if (processedField)
                        processedField.style.display = "none";
                }
            }
        }
        function showEnvironmentalExplanation() {
            var eventsYes = document.getElementById("EnvironmentalEvents_0");
            var type = document.getElementById("EnvironmentalTypeRow");
            var explanation = document.getElementById("EnvironmentalExplanationRow");

            if (eventsYes.checked) {
                type.style.display = "table-row";
                explanation.style.display = "table-row";
            }
            else {
                type.style.display = "none";
                explanation.style.display = "none";
            }
        }
        function showCemsExplanation() {
            var eventsYes = document.getElementById("CemsEvents_0");
            var type = document.getElementById("CemsTypeRow");
            var explanation = document.getElementById("CemsExplanationRow");

            if (eventsYes.checked)
                type.style.display = explanation.style.display = "table-row";
            else
                type.style.display = explanation.style.display = "none";
        }
        function showOptionalField(optionalCheckBoxID) {
            var checkbox = document.getElementById(optionalCheckBoxID);
            if (checkbox) {
                var field = document.getElementById(optionalCheckBoxID.replace("CheckBox", ""));

                if (field.id === "FireSystem") {
                    var dateField = document.getElementById("FireSystemDateRow");
                    if (checkbox.checked) {
                        field.style.display = "none";
                        dateField.style.display = "none";
                    }
                    else {
                        field.style.display = "table-row";
                        dateField.style.display = "table-row";
                    }
                }

                if (field.id === "CriticalAssets") {
                    var dateField = document.getElementById("criticalAssetsDateRow");
                    if (checkbox.checked) {
                        field.style.display = "none";
                        dateField.style.display = "none";
                    }
                    else {
                        field.style.display = "table-row";
                        dateField.style.display = "table-row";
                    }
                } 

                if (field.id === "Comments") {
                    var dateField = document.getElementById("commentsDateRow");
                    if (checkbox.checked) {
                        field.style.display = "none";
                        dateField.style.display = "none";
                    }
                    else {
                        field.style.display = "table-row";
                        dateField.style.display = "table-row";
                    }
                }

                else {
                    if (checkbox.checked)
                        field.style.display = "none";
                    else
                        field.style.display = "table-row";
                }
            }
        }
    </script>
    <form id="form1" runat="server">
        <asp:HiddenField ID="LastReportingDate" runat="server" Visible="False" />
        <asp:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
        </asp:ToolkitScriptManager>
        <div>
            <h1>Covanta Global Operations Exceptions Report
                <%--Covanta WtE Operations Morning Report - Electronic Form--%>
            </h1>
            <h2 style="color:blue"> Please use Google Chrome for best user experience</h2>
            <div id="NavigationButtonDiv">
                <asp:Button ID="ExceptionsReportButton" runat="server" Text="View Exceptions Report"
                    OnClick="ExceptionsReportButton_Click" />
                <asp:Button ID="SummaryReportButton" runat="server" Text="View Summary Report" OnClick="SummaryReportButton_Click" />
                <asp:Button ID="MassUpdateButton" runat="server" Text="Mass Update" OnClick="MassUpdateButton_Click" />
            </div>
        </div>
        <hr />
        <div>
            <h2><%--This Operation's Report must be completed DAILY,<br />
                Sunday-Saturday, INCLUDING Holidays By 7:00 a.m. Eastern Time.<br />
                <br />
                The Facility Manager and the Chief Engineer will be notified automatically if the
			report is not completed by that time.--%>
                It is the responsibility of the Operations Manager to ensure this report is properly completed and submitted by 07:00 EST each day of the week to include weekends and holidays.
                <br/><br/>
                Facilities that fail to timely submit their report will be flagged as “Not Reporting” at the top of the report when it is disseminated at 07:30 EST to senior executives and others on the distribution list.
                <%--A delinquency email will be automatically sent to the Facility Manager, Operations Manager, and Sr. Director Operations if the report is not completed by 07:00 EST each day for action.--%>
            </h2>
        </div>
        <hr />
        <div id="Instructions">
            <h3>Instructions:</h3>
            <h4>To submit the Daily Morning Report,</h4>
            <ol>
                <li>Select your facility and enter the date of the report.</li>
                <li>Complete the information in all the boxes.</li>
                <li>Then click [Submit Operation Report] at the bottom of the form to post your information.</li>
            </ol>
        </div>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div>
                    <table id="facility-date-table" cellspacing="0">
                        <tr>
                            <td></td>
                            <td style="text-align: center; padding-bottom: 5px;">
                                <b>Note that the reporting date is the day to which the data pertains,<br />
                                    NOT the day on which the data is entered.</b>
                            </td>
                        </tr>
                        <tr>
                            <td id="facility-cell">Facility:&nbsp;
							<asp:DropDownList ID="Facility" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Facility_SelectedIndexChanged">
                            </asp:DropDownList>
                            </td>
                            <td id="reporting-cell">Reporting Date:&nbsp;
							<asp:TextBox ID="ReportingDate" runat="server" OnTextChanged="ReportingDate_TextChanged"
                                AutoPostBack="True" AutoCompleteType="Disabled"></asp:TextBox>
                                <asp:CalendarExtender ID="ReportingDateCalendar" runat="server" TargetControlID="ReportingDate">
                                </asp:CalendarExtender>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="EnvironmentLabel" runat="server" EnableViewState="False" AutoPostBack="False"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="DateErrorMessage" runat="server" EnableViewState="False" AutoPostBack="False"></asp:Label>

                            </td>
                        </tr>
                    </table>
                    <div id="InputRegion" runat="server">
                        <asp:UpdateProgress ID="SubUpdateProgress" runat="server">
                            <ProgressTemplate>
                                <div id="PleaseWait">
                                    Please Wait...
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <table id="input-table">
                            <tr>
                                <td class="left-column">Reporting Employee
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="UserName" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="PeopleSoftCodeLabel" runat="server" EnableViewState="False">Facility PeopleSoft Code</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="PeopleSoftCode" runat="server" CssClass="PSCodeTextBoxValidationField"
                                        AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;&nbsp;
                                <asp:Label ID="PeopleSoftCodeHint" runat="server" EnableViewState="False" AutoPostBack="False"></asp:Label>
                                    &nbsp;&nbsp;
                                <asp:Label ID="PeopleSoftCodeMessage" runat="server" EnableViewState="False" AutoPostBack="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="TonsDeliveredLabel" runat="server" EnableViewState="False">Tons Delivered</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="TonsDelivered" runat="server" CssClass="NumericField"
                                        AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;&nbsp;
                                <asp:Label ID="TonsDeliveredMessage" runat="server" EnableViewState="False" AutoPostBack="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="TonsProcessedLabel" runat="server" EnableViewState="False">Tons Processed</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="TonsProcessed" runat="server" CssClass="NumericField"
                                        AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="TonsProcessedMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="SteamProducedLabel" runat="server" EnableViewState="False">Steam Produced (Klbs)</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="SteamProduced" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="SteamProducedMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="SteamSoldLabel" runat="server" EnableViewState="False">Steam Sold (Klbs)</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="SteamSold" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="SteamSoldMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="NetElectricLabel" runat="server" EnableViewState="False">Net Electric (MWH)</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="NetElectric" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="NetElectricMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr id="PitInventoryRow" runat="server">
                                <td class="left-column">
                                    <asp:Label ID="PitInventoryLabel" runat="server" EnableViewState="False">Fuel Inventory - (MSW tons, Wood tons)</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="PitInventory" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="PitInventoryMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr id="PreShredInventoryRow" runat="server" visible="false">
                                <td class="left-column">
                                    <asp:Label ID="PreShredInventoryLabel" runat="server" EnableViewState="False">Pre-Shred Inventory</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="PreShredInventory" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="PreShredInventoryMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr id="PostShredInventoryRow" runat="server" visible="false">
                                <td class="left-column">
                                    <asp:Label ID="PostShredInventoryLabel" runat="server" EnableViewState="False">Post-Shred Inventory</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="PostShredInventory" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="PostShredInventoryMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr id="MassBurnInventoryRow" runat="server" visible="false">
                                <td class="left-column">
                                    <asp:Label ID="MassBurnInventorylabel" runat="server" EnableViewState="False">Mass Burn Inventory</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="MassBurnInventory" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    &nbsp;
                                <asp:Label ID="MassBurnInventoryMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <span id="Boiler1Input" runat="server">
                                <tr runat="server">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler1StatusLabel" runat="server" EnableViewState="False">Boiler 1 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler1Status" runat="server" onchange="showDowntimeField('Boiler1Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Scheduled Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled Outage</asp:ListItem>
                                            <asp:ListItem Enabled="false" ID="Boiler1Decommissioned">Decommissioned</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                    </td>

                                </tr>
                                <span runat="server">
                                <tr id="Boiler1DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler1DowntimeLabel" runat="server" EnableViewState="False">Downtime Boiler 1 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler1Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Cellular"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler1ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler1ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler1ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Boiler1CalendarExtender" runat="server" TargetControlID="Boiler1ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                    <tr id="Boiler1ScheduledReasonRow">
                                        <td class="left-column">
                                            <asp:Label ID="Boiler1ScheduledReasonLabel" runat="server" EnableViewState="True">Boiler 1 Scheduled Outage Reason</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:DropDownList ID="Boiler1ScheduledReason" runat="server"
                                                CssClass="ScheduledOutageStatusField">
                                                <asp:ListItem Selected="True"></asp:ListItem>
                                                <asp:ListItem>Major Boiler Outage</asp:ListItem>
                                                <asp:ListItem>Minor Boiler Outage</asp:ListItem>
                                                <asp:ListItem>Cleaning Outage</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>

                                <tr id="Boiler1ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler1ScheduledLabel" runat="server" EnableViewState="False">Boiler 1 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler1Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler1UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler1UnscheduledLabel" runat="server" EnableViewState="False">Boiler 1 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler1Unscheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                    </span> 
                                <%--<tr id="FrontEndFerrousDateRow">
									<td class="left-column">
										<asp:Label ID="Label1" runat="server" EnableViewState="False">Front End Ferrous System Expected Back in Service Date</asp:Label>
									</td>
									<td class="right-column">
										<asp:TextBox ID="TextBox1" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
										<asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="FrontEndFerrousDate">
										</asp:CalendarExtender>
									</td>
								</tr>--%>
                            
                            </span>
                            <span id="Boiler2Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Boiler2StatusLabel" runat="server" EnableViewState="False">Boiler 2 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler2Status" runat="server" onchange="showDowntimeField('Boiler2Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Scheduled Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled Outage</asp:ListItem>
                                            <asp:ListItem Enabled="false" ID="Boiler2Decommissioned" Selected="False">Decommissioned</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Boiler2DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler2DowntimeLabel" runat="server" EnableViewState="False">Downtime Boiler 2 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler2Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler2ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler2ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler2ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Boiler2CalendarExtender" runat="server" TargetControlID="Boiler2ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                <tr id="Boiler2ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler2ScheduledReasonLabel" runat="server" EnableViewState="False">Boiler 2 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler2ScheduledReason" runat="server"
                                            CssClass="ScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Minor Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Cleaning Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Boiler2ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler2ScheduledLabel" runat="server" EnableViewState="False">Boiler 2 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler2Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler2UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler2UnscheduledLabel" runat="server" EnableViewState="False">Boiler 2 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler2Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </span>


                            <span id="Boiler3Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Boiler3StatusLabel" runat="server" EnableViewState="False">Boiler 3 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler3Status" runat="server" onchange="showDowntimeField('Boiler3Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Scheduled Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled Outage</asp:ListItem>
                                            <asp:ListItem Enabled="false" ID="Boiler3Decommissioned" Selected="False">Decommissioned</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Boiler3DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler3DowntimeLabel" runat="server" EnableViewState="False">Downtime Boiler 3 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler3Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler3ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler3ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler3ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Boiler3CalendarExtender" runat="server" TargetControlID="Boiler3ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                <tr id="Boiler3ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler3ScheduledReasonLabel" runat="server" EnableViewState="False">Boiler 3 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler3ScheduledReason" runat="server"
                                            CssClass="ScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Minor Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Cleaning Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Boiler3ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler3ScheduledLabel" runat="server" EnableViewState="False">Boiler 3 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler3Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler3UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler3UnscheduledLabel" runat="server" EnableViewState="False">Boiler 3 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler3Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </span><span id="Boiler4Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Boiler4StatusLabel" runat="server" EnableViewState="False">Boiler 4 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler4Status" runat="server" onchange="showDowntimeField('Boiler4Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Scheduled Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled Outage</asp:ListItem>
                                            <asp:ListItem Enabled="false" ID="Boiler4Decommissioned" Selected="False">Decommissioned</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Boiler4DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler4DowntimeLabel" runat="server" EnableViewState="False">Downtime Boiler 4 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler4Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler4ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler4ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler4ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Boiler4CalendarExtender" runat="server" TargetControlID="Boiler4ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                <tr id="Boiler4ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler4ScheduledReasonLabel" runat="server" EnableViewState="False">Boiler 4 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler4ScheduledReason" runat="server"
                                            CssClass="ScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Minor Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Cleaning Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Boiler4ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler4ScheduledLabel" runat="server" EnableViewState="False">Boiler 4 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler4Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler4UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler4UnscheduledLabel" runat="server" EnableViewState="False">Boiler 4 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler4Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </span><span id="Boiler5Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Boiler5StatusLabel" runat="server" EnableViewState="False">Boiler 5 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler5Status" runat="server" onchange="showDowntimeField('Boiler5Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Scheduled Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled Outage</asp:ListItem>
                                            <asp:ListItem Enabled="false" ID="Boiler5Decommissioned" Selected="False">Decommissioned</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Boiler5DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler5DowntimeLabel" runat="server" EnableViewState="False">Downtime Boiler 5 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler5Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler5ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler5ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler5ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Boiler5CalendarExtender" runat="server" TargetControlID="Boiler5ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                <tr id="Boiler5ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler5ScheduledReasonLabel" runat="server" EnableViewState="False">Boiler 5 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler5ScheduledReason" runat="server"
                                            CssClass="ScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Minor Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Cleaning Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Boiler5ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler5ScheduledLabel" runat="server" EnableViewState="False">Boiler 5 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler5Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler5UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler5UnscheduledLabel" runat="server" EnableViewState="False">Boiler 5 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler5Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </span><span id="Boiler6Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Boiler6StatusLabel" runat="server" EnableViewState="False">Boiler 6 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler6Status" runat="server" onchange="showDowntimeField('Boiler6Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Scheduled Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled Outage</asp:ListItem>
                                            <asp:ListItem Enabled="false" ID="Boiler6Decommissioned" Selected="False">Decommissioned</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Boiler6DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler6DowntimeLabel" runat="server" EnableViewState="False">Downtime Boiler 6 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler6Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler6ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler6ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler6ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Boiler6CalendarExtender" runat="server" TargetControlID="Boiler6ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                <tr id="Boiler6ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler6ScheduledReasonLabel" runat="server" EnableViewState="False">Boiler 6 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Boiler6ScheduledReason" runat="server"
                                            CssClass="ScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Minor Boiler Outage</asp:ListItem>
                                            <asp:ListItem>Cleaning Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Boiler6ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler6ScheduledLabel" runat="server" EnableViewState="False">Boiler 6 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler6Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Boiler6UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Boiler6UnscheduledLabel" runat="server" EnableViewState="False">Boiler 6 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Boiler6Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </span><span id="Turbine1Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Turbine1StatusLabel" runat="server" EnableViewState="False">Turbine/Generator 1 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Turbine1Status" runat="server" onchange="showDowntimeField('Turbine1Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Selling Steam</asp:ListItem>
                                            <asp:ListItem>Scheduled TG Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled TG Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Turbine1DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine1DowntimeLabel" runat="server" EnableViewState="False">Downtime Turbine/Generator 1 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine1Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Turbine1ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine1ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine1ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Turbine1CalendarExtender" runat="server" TargetControlID="Turbine1ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>

<%--                                <tr id="Turbine1ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine1ScheduledReasonLabel" runat="server" EnableViewState="False">Turbine Generator 1 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Turbine1ScheduledReason" runat="server"
                                            CssClass="TurbineScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major TG Outage</asp:ListItem>
                                            <asp:ListItem>Minor TG Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Turbine1ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine1ScheduledLabel" runat="server" EnableViewState="False">Turbine 1 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine1Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Turbine1UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine1UnscheduledLabel" runat="server" EnableViewState="False">Turbine/Generator 1 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine1Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled" OnTextChanged="Turbine1Unscheduled_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                            </span><span id="Turbine2Input" runat="server">
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="Turbine2StatusLabel" runat="server" EnableViewState="False">Turbine/Generator 2 Status</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Turbine2Status" runat="server" onchange="showDowntimeField('Turbine2Status')"
                                            CssClass="OutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Operational</asp:ListItem>
                                            <asp:ListItem>Selling Steam</asp:ListItem>
                                            <asp:ListItem>Scheduled TG Outage</asp:ListItem>
                                            <asp:ListItem>Unscheduled TG Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Turbine2DowntimeRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine2DowntimeLabel" runat="server" EnableViewState="False">Downtime Turbine/Generator 2 (hours)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine2Downtime" runat="server" CssClass="DowntimeField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Turbine2ExpectedRepairDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine2ExpectedRepairDateLabel" runat="server" EnableViewState="False">Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine2ExpectedRepairDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="Turbine2CalendarExtender" runat="server" TargetControlID="Turbine2ExpectedRepairDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
<%--                                <tr id="Turbine2ScheduledReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine2ScheduledReasonLabel" runat="server" EnableViewState="False">Turbine Generator 2 Scheduled Outage Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:DropDownList ID="Turbine2ScheduledReason" runat="server"
                                            CssClass="TurbineScheduledOutageStatusField">
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                            <asp:ListItem>Major TG Outage</asp:ListItem>
                                            <asp:ListItem>Minor TG Outage</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr id="Turbine2ScheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine2ScheduledLabel" runat="server" EnableViewState="False">Turbine 2 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine2Scheduled" runat="server" TextMode="MultiLine" CssClass="ScheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Turbine2UnscheduledRow">
                                    <td class="left-column">
                                        <asp:Label ID="Turbine2UnscheduledLabel" runat="server" EnableViewState="False">Turbine/Generator 2 Downtime Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="Turbine2Unscheduled" runat="server" TextMode="MultiLine" CssClass="UnscheduledField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </span>
                            <tr>
                                <td class="left-column">
                                    <asp:Label ID="FerrousTonsLabel" runat="server" EnableViewState="False">Ferrous (tons)</asp:Label>
                                </td>
                                <td class="right-column">
                                    <asp:TextBox ID="FerrousTons" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                </td>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousTonsLabel" runat="server" EnableViewState="False">Non-Ferrous (tons)</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousTons" runat="server" CssClass="NumericField" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="FerrousAvailableLabel" runat="server" EnableViewState="False">Was Ferrous System 100% available?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="FerrousAvailable" runat="server" CssClass="YesNoField" RepeatDirection="Horizontal">
                                            <asp:ListItem onClick="showSystem('FerrousAvailable')" Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('FerrousAvailable')" Value="N">No</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('FerrousAvailable')" Value="">N/A</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="FerrousHoursUnavailableRow">
                                    <td class="left-column">
                                        <asp:Label ID="FerrousHoursUnavailableLabel" runat="server" EnableViewState="False">Ferrous System Hours Unavailable</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="FerrousHoursUnavailable" runat="server" CssClass="HoursUnavailableField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="FerrousReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="FerrousReasonLabel" runat="server" EnableViewState="False">Ferrous System Hours Unavailable Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="FerrousReason" runat="server" CssClass="ReasonField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="FerrousDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="FerrousDateLabel" runat="server" EnableViewState="False">Ferrous System Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="FerrousDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="FerrousDateCalendar" runat="server" TargetControlID="FerrousDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="FerrousReprocessedRow">
                                    <td class="left-column">
                                        <asp:Label ID="FerrousReprocessedLabel" runat="server" EnableViewState="False">Will ash be reprocessed through Ferrous System?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="FerrousReprocessed" runat="server" CssClass="ReprocessedField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousAvailableLabel" runat="server" EnableViewState="False">Was Non-Ferrous System 100% available?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="NonFerrousAvailable" runat="server" CssClass="YesNoField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem onClick="showSystem('NonFerrousAvailable')" Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('NonFerrousAvailable')" Value="N">No</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('NonFerrousAvailable')" Value="">N/A</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="NonFerrousHoursUnavailableRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousHoursUnavailableLabel" runat="server" EnableViewState="False">Non-Ferrous System Hours Unavailable</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousHoursUnavailable" runat="server" CssClass="HoursUnavailableField"
                                            AutoCompleteType="Disabled"></asp:TextBox>Update
                                    </td>
                                </tr>
                                <tr id="NonFerrousReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousReasonLabel" runat="server" EnableViewState="False">Non-Ferrous System Hours Unavailable Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousReason" runat="server" CssClass="ReasonField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="NonFerrousDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousDateLabel" runat="server" EnableViewState="False">Non-Ferrous System Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="NonFerrousDateCalendar" runat="server" TargetControlID="NonFerrousDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="NonFerrousReprocessedRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousReprocessedLabel" runat="server" EnableViewState="False">Will ash be reprocessed through Non-Ferrous System?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="NonFerrousReprocessed" runat="server" CssClass="ReprocessedField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousSmallsAvailableLabel" runat="server" EnableViewState="False">Was Non-Ferrous Smalls System 100% available?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="NonFerrousSmallsAvailable" runat="server" CssClass="YesNoField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem onClick="showSystem('NonFerrousSmallsAvailable')" Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('NonFerrousSmallsAvailable')" Value="N">No</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('NonFerrousSmallsAvailable')" Value="">N/A</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="NonFerrousSmallsHoursUnavailableRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousSmallsHoursUnavailableLabel" runat="server" EnableViewState="False">Non-Ferrous Smalls System Hours Unavailable</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousSmallsHoursUnavailable" runat="server" CssClass="HoursUnavailableField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="NonFerrousSmallsReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousSmallsReasonLabel" runat="server" EnableViewState="False">Non-Ferrous Smalls System Hours Unavailable Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousSmallsReason" runat="server" CssClass="ReasonField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="NonFerrousSmallsDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousSmallsDateLabel" runat="server" EnableViewState="False">Non-Ferrous Smalls System Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="NonFerrousSmallsDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="NonFerrousSmallsDateCalendar" runat="server" TargetControlID="NonFerrousSmallsDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="NonFerrousSmallsReprocessedRow">
                                    <td class="left-column">
                                        <asp:Label ID="NonFerrousSmallsReprocessedLabel" runat="server" EnableViewState="False">Will ash be reprocessed through Non-Ferrous Smalls System?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="NonFerrousSmallsReprocessed" runat="server" CssClass="ReprocessedField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <span id="FrontEndInput" runat="server" visible="true">
                                    <tr>
                                        <td class="left-column">
                                            <asp:Label ID="FrontEndFerrousAvailableLabel" runat="server" EnableViewState="False">Was Front End Ferrous System 100% available?</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:RadioButtonList ID="FrontEndFerrousAvailable" runat="server" CssClass="YesNoField"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem onClick="showSystem('FrontEndFerrousAvailable')" Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem onClick="showSystem('FrontEndFerrousAvailable')" Value="N">No</asp:ListItem>
                                                <asp:ListItem onClick="showSystem('FrontEndFerrousAvailable')" Value="">N/A</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="FrontEndFerrousHoursUnavailableRow">
                                        <td class="left-column">
                                            <asp:Label ID="FrontEndFerrousHoursUnavailableLabel" runat="server" EnableViewState="False">Front End Ferrous System Hours Unavailable</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:TextBox ID="FrontEndFerrousHoursUnavailable" runat="server" CssClass="HoursUnavailableField"
                                                AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="FrontEndFerrousReasonRow">
                                        <td class="left-column">
                                            <asp:Label ID="FrontEndFerrousReasonLabel" runat="server" EnableViewState="False">Front End Ferrous System Hours Unavailable Reason</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:TextBox ID="FrontEndFerrousReason" runat="server" CssClass="ReasonField" TextMode="MultiLine"
                                                AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="FrontEndFerrousDateRow">
                                        <td class="left-column">
                                            <asp:Label ID="FrontEndFerrousDateLabel" runat="server" EnableViewState="False">Front End Ferrous System Expected Back in Service Date</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:TextBox ID="FrontEndFerrousDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:CalendarExtender ID="FrontEndFerrousDateCalendar" runat="server" TargetControlID="FrontEndFerrousDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="FrontEndFerrousReprocessedRow">
                                        <td class="left-column">
                                            <asp:Label ID="FrontEndFerrousReprocessedLabel" runat="server" EnableViewState="False">Will ash be reprocessed through Front End Ferrous System?</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:RadioButtonList ID="FrontEndFerrousReprocessed" runat="server" CssClass="ReprocessedField"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </span>

                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="EnhancedFerrousAvailableLabel" runat="server" EnableViewState="False">Was Enhanced Ferrous System 100% available?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="EnhancedFerrousAvailable" runat="server" CssClass="YesNoField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem onClick="showSystem('EnhancedFerrousAvailable')" Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('EnhancedFerrousAvailable')" Value="N">No</asp:ListItem>
                                            <asp:ListItem onClick="showSystem('EnhancedFerrousAvailable')" Value="">N/A</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="EnhancedFerrousHoursUnavailableRow">
                                    <td class="left-column">
                                        <asp:Label ID="EnhancedFerrousHoursUnavailableLabel" runat="server" EnableViewState="False">Enhanced Ferrous System Hours Unavailable</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="EnhancedFerrousHoursUnavailable" runat="server" CssClass="HoursUnavailableField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="EnhancedFerrousReasonRow">
                                    <td class="left-column">
                                        <asp:Label ID="EnhancedFerrousReasonLabel" runat="server" EnableViewState="False">Enhanced Ferrous System Hours Unavailable Reason</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="EnhancedFerrousReason" runat="server" CssClass="ReasonField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="EnhancedFerrousDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="EnhancedFerrousDateLabel" runat="server" EnableViewState="False">Enhanced Ferrous System Expected Back in Service Date</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="EnhancedFerrousDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:CalendarExtender ID="EnhancedFerrousDateCalendar" runat="server" TargetControlID="EnhancedFerrousDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="EnhancedFerrousReprocessedRow">
                                    <td class="left-column">
                                        <asp:Label ID="EnhancedFerrousReprocessedLabel" runat="server" EnableViewState="False">Will ash be reprocessed through Enhanced Ferrous System?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="EnhancedFerrousReprocessed" runat="server" CssClass="ReprocessedField"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                            <asp:ListItem Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>

                                <span id="EnhancedNonFerrousInput" runat="server" visible="true">
                                    <tr>
                                        <td class="left-column">
                                            <asp:Label ID="EnhancedNonFerrousAvailableLabel" runat="server" EnableViewState="False">Was Enhanced Non-Ferrous System 100% available?</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:RadioButtonList ID="EnhancedNonFerrousAvailable" runat="server" CssClass="YesNoField"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem onClick="showSystem('EnhancedNonFerrousAvailable')" Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem onClick="showSystem('EnhancedNonFerrousAvailable')" Value="N">No</asp:ListItem>
                                                <asp:ListItem onClick="showSystem('EnhancedNonFerrousAvailable')" Value="">N/A</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="EnhancedNonFerrousHoursUnavailableRow">
                                        <td class="left-column">
                                            <asp:Label ID="EnhancedNonFerrousHoursUnavailableLabel" runat="server" EnableViewState="False">Enhanced Non-Ferrous System Hours Unavailable</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:TextBox ID="EnhancedNonFerrousHoursUnavailable" runat="server" CssClass="HoursUnavailableField"
                                                AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="EnhancedNonFerrousReasonRow">
                                        <td class="left-column">
                                            <asp:Label ID="EnhancedNonFerrousReasonLabel" runat="server" EnableViewState="False">Enhanced Non-Ferrous System Hours Unavailable Reason</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:TextBox ID="EnhancedNonFerrousReason" runat="server" CssClass="ReasonField" TextMode="MultiLine"
                                                AutoCompleteType="Disabled"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="EnhancedNonFerrousDateRow">
                                        <td class="left-column">
                                            <asp:Label ID="EnhancedNonFerrousDateLabel" runat="server" EnableViewState="False">Enhanced Non-Ferrous System Expected Back in Service Date</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:TextBox ID="EnhancedNonFerrousDate" runat="server" CssClass="DateField" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:CalendarExtender ID="EnhancedNonFerrousDateCalendar" runat="server" TargetControlID="EnhancedNonFerrousDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="EnhancedNonFerrousReprocessedRow">
                                        <td class="left-column">
                                            <asp:Label ID="EnhancedNonFerrousReprocessedLabel" runat="server" EnableViewState="False">Will ash be reprocessed through Enhanced Non-Ferrous System?</asp:Label>
                                        </td>
                                        <td class="right-column">
                                            <asp:RadioButtonList ID="EnhancedNonFerrousReprocessed" runat="server" CssClass="ReprocessedField"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </span>

                                <tr>
                                    <td class="table-subheading" colspan="2">Fire Protection Impairments
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="FireSystemLabel" runat="server" EnableViewState="False">List any Fire Protection Systems out of Service</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="FireSystemCheckBox" runat="server" onClick="showOptionalField('FireSystemCheckBox')"
                                            Text="None" />
                                        <br />
                                        <asp:TextBox ID="FireSystem" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="FireSystemDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="FireSystemDateLabel" runat="server" EnableViewState="False">Date of Expected Fire Prevention Systems Repair</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="FireSystemDate" runat="server" CssClass="DateField"></asp:TextBox>
                                        <asp:CalendarExtender ID="FireSystemDateCalendar" runat="server" TargetControlID="FireSystemDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="table-subheading" colspan="2">Critical Assests in Alarm
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="CriticalAssetsLabel" runat="server" EnableViewState="False">List any Critical Assets in Alarm</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="CriticalAssetsCheckBox" runat="server" onClick="showOptionalField('CriticalAssetsCheckBox')"
                                            Text="None" />
                                        <br />
                                        <asp:TextBox ID="CriticalAssets" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="criticalAssetsDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="criticalAssetsDateLabel" runat="server" EnableViewState="False">Expected return to service:</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="CriticalAssetsDate" runat="server" CssClass="DateField"></asp:TextBox>
                                        <asp:CalendarExtender ID="CriticalAssetsDateCalendar" runat="server" TargetControlID="criticalAssetsDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="table-subheading" colspan="2">Environmental Events
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="EnvironmentalEventsLabel" runat="server" EnableViewState="False">Were there any environmental events?</asp:Label>
                                    </td>
                                    /*RITM1026096 C. Link 2023-11-08 Add button for conditional link to End of Shift Report if user selects Yes*/
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="EnvironmentalEvents" runat="server" CssClass="YesNoField"
                                            RepeatDirection="Horizontal" AutoPostBack="True" 
                                            OnSelectedIndexChanged="EnvironmentalEvents_SelectedIndexChanged">
                                            <asp:ListItem Value="True">Yes</asp:ListItem>
                                            <asp:ListItem Value="False">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:LinkButton ID="EndOfShiftReportButton" runat="server" Text="End of Shift Report"
                                            OnClientClick="if (!confirm('Are you sure you want to end the shift and fill the report?')) return false; window.open('https://forms.office.com/Pages/ResponsePage.aspx?id=C9mrSgl6F0qwTVLz9qj6sD4DTFx063tEm2Wz0QuW02dUMDJDTzhCNEwzV0FORVYwVFhDNDVISE9FTSQlQCN0PWcu'); return false;"
                                            Visible="False" />
                                    </td>
                                    
                                </tr>
                                <tr id="EnvironmentalTypeRow">
                                    <td class="left-column">
                                        <asp:Label ID="EnvironmentalEventTypeLabel" runat="server" EnableViewState="False">What type of environmental event was it?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="EnvironmentalEventType" runat="server" RepeatDirection="Horizontal"
                                            CssClass="EventTypeField">
                                            <asp:ListItem Value="Reportable">Reportable</asp:ListItem>
                                            <asp:ListItem Value="Reportable Exempt">Reportable Exempt</asp:ListItem>
                                            <asp:ListItem Value="Data Exclusions">Data Exclusions</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="EnvironmentalExplanationRow">
                                    <td class="left-column">
                                        <asp:Label ID="EnvironmentalExplanationLabel" runat="server" EnableViewState="False">Environmental Events Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="EnvironmentalExplanation" runat="server" CssClass="EventExplanationField"
                                            TextMode="MultiLine" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <td class="right-column">
                                    <asp:RadioButtonList ID="CemsEvents" runat="server" CssClass="YesNoField" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="CemsEvents_SelectedIndexChanged">
                                        <asp:ListItem Value="True">Yes</asp:ListItem>
                                        <asp:ListItem Value="False">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Button ID="EndOfShiftReportButton" runat="server" Text="End of Shift Report" OnClientClick="openEndOfShiftReport(); return false;" Visible="False" />
                                </td>
                                //RITM1026096 C. Link 2023-11-08 Add button for conditional link to End of Shift Report if user selects Yes                                
                                <tr id="CemsTypeRow">
                                    <td class="left-column">
                                        <asp:Label ID="CemsEventTypeLabel" runat="server" EnableViewState="False">What type of CEMS event was it?</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:RadioButtonList ID="CemsEventType" runat="server" RepeatDirection="Horizontal"
                                            CssClass="EventTypeField">
                                            <asp:ListItem Value="Reportable">Reportable</asp:ListItem>
                                            <asp:ListItem Value="Reportable Exempt">Reportable Exempt</asp:ListItem>
                                            <asp:ListItem Value="Data Exclusions">Data Exclusions</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr id="CemsExplanationRow">
                                    <td class="left-column">
                                        <asp:Label ID="CemsExplanationLabel" runat="server" EnableViewState="False">CEMS Events Explanation</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="CemsExplanation" runat="server" TextMode="MultiLine" CssClass="EventExplanationField"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="table-subheading" colspan="2">Health &amp; Safety Events
                                    </td>
                                </tr>                                
                                <tr style="display:none;">
                                    <td class="left-column">
                                        <asp:Label ID="OshaRecordableLabel" runat="server" EnableViewState="False" Visible="false">OSHA Recordable</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="OshaRecordableCheckBox" runat="server" onClick="showOptionalField('OshaRecordableCheckBox')"
                                            Text="None"  Visible="false"/>
                                        <br />
                                        <asp:TextBox ID="OshaRecordable" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled" Text="" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="left-column">
                                        <asp:Label ID="FirstAidLabel" runat="server" EnableViewState="False" Visible="false">First Aid</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="FirstAidCheckBox" runat="server" onClick="showOptionalField('FirstAidCheckBox')"
                                            Text="None"  Visible="false"/>
                                        <br />
                                        <asp:TextBox ID="FirstAid" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled" Text="" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="EmployeeSafetyIncidentsLabel" runat="server" EnableViewState="False">Employee Safety Incidents</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="EmployeeSafetyIncidentsCheckBox" runat="server" onClick="showOptionalField('EmployeeSafetyIncidentsCheckBox')"
                                            Text="None" />
                                        <br />
                                        <asp:TextBox ID="EmployeeSafetyIncidents" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="ContractorLabel" runat="server" EnableViewState="False">Contractor Safety Incidents</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="ContractorCheckBox" runat="server" onClick="showOptionalField('ContractorCheckBox')"
                                            Text="None" />
                                        <br />
                                        <asp:TextBox ID="Contractor" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="NearMissLabel" runat="server" EnableViewState="False">Serious Near Misses</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="NearMissCheckBox" runat="server" onClick="showOptionalField('NearMissCheckBox')"
                                            Text="None" />
                                        <br />
                                        <asp:TextBox ID="NearMiss" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="table-subheading" colspan="2">Critical Equipment OOS/Lack of Redundancy
                                    </td>
                                </tr>
                                <tr>
                                    <td class="left-column">
                                        <asp:Label ID="CommentsLabel" runat="server" EnableViewState="False">Comments</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:CheckBox ID="CommentsCheckBox" runat="server" onClick="showOptionalField('CommentsCheckBox')"
                                            Text="None" />
                                        <br />
                                        <asp:TextBox ID="Comments" runat="server" CssClass="OptionalField" TextMode="MultiLine"
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr id="commentsDateRow">
                                    <td class="left-column">
                                        <asp:Label ID="commentsDateLabel" runat="server" EnableViewState="False">Expected return to service:</asp:Label>
                                    </td>
                                    <td class="right-column">
                                        <asp:TextBox ID="CommentsDate" runat="server" CssClass="DateField"></asp:TextBox>
                                        <asp:CalendarExtender ID="CommentsDateCalendar" runat="server" TargetControlID="commentsDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="table-subheading" colspan="2">Facility Contact Information
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td colspan="2" style="text-align: center">If any of the following information is incorrect, please contact <a href="mailto:sdrew@covanta.com?Subject=<%= Facility.SelectedItem.Text %> Contact Information">Scott Drew</a>. <%--Thank you. --%></td>
                                <tr style="display:none;">
                                    <td class="left-column">Facility Manager Name: </td>
                                    <td class="right-column">
                                        <asp:Label ID="FacilityManagerName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="left-column">Facility Manager User ID: </td>
                                    <td class="right-column">
                                        <asp:Label ID="FacilityManagerUserId" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="left-column">Facility Manager Email Address: </td>
                                    <td class="right-column">
                                        <asp:Label ID="FacilityManagerEmail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="left-column"><%--Chief Engineer--%>Operations Manager Name: </td>
                                    <td class="right-column">
                                        <asp:Label ID="ChiefEngineerName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="left-column"><%--Chief Engineer--%>Operations Manager User ID: </td>
                                    <td class="right-column">
                                        <asp:Label ID="ChiefEngineerUserId" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td class="left-column"><%--Chief Engineer--%>Operations Manager Email Address: </td>
                                    <td class="right-column">
                                        <asp:Label ID="ChiefEngineerEmail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                        </table>
                        <div id="Errors" class="Error" runat="server" enableviewstate="False" visible="false">
                            <span id="ErrorsHeader">Errors<br />
                            </span>
                        </div>
                        <div id="Warnings" class="Warning" runat="server" enableviewstate="False" visible="false">
                            <span id="WarningsHeader">Warnings<br />
                            </span>
                        </div>
                        <div id="Buttons">
                            <asp:Button ID="SubmitButton" runat="server" Text="Submit Operation Report" OnClick="SubmitButton_Click" />
                            <asp:Button ID="ResetButton" runat="server" Text="Reset" OnClick="ResetButton_Click" />
                            <%-- RITM1026096 C. Link 2023-11-08 Add URL jump button for End of Shift Report to open in new window --%>
                            <asp:LinkButton ID="EndOfShiftReportButton" runat="server" OnClientClick="window.open('https://forms.office.com/Pages/ResponsePage.aspx?id=C9mrSgl6F0qwTVLz9qj6sD4DTFx063tEm2Wz0QuW02dUMDJDTzhCNEwzV0FORVYwVFhDNDVISE9FTSQlQCN0PWcu'); return false;" Text="End of Shift Report" />

                        </div>
                    </div>
                    <div id="ReportingDatesMissingPrompt" class="Prompt" runat="server" visible="false">
                        <p id="ReportingDatesMissingPromptText" runat="server">
                            Test Data.
                        </p>
                    </div>
                    <div id="UpdatePrompt" class="Prompt" runat="server" visible="false">
                        <p id="UpdatePromptText" runat="server">
                            A report for this date has already been submitted.
                        </p>
                        <p>
                            Would you like to update or replace the existing report?
                        </p>
                        <asp:Button ID="UpdateButton" runat="server" Text="Update" OnClick="UpdateButton_Click" />
                        <asp:Button ID="ReplaceButton" runat="server" Text="Replace" OnClick="ReplaceButton_Click" />
                    </div>
                    <div id="ConfirmationPrompt" class="Prompt" runat="server" visible="false">
                        <p>
                            The report has been submitted successfully.
                        </p>
                        <asp:Button ID="EditButton" runat="server" Text="Edit Report" OnClick="EditButton_Click" />
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress" runat="server">
                        <ProgressTemplate>
                            <div id="PleaseWait">
                                Please Wait...
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
		
    </script>
</body>
</html>
