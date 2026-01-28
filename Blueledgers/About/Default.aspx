<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="License" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Blueledgers</title>
    <style>
        body
        {
            font-family: Arial, Tahoma, sans-serif;
            font-size: 1.5em;
        }
        
        .logo-blue
        {
            height: 48px;
        }
        .main
        {
            width: 50%;
            margin-left: 80px;
            margin-top: 10px;
        }
        
        hr.soften
        {
            clear: both;
            border: 0;
            height: 1px;
            background-image: -webkit-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -moz-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -ms-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -o-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -webkit-linear-gradient(left, #8c8b8b, #f0f0f0);
            background-image: -moz-linear-gradient(left,  #8c8b8b, #f0f0f0);
            background-image: -ms-linear-gradient(left,  #8c8b8b, #f0f0f0);
            background-image: -o-linear-gradient(left,  #8c8b8b, #f0f0f0);
            margin: 15px 0 15px 0;
        }
        
        .title
        {
            font-weight: bold;
            width: 100px;
        }
        .copyright
        {
            margin-top: 25px;
        }
        .button
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 15px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
        .align-right
        {
            float: right;
            margin-right: 10px;
        }
    </style>
</head>
<body>
    <form id="FormMain" runat="server">
    <div class="main">
        <div>
            <h2>
                About
            </h2>
        </div>
        <hr class="soften" />
        <div style="height: 80px; width: 100%;">
            <div style="float: left; width: 60px;">
                <img alt="blueledgers" class="logo-blue" src="../App_Themes/Default/Images/login/_logo.png"></div>
            <div style="float: left; margin-left: 10px; vertical-align: top;">
                <div style="font-size: large; margin-bottom: 10px;">
                    Blueledgers
                </div>
                <div>
                    Version 3
                </div>
            </div>
        </div>
        <div style="clear: both;">
            <table>
                <tr>
                    <td class="title">
                        <span>Expired Date</span>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ExpiredDate" runat="server" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <span>License User</span>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ActiveUserLicense" runat="server" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <span>Current User</span>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ActiveUserCurrent" runat="server" />
                    </td>
                    <td>
                        <span runat="server" enableviewstate="False" id="lblCursor" style="cursor: pointer;">
                            <img id="imgButton" alt="Manage" src="../App_Themes\Default\Images\master\in\Default/icon_over_edit.png" style="width: 16px; height: 16px;" />
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="copyright">
            &copy 2020, CARMEN SOFTWARE CO.,LTD.
        </div>
        <hr class="soften" />
        <div>
            <asp:Label ID="lbl_Test" runat="server"></asp:Label>
            <br />
            <asp:GridView runat="server" ID="grd_Test">
            </asp:GridView>
        </div>
        <dx:ASPxPopupControl ID="pop_Login" runat="server" ClientInstanceName="pop_Login" EnableViewState="false" PopupElementID="imgButton" EnableHierarchyRecreation="True"
            PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Below" HeaderText="Authentication" Width="330px" Height="160px">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <asp:Panel ID="Panel1" runat="server">
                        <p>
                            <asp:TextBox ID="txt_login" runat="server" value="" placeholder="Username" /></p>
                        <p>
                            <asp:TextBox ID="txt_password" runat="server" TextMode="Password" value="" placeholder="Password" /></p>
                        <p>
                            <asp:Button ID="btn_LogIn" runat="server" Text="Log In" OnClick="btn_LogIn_Click" />
                        </p>
                    </asp:Panel>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_UserList" runat="server" ClientInstanceName="pop_UserList" HeaderText="User(s)" Modal="True" CloseAction="CloseButton" AllowDragging="True"
            PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div style="margin-bottom: 5px;">
                        <asp:Label ID="lbl_ActiveUserCount" runat="server" Font-Size="Large" />
                    </div>
                    <asp:GridView runat="server" ID="gv_Users" Width="100%" AutoGenerateColumns="false" DataKeyNames="LoginName" OnRowCommand="gv_Users_RowCommand" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField HeaderText="No." DataField="RowId" />
                            <asp:BoundField HeaderText="Login name" DataField="LoginName"  />
                            <asp:BoundField HeaderText="First name" DataField="FName" />
                            <asp:BoundField HeaderText="Middle name" DataField="MName" />
                            <asp:BoundField HeaderText="Last name" DataField="LName" />
                            
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <div style="margin: 5px;">
                                        <asp:Label runat="server" Font-Bold="true" ForeColor="Blue"  Text='<%# Eval("Status") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <div style="margin: 5px;">
                                        <asp:Button ID="btn_Active" runat="server" Text="Active" CommandArgument='<%# Eval("LoginName") %>' CommandName="active" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <div style="margin: 5px;">
                                        <asp:Button ID="btn_Inactive" runat="server" Text="Inactive" CommandArgument='<%# Eval("LoginName") %>' CommandName="inactive" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
    <!--Hidden Field -->
    <asp:HiddenField runat="server" ID="hf_UserActive" />
    <asp:HiddenField runat="server" ID="hf_UserLicense" />
    <asp:HiddenField runat="server" ID="hf_ExpiredDate" />
    </form>
</body>
</html>
