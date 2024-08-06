<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="EOPView.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOPView" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPageCuz.ascx" TagName="ListPageCuz"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPageCuz ID="ListPageCuz" runat="server" ListPageURL="~/PC/EOP/EOPList.aspx"
        PageCode="[IN].[vEOPList]" Title="End of Month Balance" />
</asp:Content>
