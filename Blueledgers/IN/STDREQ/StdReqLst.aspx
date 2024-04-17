<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StdReqLst.aspx.cs" Inherits="BlueLedger.PL.IN.STDREQ.StdReqLst" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <uc2:ListPage2 ID="ListPage2" runat="server" DetailPageURL="../STDREQ/StdReqDt.aspx"
        EditPageURL="~/IN/STDREQ/StdReqEdit.aspx" KeyFieldName="RefId" PageCode="[IN].[vStandardRequisition]"
        Title="Standard Requisition" Module="IN" SubModule="StandardRequisition" />
</asp:Content>
