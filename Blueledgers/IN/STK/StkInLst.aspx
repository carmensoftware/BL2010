<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StkInLst.aspx.cs" Inherits="BlueLedger.PL.IN.STK.StkInLst"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc2:ListPage2 ID="ListPage2" runat="server" PageCode="[IN].[vStockIn]" EditPageURL="~/IN/STK/StkInEdit.aspx"
        DetailPageURL="StkInDt.aspx" KeyFieldName="RefId" AllowDelete="False" AllowPrint="True"
        Title="Stock In" SubModule="SI" />
</asp:Content>
