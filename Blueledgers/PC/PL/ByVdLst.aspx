<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ByVdLst.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVdLst" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage" runat="server" AllowViewCreate="False" KeyFieldName="ID" PageCode="[IN].[vPriceListByVendor]" Title="Price List by Vendor"
        DetailPageURL="ByVd.aspx" EditPageURL="~/PC/PL/ByVdEdit.aspx" />
</asp:Content>
