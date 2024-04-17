<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeCategory.aspx.cs" Inherits="BlueLedger.PL.PT.RCP.RecipeCategory" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPageLookup.ascx" TagName="ListPageLookup" TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <style>
        .no-padding
        {
            padding: 0px;
        }
    </style>
    <uc2:ListPageLookup ID="ListPageLookup" runat="server" Module="PT" SubModule="Recipe" Title="Recipe Category" AllowDelete="False" AllowPrint="True" KeyFieldName="RcpCateCode"
        PageCode="[PT].[vRcpCategoryList]" EditPageURL="RecipeCategory.aspx" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="pop_EditCatgory" runat="server" Width="360px" Modal="True" ContentStyle-CssClass="no-padding" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowHeader="False" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div class="CMD_BAR">
                            <div class="CMD_BAR_LEFT">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:PT_RCP_Recipe, lbl_Title %>" />
                            </div>
                            <div class="CMD_BAR_RIGHT">
                                <dx:ASPxMenu runat="server" ID="menu_CmdBar" BackColor="Transparent" ItemSpacing="3px" OnItemClick="menu_CmdBar_ItemClick">
                                    <Items>
                                        <dx:MenuItem Name="Edit" Text="">
                                            <ItemStyle Height="16px" Width="38px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Delete" Text="">
                                            <ItemStyle Height="16px" Width="41px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Back" Text="">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                    <Paddings Padding="0px" />
                                    <SeparatorPaddings Padding="0px" />
                                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" />
                                    <Border BorderStyle="None" />
                                    <ItemStyle BackColor="Transparent">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings Padding="2px" />
                                    </ItemStyle>
                                </dx:ASPxMenu>
                            </div>
                        </div>
                        <div style="clear: both; padding: 15px;">
                            <div>
                                <asp:Label ID="lbl_CateCode" runat="server" Text="Code" Width="120px" />
                                <asp:TextBox ID="txt_CateCode" runat="server" Style="text-transform: uppercase" Width="260" />
                            </div>
                            <br />
                            <div>
                                <asp:Label ID="lbl_CateDesc" runat="server" Text="Description" Width="120px" />
                                <asp:TextBox ID="txt_CateDesc" runat="server" Width="260" />
                            </div>
                            <br />
                            <div style="text-align: right;">
                                <asp:Button runat="server" ID="btn_Save" Text="Save" OnClick="btn_Save_Click" />
                                <asp:Button runat="server" ID="btn_Cancel" Text="Cancel" OnClick="btn_Cancel_Click" />
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_Alert" ClientInstanceName="pop_Alert" HeaderText="Alert" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Modal="True" Width="300px" CloseAction="CloseButton">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div style="text-align: center;">
                            <asp:Label ID="lbl_Alert" runat="server" SkinID="LBL_NR" />
                        </div>
                        <br />
                        <div style="text-align: center;">
                            <asp:Button ID="btn_AlertOK" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClientClick="pop_Alert.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" HeaderText="Confirmation" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Modal="True" Width="300px" CloseAction="CloseButton">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <div style="text-align: center;">
                            <asp:Label ID="lbl_Confirm" runat="server" SkinID="LBL_NR" />
                        </div>
                        <br />
                        <div style="text-align: center;">
                            <asp:Button ID="btn_ConfirmYes" runat="server" Text="Yes" Width="60px" SkinID="BTN_V1" OnClick="btn_ConfirmYes_Click" />
                            <asp:Button ID="btn_ConfirmNo" runat="server" Text="No" Width="60px" SkinID="BTN_V1" OnClientClick="pop_ConfirmDelete.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
