<%@ Page Title="Item Mapping" AutoEventWireup="true" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" CodeFile="ItemMapping.aspx.cs" Inherits="BlueLedger.PL.PT.Sale.ItemMapping" %>

<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .mb-3
        {
            margin-bottom: 10px !important;
        }
        .me-3
        {
            margin-right: 10px;
        }
        .text-end
        {
            text-align: right;
        }
        .w-100
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <div class="mb-3" style="background-color: #4d4d4d; width: 100%; padding: 2px; height: 32px;">
        <div style="margin-left: 10px; float: left; margin-top: 5px;">
            <asp:Label ID="lbl_Title" runat="server" Font-Size="Small" Text="Item Mapping" SkinID="LBL_HD_WHITE" />
        </div>
        <div style="margin-right: 10px; float: right;">
        </div>
    </div>
    <asp:GridView ID="gv_Item" runat="server" ClientIDMode="Static" SkinID="GRD_V1" Width="100%" Font-Size="Small" DataKeyNames="ItemCode" AllowPaging="true"
        PageSize="25" HeaderStyle-HorizontalAlign="Left" OnPageIndexChanging="gv_Item_PageIndexChanging" OnRowDataBound="gv_Item_RowDataBound">
        <Columns>
            <%--Item code--%>
            <asp:TemplateField HeaderText="Item" ItemStyle-Width="100">
                <ItemTemplate>
                    <%#Eval("ItemCode")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--Item name--%>
            <asp:TemplateField HeaderText="Name" ItemStyle-Width="200">
                <ItemTemplate>
                    <%#Eval("ItemName")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--Recipe--%>
            <asp:TemplateField HeaderText="Recipe" ItemStyle-Width="320">
                <ItemTemplate>
                    <dx:ASPxComboBox ID="ddl_Recipe" runat="server" Width="320" DropDownWidth="340" DropDownStyle="DropDownList" EnableCallbackMode="true" CallbackPageSize="30"
                        IncrementalFilteringMode="Contains">
                    </dx:ASPxComboBox>
                </ItemTemplate>
            </asp:TemplateField>
            <%--Action--%>
            <asp:TemplateField HeaderText="" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:LinkButton ID="btn_Save" runat="server" CommandName="Save" Text="Save"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
