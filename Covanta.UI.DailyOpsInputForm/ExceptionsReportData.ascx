<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExceptionsReportData.ascx.cs"
    Inherits="Covanta.UI.DailyOpsInputForm.ExceptionsReportData" %>
<style type="text/css">
    h2.ExceptionsReportHeading, td, th {
        font-family: Arial;
        font-size: 10pt;
    }

    table {
        margin-left: auto;
        margin-right: auto;
        border-style: none;
        width: 100%;
    }

    td {
        vertical-align: top;
        border-style: none;
    }

    th {
        border-style: none;
        text-align: left;
        padding-bottom: 5px;
    }

    h2.ExceptionsReportHeading {
        font-size: 12pt;
        font-weight: bold;
        text-decoration: underline;
        text-align: center;
    }

    .SummaryData {
        width: 50%;
    }

    .TypeRow {
        padding-top: 5px;
        padding-bottom: 5px;
    }

    .FacilityRow {
        padding-bottom: 5px;
        padding-top: 5px;
    }

    .SubFacilityRow {
        padding-bottom: 5px;
    }
</style>
<h2 class="ExceptionsReportHeading">Summary</h2>
<asp:GridView ID="SummaryData" class="SummaryData" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" />
        <asp:BoundField HeaderText="Total" DataField="FacilitiesReportingExpected" />
        <asp:BoundField HeaderText="Reporting" DataField="FacilitiesReportingActual" />
        <asp:TemplateField HeaderText="Facilities Not Reported">
            <ItemTemplate>
                <asp:Literal runat="server" ID="FacilitiesNotReported"
                    Text='<%# string.Join("<br />", Eval("FacilitiesNotReported").ToString().Split(new []{","},StringSplitOptions.None)) %>'>
                </asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Health &amp; Safety Events</h2>
<asp:GridView ID="HealthSafety" runat="server" AutoGenerateColumns="false" OnRowDataBound="HealthSafety_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="25%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Event Type" DataField="EventType" ItemStyle-Width="25%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Description" DataField="EventDescription" ItemStyle-Width="25%" ItemStyle-CssClass="SubFacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Environmental Events</h2>
<asp:GridView ID="EnvironmentalEvents" runat="server" AutoGenerateColumns="false" OnRowDataBound="EnvironmentalEvents_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="25%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Event Type" DataField="EventType" ItemStyle-Width="25%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Description" DataField="EventDescription" ItemStyle-Width="25%" ItemStyle-CssClass="SubFacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Boiler Outage Log</h2>
<asp:GridView ID="BoilerOutageData1" runat="server" OnRowDataBound="BoilerOutageData1_RowDataBound" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="11%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="11%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Boiler" DataField="BoilerNumber" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Downtime (Hrs)" DataField="Downtime" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Explanation" DataField="UnscheduledOutageExplanation" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Cumulative Downtime (Days)" DataField="CumulativeDowntime" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField DataField="MonthToDate" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Estimated Return to Service Date" DataField="ExpectedRepairDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Turbine/Generator Outage Log</h2>
<asp:GridView ID="TurbineGeneratorOutage" runat="server" OnRowDataBound="TurbineGeneratorOutage_RowDataBound" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="11%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="11%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Boiler" DataField="BoilerNumber" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Downtime (Hrs)" DataField="Downtime" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Explanation" DataField="UnscheduledOutageExplanation" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Cumulative Downtime (Days)" DataField="CumulativeDowntime" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField DataField="MonthToDate" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Estimated Return to Service Date" DataField="ExpectedRepairDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Metals Systems Outage Log</h2>
<asp:GridView ID="MetalSystemsOutageData" runat="server" AutoGenerateColumns="false" OnRowDataBound="MetalSystemsOutageData_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="11%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="11%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="System" DataField="SystemType" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Will ash be reprocessed?" DataField="WasReprocessed" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Downtime (Hrs)" DataField="Downtime" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Explanation" DataField="UnscheduledOutageExplanation" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Current Event Cumulative Downtime (Days)" DataField="CumulativeDowntime" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField DataField="MonthToDate" ItemStyle-Width="11%" ItemStyle-CssClass="SubFacilityRow" />
        <asp:BoundField HeaderText="Estimated Return to Service Date" DataField="ExpectedRepairDate" ItemStyle-Width="11%" DataFormatString="{0:M/d/yyyy}" ItemStyle-CssClass="SubFacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Critical Assets in Alarm</h2>
<asp:GridView ID="CriticalAssestsInAlarm" runat="server" AutoGenerateColumns="false" OnRowDataBound="CriticalAssestsInAlarm_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="25%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Critical Assets in Alarm" DataField="CriticalAssetsInAlarm" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Estimated Return to Service Date" DataField="CriticalAssetsExpectedBackOnlineDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Critical Equipment OOS/Lack of Redundancy</h2>
<asp:GridView ID="CriticalEquipment" runat="server" AutoGenerateColumns="false" OnRowDataBound="CriticalEquipment_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="25%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Comments" DataField="Comments" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Estimated Return to Service Date" DataField="CriticalEquipmentOOSExpectedBackOnlineDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Fire Protection Impairments</h2>
<asp:GridView ID="FireProtection" runat="server" AutoGenerateColumns="false" OnRowDataBound="FireProtection_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="25%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="FacilityDescription" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Systems Out of Service" DataField="FireSystemOutOfService" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Estimated Return to Service Date" DataField="FireSystemOutOfServiceExpectedBackOnlineDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="25%" ItemStyle-CssClass="FacilityRow" />
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">MSW Inventory Exceptions</h2>
<asp:GridView ID="gridMSWInventoryExceptions" runat="server" AutoGenerateColumns="false" OnRowDataBound="gridMSWInventoryExceptions_RowDataBound">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" ItemStyle-Width="16%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Facility" DataField="Facility" ItemStyle-Width="16%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Limit" DataField="Limit" ItemStyle-Width="16%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Actual Inventory (tons)" DataField="ActualInventory" ItemStyle-Width="16%" ItemStyle-CssClass="FacilityRow" />
        <asp:BoundField HeaderText="Inventory Min Limit 1.5 days Run (tons)" DataField="InventoryMinLimit" ItemStyle-Width="16%" ItemStyle-CssClass="TypeRow" />
        <asp:BoundField HeaderText="Inventory Max 90% of Limit (tons)" DataField="InventoryMaxLimit" ItemStyle-Width="16%" ItemStyle-CssClass="FacilityRow" />
    </Columns>
</asp:GridView>
<br />
<h2 class="ExceptionsReportHeading">Need Access?
</h2>
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
                                    <td class="FacilityRow" style="width: 25%;"></td>
                                    <td class="FacilityRow" style="width: 75%;">
                                        <b style="text-decoration: underline;">Email:</b>
                                        <a href="mailto:Frontline@Covanta.com">Frontline@Covanta.com</a> requesting to be added to <b><a href="mailto:#DomesticOperationsExceptionsReport@covanta.com">#Domestic Operations Exceptions Report</a></b> distribution group.
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
