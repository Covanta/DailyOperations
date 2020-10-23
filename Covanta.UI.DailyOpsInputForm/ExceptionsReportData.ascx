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
<h2 class="ExceptionsReportHeading">Summary
</h2>
<asp:GridView ID="SummaryData" class="SummaryData" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="25%" />
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

<h2 class="ExceptionsReportHeading">Health &amp; Safety Events
</h2>
<asp:GridView ID="HealthSafetyFacilityTypeData" runat="server" AutoGenerateColumns="false"
    OnRowCreated="HealthSafetyFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="18%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="HealthSafetyData" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                    OnRowCreated="HealthSafetyData_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="20%" ItemStyle-CssClass="FacilityRow" />
                        <asp:TemplateField ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow">
                            <ItemTemplate>
                                <asp:GridView ID="HealthSafetyEventData" runat="server" AutoGenerateColumns="false"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField DataField="EventType" ItemStyle-Width="29%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Description" ItemStyle-Width="71%" ItemStyle-CssClass="SubFacilityRow" />
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Event Type" HeaderStyle-Width="21%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="51%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Environmental Events
</h2>
<asp:GridView ID="EnvironmentalFacilityTypeData" runat="server" AutoGenerateColumns="false"
    OnRowCreated="EnvironmentalFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="18%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="EnvironmentalData" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                    OnRowCreated="EnvironmentalData_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="20%" ItemStyle-CssClass="FacilityRow" />
                        <asp:TemplateField ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow">
                            <ItemTemplate>
                                <asp:GridView ID="EnvironmentalEventData" runat="server" AutoGenerateColumns="false"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField DataField="EventType" ItemStyle-Width="29%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Description" ItemStyle-Width="71%" ItemStyle-CssClass="SubFacilityRow" />
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Event Type" HeaderStyle-Width="21%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="51%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Boiler Outage Log
</h2>
<asp:GridView ID="BoilerOutageFacilityTypeData" runat="server" AutoGenerateColumns="false"
    OnRowCreated="BoilerOutageFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="10%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="BoilerOutageData" runat="server" AutoGenerateColumns="False" ShowHeader="False" OnRowCreated="BoilerOutageData_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="10.1%" ItemStyle-CssClass="FacilityRow" />
                        <asp:TemplateField ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow">
                            <ItemTemplate>
                                <asp:GridView ID="BoilerStatusData" runat="server" AutoGenerateColumns="False" ShowHeader="False" OnRowDataBound="BoilerStatusData_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="BoilerNumber" ItemStyle-Width="12.4%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Status" ItemStyle-Width="16.6%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Downtime" ItemStyle-Width="15%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="UnscheduledOutageExplanation" ItemStyle-Width="30%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="CumulativeDowntime" ItemStyle-Width="15.6%" ItemStyle-CssClass="SubFacilityRow" />
                                        <%--<asp:BoundField DataField="WeekToDate" ItemStyle-Width="13%" ItemStyle-CssClass="SubFacilityRow" />--%>
                                        <asp:BoundField DataField="MonthToDate" ItemStyle-Width="13%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="ExpectedRepairDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="16%" ItemStyle-CssClass="SubFacilityRow" />
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Boiler" HeaderStyle-Width="10%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="13.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Downtime" HeaderStyle-Width="11.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Explanation" HeaderStyle-Width="22.9%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Cumulative Downtime" HeaderStyle-Width="12.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <%--<asp:TemplateField HeaderText="Cumulative Downtime for Week Ending MM/DD" HeaderStyle-Width="10.4%" ItemStyle-CssClass="TypeRow">
		</asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Cumulative Downtime for Month of Month Year" HeaderStyle-Width="10.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Estimated Return to Service Date" HeaderStyle-Width="12.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Turbine/Generator Outage Log
</h2>
<asp:GridView ID="TurbineOutageFacilityTypeData" runat="server" AutoGenerateColumns="false"
    OnRowCreated="TurbineOutageFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="10%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="TurbineOutageData" runat="server" AutoGenerateColumns="false"
                    OnRowCreated="TurbineOutageData_RowCreated" ShowHeader="false">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="10.1%" ItemStyle-CssClass="FacilityRow" />
                        <asp:TemplateField ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow">
                            <ItemTemplate>
                                <asp:GridView ID="TurbineStatusData" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                    OnRowDataBound="TurbineStatusData_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="BoilerNumber" ItemStyle-Width="12.4%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Status" ItemStyle-Width="16.6%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Downtime" ItemStyle-Width="15%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="UnscheduledOutageExplanation" ItemStyle-Width="30%"
                                            ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="CumulativeDowntime" ItemStyle-Width="15.6%" ItemStyle-CssClass="SubFacilityRow" />
                                        <%--<asp:BoundField DataField="WeekToDate" ItemStyle-Width="13%" ItemStyle-CssClass="SubFacilityRow" />--%>
                                        <asp:BoundField DataField="MonthToDate" ItemStyle-Width="13%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="ExpectedRepairDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="16%"
                                            ItemStyle-CssClass="SubFacilityRow" />

                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Turbine" HeaderStyle-Width="10%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="13.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Downtime" HeaderStyle-Width="11.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Explanation" HeaderStyle-Width="22.9%"
            ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Cumulative Downtime" HeaderStyle-Width="12.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <%--<asp:TemplateField HeaderText="Weekly Downtime" HeaderStyle-Width="10.4%" ItemStyle-CssClass="TypeRow">
		</asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Monthly Downtime" HeaderStyle-Width="10.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Estimated Return to Service Date" HeaderStyle-Width="12.4%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Metals Systems Outage Log
