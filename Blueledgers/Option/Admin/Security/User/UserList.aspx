<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="UserList.aspx.cs"
    Inherits="BlueLedger.PL.Option.Admin.Security.User.UserList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .Background
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        
        .Popup
        {
            background-color: White;
            border-width: 1px;
            border-radius: 10px;
            padding-top: 15px;
            padding-left: 15px;
            padding-right: 15px;
            padding-bottom: 15px;
            width: 890px;
            height: 540px;
        }
        .Title
        {
            font-size: 18px;
            font-style: italic;
            font-weight: bold;
            color: Blue;
        }
    </style>
    <style type="text/css">
        .footer
        {
            background: white;
            position: fixed;
            right: 0;
            bottom: 0;
            height: auto;
            width: auto;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        function doClick(btnS, e) {
            var key;
            if (window.event) {
                key = window.event.keycode;
            } else {
                key = e.which;
            }

            if (key == 13) {
                var btn = document.getElementById(btnS);
                if (btnS != null) {
                    event.keyCode = 0;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="btnUserInfo" runat="server" />
            <div style="width: 100%">
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
                                        <asp:Label ID="lblTitle" Text="UserList" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="btnAddUser" AlternateText="Add" runat="server" OnClick="Create" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" />
                            <asp:ImageButton ID="btnPrint" AlternateText="Print" runat="server" OnClick="Print" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" />
                        </td>
                    </tr>
                </table>
                <!-- Option -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding: 5px;">
                    <tr valign="top" height="30px">
                        <td align="left">
                            <div style="width: 100%; display: inline-block;">
                                <asp:Label ID="Label1" runat="server" Text="View: "></asp:Label>
                                <asp:DropDownList ID="ddl_Status" runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddl_Status_SelectedIndexChanged">
                                    <asp:ListItem Value="" Text="All" Selected="True" />
                                    <asp:ListItem Value="1" Text="Active" />
                                    <asp:ListItem Value="0" Text="Inactive" />
                                </asp:DropDownList>
                                <asp:LinkButton ID="btnRresh" Text="Refresh" Font-Underline="false" ForeColor="Black" runat="server" OnClick="btnHome_Click"></asp:LinkButton>
                            </div>
                        </td>
                        <td align="right">
                            <asp:Panel ID="pntxt" runat="server" DefaultButton="btnS" Width="100%">
                                <div>
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200px"></asp:TextBox>
                                    <asp:ImageButton ID="btnS" AlternateText="Search" runat="server" OnClick="btnS_Click" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" />
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="">
                <asp:Label ID="lblcountA" runat="server" Font-Size="Small"></asp:Label>
            </div>
            <br />
            <asp:GridView ID="gvUserList" runat="server" AutoGenerateColumns="false" DataKeyNames="loginName" Width="100%" BackColor="White" BorderColor="#DDDDDD"
                GridLines="Horizontal" Font-Names="Arial" Font-Size="12px" OnSelectedIndexChanged="gvUserList_SelectedIndexChanged" OnRowDataBound="gvUserList_RowDataBound">
                <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" BorderColor="#DDDDDD" />
                <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                <Columns>
                    <asp:BoundField HeaderText="#" DataField="RowId">
                        <ItemStyle HorizontalAlign="Center" Width="40px" Font-Size="Small" />
                        <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgIcon" AlternateText="Icon" Height="24" ImageUrl="~/App_Themes\Default\Images\master\icon\DefaultUserIcon.png" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Login Name" DataField="LoginName">
                        <ItemStyle HorizontalAlign="Left" Width="240px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Profile">
                        <ItemTemplate>
                            <div style="font-size: small;">
                                <%# Eval("FullName") %>
                            </div>
                            <span style="color: Black">
                                <%# Eval("Email") %>
                            </span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="200px" ForeColor="#088A29" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Job Title" DataField="JobTitle">
                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span style='color: <%# Eval("Status").ToString() == "Active" ? "#2787F5" : "Silver" %>'>
                                <%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Status" DataField="Status">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                    </asp:BoundField>--%>
                    <asp:BoundField HeaderText="Last Login" DataField="LastLogin">
                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                    </asp:BoundField>
                </Columns>
                <SelectedRowStyle BackColor="#4D4D4D" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <!-- Popup -->
            <cc1:ModalPopupExtender ID="pop_UserInfo" runat="server" PopupControlID="panel_UserInfo" TargetControlID="btnUserInfo" CancelControlID="btn_CloseDialog"
                BehaviorID="pop_UserInfo" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panel_UserInfo" runat="server" CssClass="Popup" Style="display: inline-block">
                <!-- Title Bar -->
                <table style="border: 0; border-spacing: 0; background-color: White; width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="Label_UserInfo" runat="server" Text="User Information" CssClass="Title" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btn_CloseDialog" runat="server" Text="X" OnClientClick="window.top.location.reload();" />
                        </td>
                    </tr>
                </table>
                <!-- iFrame -->
                <iframe id="iFrame_UserInfo" runat="server" style="width: 100%; height: 98%" frameborder="0" />
            </asp:Panel>
            <dx:ASPxPopupControl ID="pop_ReachMaxUserNo" runat="server" Width="400px" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg2 %>" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning2 %>" SkinID="LBL_NR"></asp:Label>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
