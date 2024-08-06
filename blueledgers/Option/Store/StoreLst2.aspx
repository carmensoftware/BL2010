<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StoreLst2.aspx.cs" Inherits="BlueLedger.PL.Option.Store.StoreLst2" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage1" runat="server" KeyFieldName="LocationCode" Title="Store"
        PageCode="[IN].[vStoreLocation]" AllowViewCreate="False" DetailPageURL="Store2.aspx" />
</asp:Content>
