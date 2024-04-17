<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListPageLookup.ascx.cs" Inherits="BlueLedger.PL.UserControls.ViewHandler.ListPageLookup" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<link href="../../App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .ToolBar
    {
        width: 100%;
        margin-top: 5px;
    }
    
    .ToolBar_Left
    {
        float: left;
        padding: 0px 0px 0px 10px;
    }
    .ToolBar_Right
    {
        float: Right;
    }
</style>
<script type="text/javascript">
    /* adding to "grd_Trans_RowCreated" */
    var prevBgColor;
    var prevColor;

    function OnGridRowMouseOver(rowObj) {
        prevBgColor = rowObj.style.backgroundColor;
        prevColor = rowObj.style.color;

        rowObj.style.backgroundColor = "#4d4d4d";
        rowObj.style.color = "#ffffff";
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowMouseOut(rowObj) {
        rowObj.style.backgroundColor = prevBgColor;
        rowObj.style.color = prevColor;
        rowObj.style.cursor = "pointer";
    }
    /* ---------------------------------- */

    /* adding to "grd_Trans_RowDataBound" */
    function OnGridRowClick(buCode, id, vid) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid;

    }

    function OnSuccess(response, userContext, methodName) {
        alert(response);
    }
    
</script>
<!-- ----------------------------------------------------------------------------------------------------------------------------------------------- -->
<div style="width: 100%;">
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" EnableViewState="False" />
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" EnableViewState="False"></asp:Label>
        </div>
        <div class="CMD_BAR_RIGHT">
            <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                Height="16px">
                <ItemStyle BackColor="Transparent">
                    <HoverStyle BackColor="Transparent">
                        <Border BorderStyle="None" />
                    </HoverStyle>
                    <Paddings Padding="2px" />
                    <Border BorderStyle="None" />
                </ItemStyle>
                <Items>
                    <dx:MenuItem Name="Create" Text="">
                        <ItemStyle Height="16px" Width="49px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Delete" Text="">
                        <ItemStyle Height="16px" Width="47px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Void" Text="">
                        <ItemStyle Height="16px" Width="41px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="ClosePO" Text="">
                        <ItemStyle Height="16px" Width="57px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-closepo.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/closepo.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Print" Text="">
                        <ItemStyle Height="16px" Width="43px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Favotires" Visible="False" Text="">
                        <ItemStyle Height="26px" Width="29px">
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/favorites.gif" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                </Items>
                <Paddings Padding="0px" />
                <SeparatorPaddings Padding="0px" />
                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                <Border BorderStyle="None"></Border>
            </dx:ASPxMenu>
        </div>
    </div>
    <div class="ToolBar">
        <div class="ToolBar_Left">
            <table border="0" cellpadding="2" cellspacing="0">
                <tr valign="middle" style="height: 25px">
                    <td>
                        <asp:Label ID="lbl_View" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_View %>" SkinID="LBL_HD" EnableViewState="False"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_View2" runat="server" AutoPostBack="True" DataTextField="Desc" DataValueField="ViewNo" OnSelectedIndexChanged="ddl_View2_SelectedIndexChanged"
                            SkinID="DDL_V1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btn_ViewGo2" runat="server" OnClick="btn_ViewGo2_Click" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewGo2 %>" SkinID="BTNView_V1"
                            EnableViewState="False" />
                    </td>
                    <td>
                        |
                    </td>
                    <td>
                        <asp:Button ID="btn_ViewModify2" runat="server" OnClick="btn_ViewModify2_Click" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewModify2 %>"
                            SkinID="BTNView_V1" EnableViewState="False" />
                    </td>
                    <td>
                        |
                    </td>
                    <td>
                        <asp:Button ID="btn_ViewCreate2" runat="server" OnClick="btn_ViewCreate2_Click" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewCreate2 %>"
                            SkinID="BTNView_V1" EnableViewState="False" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ToolBar_Right">
            <asp:Panel ID="p_SearchBasic" runat="server" EnableViewState="False">
                <table border="0" cellpadding="1" cellspacing="0" width="20%">
                    <tr valign="middle">
                        <td align="center">
                            <asp:TextBox ID="txt_FullTextSearch" runat="server" SkinID="TXT_V1" Height="12px" Width="330px" AutoPostBack="True" OnTextChanged="txt_FullTextSearch_TextChanged"></asp:TextBox>
                        </td>
                        <td style="width: 16px">
                            <asp:ImageButton ID="btn_Search" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" ToolTip="Search" OnClick="btn_Search_Click"
                                EnableViewState="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="false">
        <ContentTemplate>
            <asp:GridView ID="grd_BU" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" Width="100%" ShowHeader="False" BorderStyle="Solid" BorderWidth="1px"
                BorderColor="#999999" GridLines="Horizontal" CellPadding="3" CellSpacing="0" EnableViewState="False" OnRowDataBound="grd_BU_RowDataBound">
                <%----%>
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin: auto">
                                <tr style="height: 20px">
                                    <td valign="middle">
                                        <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClick="btn_Expand_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="25px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin: auto">
                                <tr style="height: 20px">
                                    <td valign="middle">
                                        <asp:Label ID="lbl_BU" runat="server" EnableViewState="False"></asp:Label><%----%>
                                        <asp:HiddenField ID="hf_BuCode" runat="server" />
                                        <asp:Label ID="lbl_Message" Visible="False" ForeColor="Red" runat="server" EnableViewState="False">[This page found items more than 1000 items please use search for find another]</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grd_Trans" runat="server" AllowPaging="True" SkinID="GRD_V1" PageSize="25" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
                                            ShowFooter="True" OnPageIndexChanging="grd_Trans_PageIndexChanging" OnRowDataBound="grd_Trans_RowDataBound" OnSorting="grd_Trans_Sorting" OnRowCreated="grd_Trans_RowCreated"
                                            EnableViewState="false">
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <HeaderStyle ForeColor="White" Height="20px" HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2" PopupControlID="UpdateProgress2" BackgroundCssClass="POPUP_BG"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
        <ProgressTemplate>
            <div style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_PageDialog" runat="server" BehaviorID="ModalPopupExtender_PageDialog" PopupControlID="PageDialog"
        TargetControlID="btn_PageDialog_Target" CancelControlID="btn_PageDialog_Cancel" BackgroundCssClass="ShowDialog_Background">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PageDialog" runat="server" CssClass="ShowDialog">
        <!-- Title Bar -->
        <div style="background-color: White; width: 100%;">
            <div style="float: left;">
                <asp:Label ID="lbl_PageDialog_Title" runat="server" Text="" CssClass="ShowDialog_Title" />
            </div>
            <div style="float: right;">
                <asp:Button ID="btn_PageDialog_Cancel" runat="server" Text="X" OnClientClick="window.top.location.reload();" />
            </div>
        </div>
        <!-- iFrame -->
        <iframe id="iFrame_PageDialog" runat="server" style="width: 640px; height: 480px" frameborder="0" />
    </asp:Panel>
    <asp:HiddenField ID="btn_PageDialog_Target" runat="server" />
</div>
