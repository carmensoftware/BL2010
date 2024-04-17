<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdList.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdList" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Src="../../UserControl/ViewHandler/ListPage.ascx" TagName="ListPage"
    TagPrefix="uc1" %>--%>
    <%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage" runat="server" Title="Product" PageCode="[IN].[vProductList]"
        KeyFieldName="ProductCode" DetailPageURL="Prod2.aspx" ListPageCuzURL="ProdView.aspx"
        EnableTheming="True" />
</asp:Content>
