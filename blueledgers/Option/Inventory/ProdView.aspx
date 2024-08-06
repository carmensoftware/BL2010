<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdView.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdView" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPageCuz.ascx" TagName="ListPageCuz"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPageCuz ID="ListPageCuz" runat="server" ListPageURL="ProdList.aspx" 
        Title="Product" PageCode="[IN].[vProductList]" />
</asp:Content>
