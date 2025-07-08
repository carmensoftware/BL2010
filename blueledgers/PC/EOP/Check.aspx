<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Check.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.Check" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <hr />
    <div class="flex flex-justify-content-between">
        <div>
            <span class="me-20">Period</span>
            <asp:DropDownList runat="server" ID="ddl_Period" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddl_Period_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div>
            <asp:Button runat="server" ID="btn_Export" Text="Export" OnClick="btn_Export_Click" />
        </div>
        <%--&nbsp;&nbsp;&nbsp; <span class="me-20">Filter</span>
        <asp:DropDownList runat="server" ID="ddl_Filter" Width="200">
            <asp:ListItem Value="" Text="All" />
        </asp:DropDownList>--%>
    </div>
    <hr />
    <h2>Incorrect EOP Adjustment</h2>
    <asp:GridView ID="gv1" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="false" EmptyDataText="No any data">
        <HeaderStyle HorizontalAlign="Left" Font-Size="Medium" />
        <RowStyle Font-Size="Medium" />
        <Columns>
            <asp:BoundField HeaderText="Location" DataField="LocationCode" />
            <asp:BoundField HeaderText="Product" DataField="ProductCode" />
            <asp:BoundField HeaderText="EOP Qty" DataField="EopQty" />
            <asp:BoundField HeaderText="Onhand Before EOP" DataField="OnhandBeforeEop" />
            <asp:BoundField HeaderText="IN" DataField="AdjIn" />
            <asp:BoundField HeaderText="OUT" DataField="AdjOut" />
            <asp:BoundField HeaderText="Onhand" DataField="Onhand" />
            <asp:BoundField HeaderText="EopId" DataField="EopId" />
            <asp:BoundField HeaderText="EopDtId" DataField="EopDtId" />
        </Columns>
    </asp:GridView>
    <hr />
    <h2>Not found in EOP</h2>
    <asp:GridView ID="gv2" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="false" EmptyDataText="No any data">
        <HeaderStyle HorizontalAlign="Left" Font-Size="Medium" />
        <RowStyle Font-Size="Medium" />
        <Columns>
            <asp:BoundField HeaderText="Location" DataField="LocationCode" />
            <asp:BoundField HeaderText="Product" DataField="ProductCode" />
            <asp:BoundField HeaderText="Onhand" DataField="Onhand" />
        </Columns>
    </asp:GridView>
    
</asp:Content>
