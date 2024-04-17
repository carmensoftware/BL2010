<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="CnList.aspx.cs" Inherits="BlueLedger.PL.PC.CN.CnList" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage2" runat="server" DetailPageURL="Cn.aspx" KeyFieldName="CnNo"
        ListPageCuzURL="CnView.aspx" PageCode="[PC].[vCnList]" Title="Credit Note" Module="PC"
        SubModule="CN" WorkFlowEnable="false" />
</asp:Content>
