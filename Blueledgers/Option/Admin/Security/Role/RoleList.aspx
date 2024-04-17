<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" EnableEventValidation="true" CodeFile="RoleList.aspx.cs"
    Inherits="BlueLedger.PL.Option.Admin.Security.Role.RoleList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: auto;">
                <tr>
                    <td align="left">
                        <!-- Title & Command Bar -->
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="background-color: #4d4d4d; height: 17px;">
                                <td align="left" style="padding-left: 10px;">
                                    <table border="0" cellpadding="2" cellspacing="0">
                                        <tr style="color: White;">
                                            <td>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTitle" Text="Role" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <%-- <asp:ImageButton ID="btnAddUser" AlternateText="AddUser" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                        OnClick="btnAddUser_Click" />--%>
                                    <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                        Height="16px" OnItemClick="menu_CmdBar_ItemClick">
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
                                        </Items>
                                        <Paddings Padding="0px" />
                                        <SeparatorPaddings Padding="0px" />
                                        <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                                        <Border BorderStyle="None"></Border>
                                    </dx:ASPxMenu>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvRoleList" runat="server" AutoGenerateColumns="false" Width="100%" BackColor="White" BorderColor="#DDDDDD" GridLines="Horizontal" Font-Names="Arial"
                OnRowDataBound="gvRoleList_RowDataBound" Font-Size="12px">
                <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" BorderColor="#DDDDDD" />
                <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                <Columns>
                    <asp:TemplateField HeaderText="Status" Visible="true">
                        <ItemTemplate>
                            <asp:Image ID="imgCheck" runat="server" Width="16px" Height="16px" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Description" DataField="RoleDesc">
                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" Text="" runat="server" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Status" Visible="true">
                        <ItemTemplate>
                            <asp:Image ID="imgCheck" runat="server" Width="16px" Height="16px"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                </Columns>
                <SelectedRowStyle BackColor="#4D4D4D" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, Msg %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, MsgWarning %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click" Text="<%$ Resources:Option_Admin_Security_Role_Role, btn_Yes %>" Width="50px"
                                        SkinID="BTN_V1" />
                                </td>
                                <td align="left">
                                    <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px">
                            </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click" Text="<%$ Resources:Option_Admin_Security_Role_Role, btn_No %>" Width="50px"
                                        SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
