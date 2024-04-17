<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StoreLst.aspx.cs" Inherits="BlueLedger.PL.Option.Store.StoreLst" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%-- <%@ Register Src="../../UserControl/ViewHandler/ListPage.ascx" TagName="ListPage" TagPrefix="uc1" %> --%>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
<uc1:ListPage2 ID="ListPage1" runat="server" KeyFieldName="LocationCode" Title="Store" PageCode="[IN].[vStoreLocation]" AllowViewCreate="False" DetailPageURL="Store.aspx" />
<%--EditPageURL="~/Option/Store/StoreEdit.aspx"--%>
</asp:Content>

