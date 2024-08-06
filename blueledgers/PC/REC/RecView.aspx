<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="RecView.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RECView" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPageCuz.ascx" TagName="ListPageCuz"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPageCuz ID="ListPageCuz" runat="server" ListPageURL="~/PC/REC/RecLst.aspx"
        PageCode="[PC].[vRECList]" Title="Receiving" />
</asp:Content>
