<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="CALst.aspx.cs" Inherits="BlueLedger.PL.IN.CA.CALst" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage" runat="server" AllowVoid="False" AllowDelete="false"
        DetailPageURL="~/IN/CA/CA.aspx" EditPageURL="~/IN/CA/CAEdit.aspx" KeyFieldName="RefNo" ListPageCuzURL="~/IN/CA/CAView.aspx"
        Module="IN" PageCode="[IN].[vCAList]" SubModule="CA" Title="Cost Allocation" />
</asp:Content>
