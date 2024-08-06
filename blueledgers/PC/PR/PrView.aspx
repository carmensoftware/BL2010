<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="PrView.aspx.cs" Inherits="BlueLedger.PL.PC.PR.PrView" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPageCuz.ascx" TagName="ListPageCuz"
    TagPrefix="uc1" %>

 <%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPageCuz ID="ListPageCuz" runat="server" ListPageURL="~/PC/PR/PrList.aspx"
        PageCode="[PC].[vPrList]" Title="Purchase Request" />
</asp:Content>
