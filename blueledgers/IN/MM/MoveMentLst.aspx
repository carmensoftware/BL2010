<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoveMentLst.aspx.cs" Inherits="BlueLedger.PL.IN.MM.MovementLst"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage2" runat="server"
        KeyFieldName="RefId" Module="IN" PageCode="[IN].[vMoveMent]" SubModule="MM" DetailPageURL="MoveMentDt.aspx"
        Title="Movement" />
</asp:Content>
