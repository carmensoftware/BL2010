<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="PoView.aspx.cs" Inherits="BlueLedger.PL.PC.PO.PoView" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPageCuz.ascx" TagName="ListPageCuz"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">

    <uc1:ListPageCuz ID="ListPageCuz" runat="server" ListPageURL="~/PC/PO/PoList.aspx"
        PageCode="[PC].[vPoList]" Title="Purchase Order" />
</asp:Content>
