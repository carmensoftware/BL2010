<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Store.aspx.cs" Inherits="BlueLedger.PL.Option.Store.Store" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            .printable, .printable *
            {
                visibility: visible;
            }
            .printable
            {
                position: absolute;
                left: 0;
                top: 0;
            }
    </style>
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin: auto;">
                    <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_StoreLocation_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_StoreLocation_Nm %>"
                                SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                OnItemClick="menu_CmdBar_ItemClick">
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
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Edit" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" Text="">
                                        <ItemStyle Height="16px" Width="47px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                                <Paddings Padding="0px" />
                                <SeparatorPaddings Padding="0px" />
                                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                    ForeColor="#4D4D4D" />
                                <Border BorderStyle="None"></Border>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                <div class="printable">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin: auto;">
                        <tr>
                            <td align="left" style="padding-left: 10px">
                                <asp:Label ID="lbl_Code_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_Code_Nm %>"
                                    Font-Bold="True" SkinID="LBL_HD">
                                </asp:Label>
                                <%--width: 10%;--%>
                            </td>
                            <td align="left">
                                <%--style="width: 20%"--%>
                                <asp:Label ID="lbl_Code" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="left">
                                <%--style="width: 10%"--%>
                                <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_Store_Nm %>"
                                    Font-Bold="True" SkinID="LBL_HD">
                                </asp:Label>
                            </td>
                            <td align="left">
                                <%--style="width: 20%"--%>
                                <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="left">
                                <%--style="width: 10%"--%>
                                <asp:Label ID="lbl_Dept_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_Dept_Nm %>"
                                    Font-Bold="True" SkinID="LBL_HD">
                                </asp:Label>
                            </td>
                            <td align="left">
                                <%--style="width: 20%"--%>
                                <asp:Label ID="lbl_Dept" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%; padding-left: 10px">
                                <asp:Label ID="lbl_EOP_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_EOP_Nm %>"
                                    Font-Bold="True" SkinID="LBL_HD">
                                </asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_Eop" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="left" style="width: 10%">
                                <asp:Label ID="lbl_DelPoint_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_DelPoint_Nm %>"
                                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_DelPoint" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="left" style="width: 10%">
                                <asp:Label ID="lbl_AccCode_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_AccCode_Nm %>"
                                    Font-Bold="True" Visible="False" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_AccCode" runat="server" Visible="False" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%; padding-left: 10px">
                                <asp:Label ID="lbl_IsActive" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_IsActive %>"
                                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_IsActive" runat="server" SkinID="CHK_V1" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin: auto">
                        <%--<tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">--%>
                        <tr>
                            <td align="right" style="padding-right: 10px;">
                                <%-- <dx:ASPxMenu runat="server" ID="menu_Module" Font-Bold="True" BackColor="Transparent"
                                    Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                    OnItemClick="menu_Module_ItemClick">
                                    <ItemStyle BackColor="Transparent">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings Padding="2px" />
                                        <Border BorderStyle="None" />
                                    </ItemStyle>
                                    <Items>
                                        <dx:MenuItem Name="Save" Text="">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                    <Paddings Padding="0px" />
                                    <SeparatorPaddings Padding="0px" />
                                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                        ForeColor="#4D4D4D" />
                                    <Border BorderStyle="None"></Border>
                                </dx:ASPxMenu>--%>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%;">
                        <asp:Label ID="lbl_Test" runat="server"></asp:Label>
                        <asp:GridView ID="grd_Test" runat="server" AutoGenerateColumns="true">
                        </asp:GridView>
                    </div>
                    <dx:ASPxTreeList ID="tl_ProdCate" runat="server" AutoGenerateColumns="False" KeyFieldName="CategoryCode"
                        OnLoad="tl_ProdCate_Load" ParentFieldName="ParentNo" Width="100%">
                        <SettingsSelection AllowSelectAll="True" Enabled="false" Recursive="True" />
                        <SettingsBehavior AutoExpandAllNodes="false" />
                        <Columns>
                            <dx:TreeListTextColumn Caption="<%$ Resources:Option.Inventory.Store.Store, lbl_Assign_HD_TL %>"
                                FieldName="CategoryName" VisibleIndex="0">
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn Caption="<%$ Resources:PC_MLT_MLTEdit, lbl_Product_TL %>"
                                VisibleIndex="1" FieldName="ProductDesc">
                            </dx:TreeListTextColumn>
                        </Columns>
                    </dx:ASPxTreeList>
                </div>
                <dx:ASPxPopupControl ID="pop_RoleStoreLocationSave" runat="server" Width="300px"
                    HeaderText="<%$ Resources:Option.Inventory.Store.Store, pop_RoleStoreLocationSave %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table width="100%" cellpadding="5" cellspacing="0">
                                <tr align="center">
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_SaveSuc_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_SaveSuc_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center">
                                        <asp:Button ID="btn_OKsave" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, btn_OKsave %>"
                                            SkinID="BTN_V1" Width="50px" OnClick="btn_OKsave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.Inventory.Store.Store, pop_ConfrimDelete %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_ConfirmDelete_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, btn_ConfrimDelete %>"
                                            Width="50px" OnClick="btn_ConfrimDelete_Click" SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, btn_CancelDelete %>"
                                            Width="50px" OnClick="btn_CancelDelete_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_WarningDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.Inventory.Store.Store, pop_WarningDelete %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_cannotDelete_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_cannotDelete_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_OK_Delete" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, btn_OK_Delete %>"
                                            SkinID="BTN_V1" OnClick="btn_OK_Delete_Click" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_Alert" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.Inventory.Store.Store, pop_Alert %>" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_NotAllow_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, lbl_NotAllow_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_ok_alert" runat="server" OnClick="btn_ok_alert_Click" Text="<%$ Resources:Option.Inventory.Store.Store, btn_ok_alert %>"
                                            Width="50px" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_CheckDelete" runat="server" Width="300px" HeaderText="<%$ Resources:Option.Inventory.Store.Store, pop_CheckDelete %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table cellpadding="5" cellspacing="0" width="100%">
                                <tr align="center">
                                    <td height="50px">
                                        <asp:Label ID="lbl_CheckDelete" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <asp:Button ID="btn_OK_ChkDelete" runat="server" Text="<%$ Resources:Option.Inventory.Store.Store, btn_OK_ChkDelete %>"
                                            SkinID="BTN_V1" Width="50px" OnClick="btn_OK_ChkDelete_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
            <%--<asp:AsyncPostBackTrigger ControlID="menu_Module" EventName="ItemClick" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgDetail"
        PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
