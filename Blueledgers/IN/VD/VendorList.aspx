<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="VendorList.aspx.cs" Inherits="BlueLedger.PL.IN.VendorList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Src="~/UserControl/ViewHandler/ListPage.ascx" TagName="ListPage" TagPrefix="uc1" %>--%>
<%@ Register Src="~/UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc1" %>
<asp:Content ID="cnt_VendorList" runat="server" ContentPlaceHolderID="cph_Main">
    <uc1:ListPage2 ID="ListPage" runat="server" Title="Vendor Profile" PageCode="[AP].[vVendorProfile]" KeyFieldName="VendorCode" DetailPageURL="Vendor.aspx"
        ListPageCuzURL="VendorView.aspx" EditPageURL="~/IN/VD/VendorEdit.aspx" />
    <br />
    <asp:Button runat="server" ID="btn_SyncVendor" Text="Update Vendor from Carmen" OnClick="btn_SyncVendor_Click"  Visible="false"/>
    <asp:Label runat="server" ID="lbl_Test" Text="" />
</asp:Content>