</h2>
<asp:GridView ID="MetalsSystemsFacilityTypeData" runat="server" AutoGenerateColumns="false"
    OnRowCreated="MetalsSystemsFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="10%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="MetalsSystemsData" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                    OnRowCreated="MetalsSystemsData_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="9.2%" ItemStyle-CssClass="FacilityRow" />
                        <asp:TemplateField ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow">
                            <ItemTemplate>
                                <asp:GridView ID="MetalsSystemsStatusData" runat="server" AutoGenerateColumns="False"
                                    ShowHeader="False">
                                    <Columns>
                                        <asp:BoundField DataField="SystemType" ItemStyle-Width="13.4%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="WasReprocessed" ItemStyle-Width="10%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="Downtime" ItemStyle-Width="15%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="DowntimeExplanation" ItemStyle-Width="23%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="CumulativeDowntime" ItemStyle-Width="16.4%" ItemStyle-CssClass="SubFacilityRow" />
                                        <%--<asp:BoundField DataField="WeekToDate" ItemStyle-Width="12.4%" ItemStyle-CssClass="SubFacilityRow"/>--%>
                                        <asp:BoundField DataField="MonthToDate" ItemStyle-Width="13%" ItemStyle-CssClass="SubFacilityRow" />
                                        <asp:BoundField DataField="ExpectedRepairDate" DataFormatString="{0:M/d/yyyy}" ItemStyle-Width="14%"
                                            ItemStyle-CssClass="SubFacilityRow" />
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="System" HeaderStyle-Width="12%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Will ash be reprocessed?" HeaderStyle-Width="12%"
            ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Downtime" HeaderStyle-Width="11%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Explanation" HeaderStyle-Width="20%" ItemStyle-CssClass="TypeRow" ItemStyle-HorizontalAlign="Center"></asp:TemplateField>
        <asp:TemplateField HeaderText="Current Event Cumulative Downtime" HeaderStyle-Width="14%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <%--<asp:TemplateField HeaderText="Weekly Downtime" HeaderStyle-Width="11%" ItemStyle-CssClass="TypeRow">
		</asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Monthly Downtime" HeaderStyle-Width="11%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Estimated Return to Service Date" HeaderStyle-Width="15%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Critical Assets in Alarm</h2>
<asp:GridView ID="CriticalAssetsFacilityTypeData" runat="server" AutoGenerateColumns="False"
    OnRowCreated="CriticalAssetsFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="18%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="CriticalAssetsData" runat="server" AutoGenerateColumns="false"
                    ShowHeader="false">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="20%" ItemStyle-CssClass="FacilityRow" />
                        <asp:BoundField DataField="CriticalAssetsInAlarm" ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow" />
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Critical Assets in Alarm" HeaderStyle-Width="72%"
            ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Critical Equipment OOS/Lack of Redundancy 
</h2>
<asp:GridView ID="CommentsFacilityTypeData" runat="server" AutoGenerateColumns="False"
    OnRowCreated="CommentsFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="18%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="CommentsData" runat="server" AutoGenerateColumns="False" ShowHeader="false">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="20%" ItemStyle-CssClass="FacilityRow" />
                        <asp:BoundField DataField="Comments" ItemStyle-Width="80%" ItemStyle-CssClass="FacilityRow" />
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="72%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
    </Columns>
</asp:GridView>

<h2 class="ExceptionsReportHeading">Fire Protection Impairments
</h2>
<asp:GridView ID="FireProtectionFacilityTypeData" runat="server" AutoGenerateColumns="False"
    OnRowCreated="FireProtectionFacilityTypeData_RowCreated">
    <Columns>
        <asp:BoundField HeaderText="Facility Type" DataField="FacilityType" HeaderStyle-Width="10%"
            ItemStyle-CssClass="TypeRow" />
        <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="18%" ItemStyle-CssClass="TypeRow">
            <ItemTemplate>
                <asp:GridView ID="FireProtectionData" runat="server" AutoGenerateColumns="False"
                    ShowHeader="False">
                    <Columns>
                        <asp:BoundField DataField="FaciltyDescription" ItemStyle-Width="20%" ItemStyle-CssClass="FacilityRow" />
                        <asp:BoundField DataField="FireSystemOutOfService" ItemStyle-Width="60%" ItemStyle-CssClass="FacilityRow" />
                        <asp:BoundField DataField="FireSystemOutOfServiceExpectedBackOnlineDate" DataFormatString="{0:M/d/yyyy}"
                            ItemStyle-Width="20%" ItemStyle-CssClass="FacilityRow" />
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Systems Out of Service" HeaderStyle-Width="54%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
        <asp:TemplateField HeaderText="Estimated Return to Service Date" HeaderStyle-Width="18%" ItemStyle-CssClass="TypeRow"></asp:TemplateField>
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
                                    <td class="FacilityRow" style="width: 20%;"></td>
                                    <td class="FacilityRow" style="width: 80%;">
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
