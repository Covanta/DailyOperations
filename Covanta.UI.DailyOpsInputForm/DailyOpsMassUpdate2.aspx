<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyOpsMassUpdate2.aspx.cs" Inherits="Covanta.UI.DailyOpsInputForm.DailyOpsMassUpdate2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Scripts/opsscripts.js"></script>
</head>
<body>

    <label id="lblValueChanged"></label>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
        </asp:ToolkitScriptManager>
        <asp:Panel ID="OpsPanel" runat="server" DefaultButton="btnUpdate">
            <div style="font-weight: 700; font-size: large">

                <br />
                <h1 style="text-align: center">
                    <asp:Literal ID="Literal1" runat="server" Text="Covanta WtE Operations Morning Report - Mass Update Electronic Form"></asp:Literal>
                </h1>

                <br />
                <br />
                Facility:
&nbsp;&nbsp;
		<asp:DropDownList ID="Facility" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Facility_SelectedIndexChanged">
        </asp:DropDownList>
                <br />
                Choose A single date's record:&nbsp;
		<asp:TextBox ID="ReportingDate" runat="server" OnTextChanged="ReportingDate_TextChanged"
            AutoPostBack="True" AutoCompleteType="Disabled"></asp:TextBox>
                <asp:CalendarExtender ID="ReportingDateCalendar" runat="server" TargetControlID="ReportingDate"
                    Format="yyyy-MM-dd" DefaultView="Months">
                </asp:CalendarExtender>
                <br />
                <asp:Button ID="btnHome" runat="server" OnClick="btnHome_Click" Text=" Main Screen " />
                <asp:Button ID="bt_ChooseAll" runat="server" Text="All Active Records" OnClick="bt_ChooseAll_Click" />
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text=" Update " />

                <asp:Label ID="lblActiveNumber" runat="server"></asp:Label>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>

            <div>
                <asp:GridView ID="DailyOpsRecordsGridView" runat="server" AllowPaging="false" AutoGenerateColumns="false" BackColor="White" BorderColor="SpringGreen" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                    <FooterStyle BackColor="White" ForeColor="Tomato" />
                    <Columns>
                        <%--                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                        SortExpression="ID" />--%>

                        <%--                    <asp:TemplateField HeaderText="ID" SortExpression="ID" Visible="true">
                        <ItemTemplate>
                            <asp:TextBox ID="TB_ID" runat="server" Text='<%# Bind("ID") %>'
                                OnTextChanged="TextBox_TextChanged" BorderStyle="None" Visible="true"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Reporting Date" SortExpression="PRICE">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_ReportingDate" runat="server" Text='<%# Eval("ReportingDate","{0:yyyy-MM-dd}") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TonsDelivered" SortExpression="TonsDelivered">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_TonsDelivered" runat="server" Text='<%# Bind("TonsDelivered") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TonsProcessed" SortExpression="TonsProcessed">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_TonsProcessed" runat="server" Text='<%# Bind("TonsProcessed") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SteamProduced" SortExpression="SteamProduced">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_SteamProduced" runat="server" Text='<%# Bind("SteamProduced") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SteamSold" SortExpression="SteamSold">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_SteamSold" runat="server" Text='<%# Bind("SteamSold") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="NetElectric" SortExpression="NetElectric">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_NetElectric" runat="server" Text='<%# Bind("NetElectric") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="PitInventory" SortExpression="PitInventory">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_PitInventory" runat="server" Text='<%# Bind("PitInventory") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="B1 Status" SortExpression="OutageTypeBoiler1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeBoiler1" runat="server" Text='<%# Bind("OutageTypeBoiler1") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="DownTimeBoiler1" SortExpression="DownTimeBoiler1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeBoiler1" runat="server" Text='<%# Bind("DownTimeBoiler1") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="B2 Status" SortExpression="OutageTypeBoiler2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeBoiler2" runat="server" Text='<%# Bind("OutageTypeBoiler2") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="DownTimeBoiler2" SortExpression="DownTimeBoiler2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeBoiler2" runat="server" Text='<%# Bind("DownTimeBoiler2") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="B3 Status" SortExpression="OutageTypeBoiler3">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeBoiler3" runat="server" Text='<%# Bind("OutageTypeBoiler3") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="DownTimeBoiler3" SortExpression="DownTimeBoiler3">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeBoiler3" runat="server" Text='<%# Bind("DownTimeBoiler3") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="B4 Status" SortExpression="OutageTypeBoiler4">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeBoiler4" runat="server" Text='<%# Bind("OutageTypeBoiler4") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="DownTimeBoiler4" SortExpression="DownTimeBoiler4">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeBoiler4" runat="server" Text='<%# Bind("DownTimeBoiler4") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="B5 Status" SortExpression="OutageTypeBoiler5">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeBoiler5" runat="server" Text='<%# Bind("OutageTypeBoiler5") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="DownTimeBoiler5" SortExpression="DownTimeBoiler5">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeBoiler5" runat="server" Text='<%# Bind("DownTimeBoiler5") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="B6 Status" SortExpression="OutageTypeBoiler6">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeBoiler6" runat="server" Text='<%# Bind("OutageTypeBoiler6") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="DownTimeBoiler6" SortExpression="DownTimeBoiler6">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeBoiler6" runat="server" Text='<%# Bind("DownTimeBoiler6") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TG1 Status" SortExpression="OutageTypeTurbGen1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeTurbGen1" runat="server" Text='<%# Bind("OutageTypeTurbGen1") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TG1 Downtime" SortExpression="DownTimeTurbGen1">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeTurbGen1" runat="server" Text='<%# Bind("DownTimeTurbGen1") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TG2 Status" SortExpression="OutageTypeTurbGen2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_OutageTypeTurbGen2" runat="server" Text='<%# Bind("OutageTypeTurbGen2") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TG2 Downtime" SortExpression="DownTimeTurbGen2">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_DownTimeTurbGen2" runat="server" Text='<%# Bind("DownTimeTurbGen2") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="FerrousTons" SortExpression="FerrousTons">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_FerrousTons" runat="server" Text='<%# Bind("FerrousTons") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="NonFerrousTons" SortExpression="NonFerrousTons">
                            <ItemTemplate>
                                <asp:TextBox ID="tb_NonFerrousTons" runat="server" Text='<%# Bind("NonFerrousTons") %>'
                                    OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>


                    </Columns>

                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />

                </asp:GridView>
            </div>
        </asp:Panel>

    </form>
