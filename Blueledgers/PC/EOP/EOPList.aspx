<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="EOPList.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOPList" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Src="../../UserControl/ViewHandler/ListPage.ascx" TagName="ListPage"
    TagPrefix="uc1" %>--%>
    <%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage" runat="server" AllowCreate="True" DetailPageURL="EOP.aspx"
        EditPageURL="~/PC/EOP/EOPEdit.aspx" PageCode="[IN].[vEOPList]" Title="Physical Count"
        KeyFieldName="EopId" ListPageCuzURL="EOPView.aspx"/>
</asp:Content>
