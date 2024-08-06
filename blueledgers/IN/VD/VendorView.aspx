<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VendorView.aspx.cs" Inherits="BlueLedger.PL.IN.VendorView"
    MasterPageFile="~/master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="~/UserControl/ViewHandler/ListPageCuz.ascx" TagName="ListPageCuz"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPageCuz ID="ListPageCuz" runat="server" ListPageURL="~/IN/VD/VendorList.aspx"
        Title="Vendor Profile" PageCode="[AP].[vVendorProfile]" />
</asp:Content>
