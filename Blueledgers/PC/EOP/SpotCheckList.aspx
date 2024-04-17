<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="SpotCheckList.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.SpotCheckList" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage" runat="server" AllowCreate="True" DetailPageURL="SpotCheck.aspx" EditPageURL="~/PC/EOP/SpotCheck.aspx" PageCode="[IN].[vSpotCheckList]"
        Title="Spot Check" KeyFieldName="Id" ListPageCuzURL="EOPView.aspx" />
</asp:Content>
