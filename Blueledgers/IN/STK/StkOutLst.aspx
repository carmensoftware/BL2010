<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StkOutLst.aspx.cs" Inherits="BlueLedger.PL.IN.STK.StkOutLst"
    MasterPageFile="~/Master/In/SkinDefault.master" Theme="" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register src="../../UserControl/ViewHandler/ListPage2.ascx" tagname="ListPage2" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <%--<uc1:ListPageStdReq ID="ListPage" runat="server" PageCode="[IN].[vStockOut]"
        EditPageURL="~/IN/STK/StkOutEdit.aspx" DetailPageURL="StkOutDt.aspx" KeyFieldName="RefId"
        AllowDelete="False" AllowPrint="True" Title="Stock Out" />--%>
    <uc2:ListPage2 ID="ListPage2" runat="server" PageCode="[IN].[vStockOut]"
        EditPageURL="~/IN/STK/StkOutEdit.aspx" DetailPageURL="StkOutDt.aspx" KeyFieldName="RefId"
        AllowDelete="False" AllowPrint="True" Title="Stock Out"/>
</asp:Content>
