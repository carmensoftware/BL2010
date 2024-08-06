<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeList.aspx.cs" Inherits="BlueLedger.PL.PT.RCP.RecipeList"
    MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <uc2:ListPage2 ID="ListPage2" runat="server" PageCode="[PT].[vRcpList]" EditPageURL="RecipeEdit.aspx"
        DetailPageURL="RecipeDt.aspx" KeyFieldName="RcpCode" AllowDelete="False" AllowPrint="False"  Module="PT"
        SubModule="Recipe"
        Title="Recipe" />
</asp:Content>
